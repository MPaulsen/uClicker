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
            formMain.userRole = "User";
            loadMenu();
        }

        private void loadMenu()
        {
            MainMenu menu = new MainMenu(formMain);
            formMain.frmBody.Content = menu;
        }

        private void txtUser_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            txtUser.Clear();
        }

        private void txtPass_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            txtPass.Clear();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string inputUser = txtUser.Text;

            if (Convert.ToInt32(dbControls.dbQuery("SELECT COUNT(UserID) FROM Users WHERE UserID = '" + inputUser + "'")) < 1)
            {
                MessageBox.Show("No such user exists.");
                return;
            }

            if (dbControls.dbQuery("SELECT Password FROM Users WHERE UserID = '" + inputUser + "'") == txtPass.Password)
            {
                formMain.userName = inputUser;
                formMain.userRole = dbControls.dbQuery("SELECT Role FROM Users WHERE UserID = '" + inputUser + "'");
                formMain.frmBody.Content = new MainMenu(formMain);
            }

            else
                MessageBox.Show("Incorrect username/password.");
        }

        private void txtPass_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btnLogin.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }
    }
}
