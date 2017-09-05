using System;
using MvvmCross.Platform.Converters;

namespace CameraControl.Portable.Converters
{
    public class SliderPercentValueConverter : MvxValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
            var result = ((int)value / (float)100);
            return result;
        }
    }
}
