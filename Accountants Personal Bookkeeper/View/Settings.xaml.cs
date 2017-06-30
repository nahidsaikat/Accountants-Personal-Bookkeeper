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
    public sealed partial class Settings : Page
    {
        Windows.Storage.ApplicationDataCompositeValue composite;
        SettingsViewModel settings;
        AccountViewModel accountVM;

        public Settings()
        {
            this.InitializeComponent();
            settings = new SettingsViewModel();
            composite = settings.GetSettingsComposite();
            accountVM = new AccountViewModel();

            PageLoad();
        }

        private void PageLoad()
        {
            NumberOfAccountInJournalTextBox.Text = composite["NumberOfAccountInJournal"].ToString();
            LoadAccountReceivableIdCombo();
            LoadIncomeAccountIdCombo();
        }

        private void LoadAccountReceivableIdCombo()
        {
            foreach (var account in accountVM.AccountList())
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Name = account.id.ToString();
                item.Content = account.name.ToString();
                if (composite["AccountReceivableId"] != null && composite["AccountReceivableId"].ToString() != "-1"
                    && composite["AccountReceivableId"].ToString() == account.id.ToString())
                {
                    item.IsSelected = true;
                }
                AccountReceivableIdComboBox.Items.Add(item);
            }
        }

        private void LoadIncomeAccountIdCombo()
        {
            foreach (var account in accountVM.AccountList())
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Name = account.id.ToString();
                item.Content = account.name.ToString();
                if (composite["IncomeAccountId"] != null && composite["IncomeAccountId"].ToString() != "-1"
                    && composite["IncomeAccountId"].ToString() == account.id.ToString())
                {
                    item.IsSelected = true;
                }
                IncomeAccountIdComboBox.Items.Add(item);
            }
        }

        private void SettingsSaveButton_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<string, string> settingData = new Dictionary<string, string>();

            string numberOfAccount = NumberOfAccountInJournalTextBox.Text;
            settingData.Add("NumberOfAccountInJournal", numberOfAccount);

            string accountReceivableId = "-1";
            if (AccountReceivableIdComboBox.SelectedValue != null)
            {
                accountReceivableId = (AccountReceivableIdComboBox.SelectedValue as ComboBoxItem).Name.ToString();
            }
            settingData.Add("AccountReceivableId", accountReceivableId);

            string incomeAccountId = "-1";
            if (IncomeAccountIdComboBox.SelectedValue != null)
            {
                incomeAccountId = (IncomeAccountIdComboBox.SelectedValue as ComboBoxItem).Name.ToString();
            }
            settingData.Add("IncomeAccountId", incomeAccountId);

            bool success = settings.SaveSettingsComposite(settingData);
            if (success)
            {
                WarningTextBlock.Visibility = Visibility.Visible;
                WarningTextBlock.Text = "Settings Saved Successfully.";
                WarningTextBlock.Foreground = new SolidColorBrush(Windows.UI.Colors.Green);
            }
            else
            {
                WarningTextBlock.Visibility = Visibility.Visible;
                WarningTextBlock.Text = "Something Went Wrong.";
                WarningTextBlock.Foreground = new SolidColorBrush(Windows.UI.Colors.Red);
            }
        }
    }
}
