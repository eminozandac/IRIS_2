using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using OnvifWcf.Console.DeviceService;
using OnvifWcf.Console.MediaService;
using OnvifWcf.Console.PTZService;
using OnvifWcf.Console.ReplayService;
using OnvifWcf.Console.SearchService;
using DateTime = System.DateTime;

namespace OnvifWcf.Console
{
    internal class Program
    {
        private static readonly EndpointAddress EndpointAddress = new EndpointAddress("http://96.69.46.121:580/onvif/device_service");
        private static readonly PasswordDigestBehavior PasswordDigestBehavior = new PasswordDigestBehavior("admin", "admin");

        private static void Main(string[] args)
        {
            try
            {
                var deviceClient = CreateDeviceClient();
                var capabilities = deviceClient.GetCapabilities(new[] {CapabilityCategory.All});

                var mediaClient = CreateMediaClient();
                var profiles = mediaClient.GetProfiles();

                var profile = profiles.FirstOrDefault();
                if (profile != null)
                {
                    var searchPortClient = CreateSearchPortClient();
                    var searchToken = searchPortClient.FindRecordings(new SearchScope
                    {
                        IncludedSources = new[] { new SourceReference { Token = profiles[0].token } },
                        RecordingInformationFilter = $@"boolean(//Track[TrackType = “Video”]),{DateTime.Now:yyyy-MM-ddThh:mm:ssZ},99999999,20,99999999,20"
                    }, 10, "PT60S");

                    var searchResults = searchPortClient.GetRecordingSearchResults(searchToken, 0, 10, "PT60S");
                    var searchResult = searchResults.RecordingInformation?.FirstOrDefault();
                    if (searchResult != null)
                    {
                        var replayPortClient = CreateReplayPortClient();
                        var replayUri = replayPortClient.GetReplayUri(new ReplayService.StreamSetup
                        {
                            Stream = ReplayService.StreamType.RTPUnicast,
                            Transport = new ReplayService.Transport
                            {
                                Protocol = ReplayService.TransportProtocol.RTSP
                            }
                        }, searchResult.RecordingToken);

                        System.Console.WriteLine(replayUri);
                    }

                    //var ptzClient = CreatePTZClient();
                    //ptzClient.SetHomePosition(profile.token);

                    //var ptzSpeed = new PTZService.PTZSpeed
                    //{
                    //    PanTilt = new PTZService.Vector2D
                    //    {
                    //        x = -0.5f
                    //    }
                    //};

                    //ptzClient.ContinuousMove(profile.token, ptzSpeed, null);
                    //ptzClient.Stop(profile.token, true, false);
                }

                Debugger.Break();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);

                Debugger.Break();
            }
        }

        private static DeviceClient CreateDeviceClient()
        {
            var deviceClient = new DeviceClient(GetOnvifBinding(), EndpointAddress);
            deviceClient.Endpoint.Behaviors.Add(PasswordDigestBehavior);

            return deviceClient;
        }

        private static MediaClient CreateMediaClient()
        {
            var mediaClient = new MediaClient(GetOnvifBinding(), EndpointAddress);
            mediaClient.Endpoint.Behaviors.Add(PasswordDigestBehavior);

            return mediaClient;
        }

        private static PTZClient CreatePTZClient()
        {
            var ptzClient = new PTZClient(GetOnvifBinding(), EndpointAddress);
            ptzClient.Endpoint.Behaviors.Add(PasswordDigestBehavior);

            return ptzClient;
        }

        private static SearchPortClient CreateSearchPortClient()
        {
            var searchPortClient = new SearchPortClient(GetOnvifBinding(), EndpointAddress);
            searchPortClient.Endpoint.Behaviors.Add(PasswordDigestBehavior);

            return searchPortClient;
        }

        private static ReplayPortClient CreateReplayPortClient()
        {
            var replayPortClient = new ReplayPortClient(GetOnvifBinding(), EndpointAddress);
            replayPortClient.Endpoint.Behaviors.Add(PasswordDigestBehavior);

            return replayPortClient;
        }

        private static CustomBinding GetOnvifBinding()
        {
            var httpBinding = new HttpTransportBindingElement {AuthenticationScheme = AuthenticationSchemes.Digest};
            var messageElement = new TextMessageEncodingBindingElement {MessageVersion = MessageVersion.CreateVersion(EnvelopeVersion.Soap11, AddressingVersion.None)};

            return new CustomBinding(messageElement, httpBinding);
        }
    }
}
