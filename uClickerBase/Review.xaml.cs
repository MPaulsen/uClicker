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
    /// Interaction logic for Review.xaml
    /// </summary>
    public partial class Review : Page
    {
        MainWindow formMain;

        public Review(MainWindow incoming)
        {
            InitializeComponent();
            formMain = incoming;
            loadList();
        }

        private void loadList()
        {
            SqlConnection con = new SqlConnection(Properties.Settings.Default.UDB);
            String query = @"SELECT PollID, UserID, PollCode, Created, Active, Verified, Anonymous, GroupName, Question, 
                                (SELECT COUNT(*) FROM Polls_Responses WHERE Polls_Responses.PollID = P.PollID) AS Responses
                             FROM Polls P LEFT JOIN Groups G ON P.GroupID = G.GroupID" +
                             ((formMain.userRole == "Admin") ? ("") : (" WHERE UserID = '" + formMain.userName + "'"));
            SqlCommand cmd = new SqlCommand(query, con);

            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable("Polls");
            sda.Fill(dt);
            dgPolls.ItemsSource = dt.DefaultView;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            formMain.frmBody.Content = new MainMenu(formMain);
        }

        private void btnResults_Click(object sender, RoutedEventArgs e)
        {
            DataRowView row = (DataRowView)((FrameworkElement)sender).DataContext;
            formMain.frmBody.Content = new Results(formMain, row[0].ToString());
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result= MessageBox.Show("Are you sure?", "Delete Confirmation", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                DataRowView row = (DataRowView)((FrameworkElement)sender).DataContext;
                dbControls.dbNonQuery("DELETE FROM Polls_Responses WHERE PollID = " + row[0].ToString());
                dbControls.dbNonQuery("DELETE FROM Responses WHERE PollID = " + row[0].ToString());
                dbControls.dbNonQuery("DELETE FROM Polls WHERE PollID = " + row[0].ToString());
            }

            loadList();
        }
    }
}
