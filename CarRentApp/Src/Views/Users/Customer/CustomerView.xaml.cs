using System.Windows;
using System.Windows.Controls;
using CarRentApp.Src.Contexts;
using CarRentApp.Contexts;
using CarRentApp.Models;
using CarRentApp.Repositories;

namespace CarRentApp.Views.Users.Customer
{
    public partial class CustomerView : UserControl
    {
        public event RoutedEventHandler? Logout;

        private readonly AuthContext _authContext;
        private readonly RequestRepository _requestRepository;
        private User? _currentUser;
        
        public CustomerView(DatabaseContext dbContext)
        {
            InitializeComponent();
            _authContext = AuthContext.GetInstance();
            _authContext.CurrentUserChanged += LoadUserInfo;
            _requestRepository = new RequestRepository(dbContext);
            
            LoadUserInfo();
            LoadRequestListByUserId();
        }
        
        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            _authContext.Logout();
            Logout?.Invoke(this, new RoutedEventArgs());
        }

        private void LoadUserInfo()
        {
            _currentUser = _authContext.GetCurrentUser();
            UserInfoTextBlock.Text = _currentUser != null
                ? $"Logged in as: {_currentUser.Name} {_currentUser.Surname}"
                : "No user is currently logged in.";
        }
        
        private void LoadRequestListByUserId()
        {
            RequestsDataGrid.ItemsSource = _requestRepository.GetRequestsByUserId(_currentUser.Id);
        }
    }
}
