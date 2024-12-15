using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CarRentApp.Views
{
    public partial class LoginView : UserControl
    {
        public event RoutedEventHandler? SwitchToRegister;

        public LoginView()
        {
            InitializeComponent();
        }

        private void SwitchToRegisterView(object sender, MouseButtonEventArgs e)
        {
            SwitchToRegister?.Invoke(this, new RoutedEventArgs());
        }
    }
}
