using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public sealed partial class AccountAdd : Page
    {
        SQLite.Net.SQLiteConnection conn;
        AccountViewModel viewModel;
        AccountSubtypeViewModel subtypeViewModel;

        public AccountAdd()
        {
            this.InitializeComponent();
            conn = new Connection().GetConnection();
            conn.CreateTable<Account>();
            viewModel = new AccountViewModel();
            subtypeViewModel = new AccountSubtypeViewModel();

            PageLoad();
        }

        private void PageLoad()
        {
            LoadTypeCombo();
            LoadSubtypeCombo();
            LoadParentCombo();
        }

        private void LoadParentCombo()
        {
            foreach (var account in viewModel.AccountList())
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Name = account.id.ToString();
                item.Content = account.name.ToString();
                ParentComboBox.Items.Add(item);
            }
        }

        private void LoadSubtypeCombo()
        {
            foreach(var subtype in subtypeViewModel.AccountSubtypeList())
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Name = subtype.id.ToString();
                item.Content = subtype.name.ToString();
                SubtypeComboBox.Items.Add(item);
            }
        }

        private void LoadTypeCombo()
        {
            ComboBoxItem assetItem = new ComboBoxItem();
            assetItem.Name = ((int)AccountType.Assets).ToString();
            assetItem.Content = AccountType.Assets.ToString();
            TypeComboBox.Items.Add(assetItem);

            ComboBoxItem liabilityItem = new ComboBoxItem();
            liabilityItem.Name = ((int)AccountType.Liability).ToString();
            liabilityItem.Content = AccountType.Liability.ToString();
            TypeComboBox.Items.Add(liabilityItem);

            ComboBoxItem incomeItem = new ComboBoxItem();
            incomeItem.Name = ((int)AccountType.Income).ToString();
            incomeItem.Content = AccountType.Income.ToString();
            TypeComboBox.Items.Add(incomeItem);

            ComboBoxItem expenseItem = new ComboBoxItem();
            expenseItem.Name = ((int)AccountType.Expense).ToString();
            expenseItem.Content = AccountType.Expense.ToString();
            TypeComboBox.Items.Add(expenseItem);

            ComboBoxItem equityItem = new ComboBoxItem();
            equityItem.Name = ((int)AccountType.Equity).ToString();
            equityItem.Content = AccountType.Equity.ToString();
            TypeComboBox.Items.Add(equityItem);
        }

        private void AccountSaveButton_Click(object sender, RoutedEventArgs e)
        {
            string name = NameTextBox.Text;
            string code = CodeTextBox.Text;
            string typeString = "-1";
            string subtypeString = "-1";
            string parentString = "-1";
            string openingBalance = OpeningBalanceTextBox.Text;
            string openingDate = OpeningDateCalendarDatePicker.Date.ToString();
            string note = NoteTextBox.Text;

            if (TypeComboBox.SelectedValue != null)
            {
                typeString = (TypeComboBox.SelectedValue as ComboBoxItem).Name.ToString();
            }
            if (SubtypeComboBox.SelectedValue != null)
            {
                subtypeString = (SubtypeComboBox.SelectedValue as ComboBoxItem).Name.ToString();
            }
            if (ParentComboBox.SelectedValue != null)
            {
                parentString = (ParentComboBox.SelectedValue as ComboBoxItem).Name.ToString();
            }

            // Checking empty value
            if (name == string.Empty || code == string.Empty || typeString == string.Empty ||
                subtypeString == string.Empty || openingBalance == string.Empty || 
                openingDate == string.Empty)
            {
                WarningTextBlock.Visibility = Visibility.Visible;
                WarningTextBlock.Text = "Please fill up all the field.";
                WarningTextBlock.Foreground = new SolidColorBrush(Windows.UI.Colors.Red);
            }
            else
            {
                if (IsUnique(name, code))
                {
                    // Insert in database as it is not already in database
                    int success = viewModel.Add(name, code, typeString, subtypeString, parentString, openingBalance, openingDate, note);
                    if (success > 0)
                    {
                        NameTextBox.Text = string.Empty;
                        CodeTextBox.Text = string.Empty;

                        WarningTextBlock.Visibility = Visibility.Visible;
                        WarningTextBlock.Text = "Account added successfully.";
                        WarningTextBlock.Foreground = new SolidColorBrush(Windows.UI.Colors.Green);
                    }
                    else if(success == -2)
                    {
                        WarningTextBlock.Visibility = Visibility.Visible;
                        WarningTextBlock.Text = "Journal creation problem!";
                        WarningTextBlock.Foreground = new SolidColorBrush(Windows.UI.Colors.Red);
                    }
                    else
                    {
                        WarningTextBlock.Visibility = Visibility.Visible;
                        WarningTextBlock.Text = "Something went wrong!";
                        WarningTextBlock.Foreground = new SolidColorBrush(Windows.UI.Colors.Red);
                    }
                }
                else
                {
                    // as duplicate value tell user.
                    WarningTextBlock.Visibility = Visibility.Visible;
                    WarningTextBlock.Text = "Name or Code is used.";
                    WarningTextBlock.Foreground = new SolidColorBrush(Windows.UI.Colors.Red);
                }
            }
        }

        private bool IsUnique(string name, string code)
        {
            // Checking duplicate value
            bool unique = true;
            var query = conn.Table<Account>();
            foreach (var account in query)
            {
                if (account.name == name || account.code == code)
                {
                    unique = false;
                    break;
                }
            }
            return unique;
        }
    }
}
