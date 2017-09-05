using System;
using System.Linq;
using CameraControl.Portable.Messages;
using CameraControl.SoapClient.Exceptions;

namespace CameraControl.Portable.Services.Onvif.Exceptions
{
    public static class ExceptionHelpers
    {
        public static ToastMessage ToToastMessage(this Exception exception, object sender)
        {
            return new ToastMessage(sender, exception.Message);
        }

        public static ToastMessage ToToastMessage(this FaultException exception, object sender)
        {
            return new ToastMessage(sender, string.Join(", ", exception.Reason.Texts.Select(t => t.Text)));
        }
    }
}
