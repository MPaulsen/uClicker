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
    /// Interaction logic for ManageGroup.xaml
    /// </summary>
    public partial class ManageGroup : Page
    {
        MainWindow frmMain;
        String groupID;
        String groupName;
        public ManageGroup(MainWindow _frmMain, String _groupID, String _groupName)
        {
            InitializeComponent();
            groupID = _groupID;
            frmMain = _frmMain;
            groupName = _groupName;

            lblGroupName.Content = groupName;

            loadList();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            frmMain.frmBody.Content = new Groups(frmMain);
        }

        private void loadList()
        {
            SqlConnection con = new SqlConnection(Properties.Settings.Default.UDB);
            String query = @"SELECT GroupID, PollerID
                             FROM [uClicker].[dbo].[Groups_Members]
                             WHERE GroupID = " + groupID;
            SqlCommand cmd = new SqlCommand(query, con);

            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable("GroupMembers");
            sda.Fill(dt);
            dgGroupMembers.ItemsSource = dt.DefaultView;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            DataRowView row = (DataRowView)((FrameworkElement)sender).DataContext;
            string userID = row[1].ToString();

            dbControls.dbNonQuery("DELETE FROM Groups_Members WHERE GroupID = '" + groupID + "' AND PollerID = '" + userID + "'");
            loadList();
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {

            if (txtMemberID.Text == "")
            {
                MessageBox.Show("Please provide a username.");
                return;
            }
            
            dbControls.dbNonQuery("INSERT INTO Groups_Members (GroupID, PollerID) VALUES('" + groupID + "', '" + txtMemberID.Text.ToString() + "')");
            txtMemberID.Clear();
            loadList();
        }

    }
}
