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
using Newtonsoft.Json;
using System.Diagnostics;
using Accountants_Personal_Bookkeeper.DB;
using Accountants_Personal_Bookkeeper.Model;
using Accountants_Personal_Bookkeeper.ViewModel;


namespace Accountants_Personal_Bookkeeper.View
{
    public sealed partial class JournalAdd : Page
    {
        SQLite.Net.SQLiteConnection conn;
        JournalViewModel viewModel;
        AccountViewModel accountVM;
        PartyViewModel partyVM;
        JournalTypeViewModel typeVM;
        SettingsViewModel settings;

        public JournalAdd()
        {
            this.InitializeComponent();
            conn = new Connection().GetConnection();
            conn.CreateTable<Journal>();
            viewModel = new JournalViewModel();
            accountVM = new AccountViewModel();
            partyVM = new PartyViewModel();
            typeVM = new JournalTypeViewModel();
            settings = new SettingsViewModel();

            LoadPage();


            // Testing JSON Operation
            Dictionary<int, int> data = new Dictionary<int, int>()
            {
                { 1, 2 },
                { 3, 4 }
            };
            string abc = JsonConvert.SerializeObject(data, Formatting.Indented);
            Debug.WriteLine("###################################################");
            Debug.WriteLine(abc.ToString());

            Dictionary<int, int> returnData = JsonConvert.DeserializeObject<Dictionary<int, int>>(abc);
            Debug.WriteLine("###################################################");
            foreach(KeyValuePair<int, int> acc in returnData)
            {
                Debug.WriteLine("Key = {0}, Value = {1}", acc.Key, acc.Value);
            }

        }

        public void LoadPage()
        {
            LoadAccountGrid();
            LoadTypeCombo();
            LoadPartyCombo();
            LoadReferenceCombo();
        }

        private void LoadReferenceCombo()
        {
            foreach (var journal in viewModel.JournalList())
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Name = journal.id.ToString();
                item.Content = journal.number.ToString();
                ReferenceComboBox.Items.Add(item);
            }
        }

        private void LoadTypeCombo()
        {
            foreach (var type in typeVM.JournalTypeList())
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Name = type.id.ToString();
                item.Content = type.name.ToString();
                TypeComboBox.Items.Add(item);
            }
        }

        private void LoadPartyCombo()
        {
            foreach (var party in partyVM.PartyList())
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Name = party.id.ToString();
                item.Content = party.name.ToString();
                PartyComboBox.Items.Add(item);
            }
        }

        private void LoadAccountGrid()
        {
            Windows.Storage.ApplicationDataCompositeValue composite = settings.GetSettingsComposite();
            int length = int.Parse(composite["NumberOfAccountInJournal"].ToString());

            for (int counter=0; counter<length; counter++)
            {
                StackPanel stackpanel = new StackPanel();
                stackpanel.Name = "StackPanel" + counter.ToString();
                stackpanel.Orientation = Orientation.Horizontal;
                stackpanel.Margin = new Thickness(5, 0, 0, 0);

                ComboBox AccountComboBox = new ComboBox();
                AccountComboBox.Name = "ComboBox" + counter.ToString();
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
                DebitTextBox.Name = "DebitTextBox" + counter.ToString();
                DebitTextBox.PlaceholderText = "Debit";
                DebitTextBox.FontSize = 18;
                DebitTextBox.Width = 100;
                DebitTextBox.Margin = new Thickness(5, 0, 0, 0);
                stackpanel.Children.Add(DebitTextBox);

                TextBox CreditTextBox = new TextBox();
                CreditTextBox.Name = "CreditTextBox" + counter.ToString();
                CreditTextBox.PlaceholderText = "Credit";
                CreditTextBox.FontSize = 18;
                CreditTextBox.Width = 100;
                CreditTextBox.Margin = new Thickness(5, 0, 0, 0);
                stackpanel.Children.Add(CreditTextBox);

                AccountInfoStackPanel.Children.Add(stackpanel);
            }
        }

        private void JournalSaveButton_Click(object sender, RoutedEventArgs e)
        {
            string journalDate = JournalDateCalendarDatePicker.Date.ToString();
            string typeString = "-1";
            string partyString = "-1";
            string referenceString = "-1";
            string note = DescriptionTextBox.Text;
            
            if (TypeComboBox.SelectedValue != null)
            {
                typeString = (TypeComboBox.SelectedValue as ComboBoxItem).Name.ToString();
            }
            if (PartyComboBox.SelectedValue != null)
            {
                partyString = (PartyComboBox.SelectedValue as ComboBoxItem).Name.ToString();
            }
            if (ReferenceComboBox.SelectedValue != null)
            {
                referenceString = (ReferenceComboBox.SelectedValue as ComboBoxItem).Name.ToString();
            }

            // Get ledger data
            Dictionary<int, double> accountInfo = new Dictionary<int, double>();
            double totalDebit = 0.0, totalCredit = 0.0;
            Windows.Storage.ApplicationDataCompositeValue composite = settings.GetSettingsComposite();
            int length = int.Parse(composite["NumberOfAccountInJournal"].ToString());

            for (int i=0; i<length; i++)
            {
                ComboBox AccountComboBox = (this.AccountInfoStackPanel.Children[i] as StackPanel).Children[0] as ComboBox;
                TextBox DebitTextBox = (this.AccountInfoStackPanel.Children[i] as StackPanel).Children[1] as TextBox;
                TextBox CreditTextBox = (this.AccountInfoStackPanel.Children[i] as StackPanel).Children[2] as TextBox;
                if (AccountComboBox.SelectedValue != null && (DebitTextBox.Text != string.Empty || CreditTextBox.Text != string.Empty))
                {
                    var account_id = (AccountComboBox.SelectedValue as ComboBoxItem).Name.ToString();
                    double amount = 0.0;
                    if (DebitTextBox.Text != string.Empty)
                    {
                        amount = double.Parse(DebitTextBox.Text);
                        totalDebit += amount;
                    }
                    if (CreditTextBox.Text != string.Empty)
                    {
                        amount = double.Parse(CreditTextBox.Text);
                        totalCredit += amount;
                        amount *= -1;
                    }

                    accountInfo.Add(int.Parse(account_id), amount);
                }
            }

            // Checking empty value
            if ( journalDate == string.Empty || typeString == string.Empty || partyString == string.Empty ||
                referenceString == string.Empty || note == string.Empty )
            {
                WarningTextBlock.Visibility = Visibility.Visible;
                WarningTextBlock.Text = "Please fill up all the field.";
                WarningTextBlock.Foreground = new SolidColorBrush(Windows.UI.Colors.Red);
            }
            else
            {
                // Insert in database as it is not already in database
                string message = viewModel.Add(journalDate, typeString, partyString, referenceString, note, accountInfo, totalDebit, totalCredit);
                if (message == "Success")
                {
                    DescriptionTextBox.Text = string.Empty;
                    JournalDateCalendarDatePicker.Date = null;
                    JournalDateCalendarDatePicker.PlaceholderText = "Enter Journal date";

                    WarningTextBlock.Visibility = Visibility.Visible;
                    WarningTextBlock.Text = "Journal added successfully.";
                    WarningTextBlock.Foreground = new SolidColorBrush(Windows.UI.Colors.Green);
                }
                else
                {
                    // something went wrong.
                    WarningTextBlock.Visibility = Visibility.Visible;
                    WarningTextBlock.Text = message;
                    WarningTextBlock.Foreground = new SolidColorBrush(Windows.UI.Colors.Red);
                }
            }
        }
    }
}
