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
    /// Interaction logic for ResultDetails.xaml
    /// </summary>
    public partial class ResultDetails : Page
    {
        MainWindow frmMain;
        String pollID;

        public ResultDetails(MainWindow _frmMain, String _pollID)
        {
            frmMain = _frmMain;
            pollID = _pollID;
            InitializeComponent();
            lblQuestion.Content = (dbControls.dbQuery("SELECT Question FROM Polls WHERE PollID = " + pollID));
            loadList();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            frmMain.frmBody.Content = new Results(frmMain, pollID);
        }

        private void loadList()
        {
            SqlConnection con = new SqlConnection(Properties.Settings.Default.UDB);
            String query = @"SELECT PollerID, Response
                             FROM [Polls_Responses] PR, [Responses] R
                             WHERE PR.ResponseID = R.ResponseID and R.PollID = " + pollID;
            SqlCommand cmd = new SqlCommand(query, con);

            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable("Responses");
            sda.Fill(dt);
            dgResponses.ItemsSource = dt.DefaultView;
        }
    }
}