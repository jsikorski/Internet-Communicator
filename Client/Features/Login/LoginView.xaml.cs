using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Client.Features.Login
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();
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
