using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Infrastructure.Utilities
{
    public class EncryptUtil
    {
        public static string MD5(string password)
        {
            byte[] textBytes = System.Text.Encoding.Default.GetBytes(password);
            try
            {
                System.Security.Cryptography.MD5CryptoServiceProvider cryptHandler;
                cryptHandler = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] hash = cryptHandler.ComputeHash(textBytes);
                string ret = "";
                foreach (byte a in hash)
                {
                    if (a < 16)
                        ret += "0" + a.ToString("x");
                    else
                        ret += a.ToString("x");
                }
                return ret;
            }
            catch
            {
                throw;
            }
        }

        public static String Encrypt(String original)
        {
            return Encrypt(original, new ASCIIEncoding());
        }

        /// <summary>
        /// Perform a rjindael encryption
        /// </summary>
        /// <param name="original">Orifinal text</param>
        /// <param name="rKey">Key</param>
        /// <param name="rIV">IV</param>
        /// <returns></returns>
        public static String Encrypt(String original, Encoding encoding)
        {
            Encoding textConverter = encoding;
            RijndaelManaged myRijndael = new RijndaelManaged();

            byte[] encrypted;
            byte[] toEncrypt;

            //Get the key and IV.

            //Get an encryptor.
            ICryptoTransform encryptor = myRijndael.CreateEncryptor(Constant.key, Constant.IV);
            //Encrypt the data.
            MemoryStream msEncrypt = new MemoryStream();

            CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);

            //Convert the data to a byte array.

            double tmp = 0;
            string _original = original;
            bool isDouble = Double.TryParse(_original, out tmp);

            if (isDouble)
                original = tmp.ToString();
            toEncrypt = textConverter.GetBytes(original);

            //Write all data to the crypto stream and flush it.
            csEncrypt.Write(toEncrypt, 0, toEncrypt.Length);
            csEncrypt.FlushFinalBlock();

            //Get encrypted array of bytes.
            encrypted = msEncrypt.ToArray();
            String result = "";
            for (int index = 0; index < encrypted.Length; index++)
            {
                String str = Convert.ToString(encrypted[index], 16);
                if (str.Length < 2)
                    str = "0" + str;
                result += str;

            }
            return result;
        }

        public static String Decrypt(String encrypted)
        {
            return Decrypt(encrypted, new ASCIIEncoding());
        }

        /// <summary>
        /// Perform a Rjindael Descryption
        /// </summary>
        /// <param name="encrypted">Encrypted Text</param>
        /// <param name="rKey">Key</param>
        /// <param name="rIV">IV</param>
        /// <returns></returns>
        public static String Decrypt(String encrypted, Encoding encoding)
        {
            if (encrypted == null)
                return String.Empty;
            String roundtrip = "";

            try
            {
                byte[] fromEncrypt;
                Encoding textConverter = encoding;
                RijndaelManaged myRijndael = new RijndaelManaged();
                //             myRijndael.Padding = PaddingMode.PKCS7;
                //Get a decryptor that uses the same key and IV as the encryptor.

                byte[] encryptByte = new byte[encrypted.Length / 2];
                for (int index = 0; index < encryptByte.Length; index++)
                {
                    String str = encrypted.Substring(index * 2, 2);
                    encryptByte[index] = Convert.ToByte(str, 16);
                }

                //             byte[] key = textConverter.GetBytes(rKey);
                //             byte[] IV = textConverter.GetBytes(rIV);
                ICryptoTransform decryptor = myRijndael.CreateDecryptor(Constant.key
                                                                        , Constant.IV);

                //Now decrypt the previously encrypted message using the decryptor
                // obtained in the above step.
                MemoryStream msDecrypt = new MemoryStream(encryptByte);
                CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);

                fromEncrypt = new byte[encrypted.Length];

                //Read the data out of the crypto stream.
                csDecrypt.Read(fromEncrypt, 0, fromEncrypt.Length);

                //Convert the byte array back into a string.
                roundtrip = textConverter.GetString(fromEncrypt);
            }
            catch (System.Exception e)
            {
                //throw new InvalidDataDecryption();
                //throw new Exception("InvalidDataDecryption.Decrypt fail",e);
            }
            int _index = roundtrip.IndexOf("\0");
            if (_index >= 0)
                roundtrip = roundtrip.Substring(0, _index);
            return roundtrip;
        }
        /// <summary>
        /// Perform a rjindael encryption
        /// </summary>
        /// <param name="original">Orifinal text</param>
        /// <param name="rKey">Key</param>
        /// <param name="rIV">IV</param>
        /// <returns></returns>
        public static String Encrypt(String original, out String rKey, out String rIV)
        {
            ASCIIEncoding textConverter = new ASCIIEncoding();
            RijndaelManaged myRijndael = new RijndaelManaged();

            byte[] encrypted;
            byte[] toEncrypt;
            byte[] key;
            byte[] IV;

            //Create a new key and initialization vector.
            //             myRijndael.Padding=PaddingMode.PKCS7;
            myRijndael.GenerateKey();
            myRijndael.GenerateIV();
            //Get the key and IV.
            key = myRijndael.Key;
            IV = myRijndael.IV;
            //Get an encryptor.
            ICryptoTransform encryptor = myRijndael.CreateEncryptor(key, IV);
            //Encrypt the data.
            MemoryStream msEncrypt = new MemoryStream();

            CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);

            //Convert the data to a byte array.
            toEncrypt = textConverter.GetBytes(original);

            //Write all data to the crypto stream and flush it.
            csEncrypt.Write(toEncrypt, 0, toEncrypt.Length);
            csEncrypt.FlushFinalBlock();

            //Get encrypted array of bytes.
            encrypted = msEncrypt.ToArray();
            rKey = "";
            for (int index = 0; index < key.Length; index++)
            {
                String str = Convert.ToString(key[index], 16);
                if (str.Length < 2)
                    str = "0" + str;
                rKey += str;

            }

            rIV = "";
            for (int index = 0; index < IV.Length; index++)
            {
                String str = Convert.ToString(IV[index], 16);
                if (str.Length < 2)
                    str = "0" + str;
                rIV += str;

            }

            String result = "";
            for (int index = 0; index < encrypted.Length; index++)
            {
                String str = Convert.ToString(encrypted[index], 16);
                if (str.Length < 2)
                    str = "0" + str;
                result += str;

            }
            return result;
        }

        /// <summary>
        /// Perform a Rjindael Descryption
        /// </summary>
        /// <param name="encrypted">Encrypted Text</param>
        /// <param name="rKey">Key</param>
        /// <param name="rIV">IV</param>
        /// <returns></returns>
        public static String Decrypt(String encrypted, String rKey, String rIV)
        {
            String roundtrip = "";
            try
            {
                byte[] fromEncrypt;
                ASCIIEncoding textConverter = new ASCIIEncoding();
                RijndaelManaged myRijndael = new RijndaelManaged();
                //             myRijndael.Padding = PaddingMode.PKCS7;
                //Get a decryptor that uses the same key and IV as the encryptor.
                byte[] key = new byte[32];
                byte[] IV = new byte[16];
                for (int index = 0; index < 32; index++)
                {
                    String str = rKey.Substring(index * 2, 2);
                    key[index] = Convert.ToByte(str, 16);
                }

                for (int index = 0; index < 16; index++)
                {
                    String str = rIV.Substring(index * 2, 2);
                    IV[index] = Convert.ToByte(str, 16);
                }
                byte[] encryptByte = new byte[encrypted.Length / 2];
                for (int index = 0; index < encryptByte.Length; index++)
                {
                    String str = encrypted.Substring(index * 2, 2);
                    encryptByte[index] = Convert.ToByte(str, 16);


                }

                //             byte[] key = textConverter.GetBytes(rKey);
                //             byte[] IV = textConverter.GetBytes(rIV);
                ICryptoTransform decryptor = myRijndael.CreateDecryptor(key, IV);

                //Now decrypt the previously encrypted message using the decryptor
                // obtained in the above step.
                MemoryStream msDecrypt = new MemoryStream(encryptByte);
                CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);

                fromEncrypt = new byte[encrypted.Length];

                //Read the data out of the crypto stream.
                csDecrypt.Read(fromEncrypt, 0, fromEncrypt.Length);

                //Convert the byte array back into a string.
                roundtrip = textConverter.GetString(fromEncrypt);
            }
            catch (Exception ex)
            {
                //throw new InvalidDataDecryption("Decrypt fail");
                throw new Exception("InvalidDataDecryption.Decrypt fail", ex);
            }


            return roundtrip;
        }

        public static void CreateKey()
        {
            ASCIIEncoding textConverter = new ASCIIEncoding();
            RijndaelManaged myRijndael = new RijndaelManaged();
            byte[] key;
            byte[] IV;
            myRijndael.GenerateKey();
            myRijndael.GenerateIV();
            //Get the key and IV.
            key = myRijndael.Key;
            IV = myRijndael.IV;

            String rKey = "";
            for (int index = 0; index < key.Length; index++)
            {
                String str = Convert.ToString(key[index], 16);
                if (str.Length < 2)
                    str = "0" + str;
                rKey += str;

            }

            String rIV = "";
            for (int index = 0; index < IV.Length; index++)
            {
                String str = Convert.ToString(IV[index], 16);
                if (str.Length < 2)
                    str = "0" + str;
                rIV += str;

            }

        }

    }
}
