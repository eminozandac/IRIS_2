﻿#region License

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
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CameraControl.SoapClient.Exceptions;
using CameraControl.SoapClient.Handlers;
using CameraControl.SoapClient.Models;

namespace CameraControl.SoapClient
{
    /// <summary>
    ///     The SOAP client that can be used to invoke SOAP Endpoints
    /// </summary>
    public class SoapClient : ISoapClient, IDisposable
    {
        private readonly bool _disposeHttpClient = true;
        private readonly List<ISoapHandler> _handlers = new List<ISoapHandler>();
        private SoapClientSettings _settings;

        /// <summary>
        ///     The used HTTP client
        /// </summary>
        public HttpClient HttpClient { get; }

        #region Constructors

        /// <summary>
        ///     Creates a new SOAP Client
        /// </summary>
        /// <param name="settings">The settings to be used</param>
        /// <exception cref="ArgumentNullException"></exception>
        public SoapClient(SoapClientSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            _settings = settings;
            HttpClient = _settings.HttpClientFactory.Get();
        }

        /// <summary>
        ///     Creates a new SOAP Client
        /// </summary>
        /// <param name="settings">The settings to be used</param>
        /// <param name="handler">The handler to be used by the <see cref="HttpClient" /></param>
        /// <exception cref="ArgumentNullException"></exception>
        public SoapClient(SoapClientSettings settings, HttpMessageHandler handler)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            _settings = settings;
            HttpClient = _settings.HttpClientFactory.Get(handler);
        }

