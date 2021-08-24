using System;using System.Collections.Generic;using System.IO;using System.Linq;using System.Security.Cryptography;using System.Text;using System.Threading.Tasks;using Microsoft.AspNetCore.Http;namespace AdminLTE1.Security{    public class Encryption    {

        public string EncryptString(string originalText, string saltKey)
        {
            string encryptedText, passwordText;
            TripleDESCryptoServiceProvider des;
            MD5CryptoServiceProvider hashmd5;
            byte[] pwdhash, buff;

            //create a secret password. the password is used to encrypt
            //and decrypt strings. Without the password, the encrypted
            //string cannot be decrypted and is just garbage. You must
            //use the same password to decrypt an encrypted string as the
            //string was originally encrypted with.
            passwordText = saltKey;

            //generate an MD5 hash from the password. 
            //a hash is a one way encryption meaning once you generate
            //the hash, you cant derive the password back from it.
            hashmd5 = new MD5CryptoServiceProvider();
            pwdhash = hashmd5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(passwordText));
            hashmd5 = null;

            //implement DES3 encryption
            des = new TripleDESCryptoServiceProvider();

            //the key is the secret password hash.
            des.Key = pwdhash;

            //the mode is the block cipher mode which is basically the
            //details of how the encryption will work. There are several
            //kinds of ciphers available in DES3 and they all have benefits
            //and drawbacks. Here the Electronic Codebook cipher is used
            //which means that a given bit of text is always encrypted
            //exactly the same when the same password is used.
            des.Mode = CipherMode.ECB; //CBC, CFB


            //----- encrypt an un-encrypted string ------------
            //the original string, which needs encrypted, must be in byte
            //array form to work with the des3 class. everything will because
            //most encryption works at the byte level so you'll find that
            //the class takes in byte arrays and returns byte arrays and
            //you'll be converting those arrays to strings.
            buff = ASCIIEncoding.ASCII.GetBytes(originalText);

            //encrypt the byte buffer representation of the original string
            //and base64 encode the encrypted string. the reason the encrypted
            //bytes are being base64 encoded as a string is the encryption will
            //have created some weird characters in there. Base64 encoding
            //provides a platform independent view of the encrypted string 
            //and can be sent as a plain text string to wherever.
            encryptedText = Convert.ToBase64String(
des.CreateEncryptor().TransformFinalBlock(buff, 0, buff.Length)
);

            //cleanup
            des = null;

            //Return result
            return encryptedText;

        }

        public string DecryptString(string encryptedText, string saltKey)
        {
            string decryptedText, passwordText;
            TripleDESCryptoServiceProvider des;
            MD5CryptoServiceProvider hashmd5;
            byte[] pwdhash, buff;

            //create a secret password. the password is used to encrypt
            //and decrypt strings. Without the password, the encrypted
            //string cannot be decrypted and is just garbage. You must
            //use the same password to decrypt an encrypted string as the
            //string was originally encrypted with.
            passwordText = saltKey;

            //generate an MD5 hash from the password. 
            //a hash is a one way encryption meaning once you generate
            //the hash, you cant derive the password back from it.
            hashmd5 = new MD5CryptoServiceProvider();
            pwdhash = hashmd5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(passwordText));
            hashmd5 = null;

            //implement DES3 encryption
            des = new TripleDESCryptoServiceProvider();

            //the key is the secret password hash.
            des.Key = pwdhash;

            //the mode is the block cipher mode which is basically the
            //details of how the encryption will work. There are several
            //kinds of ciphers available in DES3 and they all have benefits
            //and drawbacks. Here the Electronic Codebook cipher is used
            //which means that a given bit of text is always encrypted
            //exactly the same when the same password is used.
            des.Mode = CipherMode.ECB; //CBC, CFB

            //----- encrypt an un-encrypted string ------------
            //the original string, which needs encrypted, must be in byte
            //array form to work with the des3 class. everything will because
            //most encryption works at the byte level so you'll find that
            //the class takes in byte arrays and returns byte arrays and
            //you'll be converting those arrays to strings.
            buff = Convert.FromBase64String(encryptedText.Replace(' ', '+'));

            //----- decrypt an encrypted string ------------
            //whenever you decrypt a string, you must do everything you
            //did to encrypt the string, but in reverse order. To encrypt,
            //first a normal string was des3 encrypted into a byte array
            //and then base64 encoded for reliable transmission. So, to 
            //decrypt this string, first the base64 encoded string must be 
            //decoded so that just the encrypted byte array remains.
            //buff = Convert.FromBase64String(encryptedText);

            //decrypt DES 3 encrypted byte buffer and return ASCII string
            decryptedText = ASCIIEncoding.ASCII.GetString(
des.CreateDecryptor().TransformFinalBlock(buff, 0, buff.Length)
);

            //cleanup
            des = null;

            return decryptedText;

        }




    }}