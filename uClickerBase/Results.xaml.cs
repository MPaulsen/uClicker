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
    /// Interaction logic for Results.xaml
    /// </summary>
    public partial class Results : Page
    {
        MainWindow formMain;
        String currentPoll;
        public Results(MainWindow frmMain, String pollID)
        {
            InitializeComponent();
            formMain = frmMain;
            currentPoll = pollID;

            LoadPoll();
        }

        private void LoadPoll()
        {
            //Query for poll data with currentPoll as pollID
            for (int i = 0; i < 5; i++)
            {
                ProgressBar resultBar = new ProgressBar();
                resultBar.Value = i*20;
                Label resultLabel = new Label();
                resultLabel.Content = "This is response " + i + ".";
                stkResults.Children.Add(resultLabel);
                stkResults.Children.Add(resultBar);
            }
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            if (formMain.userName == "Guest")
                ClosePoll();
            MainMenu mainMenu = new MainMenu(formMain);
            formMain.frmBody.Content = mainMenu;
        }

        private void ClosePoll()
        {
            ;
        }
    }
}
