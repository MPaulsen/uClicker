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
    /// Interaction logic for Groups.xaml
    /// </summary>
    public partial class Groups : Page
    {
        MainWindow frmMain;
        public Groups(MainWindow incoming)
        {
            InitializeComponent();
            frmMain = incoming;
            loadList();
            if (frmMain.userRole != "Admin")
                dgGroups.Columns[2].Visibility = System.Windows.Visibility.Hidden;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            frmMain.frmBody.Content = new MainMenu(frmMain);
        }

        private void loadList()
        {
            SqlConnection con = new SqlConnection(Properties.Settings.Default.UDB);
            String query = @"SELECT GroupID, GroupName, GroupOwner, (SELECT COUNT(*) FROM Groups_Members WHERE Groups.GroupID = GroupID) AS 'Members'
                             FROM Groups" + ((frmMain.userRole == "Admin") ? "" : " WHERE GroupOwner = '" + frmMain.userName + "'");
            SqlCommand cmd = new SqlCommand(query, con);

            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable("Groups");
            sda.Fill(dt);
            dgGroups.ItemsSource = dt.DefaultView;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            DataRowView row = (DataRowView)((FrameworkElement)sender).DataContext;
            string groupID = row[0].ToString();

            dbControls.dbNonQuery("DELETE FROM Groups_Members WHERE GroupID = '" + groupID + "'");
            dbControls.dbNonQuery("DELETE FROM Groups WHERE GroupID = '" + groupID + "'");
            loadList();
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {

            if (txtGroupName.Text == "")
            {
                MessageBox.Show("Please provide a username.");
                return;
            }

            string group = txtGroupName.Text;


            dbControls.dbNonQuery("INSERT INTO Groups (GroupName, GroupOwner) VALUES('" + group + "', '" + frmMain.userName + "')");
            txtGroupName.Clear();
            loadList();
        }

        private void btnManage_Click(object sender, RoutedEventArgs e)
        {
            DataRowView row = (DataRowView)((FrameworkElement)sender).DataContext;
            frmMain.frmBody.Content = new ManageGroup(frmMain, row[0].ToString(), row[1].ToString());
        }
    }
}
