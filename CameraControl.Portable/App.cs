using CameraControl.Portable.Services.Onvif;
using CameraControl.Portable.Services.Onvif.Abstract;
using CameraControl.Portable.ViewModels;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;

namespace CameraControl.Portable
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            Mvx.RegisterSingleton<IOnvifServiceAggregator>(() => new OnvifServiceAggregator());

            RegisterAppStart<MainViewModel>();
        }
    }
}
