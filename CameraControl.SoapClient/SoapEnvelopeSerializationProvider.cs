#region License

// The MIT License (MIT)
// 
// Copyright (c) 2016 Jo�o Sim�es
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
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using CameraControl.SoapClient.Exceptions;
using CameraControl.SoapClient.Models;

namespace CameraControl.SoapClient
{
    /// <summary>
    ///     Provider for serialization and deserialization of <see cref="SoapEnvelope" /> instances.
    /// </summary>
    public class SoapEnvelopeSerializationProvider : ISoapEnvelopeSerializationProvider
    {
        private XmlWriterSettings _xmlWriterSettings;
        private XmlSerializerNamespaces _xmlSerializerNamespaces;

        /// <summary>
        ///     XML writer settings to be used when serializing <see cref="SoapEnvelope" />
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public XmlWriterSettings XmlWriterSettings
        {
            get { return _xmlWriterSettings; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }
                _xmlWriterSettings = value;
            }
        }

        /// <summary>
        ///     XML serializer namespaces to be used when serializing <see cref="SoapEnvelope" />
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public XmlSerializerNamespaces XmlSerializerNamespaces
        {
            get { return _xmlSerializerNamespaces; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }
                _xmlSerializerNamespaces = value;
            }
        }

        /// <summary>
        ///     Creates a new instance
        /// </summary>
        public SoapEnvelopeSerializationProvider()
        {
            _xmlWriterSettings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                Indent = false,
                NamespaceHandling = NamespaceHandling.OmitDuplicates
            };

            _xmlSerializerNamespaces = new XmlSerializerNamespaces();
            _xmlSerializerNamespaces.Add("xs", Constants.OrgW3Www2001XmlSchema);
            _xmlSerializerNamespaces.Add("xml", Constants.OrgW3WwwXml1998Namespace);
            _xmlSerializerNamespaces.Add("env", Constants.OrgW3Www200305SoapEnvelope);
        }

        #region Implementation of ISoapEnvelopeSerializationProvider

        /// <summary>
        ///     Serializes a given <see cref="SoapEnvelope" /> instance into a XML string.
        /// </summary>
        /// <param name="envelope">The instance to serialize</param>
        /// <returns>The resulting XML string</returns>
        public string ToXmlString(SoapEnvelope envelope)
        {
            try
            {
                return ToXmlString<SoapEnvelope>(envelope);
            }
            catch (Exception e)
            {
                throw new SoapEnvelopeSerializationException(envelope, e);
            }
        }

        /// <summary>
        ///     Deserializes a given XML string into a <see cref="SoapEnvelope" />.
        /// </summary>
        /// <param name="xml">The XML string do deserialize</param>
        /// <returns>The resulting <see cref="SoapEnvelope" /></returns>
        public SoapEnvelope ToSoapEnvelope(string xml)
        {
            try
            {
                return ToObject<SoapEnvelope>(xml);
            }
            catch (Exception e)
            {
                throw new SoapEnvelopeDeserializationException(xml, e);
            }
        }

        /// <summary>
        ///     Converts the given item into a <see cref="XElement" />.
        /// </summary>
        /// <typeparam name="T">The item type</typeparam>
        /// <param name="item">The item to be converted</param>
        /// <returns>The resulting XElement</returns>
        public XElement ToXElement<T>(T item)
        {
            return item == null
                ? null
                : XElement.Parse(ToXmlString(item));
        }

        /// <summary>
        ///     Converts the given <see cref="XElement" /> into an item of expected type.
        /// </summary>
        /// <typeparam name="T">The item type</typeparam>
        /// <param name="element">The element to be converted</param>
        /// <returns>The resulting item</returns>
        public T ToObject<T>(XElement element)
        {
            return element == null
                ? default(T)
                : ToObject<T>(element.ToString());
        }

        #endregion

        public string ToXmlString<T>(T instance)
        {
            if (instance == null)
            {
                return null;
            }

            using (var textWriter = new StringWriter())
            using (var xmlWriter = XmlWriter.Create(textWriter, XmlWriterSettings))
            {
                new XmlSerializer(typeof(T)).Serialize(xmlWriter, instance, XmlSerializerNamespaces);

                return textWriter.ToString();
            }
        }

        private static T ToObject<T>(string xml)
        {
            if (string.IsNullOrWhiteSpace(xml))
            {
                return default(T);
            }

            using (var textWriter = new StringReader(xml))
            {
                var result = (T) new XmlSerializer(typeof(T)).Deserialize(textWriter);

                return result;
            }
        }
    }
}
