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
    public sealed partial class AccountList : Page
    {
        List<Account> accounts;
        SQLite.Net.SQLiteConnection conn;
        AccountViewModel viewModel;

        public AccountList()
        {
            this.InitializeComponent();
            conn = new Connection().GetConnection();
            viewModel = new AccountViewModel();
            accounts = viewModel.AccountChart();
            LoadListView();
        }

        private void LoadListView()
        {
            foreach(Account account in accounts)
            {
                StackPanel stackpanel = new StackPanel();
                stackpanel.Name = account.id.ToString();
                stackpanel.Orientation = Orientation.Horizontal;

                TextBlock textblock = new TextBlock();
                textblock.Text = account.name;
                textblock.FontSize = 18;
                stackpanel.Children.Add(textblock);

                Button myButton = new Button();
                myButton.Name = account.id.ToString();
                myButton.Content = "view";
                myButton.Margin = new Thickness(10, 0, 0, 0);
                myButton.Background = new SolidColorBrush(Windows.UI.Colors.LightSlateGray);
                myButton.Tag = account.id;
                myButton.Click += ViewAccountButton_Click;
                if (account.id > 0)
                {

                    stackpanel.Children.Add(myButton);
                }

                AccountListView.Items.Add(stackpanel);
            }
        }

        private void EditAccountButton_Click(object sender, RoutedEventArgs e)
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

        private async void ViewAccountButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int id = (Int32)((Button)sender).Tag;
                var account = (from acc in conn.Table<Account>()
                               where acc.id == id
                               select acc
                                  ).First<Account>();
                string message = "Name : " + account.name + "\nCode : " + account.code +
                    "\nType : " + Enum.GetName(typeof(AccountType), account.type) + 
                    "\nSubtype : " + viewModel.GetSubtype(account.subtype).name
                    ;
                if (account.parent_id != -1)
                {
                    message += "\nParent : " + viewModel.Get(account.parent_id).name;
                }
                message += "\nOpening Balance : " + account.opening_balance.ToString() +
                    "\nOpening Date : " + account.entry_date.ToString() +
                    "\nNote : " + account.note;

                var dialog = new Windows.UI.Popups.MessageDialog(message, account.name);
                var result = await dialog.ShowAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Processor Usage" + ex.Message);
            }
        }
    }
}
