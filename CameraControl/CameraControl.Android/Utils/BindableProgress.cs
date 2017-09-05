using System;
using Android.App;
using Android.Content;

namespace CameraControl.Droid.Utils
{
    public class BindableProgress
    {
        private readonly Context _context;
        private ProgressDialog _progressDialog;

        public BindableProgress(Context context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            _context = context;
        }

        public bool Visible
        {
            get { return _progressDialog != null; }
            set
            {
                if (value == (_progressDialog != null))
                {
                    return;
                }

                if (value)
                {
                    _progressDialog = ProgressDialog.Show(_context, "Loading...", null);
                    _progressDialog.Show();
                }
                else
                {
                    _progressDialog.Hide();
                    _progressDialog = null;
                }
            }
        }
    }
}
