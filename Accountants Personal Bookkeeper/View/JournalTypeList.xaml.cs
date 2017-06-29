﻿using System;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Accountants_Personal_Bookkeeper.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class JournalTypeList : Page
    {
        List<JournalType> journalTypes;
        SQLite.Net.SQLiteConnection conn;
        JournalTypeViewModel viewModel;
        AccountViewModel accountVM;

        public JournalTypeList()
        {
            this.InitializeComponent();
            conn = new Connection().GetConnection();
            viewModel = new JournalTypeViewModel();
            journalTypes = viewModel.JournalTypeList();
            accountVM = new AccountViewModel();
        }

        private async void ViewJournalTypeButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int id = (Int32)((Button)sender).Tag;
                var journaltype = (from sub in conn.Table<JournalType>()
                                      where sub.id == id
                                      select sub
                                  ).First<JournalType>();
                string message = "Name : " + journaltype.name + "\nCode : " + journaltype.prefix + "\nStart NO : " + journaltype.start_number +
                    "\nDebit Account : " + accountVM.Get(journaltype.debit_account_id).name +
                    "\nCredit Account : " + accountVM.Get(journaltype.credit_account_id).name;

                var dialog = new Windows.UI.Popups.MessageDialog(message, journaltype.name);
                var result = await dialog.ShowAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Processor Usage" + ex.Message);
            }
        }

        private void EditJournalTypeButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int id = (Int32)((Button)sender).Tag;
                var accountSubtype = viewModel.Get(id);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Processor Usage" + ex.Message);
            }
        }
    }
}
