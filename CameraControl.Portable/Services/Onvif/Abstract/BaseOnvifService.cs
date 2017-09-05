using System;
using System.Threading.Tasks;
using CameraControl.Portable.Services.Onvif.Exceptions;
using CameraControl.SoapClient.Exceptions;
using CameraControl.SoapClient.Handlers;
using CameraControl.SoapClient.Helpers;
using CameraControl.SoapClient.Models;
using CameraControl.SoapClient.Models.Headers;

namespace CameraControl.Portable.Services.Onvif.Abstract
{
    public abstract class BaseOnvifService
    {
        private readonly IOnvifSettingsProvider _onvifSettingsProvider;

        protected BaseOnvifService(IOnvifSettingsProvider onvifSettingsProvider, string servicePath)
        {
            if (onvifSettingsProvider == null)
            {
                throw new ArgumentNullException(nameof(onvifSettingsProvider));
            }
            if (servicePath == null)
            {
                throw new ArgumentNullException(nameof(servicePath));
            }

            _onvifSettingsProvider = onvifSettingsProvider;
            ServicePath = servicePath;
        }

        public string ServicePath { get; }

        protected async Task<TResponse> ExecuteAsync<TRequestBody, TResponse>(TRequestBody body, string action)
            where TResponse : new()
            where TRequestBody : new()
        {
            if (body == null)
            {
                throw new ArgumentNullException(nameof(body));
            }
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }
            if (_onvifSettingsProvider.BaseUri == null)
            {
                throw new OnvifServiceException("Onvif service Uri is not set.");
            }

            var soapHandler = new DelegatingSoapHandler
            {
                Order = int.MaxValue,
                OnSoapEnvelopeRequestAction = (c, args) =>
                {
                    if (_onvifSettingsProvider.Credentials != null)
                    {
                        args.Envelope
                            .WithHeader(KnownHeader.Oasis.Security.UsernameTokenWithPasswordDigest(_onvifSettingsProvider.Credentials.Username, _onvifSettingsProvider.Credentials.Password, DateTime.Now));
                    }
                }
            };

            using (var client = SoapClient.SoapClient.Prepare().WithHandler(soapHandler))
            {
                var requestEnvelope = SoapEnvelope
                    .Prepare()
                    .Body(body);

                try
                {
                    var responseEnvelope = await client.SendAsync($"http://{_onvifSettingsProvider.BaseUri}/{ServicePath}", action, requestEnvelope);
                    return responseEnvelope.Body<TResponse>();
                }
                catch (SoapEnvelopeSerializationException e)
                {
                    throw new OnvifServiceException("Unable to serialize request SOAP envelope. See inner exception for details.", e);
                }
                catch (SoapEnvelopeDeserializationException e)
                {
                    throw new OnvifServiceException("Unable to deserialize response to SOAP envelope. See inner exception for details.", e);
                }
                catch (FaultException)
                {
                    throw;
                }
                catch (Exception e)
                {
                    throw new OnvifServiceException("Onvif connection failed. See inner exception for details.", e);
                }
            }
        }
    }
}
