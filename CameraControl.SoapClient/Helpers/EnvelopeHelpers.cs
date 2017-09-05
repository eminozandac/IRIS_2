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
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using CameraControl.SoapClient.Exceptions;
using CameraControl.SoapClient.Models;

namespace CameraControl.SoapClient.Helpers
{
    /// <summary>
    ///     Helper methods for working with <see cref="SoapEnvelope" /> instances.
    /// </summary>
    public static class EnvelopeHelpers
    {
        private static readonly XName SoapFaultXName = XName.Get("Fault", Constants.OrgW3Www200305SoapEnvelope);

        #region Body

        /// <summary>
        ///     Sets the given <see cref="XElement" /> as the envelope body.
        /// </summary>
        /// <param name="envelope">The <see cref="SoapEnvelope" /> to be used.</param>
        /// <param name="body">The <see cref="XElement" /> to set as the body.</param>
        /// <returns>The <see cref="SoapEnvelope" /> after changes.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static SoapEnvelope Body(this SoapEnvelope envelope, XElement body)
        {
            if (envelope == null)
            {
                throw new ArgumentNullException(nameof(envelope));
            }

            envelope.Body = body;

            return envelope;
        }

        /// <summary>
        ///     Sets the given entity as the envelope body.
        /// </summary>
        /// <typeparam name="T">The object type</typeparam>
        /// <param name="envelope">The <see cref="SoapEnvelope" /> to be used.</param>
        /// <param name="body">The entity to set as the body.</param>
        /// <returns>The <see cref="SoapEnvelope" /> after changes.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static SoapEnvelope Body<T>(this SoapEnvelope envelope, T body)
        {
            return envelope.Body(SoapClientSettings.Default.SerializationProvider.ToXElement(body));
        }

        /// <summary>
        ///     Extracts the <see cref="SoapEnvelope.Body" /> as an object of the given type.
        /// </summary>
        /// <typeparam name="T">The type do be deserialized.</typeparam>
        /// <param name="envelope">The <see cref="SoapEnvelope" /></param>
        /// <returns>The deserialized object</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="FaultException">Thrown if the body contains a fault</exception>
        public static T Body<T>(this SoapEnvelope envelope)
        {
            if (envelope == null)
            {
                throw new ArgumentNullException(nameof(envelope));
            }

            envelope.ThrowIfFaulted();

            var bodyElement = envelope.Body.Descendants().First();
            return SoapClientSettings.Default.SerializationProvider.ToObject<T>(bodyElement);
        }

        #endregion

        #region Headers

        /// <summary>
        ///     Appends the received <see cref="XElement" /> collection to the existing
        ///     ones in the received <see cref="SoapEnvelope" />.
        /// </summary>
        /// <param name="envelope">The <see cref="SoapEnvelope" /> to append the headers</param>
        /// <param name="headers">The <see cref="SoapHeader" /> collection to append</param>
        /// <returns>The <see cref="SoapEnvelope" /> after changes</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static SoapEnvelope WithHeaders(this SoapEnvelope envelope, params XElement[] headers)
        {
            if (envelope == null)
            {
                throw new ArgumentNullException(nameof(envelope));
            }
            if (headers == null)
            {
                throw new ArgumentNullException(nameof(headers));
            }

            if (headers.Length == 0)
            {
                return envelope;
            }

            if (envelope.Headers == null)
            {
                envelope.Headers = new XElement[0];
            }
            else
            {
                var envelopeHeaders = new List<XElement>(envelope.Headers);
                envelopeHeaders.AddRange(headers);
                envelope.Headers = envelopeHeaders.ToArray();
            }

            return envelope;
        }

        /// <summary>
        ///     Appends the received <see cref="XElement" /> collection to the existing
        ///     ones in the received <see cref="SoapEnvelope" />.
        /// </summary>
        /// <param name="envelope">The <see cref="SoapEnvelope" /> to append the headers</param>
        /// <param name="headers">The <see cref="SoapHeader" /> collection to append</param>
        /// <returns>The <see cref="SoapEnvelope" /> after changes</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static SoapEnvelope WithHeaders(this SoapEnvelope envelope, IEnumerable<XElement> headers)
        {
            return envelope.WithHeaders(headers as XElement[] ?? headers.ToArray());
        }

        /// <summary>
        ///     Appends the received <see cref="SoapHeader" /> header to the existing
        ///     ones in the received <see cref="SoapEnvelope" />.
        /// </summary>
        /// <param name="envelope">The <see cref="SoapEnvelope" /> to append the headers</param>
        /// <param name="header">The <see cref="SoapHeader" /> to append</param>
        /// <returns>The <see cref="SoapEnvelope" /> after changes</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static SoapEnvelope WithHeader<THeader>(this SoapEnvelope envelope, THeader header)
            where THeader : SoapHeader
        {
            if (envelope == null)
            {
                throw new ArgumentNullException(nameof(envelope));
            }
            if (header == null)
            {
                throw new ArgumentNullException(nameof(header));
            }

            var xElementHeader = SoapClientSettings.Default.SerializationProvider.ToXElement(header);

            return envelope.WithHeaders(xElementHeader);
        }

