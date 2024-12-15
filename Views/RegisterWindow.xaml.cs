using System.Linq;
using System.Windows;
using CarRentApp.Data;
using CarRentApp.Services;
using CarRentApp.Models;

namespace CarRentApp.Views
{
    public partial class RegisterWindow : Window
    {
        private readonly UserService _userService;

        public RegisterWindow()
        {
            InitializeComponent();
            _userService = new UserService(new AppDbContext());
        }
    }
}