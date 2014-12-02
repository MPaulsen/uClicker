using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
    /// Interaction logic for Users.xaml
    /// </summary>
    public partial class Users : Page
    {
        MainWindow frmMain;
        public Users(MainWindow incoming)
        {
            InitializeComponent();
            frmMain = incoming;
            loadList();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            frmMain.frmBody.Content = new MainMenu(frmMain);
        }

        private void loadList()
        {
            SqlConnection con = new SqlConnection(Properties.Settings.Default.UDB);
            String query = @"SELECT UserID, Role
                             FROM Users";
            SqlCommand cmd = new SqlCommand(query, con);

            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable("Users");
            sda.Fill(dt);
            dgUsers.ItemsSource = dt.DefaultView;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            DataRowView row = (DataRowView)((FrameworkElement)sender).DataContext;
            string userID = row[0].ToString();

            dbControls.dbNonQuery("DELETE FROM Users WHERE UserID = '" + userID + "'");
            loadList();
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            if (cbRole.SelectedItem == null)
            {
                MessageBox.Show("Please select a role.");
                return;
            }

            if (txtUserName.Text == "")
            {
                MessageBox.Show("Please provide a username.");
                return;
            }

            string user = txtUserName.Text;
            string password = txtPassword.Text;
            string role = ((ComboBoxItem)(cbRole.SelectedItem)).Content.ToString();

            int count = Convert.ToInt32(dbControls.dbQuery("SELECT COUNT(UserID) FROM Users WHERE UserID = '" + user + "'"));
            if (count > 0)
            {
                MessageBox.Show("That username already exists.");
                return;
            }

            dbControls.dbNonQuery("INSERT INTO Users (UserID, Password, Role) VALUES('" + user + "', '" + password + "', '" + role + "')");
            txtUserName.Clear();
            txtPassword.Clear();
            cbRole.SelectedIndex = 0;
            loadList();
        }
    }
}