        /// <summary>
        ///     Gets a collection of <see cref="XElement" /> headers by its <see cref="XName" />.
        /// </summary>
        /// <param name="envelope">The <see cref="SoapEnvelope" /> with the headers.</param>
        /// <param name="name">The <see cref="XName" /> to search.</param>
        /// <returns>The <see cref="XElement" /> or null if not match is found</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IEnumerable<XElement> HeadersWithName(this SoapEnvelope envelope, XName name)
        {
            if (envelope == null)
            {
                throw new ArgumentNullException(nameof(envelope));
            }

            return envelope.Headers == null
                ? Enumerable.Empty<XElement>()
                : envelope.Headers.Where(xElement => xElement.Name == name);
        }

        /// <summary>
        ///     Gets the first <see cref="XElement" /> header by its <see cref="XName" />.
        /// </summary>
        /// <param name="envelope">The <see cref="SoapEnvelope" /> with the headers.</param>
        /// <param name="name">The <see cref="XName" /> to search.</param>
        /// <returns>The <see cref="XElement" /> or null if not match is found</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static XElement HeaderWithName(this SoapEnvelope envelope, XName name)
        {
            if (envelope == null)
            {
                throw new ArgumentNullException(nameof(envelope));
            }

            if (envelope.Headers == null || envelope.Headers.Length == 0)
            {
                return null;
            }

            return envelope.Headers.FirstOrDefault(xElement => xElement.Name == name);
        }

        /// <summary>
        ///     Gets a given <see cref="SoapHeader" /> by its <see cref="XName" />.
        /// </summary>
        /// <param name="envelope">The <see cref="SoapEnvelope" /> with the headers.</param>
        /// <param name="name">The <see cref="XName" /> to search.</param>
        /// <returns>The <see cref="SoapHeader" /> or null if not match is found</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IEnumerable<T> HeadersWithName<T>(this SoapEnvelope envelope, XName name)
            where T : SoapHeader
        {
            return envelope.HeadersWithName(name)
                .Select(e => SoapClientSettings.Default.SerializationProvider.ToObject<T>(e));
        }

        /// <summary>
        ///     Gets a given <see cref="SoapHeader" /> by its <see cref="XName" />.
        /// </summary>
        /// <param name="envelope">The <see cref="SoapEnvelope" /> with the headers.</param>
        /// <param name="name">The <see cref="XName" /> to search.</param>
        /// <returns>The <see cref="SoapHeader" /> or null if not match is found</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static T HeaderWithName<T>(this SoapEnvelope envelope, XName name)
            where T : SoapHeader
        {
            return SoapClientSettings.Default.SerializationProvider.ToObject<T>(envelope.HeaderWithName(name));
        }

        #endregion

        #region Faulted

        /// <summary>
        ///     Does the <see cref="SoapEnvelope.Body" /> contains a fault?
        /// </summary>
        /// <param name="envelope">The <see cref="SoapEnvelope" /> to validate</param>
        /// <returns>True if a fault exists</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool IsFaulted(this SoapEnvelope envelope)
        {
            if (envelope == null)
            {
                throw new ArgumentNullException(nameof(envelope));
            }

            var bodyElement = envelope.Body.Descendants().FirstOrDefault();
            return bodyElement != null && bodyElement.Name == SoapFaultXName;
        }

        /// <summary>
        ///     Extracts the <see cref="SoapEnvelope.Body" /> as a <see cref="SoapFault" />.
        ///     It will fail to deserialize if the body is not a fault. Consider to
        ///     use <see cref="IsFaulted(SoapEnvelope)" /> first.
        /// </summary>
        /// <param name="envelope">The <see cref="SoapEnvelope" /> to be used</param>
        /// <returns>The <see cref="SoapFault" /></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static SoapFault Fault(this SoapEnvelope envelope)
        {
            if (envelope == null)
            {
                throw new ArgumentNullException(nameof(envelope));
            }

            var bodyElement = envelope.Body.Descendants().FirstOrDefault();
            return bodyElement == null
                ? null
                : SoapClientSettings.Default.SerializationProvider.ToObject<SoapFault>(bodyElement);
        }

        /// <summary>
        ///     Checks if the <see cref="SoapEnvelope.Body" /> contains a fault
        ///     and throws an <see cref="FaultException" /> if true.
        /// </summary>
        /// <param name="envelope">The <see cref="SoapEnvelope" /> to validate.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="FaultException">Thrown if the body contains a fault</exception>
        public static void ThrowIfFaulted(this SoapEnvelope envelope)
        {
            if (envelope == null)
            {
                throw new ArgumentNullException(nameof(envelope));
            }

            if (!envelope.IsFaulted())
            {
                return;
            }

            var fault = envelope.Fault();
            throw new FaultException
            {
                Code = fault.Code,
                Reason = fault.Reason,
                Node = fault.Node,
                Role = fault.Role,
                Detail = fault.Detail
            };
        }

        #endregion
    }
}
