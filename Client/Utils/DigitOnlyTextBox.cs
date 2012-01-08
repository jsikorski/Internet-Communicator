using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;

namespace Client.Utils
{
    public class DigitOnlyTextBox : TextBox
    {
        public DigitOnlyTextBox()
        {
            PreviewTextInput += NumberTextBoxPreviewInput;
        }

        private void NumberTextBoxPreviewInput(object sender, TextCompositionEventArgs textCompositionEventArgs)
        {
            if (!char.IsDigit(textCompositionEventArgs.Text.ToCharArray().First()))
            {
                textCompositionEventArgs.Handled = true;
            }
        }
    }
}
