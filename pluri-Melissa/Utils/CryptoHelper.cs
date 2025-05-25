using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public static class CryptoHelper
{
    public static byte[] GenerateSalt(int length = 16)
    {
        var salt = new byte[length];
        using (var rng = RandomNumberGenerator.Create())
            rng.GetBytes(salt);
        return salt;
    }

    public static byte[] DeriveKey(string email, byte[] salt, int keySize = 32)
    {
        using var pbkdf2 = new Rfc2898DeriveBytes(email, salt, 100_000);
        return pbkdf2.GetBytes(keySize);
    }

    public static byte[] Encrypt(string plainText, byte[] key)
    {
        using var aes = Aes.Create();
        aes.Key = key;
        aes.GenerateIV();
        using var encryptor = aes.CreateEncryptor();
        byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
        byte[] encrypted = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

        byte[] result = new byte[aes.IV.Length + encrypted.Length];
        Buffer.BlockCopy(aes.IV, 0, result, 0, aes.IV.Length);
        Buffer.BlockCopy(encrypted, 0, result, aes.IV.Length, encrypted.Length);
        return result;
    }

    public static string Decrypt(byte[] encryptedData, byte[] key)
    {
        using var aes = Aes.Create();
        aes.Key = key;

        byte[] iv = new byte[16];
        Buffer.BlockCopy(encryptedData, 0, iv, 0, 16);
        aes.IV = iv;

        using var decryptor = aes.CreateDecryptor();
        byte[] cipherText = new byte[encryptedData.Length - 16];
        Buffer.BlockCopy(encryptedData, 16, cipherText, 0, cipherText.Length);

        byte[] decryptedBytes = decryptor.TransformFinalBlock(cipherText, 0, cipherText.Length);
        return Encoding.UTF8.GetString(decryptedBytes);
    }
}
