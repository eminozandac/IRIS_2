using System;
using System.Threading.Tasks;
using CameraControl.Portable.Services.Onvif.Abstract;
using CameraControl.Portable.Services.Onvif.Constants;
using CameraControl.Portable.Services.Onvif.Models.Contracts.Media;

namespace CameraControl.Portable.Services.Onvif
{
    public class OnvifMediaService : BaseOnvifService, IOnvifMediaService
    {
        public OnvifMediaService(IOnvifSettingsProvider onvifSettingsProvider) : base(onvifSettingsProvider, Paths.MediaPath)
        {
        }

        public async Task<GetProfilesResponse> GetProfilesAsync()
        {
            var request = new GetProfiles();
            var response = await ExecuteAsync<GetProfiles, GetProfilesResponse>(request, MediaActions.GetProfiles);

            return response;
        }

        public async Task<GetStreamUriResponse> GetStreamUriAsync(StreamSetup streamSetup, string profileToken)
        {
            if (profileToken == null)
            {
                throw new ArgumentNullException(nameof(profileToken));
            }

            var request = new GetStreamUri
            {
                StreamSetup = streamSetup,
                ProfileToken = profileToken
            };
            var response = await ExecuteAsync<GetStreamUri, GetStreamUriResponse>(request, MediaActions.GetStreamUri);

            return response;
        }
    }
}
