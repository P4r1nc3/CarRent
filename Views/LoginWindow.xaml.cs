using System.Linq;
using System.Windows;
using System.Windows.Input;
using CarRentApp.Data;
using CarRentApp.Services;
using CarRentApp.Models;

namespace CarRentApp.Views
{
    public partial class LoginWindow : Window
    {
        private readonly UserService _userService;

        public LoginWindow()
        {
            InitializeComponent();
            _userService = new UserService(new AppDbContext());
        }

        private void RegisterHere_Click(object sender, MouseButtonEventArgs e)
        {
            var registerWindow = new RegisterWindow();
            registerWindow.Show();
            this.Close();
        }
    }
}
