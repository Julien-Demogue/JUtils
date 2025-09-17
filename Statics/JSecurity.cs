using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

/// <summary>
/// JSecurity provides methods to encrypt, decrypt, and hash data.
/// Uses AES for encryption and SHA256 for hashing.
/// </summary>
public class JSecurity
{
    private const string ENCRYPTION_KEY = "m2LJocuSAxbSiPxczcLW6dmccDtTGs9I";

    /// <summary>
    /// Encrypts a string using AES.
    /// </summary>
    /// <param name="data">Data to encrypt.</param>
    /// <returns>Encrypted data as base64.</returns>
    public static string Encrypt(string data)
    {
        using (AesManaged aes = new AesManaged())
        {
            byte[] key = Encoding.UTF8.GetBytes(ENCRYPTION_KEY.Substring(0, 32));
            aes.Key = key;
            aes.GenerateIV();
            ICryptoTransform encryptor = aes.CreateEncryptor();

            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(aes.IV, 0, aes.IV.Length); // Store IV at the beginning
                using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    byte[] plainBytes = Encoding.UTF8.GetBytes(data);
                    cs.Write(plainBytes, 0, plainBytes.Length);
                    cs.FlushFinalBlock();
                }
                return Convert.ToBase64String(ms.ToArray());
            }
        }
    }

    /// <summary>
    /// Decrypts a string encrypted with AES.
    /// </summary>
    /// <param name="encryptedData">Encrypted data as base64.</param>
    /// <returns>Decrypted data.</returns>
    public static string Decrypt(string encryptedData)
    {
        byte[] cipherBytes = Convert.FromBase64String(encryptedData);
        using (AesManaged aes = new AesManaged())
        {
            byte[] key = Encoding.UTF8.GetBytes(ENCRYPTION_KEY.Substring(0, 32));
            aes.Key = key;
            byte[] iv = new byte[16];
            Array.Copy(cipherBytes, 0, iv, 0, iv.Length);
            aes.IV = iv;
            ICryptoTransform decryptor = aes.CreateDecryptor();

            using (MemoryStream ms = new MemoryStream(cipherBytes, 16, cipherBytes.Length - 16))
            using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
            using (StreamReader reader = new StreamReader(cs, Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }
    }

    // ----------------------------------------------------------------------------------------------

    /// <summary>
    /// Returns the SHA256 hash.
    /// </summary>
    public static string Hash()
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(ENCRYPTION_KEY);
            byte[] hashBytes = sha256.ComputeHash(keyBytes);
            return Convert.ToBase64String(hashBytes);
        }
    }
}
