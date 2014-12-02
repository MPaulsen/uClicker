using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for CreatePoll.xaml
    /// </summary>
    public partial class CreatePoll : Page
    {
        MainWindow formMain;
        public CreatePoll(MainWindow frmMain)
        {
            InitializeComponent();
            formMain = frmMain;
        }

        public void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (tbAnswer.Text.ToString() == "")
                return;
            Response input = new Response(tbAnswer.Text.ToString());
            dgResponses.Items.Add(input);
            tbAnswer.Clear();
        }

        private void tbAnswer_Enter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnAdd.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
        }

        public void chkAnon_Checked(object sender, RoutedEventArgs e)
        {
            if (chkAnon.IsChecked.Value)
            {
                chkVerified.IsChecked = true;
                chkVerified.IsEnabled = false;
            }
            else
                chkVerified.IsEnabled = true;
        }

        public void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            ;
        }

        public class Response
        {
            public string strResponse { get; set; }
            public Response(string input)
            {
                strResponse = input;
            }
        }

        public void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            //Do the database work.
            string question = tbQuestion.Text.ToString();
            List<String> responses = new List<String>();

            foreach(Response re in dgResponses.Items)
                responses.Add(re.strResponse);

            DateTime created = DateTime.Now;

            dbControls.dbNonQuery(@"
            INSERT INTO Polls (UserID, PollCode, Created, Active, Verified, Anonymous, GroupID, Question)
            VALUES('" + formMain.userName + "', '" + getPollCode() + "', '" + created + "', 1, 0, 0, NULL, '" + question + "')");
            
            foreach (String answer in responses)
                dbControls.dbNonQuery(@"
                INSERT INTO Polls_Responses (PollID, Response)
                VALUES((SELECT PollID FROM Polls WHERE Created = '" + created + "' AND UserID = '" + formMain.userName + "' AND Question = '" + question + "'), '" + answer + "')");

            //Redirect to live results view.
            Results pgResults = new Results(formMain, "1234");
            formMain.frmBody.Content = pgResults;

        }

        private string getPollCode()
        {
            return "ALDK";
        }
    }
}
