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
    public sealed partial class PartyAdd : Page
    {
        SQLite.Net.SQLiteConnection conn;
        PartyViewModel viewModel;
        PartySubtypeViewModel subtypeViewModel;

        public PartyAdd()
        {
            this.InitializeComponent();
            conn = new Connection().GetConnection();
            conn.CreateTable<Party>();
            viewModel = new PartyViewModel();
            subtypeViewModel = new PartySubtypeViewModel();

            PageLoad();
        }

        private void PageLoad()
        {
            LoadSubtypeCombo();
        }

        private void LoadSubtypeCombo()
        {
            foreach (var subtype in subtypeViewModel.PartySubtypeList())
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Name = subtype.id.ToString();
                item.Content = subtype.name.ToString();
                SubtypeComboBox.Items.Add(item);
            }
        }

        private void PartySaveButton_Click(object sender, RoutedEventArgs e)
        {
            string name = NameTextBox.Text;
            string code = CodeTextBox.Text;
            string subtypeString = "-1";
            string phone = PhoneTextBox.Text;
            string email = EmailTextBox.Text;
            string address = AddressTextBox.Text;
            string company_name = CompanyNameTextBox.Text;
            string entry_date = EntryDateCalendarDatePicker.Date.ToString();
            string opening_balance = OpeningBalanceTextBox.Text;
            
            if (SubtypeComboBox.SelectedValue != null)
            {
                subtypeString = (SubtypeComboBox.SelectedValue as ComboBoxItem).Name.ToString();
            }

            // Checking empty value
            if (name == string.Empty || code == string.Empty || phone == string.Empty || code == string.Empty ||
                subtypeString == string.Empty || opening_balance == string.Empty ||
                entry_date == string.Empty)
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
                    bool flag = viewModel.Add(name, code, subtypeString, phone, email, address, 
                        company_name, entry_date, opening_balance);
                    if (flag)
                    {
                        NameTextBox.Text = string.Empty;
                        CodeTextBox.Text = string.Empty;
                        PhoneTextBox.Text = string.Empty;
                        EmailTextBox.Text = string.Empty;
                        AddressTextBox.Text = string.Empty;
                        CompanyNameTextBox.Text = string.Empty;
                        OpeningBalanceTextBox.Text = string.Empty;

                        WarningTextBlock.Visibility = Visibility.Visible;
                        WarningTextBlock.Text = "Party added successfully.";
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
                    WarningTextBlock.Text = "Name or Code is used.";
                    WarningTextBlock.Foreground = new SolidColorBrush(Windows.UI.Colors.Red);
                }
            }
        }

        private bool IsUnique(string name, string code)
        {
            // Checking duplicate entry
            bool unique = true;
            var query = conn.Table<Party>();
            foreach (var party in query)
            {
                if (party.name == name || party.code == code)
                {
                    unique = false;
                    break;
                }
            }
            return unique;
        }
    }
}
