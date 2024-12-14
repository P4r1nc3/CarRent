using System.Linq;
using System.Windows;
using CarRentApp.Data;

namespace CarRentApp
{
    public partial class MainWindow : Window
    {
        private AppDbContext _dbContext;

        public MainWindow()
        {
            InitializeComponent();
            _dbContext = new AppDbContext();
        }
    }
}