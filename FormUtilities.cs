using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Nippy_Notes
{
    public static class FormUtilities
    {
        public static void EnsureFormIsVisible(Form form)
        {
            // Check if the form is disposed
            if (form == null || form.IsDisposed)
            {
                return;
            }

            Rectangle screenBounds = Screen.FromControl(form).WorkingArea;

            if (form.Left < screenBounds.Left)
            {
                form.Left = screenBounds.Left;
            }
            else if (form.Right > screenBounds.Right)
            {
                form.Left = screenBounds.Right - form.Width;
            }

            if (form.Top < screenBounds.Top)
            {
                form.Top = screenBounds.Top;
            }
            else if (form.Bottom > screenBounds.Bottom)
            {
                form.Top = screenBounds.Bottom - form.Height;
            }
        }

    }
}