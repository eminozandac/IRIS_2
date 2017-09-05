using System.Threading.Tasks;
using CameraControl.Portable.Services.Onvif.Models.Contracts.PTZ;

namespace CameraControl.Portable.Services.Onvif.Abstract
{
    public interface IOnvifPTZService
    {
        Task ContinuousMoveAsync(ContinuousMove continuousMove);

        Task StopAsync(Stop stop);

        Task SetHomePositionAsync(SetHomePosition setHomePosition);

        Task GoToHomePositionAsync(GotoHomePosition gotoHomePosition);
    }
}