        /// <summary>
        ///     Creates a new SOAP Client
        /// </summary>
        /// <param name="settings">The settings to be used</param>
        /// <param name="httpClient">The <see cref="HttpClient" /> to be used</param>
        /// <param name="disposeHttpClient">Should the client also be disposed</param>
        /// <exception cref="ArgumentNullException"></exception>
        public SoapClient(SoapClientSettings settings, HttpClient httpClient, bool disposeHttpClient = true)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }
            if (httpClient == null)
            {
                throw new ArgumentNullException(nameof(httpClient));
            }

            _settings = settings;
            HttpClient = httpClient;
            _disposeHttpClient = disposeHttpClient;
        }

        /// <summary>
        ///     Creates a new SOAP Client
        /// </summary>
        public SoapClient()
            : this(SoapClientSettings.Default)
        {
        }

        /// <summary>
        ///     Creates a new SOAP Client
        /// </summary>
        /// <param name="handler">The handler to be used by the <see cref="HttpClient" /></param>
        public SoapClient(HttpMessageHandler handler)
            : this(SoapClientSettings.Default, handler)
        {
        }

        /// <summary>
        ///     Creates a new SOAP Client
        /// </summary>
        /// <param name="httpClient">The <see cref="HttpClient" /> to be used</param>
        /// <param name="disposeHttpClient">Should the client also be disposed</param>
        public SoapClient(HttpClient httpClient, bool disposeHttpClient = true)
            : this(SoapClientSettings.Default, httpClient, disposeHttpClient)
        {
        }

        /// <summary>
        ///     Allows an object to try to free resources and perform other cleanup operations before it is reclaimed by
        ///     garbage collection.
        /// </summary>
        ~SoapClient()
        {
            Dispose(false);
        }

        #endregion

        #region Implementation of ISoapClient

        /// <summary>
        ///     The handler
        /// </summary>
        public IReadOnlyCollection<ISoapHandler> Handlers => _handlers;

        /// <summary>
        ///     The client settings
        /// </summary>
        public SoapClientSettings Settings
        {
            get { return _settings; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }
                _settings = value;
            }
        }

        #region Send

        /// <summary>
        ///     Sends the given <see cref="SoapEnvelope" /> into the specified url.
        /// </summary>
        /// <param name="url">The url that will receive the request</param>
        /// <param name="action">The SOAP action beeing performed</param>
        /// <param name="requestEnvelope">The <see cref="SoapEnvelope" /> to be sent</param>
        /// <param name="ct">The cancellation token</param>
        /// <returns>A task to be awaited for the result</returns>
        /// <exception cref="SoapEnvelopeSerializationException"></exception>
        /// <exception cref="SoapEnvelopeDeserializationException"></exception>
        public virtual async Task<SoapEnvelope> SendAsync(string url, string action, SoapEnvelope requestEnvelope, CancellationToken ct = default(CancellationToken))
        {
            var trackingId = Guid.NewGuid();
            var orderedHandlers = _handlers.OrderBy(e => e.Order).ToArray();

            var beforeSoapEnvelopeSerializationHandlersResult = await RunBeforeSoapEnvelopeSerializationHandlers(requestEnvelope, url, action, trackingId, orderedHandlers, ct);

            string requestXml;
            try
            {
                requestXml = Settings.SerializationProvider.ToXmlString(beforeSoapEnvelopeSerializationHandlersResult.Envelope);
            }
            catch (SoapEnvelopeSerializationException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new SoapEnvelopeSerializationException(requestEnvelope, e);
            }

            var beforeHttpRequestHandlersResult = await RunBeforeHttpRequestHandlers(requestXml, url, action, trackingId, beforeSoapEnvelopeSerializationHandlersResult.State, orderedHandlers, ct);

            var response = await HttpClient.SendAsync(beforeHttpRequestHandlersResult.Request, ct).ConfigureAwait(false);

            var afterHttpResponseHandlersResult = await RunAfterHttpResponseHandlers(response, url, action, trackingId, beforeHttpRequestHandlersResult.State, orderedHandlers, ct);

            var responseXml = await afterHttpResponseHandlersResult.Response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (string.IsNullOrWhiteSpace(responseXml))
            {
                throw new SoapEnvelopeDeserializationException(responseXml, "The response content is empty.");
            }

            SoapEnvelope responseEnvelope;
            try
            {
                responseEnvelope = Settings.SerializationProvider.ToSoapEnvelope(responseXml);
            }
            catch (SoapEnvelopeDeserializationException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new SoapEnvelopeDeserializationException(responseXml, e);
            }

            var afterSoapEnvelopeDeserializationHandlerResult = await RunAfterSoapEnvelopeDeserializationHandler(responseEnvelope, url, action, trackingId, afterHttpResponseHandlersResult.State, orderedHandlers, ct);

            return afterSoapEnvelopeDeserializationHandlerResult.Envelope;
        }

        /// <summary>
        ///     Sends the given <see cref="SoapEnvelope" /> into the specified url.
        /// </summary>
        /// <param name="url">The url that will receive the request</param>
        /// <param name="action">The SOAP action beeing performed</param>
        /// <param name="requestEnvelope">The <see cref="SoapEnvelope" /> to be sent</param>
        /// <returns>The resulting <see cref="SoapEnvelope" /></returns>
        /// <exception cref="SoapEnvelopeSerializationException"></exception>
        /// <exception cref="SoapEnvelopeDeserializationException"></exception>
        public virtual SoapEnvelope Send(string url, string action, SoapEnvelope requestEnvelope)
        {
            return SendAsync(url, action, requestEnvelope).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        #endregion

        /// <summary>
        ///     Adds the <see cref="ISoapHandler" /> to the <see cref="Handlers" /> collection.
        /// </summary>
        /// <param name="handler">The handler to add</param>
        /// <exception cref="ArgumentNullException"></exception>
        public void AddHandler(ISoapHandler handler)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            _handlers.Add(handler);
        }

        #endregion

        #region Implementation of IDisposable

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Disposes the underline <see cref="HttpClient" />
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing && _disposeHttpClient)
            {
                HttpClient.Dispose();
            }
        }

        #endregion

        #region Prepare

        /// <summary>
        ///     Prepares a new <see cref="SoapClient" /> instance to be configured.
        /// </summary>
        /// <returns>The SOAP client to be configured</returns>
        public static SoapClient Prepare()
        {
            return new SoapClient();
        }

        /// <summary>
        ///     Prepares a new <see cref="SoapClient" /> instance to be configured.
        /// </summary>
        /// <param name="handler">The handler to be used by the <see cref="HttpClient" /></param>
        /// <returns>The SOAP client to be configured</returns>
        public static SoapClient Prepare(HttpMessageHandler handler)
        {
            return new SoapClient(handler);
        }

        /// <summary>
        ///     Prepares a new <see cref="SoapClient" /> instance to be configured.
        /// </summary>
        /// <param name="httpClient">The <see cref="HttpClient" /> to be used</param>
        /// <param name="disposeHttpClient">Should the client also be disposed</param>
        /// <returns>The SOAP client to be configured</returns>
        public static SoapClient Prepare(HttpClient httpClient, bool disposeHttpClient = true)
        {
            return new SoapClient(httpClient, disposeHttpClient);
        }

        #endregion

        #region Private

        private async Task<OnSoapEnvelopeRequestArguments> RunBeforeSoapEnvelopeSerializationHandlers(
            SoapEnvelope envelope, string url, string action, Guid trackingId, ISoapHandler[] orderedHandlers, CancellationToken ct)
        {
            var beforeSoapEnvelopeSerializationArg = new OnSoapEnvelopeRequestArguments(envelope, url, action, trackingId);
            foreach (var handler in orderedHandlers)
            {
                await handler.OnSoapEnvelopeRequestAsync(this, beforeSoapEnvelopeSerializationArg, ct);
                handler.OnSoapEnvelopeRequest(this, beforeSoapEnvelopeSerializationArg);
            }

            return beforeSoapEnvelopeSerializationArg;
        }

        private async Task<OnHttpRequestArguments> RunBeforeHttpRequestHandlers(string xml, string url, string action, Guid trackingId, object state, IEnumerable<ISoapHandler> orderedHandlers, CancellationToken ct)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);

            var beforeHttpRequestArguments = new OnHttpRequestArguments(requestMessage, url, action, trackingId)
            {
                State = state,
                Request = {Content = new StringContent(xml, Encoding.UTF8)}
            };
            beforeHttpRequestArguments.Request.Content.Headers.Clear();

            var contentTypeValue = $"application/soap+xml;charset=UTF-8;action=\"{action}\"";
            if (!beforeHttpRequestArguments.Request.Content.Headers.TryAddWithoutValidation("Content-Type", contentTypeValue))
            {
                throw new SoapClientException($"Could not assign '{contentTypeValue}' as the 'Content-Type' header value");
            }

            foreach (var handler in orderedHandlers)
            {
                await handler.OnHttpRequestAsync(this, beforeHttpRequestArguments, ct);
                handler.OnHttpRequest(this, beforeHttpRequestArguments);
            }

            return beforeHttpRequestArguments;
        }

        private async Task<OnHttpResponseArguments> RunAfterHttpResponseHandlers(HttpResponseMessage response, string url, string action, Guid trackingId, object state, IReadOnlyList<ISoapHandler> orderedHandlers,
            CancellationToken ct)
        {
            var afterHttpResponseArguments = new OnHttpResponseArguments(response, url, action, trackingId)
            {
                State = state
            };

            for (var index = orderedHandlers.Count - 1; index >= 0; index--)
            {
                var handler = orderedHandlers[index];
                await handler.OnHttpResponseAsync(this, afterHttpResponseArguments, ct);
                handler.OnHttpResponse(this, afterHttpResponseArguments);
            }

            return afterHttpResponseArguments;
        }

        private async Task<OnSoapEnvelopeResponseArguments> RunAfterSoapEnvelopeDeserializationHandler(SoapEnvelope envelope, string url, string action, Guid trackingId, object state,
            IReadOnlyList<ISoapHandler> orderedHandlers, CancellationToken ct)
        {
            var afterSoapEnvelopeDeserializationArguments = new OnSoapEnvelopeResponseArguments(envelope, url, action, trackingId)
            {
                State = state
            };

            for (var index = orderedHandlers.Count - 1; index >= 0; index--)
            {
                var handler = orderedHandlers[index];
                await handler.OnSoapEnvelopeResponseAsync(this, afterSoapEnvelopeDeserializationArguments, ct);
                handler.OnSoapEnvelopeResponse(this, afterSoapEnvelopeDeserializationArguments);
            }

            return afterSoapEnvelopeDeserializationArguments;
        }

        #endregion
    }
}
