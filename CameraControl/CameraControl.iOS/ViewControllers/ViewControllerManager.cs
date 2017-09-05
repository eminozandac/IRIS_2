using System;

using Foundation;
using System.Collections.Generic;
using System.Linq;
using CameraControl.iOS.ViewControllers;

namespace CameraControl.iOS
{
    public partial class ViewControllerManager : NSObject
    {
        public List<BasicViewController> viewControllers;

        public LiveViewController rootViewController;
        public ViewControllerManager()
        {
            // Note: this .ctor should not contain any initialization logic.
            viewControllers = new List<BasicViewController>();
        }
    }
}
