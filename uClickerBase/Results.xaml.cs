using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Interaction logic for Results.xaml
    /// </summary>
    public partial class Results : Page
    {
        MainWindow formMain;
        String currentPoll;
        List<Brush> brushList;
        bool active;
        bool open;

        private readonly BackgroundWorker bg_worker = new BackgroundWorker();


        public Results(MainWindow frmMain, String pollID)
        {
            InitializeComponent();
            formMain = frmMain;
            currentPoll = pollID;
            lblQuestion.Content = dbControls.dbQuery("SELECT Question FROM Polls WHERE PollID = " + currentPoll);
            lblPollCode.Content = "Poll Code: " + dbControls.dbQuery("SELECT PollCode FROM Polls WHERE PollID = " + currentPoll);
            active = (dbControls.dbQuery("SELECT Active FROM Polls WHERE PollID = " + currentPoll) == "True");
            open = true;

            brushList = new List<Brush>();
            brushList.Add(Brushes.Red);
            brushList.Add(Brushes.Green);
            brushList.Add(Brushes.Blue);
            brushList.Add(Brushes.Orange);
            brushList.Add(Brushes.Purple);



            if (active)
            {
                lblStatus.Content = "Open";
                lblStatus.Foreground = new SolidColorBrush(Colors.Lime);
            }
            else
            {
                lblStatus.Content = "Closed";
                lblStatus.Foreground = new SolidColorBrush(Colors.Red);
            }

            bg_worker.DoWork += bg_worker_DoWork;
            bg_worker.RunWorkerCompleted += bg_worker_Completed;
            bg_worker.RunWorkerAsync();
        }

        private void bg_worker_DoWork(object sender, DoWorkEventArgs e)
        {
            this.Dispatcher.Invoke((Action)(() =>
                {
                    RefreshResults();
                }));

            //Time between updates.
            Thread.Sleep(Properties.Settings.Default.PollRate);
        }

        private void bg_worker_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            if (active && open)
            {
                bg_worker.RunWorkerAsync();
                return;
            }
            if (open)
            {
                open = false;
                bg_worker.RunWorkerAsync();
            }
        }

        private void RefreshResults()
        {
            stkResults.Children.Clear();

            //Query for poll data with currentPoll as pollID
            int responseCount = Convert.ToInt32(dbControls.dbQuery("SELECT COUNT(*) FROM Responses WHERE PollID = " + currentPoll));
            int totalVotes = Convert.ToInt32(dbControls.dbQuery("SELECT COUNT(*) FROM Polls_Responses WHERE PollID = " + currentPoll));
            List<Response> responseList = new List<Response>();

            SqlConnection con = new SqlConnection(Properties.Settings.Default.UDB);
            String query = @"SELECT Response, COUNT(PollerID) AS Votes
                             FROM Responses R LEFT JOIN Polls_Responses PR ON R.ResponseID = PR.ResponseID
                             WHERE R.PollID = " + currentPoll + @"
                             GROUP BY Response";
            SqlCommand cmd = new SqlCommand(query, con);

            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                Response tmpResp = new Response(rdr[0].ToString());
                tmpResp.voterCount = Convert.ToInt32(rdr[1].ToString());
                responseList.Add(tmpResp);
            }
            con.Close();

            for (int i = 0; i < responseCount; i++)
            {
                ProgressBar resultBar = new ProgressBar();
                float result = (totalVotes == 0) ? 0 :  ((float)responseList[i].voterCount / (float)totalVotes) * 100;
                resultBar.Value = result;
                resultBar.Height = 50;
                resultBar.Foreground = brushList[i%5];
                
                Label lblResultName = new Label();
                lblResultName.Content = responseList[i].strResponse;
                Label lblResultStats = new Label();
                lblResultStats.Content = responseList[i].voterCount.ToString() + " Votes | " + result + "%";
                stkResults.Children.Add(lblResultName);
                stkResults.Children.Add(resultBar);
                stkResults.Children.Add(lblResultStats);
            }
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            if (formMain.userName == "Guest")
                ClosePoll();
            open = false;
            MainMenu mainMenu = new MainMenu(formMain);
            formMain.frmBody.Content = mainMenu;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            ClosePoll();
        }

        private void btnDetails_Click(object sender, RoutedEventArgs e)
        {
            formMain.frmBody.Content = new ResultDetails(formMain, currentPoll);
        }

        private void ClosePoll()
        {
            active = false;
            lblStatus.Content = "Closed";
            lblStatus.Foreground = new SolidColorBrush(Colors.Red);
            dbControls.dbNonQuery("UPDATE Polls SET Active = 0 WHERE PollID = " + currentPoll);
        }
    }
}
