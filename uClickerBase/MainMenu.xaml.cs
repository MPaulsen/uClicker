﻿using System;
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
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Page
    {
        MainWindow formMain;
        public MainMenu(MainWindow frmMain)
        {
            InitializeComponent();

            formMain = frmMain;

            if (formMain.userName == "Guest")
            {
                btnGroups.Visibility = Visibility.Hidden;
                btnReview.Visibility = Visibility.Hidden;
                btnUsers.Visibility = Visibility.Hidden;
            }

            else if (formMain.userRole != "Admin")
            {
                btnUsers.Visibility = Visibility.Hidden;
            }

        }

        public void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            CreatePoll newPoll = new CreatePoll(formMain);
            formMain.frmBody.Content = newPoll;
        }

        private void btnUsers_Click(object sender, RoutedEventArgs e)
        {
            formMain.frmBody.Content = new Users(formMain);
        }

        private void btnLogOut_Click(object sender, RoutedEventArgs e)
        {
            formMain.frmBody.Content = new Login(formMain);
        }

        private void btnReview_Click(object sender, RoutedEventArgs e)
        {
            formMain.frmBody.Content = new Review(formMain);
        }

        private void btnGroups_Click(object sender, RoutedEventArgs e)
        {
            formMain.frmBody.Content = new Groups(formMain);
        }
    }
}
