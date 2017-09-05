using CameraControl.Portable.Services.Onvif.Abstract;
using CameraControl.Portable.Services.Onvif.Models;

namespace CameraControl.Portable.Services.Onvif
{
    public class OnvifServiceAggregator : IOnvifServiceAggregator
    {
        private IOnvifMediaService _onvifMediaService;
        private IOnvifPTZService _onvifPTZService;
        private IOnvifImagingService _onvifImagingService;
        private IOnvifReplayService _onvifReplayService;

        public OnvifCredentials Credentials { get; set; }

        public string BaseUri { get; set; }

        public IOnvifMediaService OnvifMediaService => _onvifMediaService ?? (_onvifMediaService = new OnvifMediaService(this));

        public IOnvifPTZService OnvifPTZService => _onvifPTZService ?? (_onvifPTZService = new OnvifPTZService(this));

        public IOnvifImagingService OnvifImagingService => _onvifImagingService ?? (_onvifImagingService = new OnvifImagingService(this));

        public IOnvifReplayService OnvifReplayService => _onvifReplayService ?? (_onvifReplayService = new OnvifReplayService(this));
    }
}
