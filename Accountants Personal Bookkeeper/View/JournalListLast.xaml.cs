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
using System.Diagnostics;
using Accountants_Personal_Bookkeeper.DB;
using Accountants_Personal_Bookkeeper.Model;
using Accountants_Personal_Bookkeeper.ViewModel;


namespace Accountants_Personal_Bookkeeper.View
{
    public sealed partial class JournalListLast : Page
    {
        List<Journal> journals;
        SQLite.Net.SQLiteConnection conn;
        JournalViewModel viewModel;

        public JournalListLast()
        {
            this.InitializeComponent();
            conn = new Connection().GetConnection();
            viewModel = new JournalViewModel();
            journals = viewModel.JournalListLastMonth();
        }

        private async void ViewJournalButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int id = (Int32)((Button)sender).Tag;
                var journal = (from jour in conn.Table<Journal>()
                               where jour.id == id
                               select jour
                               ).First<Journal>();
                string message = "Number : " + journal.number +
                    "\nType : " + viewModel.GetType(journal.type).name + "\nAmount : " + journal.amount.ToString();
                if (journal.party_id != -1)
                {
                    message += "\nParty : " + viewModel.GetParty(journal.party_id).name;
                }
                if (journal.ref_journal_id != -1)
                {
                    message += "\nRef Journal : " + viewModel.Get(journal.ref_journal_id).number;
                }
                message += "\nDescription : " + journal.description +
                    "\nAccounts Information :\n" + viewModel.GetAccountsName(journal.account_info);

                var dialog = new Windows.UI.Popups.MessageDialog(message, journal.number);
                var result = await dialog.ShowAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Processor Usage" + ex.Message);
            }
        }

        private void EditJournalButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int id = (Int32)((Button)sender).Tag;
                var account = viewModel.Get(id);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Processor Usage" + ex.Message);
            }
        }
    }
}
