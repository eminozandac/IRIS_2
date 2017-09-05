#region License

// The MIT License (MIT)
// 
// Copyright (c) 2016 João Simões
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

#endregion

using System;
using System.Text;
using CameraControl.SoapClient.Models.Headers.Microsoft;
using CameraControl.SoapClient.Models.Headers.Oasis.Security;
using PCLCrypto;

namespace CameraControl.SoapClient.Models.Headers
{
    /// <summary>
    ///     Class with known <see cref="SoapHeader" /> builder methods.
    /// </summary>
    public static class KnownHeader
    {
        /// <summary>
        ///     Class with Microsoft specific <see cref="SoapHeader" /> builder methods.
        /// </summary>
        public static class Microsoft
        {
            /// <summary>
            ///     Creates a new Microsoft Action SOAP Header.
            /// </summary>
            /// <param name="action">The action for the header</param>
            /// <param name="mustUnderstand">Does the server must understand the header?</param>
            /// <returns>The new <see cref="ActionSoapHeader" /></returns>
            public static ActionSoapHeader Action(string action, bool mustUnderstand = true)
            {
                return new ActionSoapHeader
                {
                    Action = action,
                    MustUnderstand = mustUnderstand ? 1 : 0
                };
            }

            /// <summary>
            ///     Creates a new Microsoft To SOAP Header.
            /// </summary>
            /// <param name="to">The action for the header</param>
            /// <param name="mustUnderstand">Does the server must understand the header?</param>
            /// <returns>The new <see cref="ToSoapHeader" /></returns>
            public static ToSoapHeader To(string to, bool mustUnderstand = true)
            {
                return new ToSoapHeader
                {
                    To = to,
                    MustUnderstand = mustUnderstand ? 1 : 0
                };
            }
        }

        /// <summary>
        ///     Class with Oasis specific <see cref="SoapHeader" /> builder methods.
        /// </summary>
        public static class Oasis
        {
            /// <summary>
            ///     Class with Oasis Security specific <see cref="SoapHeader" /> builder methods.
            /// </summary>
            public static class Security
            {
                /// <summary>
                ///     Creates a new Oasis Security Username Token with password text SOAP header.
                /// </summary>
                /// <param name="username">The username</param>
                /// <param name="password">The password</param>
                /// <param name="mustUnderstand">Does the server must understand the header?</param>
                /// <returns>The new <see cref="UsernameTokenAndPasswordTextSoapHeader" /></returns>
                public static UsernameTokenAndPasswordTextSoapHeader UsernameTokenAndPasswordText(string username, string password, bool mustUnderstand = true)
                {
                    var randomId = Guid.NewGuid().ToString("N");

                    return new UsernameTokenAndPasswordTextSoapHeader
                    {
                        Timestamp = new Timestamp
                        {
                            Id = string.Concat("_TS", randomId),
                            Created = DateTime.UtcNow,
                            Expires = DateTime.UtcNow.AddMinutes(15)
                        },
                        UsernameToken = new UsernameTokenWithPasswordText
                        {
                            Id = string.Concat("_UT", randomId),
                            Username = username,
                            Password = new UsernameTokenPasswordText
                            {
                                Value = password
                            }
                        },
                        MustUnderstand = mustUnderstand ? 1 : 0
                    };
                }

                /// <summary>
                ///     Creates a new Oasis Security Username Token with digested password SOAP header.
                /// </summary>
                /// <param name="username">The username</param>
                /// <param name="password">The password</param>
                /// <param name="createdDate">Created date</param>
                /// <param name="mustUnderstand">Does the server must understand the header?</param>
                /// <returns>The new <see cref="UsernameTokenAndPasswordDigestSoapHeader" /></returns>
                public static UsernameTokenAndPasswordDigestSoapHeader UsernameTokenWithPasswordDigest(string username, string password, DateTime createdDate, bool mustUnderstand = true)
                {
                    var nonce = GetNonce();

                    byte[] passwordHash = null;
                    if (password != null)
                    {
                        passwordHash = ComputePasswordDigest(nonce, createdDate, password);
                    }

                    var header = new UsernameTokenAndPasswordDigestSoapHeader
                    {
                        MustUnderstand = mustUnderstand ? 1 : 0,
                        UsernameToken = new UsernameTokenWithPasswordDigest
                        {
                            Username = username,
                            Password = new UsernameTokenPasswordDigest
                            {
                                Value = passwordHash != null ? Convert.ToBase64String(passwordHash) : null
                            },
                            Nonce = new UsernameTokenNonce
                            {
                                Value = Convert.ToBase64String(nonce)
                            },
                            Created = createdDate.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ")
                        }
                    };

                    return header;
                }

                /// <summary>
                ///     Returns random nonce bytes
                /// </summary>
                public static byte[] GetNonce()
                {
                    var phrase = Guid.NewGuid().ToString();
                    var phraseBytes = Encoding.UTF8.GetBytes(phrase);

                    return ComputeSha1(phraseBytes);
                }

                /// <summary>
                ///     Computes password digest
                /// </summary>
                public static byte[] ComputePasswordDigest(byte[] nonce, DateTime created, string password)
                {
                    if (nonce == null || nonce.Length == 0)
                    {
                        throw new ArgumentNullException(nameof(nonce));
                    }
                    if (password == null)
                    {
                        throw new ArgumentNullException(nameof(password));
                    }

                    var createdBytes = Encoding.UTF8.GetBytes(created.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ"));
                    var passwordBytes = Encoding.UTF8.GetBytes(password);
                    var passwordHashBytes = new byte[nonce.Length + createdBytes.Length + passwordBytes.Length];

                    Array.Copy(nonce, passwordHashBytes, nonce.Length);
                    Array.Copy(createdBytes, 0, passwordHashBytes, nonce.Length, createdBytes.Length);
                    Array.Copy(passwordBytes, 0, passwordHashBytes, nonce.Length + createdBytes.Length, passwordBytes.Length);

                    return ComputeSha1(passwordHashBytes);
                }

                /// <summary>
                ///     Computes SHA1 hash
                /// </summary>
                private static byte[] ComputeSha1(byte[] phrase)
                {
                    if (phrase == null)
                    {
                        throw new ArgumentNullException(nameof(phrase));
                    }

                    var hasher = WinRTCrypto.HashAlgorithmProvider.OpenAlgorithm(HashAlgorithm.Sha1);
                    var hashBytes = hasher.HashData(phrase);

                    return hashBytes;
                }
            }
        }
    }
}
