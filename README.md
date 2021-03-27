# Sign PDF with iText 4.1

This is a sample project to demonstrate how to use iText 4.1 (this is the last version with LGPL license) to sign a pdf without a graphical annotation.

## Usage

1. Compile;

2. Execute:

```
sign_pdf_itext.exe <original_pdf> <output_signed_pdf> <private_key.pfx> <private_key_password> <signature_reason> <signature_location>
```

## Generating a self-singed pfx file for testing

The following steps where extracted from [here](https://stackoverflow.com/questions/20445365/create-pkcs12-file-with-self-signed-certificate-via-openssl-in-windows-for-my-a):

1. Generate an RSA private key:

>C:\Openssl\bin\openssl.exe genrsa -out <Key Filename> <Key Size>

Where:

<Key Filename> is the desired filename for the private key file

<Key Size> is the desired key length of either 1024, 2048, or 4096

For example, type:

>C:\Openssl\bin\openssl.exe genrsa -out my_key.key 2048.

2. Generate a Certificate Signing Request:

In version 0.9.8h and later:

>C:\Openssl\bin\openssl.exe req -new -key <Key Filename> -out <Request Filename> -config C:\Openssl\bin\openssl.cfg

Where:

<Key Filename> is the input filename of the previously generated private key

<Request Filename> is the output filename of the certificate signing request

For example, type:

>C:\Openssl\bin\openssl.exe req -new -key my_key.key -out my_request.csr

3. Follow the on-screen prompts for the required certificate request information.

4. Generate a self-signed public certificate based on the request:

>C:\Openssl\bin\openssl.exe x509 -req -days 3650 -in <Request Filename> -signkey <Key Filename> -out <Certificate Filename>

Where:

<Request Filename> is the input filename of the certificate signing request

<Key Filename> is the input filename of the previously generated private key

<Certificate Filename> is the output filename of the public certificate

For example, type:

>C:\Openssl\bin\openssl.exe x509 -req -days 3650 -in my_request.csr -signkey my_key.key -out my_cert.crt

5. Generate a PKCS#12 file:

>C:\Openssl\bin\openssl.exe pkcs12 -keypbe PBE-SHA1-3DES -certpbe PBE-SHA1-3DES -export -in <Public Certificate Filename> -inkey <Private Key Filename> -out <PKCS#12 Filename> -name "<Display Name>"

Where:

<Public Certificate Filename> is the input filename of the public certificate, in PEM format

<Private Key Filename> is the input filename of the private key

<PKCS#12 Filename> is the output filename of the pkcs#12 format file

<Display Name> is the desired name that will sometimes be displayed in user interfaces.

For example, type:

>C:\Openssl\bin\openssl.exe pkcs12 -keypbe PBE-SHA1-3DES -certpbe PBE-SHA1-3DES -export -in my_cert.crt -inkey my_key.key -out my_pkcs12.pfx -name "my-name"

