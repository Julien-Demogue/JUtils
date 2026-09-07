using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

/// <summary>
/// JSecurity provides methods to encrypt, decrypt, and hash data.
/// Uses AES for encryption and SHA256 for hashing.
/// The key is never stored in this file: it is loaded from a JSecurityConfig asset in
/// Resources, or injected at runtime via <see cref="SetKey"/>.
/// </summary>
public class JSecurity
{
    private const string CONFIG_RESOURCE_PATH = "JSecurityConfig";
    private const int KEY_LENGTH = 32;

    private static string cachedKey;

    /// <summary>
    /// Injects the encryption key at runtime (e.g. from a build script or a CI secret),
    /// bypassing the JSecurityConfig asset lookup.
    /// </summary>
    public static void SetKey(string key)
    {
        cachedKey = key;
    }

    private static byte[] GetKeyBytes()
    {
        if (string.IsNullOrEmpty(cachedKey))
        {
            JSecurityConfig config = Resources.Load<JSecurityConfig>(CONFIG_RESOURCE_PATH);
            cachedKey = config != null ? config.EncryptionKey : null;

            if (string.IsNullOrEmpty(cachedKey))
            {
                throw new InvalidOperationException(
                    $"JSecurity has no encryption key. Create a JSecurityConfig asset at " +
                    $"Resources/{CONFIG_RESOURCE_PATH}.asset (Assets > Create > JUtils > Security Config), " +
                    "or call JSecurity.SetKey(...) before using Encrypt/Decrypt/Hash.");
            }
        }

        if (cachedKey.Length < KEY_LENGTH)
        {
            throw new InvalidOperationException($"JSecurity encryption key must be at least {KEY_LENGTH} characters long.");
        }

        return Encoding.UTF8.GetBytes(cachedKey.Substring(0, KEY_LENGTH));
    }

    /// <summary>
    /// Encrypts a string using AES.
    /// </summary>
    /// <param name="data">Data to encrypt.</param>
    /// <returns>Encrypted data as base64.</returns>
    public static string Encrypt(string data)
    {
        using (AesManaged aes = new AesManaged())
        {
            aes.Key = GetKeyBytes();
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
            aes.Key = GetKeyBytes();
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
            byte[] keyBytes = GetKeyBytes();
            byte[] hashBytes = sha256.ComputeHash(keyBytes);
            return Convert.ToBase64String(hashBytes);
        }
    }
}
