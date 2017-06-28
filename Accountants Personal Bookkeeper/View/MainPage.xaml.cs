using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Accountants_Personal_Bookkeeper.ViewModel;
using Accountants_Personal_Bookkeeper.DB;
using Accountants_Personal_Bookkeeper.View;


namespace Accountants_Personal_Bookkeeper
{
    public sealed partial class MainPage: Page
    {
        
        MainPageViewModel viewmodel;
        SQLite.Net.SQLiteConnection conn;

        public MainPage()
        {
            this.InitializeComponent();
            MyFrame.Navigate(typeof(HomeFrame));
            HomeListBoxItem.IsSelected = true;

            viewmodel = new MainPageViewModel();
            conn = new Connection().GetConnection();
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
        }

        private void MenuListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (HomeListBoxItem.IsSelected)
            {
                MyFrame.Navigate(typeof(HomeFrame));
                SettingsListBoxItem.IsSelected = false;
                TitleTextBlock.Text = "Home";
            }
            else if (ChartListBoxItem.IsSelected)
            {
                MyFrame.Navigate(typeof(AccountFrame));
                SettingsListBoxItem.IsSelected = false;
                TitleTextBlock.Text = "Accounts";
            }
            else if (JournalsListBoxItem.IsSelected)
            {
                MyFrame.Navigate(typeof(JournalFrame));
                SettingsListBoxItem.IsSelected = false;
                TitleTextBlock.Text = "Journals";
            }
            else if (LedgersListBoxItem.IsSelected)
            {
                MyFrame.Navigate(typeof(LedgerFrame));
                SettingsListBoxItem.IsSelected = false;
                TitleTextBlock.Text = "Ledgers";
            }
            else if (PartyListBoxItem.IsSelected)
            {
                MyFrame.Navigate(typeof(PartyFrame));
                SettingsListBoxItem.IsSelected = false;
                TitleTextBlock.Text = "Party";
            }
            else { }
        }

        private void SettingsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SettingsListBoxItem.IsSelected)
            {
                MyFrame.Navigate(typeof(Settings));
                TitleTextBlock.Text = "Settings";
                HomeListBoxItem.IsSelected = false;
                ChartListBoxItem.IsSelected = false;
                JournalsListBoxItem.IsSelected = false;
                LedgersListBoxItem.IsSelected = false;
                PartyListBoxItem.IsSelected = false;
            }
            else { }
        }
    }
}

