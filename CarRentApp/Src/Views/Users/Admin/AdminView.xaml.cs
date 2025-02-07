using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

using CarRentApp.Src.Contexts;
using CarRentApp.Src.Repositories;
using CarRentApp.Src.Models;



namespace CarRentApp.Views.Users.Admin
{
    public partial class AdminView : UserControl
    {
        public event RoutedEventHandler? Logout;

        private readonly AuthContext _authContext;
        private readonly UserRepository _userRepository;
        private User? _selectedItem; // keep track on the current selected item 

        public AdminView(DatabaseContext dbContext)
        {
            InitializeComponent();
            _authContext = AuthContext.GetInstance();
            _authContext.CurrentUserChanged += LoadUserInfo;

            _userRepository = new UserRepository(dbContext);

            LoadUserRoles();
            LoadUserList();
            LoadUserInfo();
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

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            LoadUserRoles();
            LoadUserList();
            LoadUserInfo();
        }
        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            var firstName = FirstnameTextBox.Text.Trim();
            var surname = SurnameTextBox.Text.Trim();
            var email = EmailTextBox.Text.Trim();
            var role = (Role)RoleComboBox.SelectedItem;

            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(surname) ||
                string.IsNullOrEmpty(email))
            {
                MessageBox.Show("All fields are required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                _userRepository.AddUser(firstName, surname, email, "password", role);
                LoadUserList();
                ClearUserForm();
                MessageBox.Show("User added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EditUser_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedItem != null)
            {
                CreateUserTab(_selectedItem);
            }
        }

        private void DeleteUser_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedItem != null)
            {
                if (_selectedItem.Id == _authContext.GetCurrentUser().Id)
                {
                    MessageBox.Show($"An error occurred: Administrator cannot remove itself", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                MessageBoxResult result = MessageBox.Show($"You are about to delete user {_selectedItem.Email} ({_selectedItem.Role}). Are you sure?", "Information", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    _userRepository.RemoveUser(_selectedItem.Id);
                }
                LoadUserList();
            }
        }

        private void ModifyUser_Click(Object sender, RoutedEventArgs e)
        {
            TabItem selectedTab = AdminTabControl.SelectedItem as TabItem;
            if (selectedTab != null)
            {
                User temp_user = selectedTab.DataContext as User;
                if (temp_user != null)
                {
                    try
                    {
                        User final_user = _userRepository.GetUser(temp_user.Id);
                        var firstName = temp_user.Name;
                        var surname = temp_user.Surname;
                        var email = temp_user.Email;
                        if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(surname) ||
                        string.IsNullOrEmpty(email))
                        {
                            MessageBox.Show("All fields are required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                        _userRepository.UpdateUser(final_user.Id, firstName, surname, email, final_user.Password, final_user.Role);
                        MessageBox.Show("User updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        AdminTabControl.Items.Remove(AdminTabControl.SelectedItem);
                        LoadUserList();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedItem = UserDataGrid.SelectedItem as User;
        }

        private void LoadUserRoles()
        {
            RoleComboBox.ItemsSource = System.Enum.GetValues(typeof(Role)).Cast<Role>();
            RoleComboBox.SelectedIndex = 2;
        }

        private void LoadUserList()
        {
            UserDataGrid.ItemsSource = _userRepository.GetUsers();
        }

        private void ClearUserForm()
        {
            FirstnameTextBox.Clear();
            SurnameTextBox.Clear();
            EmailTextBox.Clear();
            RoleComboBox.SelectedIndex = 2;
        }

        private void CreateUserTab(User user)
        {
            TabItem newTab = new TabItem
            {
                Header = CreateTabHeader(user),
                Content = CreateTabContent(user),
                DataContext = new User() { Id = user.Id, Name = user.Name, Surname = user.Surname, Email = user.Email, Password = "", Role = user.Role }
            };

            AdminTabControl.Items.Add(newTab);
            AdminTabControl.SelectedItem = newTab;
        }

        private UIElement CreateTabContent(User user)
        {
            StackPanel stackPanel = new StackPanel
            {
                Margin = new Thickness(10)
            };

            TextBlock title = new TextBlock
            {
                Text = $"Modify User",
                FontSize = 20,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 0, 0, 10)
            };

            stackPanel.Children.Add(title);
            List<String> list = new List<String>
            {
                "Name",
                "Surname",
                "Email",
                "Role"
            };

            foreach (String item in list)
            {
                StackPanel subStackPanel = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    Margin = new Thickness(5)
                };
                subStackPanel.Children.Add(new TextBlock
                {
                    Text = $"{item}:",
                    Width = 100,
                    VerticalAlignment = VerticalAlignment.Center
                });

                if (item.Equals("Role"))
                {
                    TextBox t = new TextBox
                    {
                        Name = $"{item}ComboBox",
                        Width = 300,
                        IsEnabled = false
                    };
                    t.SetBinding(TextBox.TextProperty, new Binding($"{item}") { Mode = BindingMode.TwoWay });
                    subStackPanel.Children.Add(t);
                }
                else
                {
                    TextBox t = new TextBox
                    {
                        Name = $"{item}TextBox",
                        Width = 300
                    };
                    t.SetBinding(TextBox.TextProperty, new Binding($"{item}") { Mode = BindingMode.TwoWay });
                    subStackPanel.Children.Add(t);
                }

                stackPanel.Children.Add(subStackPanel);
            }

            Button save = new Button
            {
                Content = "Save",
                Width = 100,
                Height = 30,
                Margin = new Thickness(0, 10, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left,
            };

            save.Click += (s, e) => { ModifyUser_Click(s, e); };
            stackPanel.Children.Add(save);
            return stackPanel;
        }

        private UIElement CreateTabHeader(User user)
        {
            StackPanel stackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Stretch
            };

            TextBlock headerTextBlock = new TextBlock
            {
                Text = $"[{user.Id}] {user.Name} {user.Surname}",
                VerticalAlignment = VerticalAlignment.Center
            };

            Button close = new Button
            {
                Content = "X",
                Width = 20,
                Height = 20,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Center,
                Padding = new Thickness(0),
                Margin = new Thickness(2)
            };

            close.Click += (s, e) => {
                AdminTabControl.Items.Remove(((close.Parent as StackPanel).Parent as TabItem));
            };

            stackPanel.Children.Add(headerTextBlock);
            stackPanel.Children.Add(close);

            return stackPanel;
        }
    }
}
