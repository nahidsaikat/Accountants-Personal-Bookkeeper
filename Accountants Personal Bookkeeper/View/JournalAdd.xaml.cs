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
    public sealed partial class JournalAdd : Page
    {
        SQLite.Net.SQLiteConnection conn;
        AccountViewModel accountVM;

        public JournalAdd()
        {
            this.InitializeComponent();
            conn = new Connection().GetConnection();
            //conn.CreateTable<Journal>();
            accountVM = new AccountViewModel();

            LoadPage();
        }

        public void LoadPage()
        {
            LoadAccountGrid();
        }

        public void LoadAccountGrid()
        {
            for (int counter=0; counter < 4; counter++)
            {
                StackPanel stackpanel = new StackPanel();
                stackpanel.Name = counter.ToString() + "StackPanel";
                stackpanel.Orientation = Orientation.Horizontal;
                stackpanel.Margin = new Thickness(5, 0, 0, 0);

                ComboBox AccountComboBox = new ComboBox();
                AccountComboBox.Name = counter.ToString() + "ComboBox";
                AccountComboBox.Width = 100;
                foreach (var account in accountVM.AccountList())
                {
                    ComboBoxItem item = new ComboBoxItem();
                    item.Name = account.id.ToString();
                    item.Content = account.name.ToString();
                    AccountComboBox.Items.Add(item);
                }
                stackpanel.Children.Add(AccountComboBox);

                TextBox DebitTextBox = new TextBox();
                DebitTextBox.Name = counter.ToString() + "TextBox";
                DebitTextBox.PlaceholderText = "Debit";
                DebitTextBox.FontSize = 18;
                DebitTextBox.Width = 100;
                DebitTextBox.Margin = new Thickness(5, 0, 0, 0);
                stackpanel.Children.Add(DebitTextBox);

                TextBox CreditTextBox = new TextBox();
                CreditTextBox.Name = counter.ToString() + "TextBox";
                CreditTextBox.PlaceholderText = "Credit";
                CreditTextBox.FontSize = 18;
                CreditTextBox.Width = 100;
                CreditTextBox.Margin = new Thickness(5, 0, 0, 0);
                stackpanel.Children.Add(CreditTextBox);

                AccountInfoGridView.Items.Add(stackpanel);
            }
        }

        private void JournalSaveButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
