using System;
using System.Globalization;
using System.Windows.Input;
using CameraControl.Portable.Models;
using MvvmCross.Binding.ValueConverters;
using MvvmCross.Platform.Converters;

namespace CameraControl.Portable.Converters
{
    public class ContinuousMoveCommandValueConverter : MvxValueConverter<ICommand, ICommand>
    {
        protected override ICommand Convert(ICommand value, Type targetType, object parameter, CultureInfo culture)
        {
            return new MvxWrappingCommand(value, Enum.Parse(typeof(ContinuousMoveDirection), parameter.ToString()));
        }
    }
}
