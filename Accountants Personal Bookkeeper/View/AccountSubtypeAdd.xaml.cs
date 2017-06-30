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
    public sealed partial class AccountSubtypeAdd : Page
    {
        SQLite.Net.SQLiteConnection conn;
        AccountSubtypeViewModel viewModel;
        int id;

        public AccountSubtypeAdd()
        {
            this.InitializeComponent();
            conn = new Connection().GetConnection();
            conn.CreateTable<AccountSubtype>();
            viewModel = new AccountSubtypeViewModel();
            id = -1;
            PageLoad();
        }

        private void PageLoad()
        {
            LoadTypeCombo();
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

        private void AccountSubtypeSaveButton_Click(object sender, RoutedEventArgs e)
        {
            string id = IdTextBlock.Text;
            string name = NameTextBox.Text;
            string code = CodeTextBox.Text;
            string typeString = "-1";

            if (TypeComboBox.SelectedValue != null)
            {
                typeString = (TypeComboBox.SelectedValue as ComboBoxItem).Name.ToString();
            }

            // Checking empty value
            if (name == string.Empty || code == string.Empty || typeString == "-1")
            {
                WarningTextBlock.Visibility = Visibility.Visible;
                WarningTextBlock.Text = "Please fill up all the field.";
                WarningTextBlock.Foreground = new SolidColorBrush(Windows.UI.Colors.Red);
            }
            else
            {
                // Checking duplicate value
                bool same = false;
                var querySubtype = conn.Table<AccountSubtype>();
                foreach (var subtype in querySubtype)
                {
                    if (subtype.name == name || subtype.code == code)
                    {
                        same = true;
                        break;
                    }
                }

                if (!same)
                {
                    // Insert in database as it is not already in database
                    int success = viewModel.Add(name, code, typeString);
                    if (success > 0)
                    {
                        NameTextBox.Text = string.Empty;
                        CodeTextBox.Text = string.Empty;

                        WarningTextBlock.Visibility = Visibility.Visible;
                        WarningTextBlock.Text = "Subtype added successfully.";
                        WarningTextBlock.Foreground = new SolidColorBrush(Windows.UI.Colors.Green);
                    }
                    else
                    {
                        // something went wrong.
                        WarningTextBlock.Visibility = Visibility.Visible;
                        WarningTextBlock.Text = "Something went wrong!";
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
