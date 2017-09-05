using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Android.Content;
using Android.Views;
using Android.Widget;
using CameraControl.Droid.Bindings;
using CameraControl.Droid.Bootstrap;
using CameraControl.Droid.Utils;
using CameraControl.Portable;
using CameraControl.Portable.Converters;
using CameraControl.Portable.RestAPI;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Platform;
using MvvmCross.Droid.Views;
using MvvmCross.Platform;
using MvvmCross.Platform.Plugins;
using MvvmCross.Plugins.Visibility;
using Veg.Mediaplayer.Sdk;

namespace CameraControl.Droid
{
    public class Setup : MvxAndroidSetup
    {
        public Setup(Context applicationContext) : base(applicationContext)
        {
        }

        protected override IEnumerable<Assembly> ValueConverterAssemblies
        {
            get
            {
                var toReturn = base.ValueConverterAssemblies.ToList();
                toReturn.Add(typeof(MvxVisibilityValueConverter).Assembly);
                toReturn.Add(typeof(ContinuousMoveCommandValueConverter).Assembly);
                toReturn.Add(typeof(FocusMoveCommandValueConverter).Assembly);
                return toReturn;
            }
        }

        protected override IMvxApplication CreateApp()
        {
            return new App();
        }

        public override void LoadPlugins(IMvxPluginManager pluginManager)
        {
            pluginManager.EnsurePluginLoaded<VisibilityPluginBootstrap>();
            pluginManager.EnsurePluginLoaded<MessengerPluginBootstrap>();
            pluginManager.EnsurePluginLoaded<JsonPluginBootstrap>();
            pluginManager.EnsurePluginLoaded<SettingsPluginBootstrap>();
            base.LoadPlugins(pluginManager);
        }

        protected override void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            base.FillTargetFactories(registry);

            registry.RegisterFactory(
                new MvxCustomBindingFactory<MediaPlayer>(
                    "MediaPlayerViewModel",
                    view => new MediaPlayerViewModelBinding(view)));

            registry.RegisterFactory(
                new MvxCustomBindingFactory<View>(
                    "Down",
                    button => new ViewTouchDownCommandBinding(button)));

            registry.RegisterFactory(
                new MvxCustomBindingFactory<View>(
                    "Up",
                    button => new ViewTouchUpCommandBinding(button)));

            registry.RegisterFactory(
                new MvxCustomBindingFactory<SeekBar>(
                    "StartTrackingCommand",
                    seekBar => new SeekBarStartTrackingCommandBinding(seekBar)));

            registry.RegisterFactory(
                new MvxCustomBindingFactory<SeekBar>(
                    "StopTrackingCommand",
                    seekBar => new SeekBarStopTrackingCommandBinding(seekBar)));
        }

        protected override IMvxAndroidViewPresenter CreateViewPresenter()
        {
            var customPresenter = new CustomAndroidViewPresenter();
            Mvx.RegisterSingleton<IMvxAndroidViewPresenter>(customPresenter);
            return customPresenter;
        }

		protected override void InitializeFirstChance()
		{
			Mvx.RegisterSingleton<IIrisRestAPI>(new IrisRestAPIAndroid());
			base.InitializeFirstChance();
		}
    }
}
