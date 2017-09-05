using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CameraControl.Portable.Services.Onvif.Abstract;
using CameraControl.Portable.Services.Onvif.Constants;
using CameraControl.Portable.Services.Onvif.Models.Contracts.Replay;

namespace CameraControl.Portable.Services.Onvif
{
    public class OnvifReplayService : BaseOnvifService, IOnvifReplayService
    {
        public OnvifReplayService(IOnvifSettingsProvider onvifSettingsProvider) : base(onvifSettingsProvider, Paths.ReplayPath)
        {
        }

        public async Task<GetReplayUriResponse> GetStreamUriAsync(StreamSetup streamSetup, string recordingToken)
        {
            if (recordingToken == null)
            {
                throw new ArgumentNullException(nameof(recordingToken));
            }

            var request = new GetReplayUri
            {
                StreamSetup = streamSetup,
                RecordingToken = recordingToken
            };
            var response = await ExecuteAsync<GetReplayUri, GetReplayUriResponse>(request, ReplayActions.GetReplayUri);

            return response;
        }
    }
}
