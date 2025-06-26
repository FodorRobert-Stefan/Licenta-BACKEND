using Backend.DataAbstraction;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Backend.Service
{
  public class HybridCryptoService : IHybridCryptoService
  {
    private readonly RSA _rsa;

    public HybridCryptoService(string base64PrivateKey = null)
    {
      _rsa = RSA.Create();
      if (!string.IsNullOrEmpty(base64PrivateKey))
      {
        _rsa.ImportRSAPrivateKey(Convert.FromBase64String(base64PrivateKey), out _);
      }
    }

    public string GetPublicKey()
    {
      return Convert.ToBase64String(_rsa.ExportRSAPublicKey());
    }

    public string GetPrivateKey()
    {
      return Convert.ToBase64String(_rsa.ExportRSAPrivateKey());
    }

    public string EncryptHybrid(string plainText, string base64RsaPublicKey)
    {
      using var aes = Aes.Create();
      aes.KeySize = 256;
      aes.GenerateKey();
      aes.GenerateIV();

      byte[] aesKey = aes.Key;
      byte[] aesIv = aes.IV;

      byte[] encryptedData;
      using (var encryptor = aes.CreateEncryptor())
      {
        byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
        encryptedData = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
      }

      using var rsa = RSA.Create();
      rsa.ImportRSAPublicKey(Convert.FromBase64String(base64RsaPublicKey), out _);
      byte[] encryptedAesKey = rsa.Encrypt(aesKey, RSAEncryptionPadding.Pkcs1);

      var payload = new
      {
        encryptedData = Convert.ToBase64String(encryptedData),
        encryptedAesKey = Convert.ToBase64String(encryptedAesKey),
        iv = Convert.ToBase64String(aesIv)
      };

      return JsonSerializer.Serialize(payload);
    }

    public string DecryptHybrid(string encryptedDataBase64, string encryptedAesKeyBase64, string ivBase64)
    {
      byte[] aesKey = _rsa.Decrypt(Convert.FromBase64String(encryptedAesKeyBase64), RSAEncryptionPadding.Pkcs1);
      byte[] iv = Convert.FromBase64String(ivBase64);
      byte[] encryptedData = Convert.FromBase64String(encryptedDataBase64);

      using var aes = Aes.Create();
      aes.Key = aesKey;
      aes.IV = iv;

      using var decryptor = aes.CreateDecryptor();
      byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedData, 0, encryptedData.Length);

      return Encoding.UTF8.GetString(decryptedBytes);
    }
  }
}
