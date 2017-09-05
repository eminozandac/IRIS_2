using System;

namespace CameraControl.Portable.Services.Onvif.Models
{
    public class OnvifCredentials
    {
        public OnvifCredentials(string username, string password)
        {
            if (username == null)
            {
                throw new ArgumentNullException(nameof(username));
            }
            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            Username = username;
            Password = password;
        }

        public string Username { get; }

        public string Password { get; }
    }
}
