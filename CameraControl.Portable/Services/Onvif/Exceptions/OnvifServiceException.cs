using System;

namespace CameraControl.Portable.Services.Onvif.Exceptions
{
    /// <summary>
    ///     Represents specialized exception thrown by the Onvif Service
    /// </summary>
    public class OnvifServiceException : Exception
    {
        /// <summary>
        ///     Initializes a new instance of <see cref="OnvifServiceException" />
        /// </summary>
        /// <param name="message">The message to be used</param>
        public OnvifServiceException(string message) : base(message)
        {
        }

        /// <summary>
        ///     Initializes a new instance of <see cref="OnvifServiceException" />
        /// </summary>
        /// <param name="message">The message to be used</param>
        /// <param name="innerException">The inner exception</param>
        public OnvifServiceException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
