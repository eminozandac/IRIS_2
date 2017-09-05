using System;
using System.Threading.Tasks;
using CameraControl.Portable.Services.Onvif.Abstract;
using CameraControl.Portable.Services.Onvif.Constants;
using CameraControl.Portable.Services.Onvif.Models.Contracts.Imaging;

namespace CameraControl.Portable.Services.Onvif
{
    public class OnvifImagingService : BaseOnvifService, IOnvifImagingService
    {
        public OnvifImagingService(IOnvifSettingsProvider onvifSettingsProvider) : base(onvifSettingsProvider, Paths.ImagingPath)
        {
        }

        public async Task MoveAsync(Move move)
        {
            if (move == null)
            {
                throw new ArgumentNullException(nameof(move));
            }

            await ExecuteAsync<Move, MoveResponse>(move, ImagingActions.Move);
        }

        public async Task StopAsync(Stop stop)
        {
            if (stop == null)
            {
                throw new ArgumentNullException(nameof(stop));
            }

            await ExecuteAsync<Stop, StopResponse>(stop, ImagingActions.Stop);
        }

        public async Task SetImagingSettingsAsync(SetImagingSettings imagingSettings)
        {
            if (imagingSettings == null)
            {
                throw new ArgumentNullException(nameof(imagingSettings));
            }

            await ExecuteAsync<SetImagingSettings, SetImagingSettingsResponse>(imagingSettings, ImagingActions.SetImagingSettings);
        }
    }
}
