using System;
using System.Drawing;
using ObjCRuntime;
using CoreGraphics;
using Foundation;
using UIKit;

namespace CameraControl.iOS.Views
{
    [Register("StatusProgressView")]
    public partial class StatusProgressView : UIView
    {
        public UIColor greenProgressColor = UIColor.FromRGB(102 / 255.0f, 153 / 255.0f, 0);
        public UIColor redProgressColor = UIColor.FromRGB(255 / 255.0f, 68 / 255.0f, 68 / 255.0f);
		public UIColor orangeProgressColor = UIColor.FromRGB(255 / 255.0f, 136 / 255.0f, 0);

        UIColor progressColor { get; set; }

        UIView holderView; 
        UIView valueView;

        private int progressValue;
        public int GetProgressValue()
		{
			return this.progressValue;
		}

        public void SetProgressValue(int theProgressValue)
		{
			this.progressValue = theProgressValue;
            valueView.Frame = new CGRect(0.0f, 0.0f, holderView.Frame.Size.Width * progressValue / 100.0f , holderView.Frame.Size.Height);
		}

        public StatusProgressView()
        {
            Initialize();
        }

        public StatusProgressView(RectangleF bounds) : base(bounds)
        {
            Initialize();
        }

		protected StatusProgressView(IntPtr handle) : base(handle)
        {
			// Note: this .ctor should not contain any initialization logic.
            Initialize();
		}


		void Initialize()
        {
            holderView = new UIView();
            holderView.Frame = new CGRect(3.0f, 3.0f, Frame.Width - 3.0f * 2, Frame.Height - 3.0f * 2);
            AddSubview(holderView);

            valueView = new UIView();
			valueView.Frame = new CGRect(0, 0, holderView.Frame.Width, holderView.Frame.Height);
            holderView.AddSubview(valueView);
            valueView.Layer.CornerRadius = .0f;

            BackgroundColor = UIColor.FromRGB(60 / 255.0f, 58 / 255.0f, 59 / 255.0f);
            Layer.CornerRadius = 5.0f;

            progressColor = greenProgressColor;

            valueView.BackgroundColor = greenProgressColor;

			SetProgressValue(0);
        }
    }
}