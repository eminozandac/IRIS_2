namespace CameraControl.Portable.Services.Onvif.Abstract
{
    public interface IOnvifServiceAggregator : IOnvifSettingsProvider
    {
        IOnvifMediaService OnvifMediaService { get; }

        IOnvifPTZService OnvifPTZService { get; }

        IOnvifImagingService OnvifImagingService { get; }

        IOnvifReplayService OnvifReplayService { get; }
    }
}
