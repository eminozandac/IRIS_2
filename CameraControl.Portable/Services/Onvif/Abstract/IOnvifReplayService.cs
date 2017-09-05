using System.Threading.Tasks;
using CameraControl.Portable.Services.Onvif.Models.Contracts.Replay;

namespace CameraControl.Portable.Services.Onvif.Abstract
{
    public interface IOnvifReplayService
    {
        Task<GetReplayUriResponse> GetStreamUriAsync(StreamSetup streamSetup, string recordingToken);
    }
}
