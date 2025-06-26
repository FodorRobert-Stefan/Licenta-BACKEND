namespace Backend.DataAbstraction
{
  public interface IHybridCryptoService
  {
    string GetPublicKey();
    string GetPrivateKey();
    string EncryptHybrid(string plainText, string base64RsaPublicKey);
    string DecryptHybrid(string encryptedDataBase64, string encryptedAesKeyBase64, string ivBase64);
  }
}
