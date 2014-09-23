namespace Geocrest.Web.Infrastructure.Security
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// Provides an encryption algorithm based on the <see cref="T:System.Security.Cryptography.Aesmanaged"/> class.
    /// </summary>
    public static class Encryption
    {
        /// <summary>
        /// Encrypts the specified plain text argument into a base 64 string.
        /// </summary>
        /// <param name="plainText">The plain text to encrypt.</param>
        /// <param name="password">The password used for encryption.</param>
        /// <returns>A base64-encoded string.</returns>
        public static string Encrypt(string plainText, string password)
        {
            byte[] keyBytes = UTF8Encoding.UTF8.GetBytes(password);
            Rfc2898DeriveBytes rfc = new Rfc2898DeriveBytes(password, keyBytes, 1000);

            // Use the AES managed encryption provider
            AesManaged encryptor = new AesManaged();
            encryptor.Key = rfc.GetBytes(16);
            encryptor.IV = rfc.GetBytes(16);

            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream encrypt = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    byte[] dataBytes = new UTF8Encoding(false).GetBytes(plainText);
                    encrypt.Write(dataBytes, 0, dataBytes.Length);
                    encrypt.FlushFinalBlock();
                    encrypt.Close();
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        /// <summary>
        /// Decrypts the specified encrypted text argument into it's plain text equivalent.
        /// </summary>
        /// <param name="encryptedText">The encrypted text to decrypt.</param>
        /// <param name="password">The password used for decryption.</param>
        /// <returns>The unencrypted plain text.</returns>
        public static string Decrypt(string encryptedText, string password)
        {
            byte[] keyBytes = new UTF8Encoding(false).GetBytes(password);
            Rfc2898DeriveBytes rfc = new Rfc2898DeriveBytes(password, keyBytes, 1000);

            AesManaged decryptor = new AesManaged();
            decryptor.Key = rfc.GetBytes(16);
            decryptor.IV = rfc.GetBytes(16);

            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream decrypt = new CryptoStream(ms, decryptor.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    byte[] dataBytes = Convert.FromBase64String(encryptedText);
                    decrypt.Write(dataBytes, 0, dataBytes.Length);
                    decrypt.FlushFinalBlock();
                    decrypt.Close();
                    return new UTF8Encoding(false).GetString(ms.ToArray());
                }
            }
        }
    }
}
