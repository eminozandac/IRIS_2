using System;
using CameraControl.SoapClient.Models.Headers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Web.Services3.Security.Tokens;

namespace CameraControl.Tests
{
    [TestClass]
    public class UsernameTokenTest
    {
        [TestMethod]
        public void DoesUsernameTokenPasswordDigestHashEqualsToWseUsernameTokenPasswordHash()
        {
            // Arrange
            const string password = "admin";
            var nonce = KnownHeader.Oasis.Security.GetNonce();
            var created = DateTime.Now;

            // Act
            var wsePasswordHash = UsernameToken.ComputePasswordDigest(nonce, created, password);
            var digestPasswordHash = KnownHeader.Oasis.Security.ComputePasswordDigest(nonce, created, password);

            var wsePassword = Convert.ToBase64String(wsePasswordHash);
            var digestPassword = Convert.ToBase64String(digestPasswordHash);

            // Assert
            Assert.AreEqual(wsePassword, digestPassword);
        }
    }
}
