using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PCLCrypto;
using HashAlgorithm = PCLCrypto.HashAlgorithm;

namespace CameraControl.Tests
{
    [TestClass]
    public class PclCryptoTest
    {
        [TestMethod]
        public void DoesPclCryptoSha1HashEqualsToDotNetSha1Hash()
        {
            // Arrange
            var bytes = Encoding.UTF8.GetBytes("test");
            var pclCryptoHasher = WinRTCrypto.HashAlgorithmProvider.OpenAlgorithm(HashAlgorithm.Sha1);
            var dotNetHasher = SHA1.Create();

            // Act
            var pclCryptoHash = pclCryptoHasher.HashData(bytes);
            var dotNetHash = dotNetHasher.ComputeHash(bytes);

            var pclCryptoHashString = Convert.ToBase64String(pclCryptoHash);
            var dotNetHashString = Convert.ToBase64String(dotNetHash);

            // Assert
            Assert.AreEqual(pclCryptoHashString, dotNetHashString);
        }
    }
}
