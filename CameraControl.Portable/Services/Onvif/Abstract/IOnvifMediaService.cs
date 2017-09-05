using System.Threading.Tasks;
using CameraControl.Portable.Services.Onvif.Models.Contracts.Media;

namespace CameraControl.Portable.Services.Onvif.Abstract
{
    public interface IOnvifMediaService
    {
        Task<GetProfilesResponse> GetProfilesAsync();

        Task<GetStreamUriResponse> GetStreamUriAsync(StreamSetup streamSetup, string profileToken);
    }
}
