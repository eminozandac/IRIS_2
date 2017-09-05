using System.Threading.Tasks;
using CameraControl.Portable.Services.Onvif.Models.Contracts.Imaging;

namespace CameraControl.Portable.Services.Onvif.Abstract
{
    public interface IOnvifImagingService
    {
        Task MoveAsync(Move move);

        Task StopAsync(Stop stop);

        Task SetImagingSettingsAsync(SetImagingSettings imagingSettings);
    }
}
