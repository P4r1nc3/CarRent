using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using CarRentApp.Src.Contexts;
using CarRentApp.Src.Models;
using CarRentApp.Src.Repositories;
using Microsoft.VisualBasic.Logging;

namespace CarRentApp.Views.Users.Customer
{
    public partial class CustomerView : UserControl
    {
        public event RoutedEventHandler? Logout;

        private readonly AuthContext _authContext;
        private readonly RequestRepository _requestRepository;
        
        public CustomerView(DatabaseContext dbContext)
        {
            InitializeComponent();
            
            _authContext = AuthContext.GetInstance();
            _authContext.CurrentUserChanged += LoadUserInfo;
            _authContext.CurrentUserChanged += LoadRequestListByUserId;
            
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
            var currentUser = _authContext.GetCurrentUser();
            UserInfoTextBlock.Text = currentUser != null
                ? $"Logged in as: {currentUser.Name} {currentUser.Surname}"
                : "No user is currently logged in.";
        }
        
        private void LoadRequestListByUserId()
        {
            var currentUser = _authContext.GetCurrentUser();
            RequestsDataGrid.ItemsSource = currentUser != null ? _requestRepository.GetRequestsByUserIdDescending(currentUser.Id) : [];
        }
    }
}
