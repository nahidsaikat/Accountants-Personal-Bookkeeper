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
    public sealed partial class LedgerAccount : Page
    {
        SQLite.Net.SQLiteConnection conn;
        AccountViewModel viewModel;
        List<AccountLedger> ledgers;

        public LedgerAccount()
        {
            this.InitializeComponent();
            conn = new Connection().GetConnection();
            viewModel = new AccountViewModel();
            ledgers = viewModel.AccountLedgerList();
            LoadListView();
        }

        private void LoadListView()
        {
            foreach (AccountLedger ledger in ledgers)
            {
                StackPanel stackpanel = new StackPanel();
                stackpanel.Name = ledger.account_id.ToString();
                stackpanel.Orientation = Orientation.Horizontal;

                TextBlock textblock = new TextBlock();
                textblock.Text = ledger.account_name;
                if (ledger.account_id != -1)
                {
                    textblock.Text += " # " + ledger.balance.ToString();
                }
                textblock.FontSize = 18;
                textblock.Margin = new Thickness(5, 0, 0, 0);
                stackpanel.Children.Add(textblock);

                AccountLedgerListView.Items.Add(stackpanel);
            }
        }
    }
}
