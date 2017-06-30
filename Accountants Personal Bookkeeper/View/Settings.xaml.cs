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
        SettingsViewModel settings;
        public Settings()
        {
            this.InitializeComponent();
            settings = new SettingsViewModel();

            PageLoad();
        }

        private void PageLoad()
        {
            Windows.Storage.ApplicationDataCompositeValue composite = settings.GetSettingsComposite();
            NumberOfAccountInJournalTextBox.Text = composite["NumberOfAccountInJournal"].ToString();
        }

        private void SettingsSaveButton_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<string, string> settingData = new Dictionary<string, string>();
            string numberOfAccount = NumberOfAccountInJournalTextBox.Text;
            settingData.Add("NumberOfAccountInJournal", numberOfAccount);
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
