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
    public sealed partial class AccountSubtypeList : Page
    {
        List<AccountSubtype> accountsSubtype;
        SQLite.Net.SQLiteConnection conn;
        AccountSubtypeViewModel viewModel;

        public AccountSubtypeList()
        {
            this.InitializeComponent();
            conn = new Connection().GetConnection();
            viewModel = new AccountSubtypeViewModel();
            accountsSubtype = viewModel.AccountSubtypeList();
        }

        private void EditAccountSubtypeButton_Click(object sender, RoutedEventArgs e)
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

        private async void ViewAccountSubtypeButton_ClickAsync(object sender, RoutedEventArgs e)
        {
            try {
                int id = (Int32)((Button)sender).Tag;
                var accountSubtype = (from sub in conn.Table<AccountSubtype>()
                                      where sub.id == id
                                      select sub
                                  ).First<AccountSubtype>();
                string message = "Name : " + accountSubtype.name + "\nCode : " + accountSubtype.code +
                    "\nType : " + Enum.GetName(typeof(AccountType), accountSubtype.type);
                
                var dialog = new Windows.UI.Popups.MessageDialog(message, accountSubtype.name);
                var result = await dialog.ShowAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Processor Usage" + ex.Message);
            }
        }
    }
}
