using System.Linq;
using System.Windows;
using CarRentApp.Data;
using CarRentApp.Services;

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
    }
}
