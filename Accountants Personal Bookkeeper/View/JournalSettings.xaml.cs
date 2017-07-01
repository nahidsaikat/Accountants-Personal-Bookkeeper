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


namespace Accountants_Personal_Bookkeeper.View
{
    public sealed partial class JournalSettings : Page
    {
        public JournalSettings()
        {
            this.InitializeComponent();
            JournalTypeListFrame.Navigate(typeof(JournalTypeList));
            JournalTypeAddFrame.Navigate(typeof(JournalTypeAdd));
        }

        private void JournalSettingsPivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            JournalTypeListFrame.Navigate(typeof(JournalTypeList));
            JournalTypeAddFrame.Navigate(typeof(JournalTypeAdd));
        }
    }
}
