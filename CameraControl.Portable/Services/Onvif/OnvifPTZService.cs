using System;
using System.Threading.Tasks;
using CameraControl.Portable.Services.Onvif.Abstract;
using CameraControl.Portable.Services.Onvif.Constants;
using CameraControl.Portable.Services.Onvif.Models.Contracts.PTZ;

namespace CameraControl.Portable.Services.Onvif
{
    public class OnvifPTZService : BaseOnvifService, IOnvifPTZService
    {
        public OnvifPTZService(IOnvifSettingsProvider onvifSettingsProvider) : base(onvifSettingsProvider, Paths.PTZPath)
        {
        }

        public async Task ContinuousMoveAsync(ContinuousMove continuousMove)
        {
            if (continuousMove == null)
            {
                throw new ArgumentNullException(nameof(continuousMove));
            }

            await ExecuteAsync<ContinuousMove, ContinuousMoveResponse>(continuousMove, PTZActions.ContinuousMove);
        }

        public async Task StopAsync(Stop stop)
        {
            if (stop == null)
            {
                throw new ArgumentNullException(nameof(stop));
            }

            await ExecuteAsync<Stop, StopResponse>(stop, PTZActions.Stop);
        }

        public async Task SetHomePositionAsync(SetHomePosition setHomePosition)
        {
            if (setHomePosition == null)
            {
                throw new ArgumentNullException(nameof(setHomePosition));
            }

            await ExecuteAsync<SetHomePosition, SetHomePositionResponse>(setHomePosition, PTZActions.SetHomePosition);
        }

        public async Task GoToHomePositionAsync(GotoHomePosition gotoHomePosition)
        {
            if (gotoHomePosition == null)
            {
                throw new ArgumentNullException(nameof(gotoHomePosition));
            }

            await ExecuteAsync<GotoHomePosition, GotoHomePositionResponse>(gotoHomePosition, PTZActions.GoToHomePosition);
        }
    }
}
