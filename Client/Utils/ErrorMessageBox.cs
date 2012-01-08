using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Client.Utils
{
    public static class ErrorMessageBox
    {
        public static void Show(Exception exception)
        {
            MessageBox.Show(exception.Message, "Error");
        }
    }
}
