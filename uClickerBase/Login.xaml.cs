using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace uClickerBase
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        MainWindow formMain;
        public Login(MainWindow frmMain)
        {
            InitializeComponent();
            formMain = frmMain;
        }

        public void btnGuest_Click(object sender, RoutedEventArgs e)
        {
            formMain.userName = "Guest";
            loadMenu();
        }

        private void loadMenu()
        {
            MainMenu menu = new MainMenu(formMain);
            formMain.frmBody.Content = menu;
        }
    }
}
