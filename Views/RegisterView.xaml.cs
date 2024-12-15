using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CarRentApp.Views
{
    public partial class RegisterView : UserControl
    {
        public event RoutedEventHandler? SwitchToLogin;

        public RegisterView()
        {
            InitializeComponent();
        }

        private void SwitchToLoginView(object sender, MouseButtonEventArgs e)
        {
            SwitchToLogin?.Invoke(this, new RoutedEventArgs());
        }
    }
}
