using CameraControl.Portable.Services.Onvif.Models;

namespace CameraControl.Portable.Services.Onvif.Abstract
{
    public interface IOnvifSettingsProvider
    {
        OnvifCredentials Credentials { get; set; }

        string BaseUri { get; set; }
    }
}
