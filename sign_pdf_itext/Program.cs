using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.X509;

namespace sign_pdf_itext
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 4)
            {
                Console.WriteLine("Too few arguments. Required arguments: <original_pdf> <signed_pdf> <privatekey_file> <password> <signature_reason> <signature_location>");
                return;
            }
            string originalFile = args[0];
            string signedPdfFile = args[1];
            string privateKeyFile = args[2];
            string privateKeyPassword = args[3];
            string reason = args[4]!=null? args[4]:"";
            string location = args[5]!=null? args[5]:"";
            using (FileStream privateKeyStream = new FileStream(privateKeyFile, System.IO.FileMode.Open))
            {
                signPdfFile(originalFile, signedPdfFile, privateKeyStream, privateKeyPassword, reason, location);
            }
        }

        public static void signPdfFile(string sourceDocument, string destinationPath, Stream privateKeyStream, string keyPassword, string reason, string location)
        {
            Pkcs12Store pk12 = new Pkcs12Store(privateKeyStream, keyPassword.ToCharArray());
            privateKeyStream.Dispose();

            //then Iterate throught certificate entries to find the private key entry
            string alias = null;
            foreach (string tAlias in pk12.Aliases)
            {
                if (pk12.IsKeyEntry(tAlias))
                {
                    alias = tAlias;
                    break;
                }
            }
            var pk = pk12.GetKey(alias).Key;
            var ce = pk12.GetCertificateChain(alias);
            var chain = new X509Certificate[ce.Length];
            for (int k = 0; k < ce.Length; ++k)
                chain[k] = ce[k].Certificate;
            // reader and stamper
            PdfReader reader = new PdfReader(sourceDocument);
            FileStream fout = new FileStream(destinationPath, FileMode.Create, FileAccess.ReadWrite);


            PdfStamper stamper = PdfStamper.CreateSignature(reader, fout, '\0', null, true);
            PdfSignatureAppearance appearance = stamper.SignatureAppearance;
            appearance.SetCrypto(pk, chain, null, PdfSignatureAppearance.SELF_SIGNED);
            appearance.Reason = reason;
            appearance.Location = location;

            stamper.Close();


        }
    }
}
