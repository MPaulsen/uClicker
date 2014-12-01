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
            ;
            //Do the database work.
            //MessageBox.Show "Created!"
            //Redirect to live results view.
            Results pgResults = new Results(formMain, "1234");
            formMain.frmBody.Content = pgResults;
        }
    }
}
