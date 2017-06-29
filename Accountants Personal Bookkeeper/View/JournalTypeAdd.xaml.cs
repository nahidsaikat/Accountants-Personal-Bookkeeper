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
    public sealed partial class JournalTypeAdd : Page
    {
        SQLite.Net.SQLiteConnection conn;
        JournalTypeViewModel viewModel;
        AccountViewModel accountVM;

        public JournalTypeAdd()
        {
            this.InitializeComponent();
            conn = new Connection().GetConnection();
            conn.CreateTable<JournalType>();
            viewModel = new JournalTypeViewModel();
            accountVM = new AccountViewModel();

            PageLoad();
        }

        private void PageLoad()
        {
            DebitAccountCombo();
            CreditAccountCombo();
        }

        private void DebitAccountCombo()
        {
            foreach (var account in accountVM.AccountList())
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Name = account.id.ToString();
                item.Content = account.name.ToString();
                DebitComboBox.Items.Add(item);
            }
        }

        private void CreditAccountCombo()
        {
            foreach (var account in accountVM.AccountList())
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Name = account.id.ToString();
                item.Content = account.name.ToString();
                CreditComboBox.Items.Add(item);
            }
        }

        private void JournalTypeSaveButton_Click(object sender, RoutedEventArgs e)
        {
            string name = NameTextBox.Text;
            string prefix = PrefixTextBox.Text;
            string start_no = StartNumberTextBox.Text;
            string debit_account = "-1";
            string credit_account = "-1";

            if (DebitComboBox.SelectedValue != null)
            {
                debit_account = (DebitComboBox.SelectedValue as ComboBoxItem).Name.ToString();
            }
            if (CreditComboBox.SelectedValue != null)
            {
                credit_account = (CreditComboBox.SelectedValue as ComboBoxItem).Name.ToString();
            }

            // Checking empty value
            if (name == string.Empty || prefix == string.Empty || start_no == string.Empty || 
                debit_account == "-1" || credit_account == "-1")
            {
                WarningTextBlock.Visibility = Visibility.Visible;
                WarningTextBlock.Text = "Please fill up all the field.";
                WarningTextBlock.Foreground = new SolidColorBrush(Windows.UI.Colors.Red);
            }
            else
            {
                // Checking duplicate value
                bool same = false;
                var queryType = conn.Table<JournalType>();
                foreach (var type in queryType)
                {
                    if (type.name == name || type.prefix == prefix)
                    {
                        same = true;
                        break;
                    }
                }

                if (!same)
                {
                    // Insert in database as it is not already in database
                    bool flag = viewModel.Add(name, prefix, start_no, debit_account, credit_account);
                    if (flag)
                    {
                        NameTextBox.Text = string.Empty;
                        PrefixTextBox.Text = string.Empty;
                        StartNumberTextBox.Text = string.Empty;

                        WarningTextBlock.Visibility = Visibility.Visible;
                        WarningTextBlock.Text = "Journal type added successfully.";
                        WarningTextBlock.Foreground = new SolidColorBrush(Windows.UI.Colors.Green);
                    }
                    else
                    {
                        // something went wrong.
                        WarningTextBlock.Visibility = Visibility.Visible;
                        WarningTextBlock.Text = "Something went Wrong.";
                        WarningTextBlock.Foreground = new SolidColorBrush(Windows.UI.Colors.Red);
                    }
                }
                else
                {
                    // as duplicate value tell user.
                    WarningTextBlock.Visibility = Visibility.Visible;
                    WarningTextBlock.Text = "Name & code is already in used.";
                    WarningTextBlock.Foreground = new SolidColorBrush(Windows.UI.Colors.Red);
                }
            }
        }
    }
}
