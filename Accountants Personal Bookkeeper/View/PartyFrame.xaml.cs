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
    public sealed partial class PartyFrame : Page
    {

        SQLite.Net.SQLiteConnection conn;

        public PartyFrame()
        {
            this.InitializeComponent();
            conn = new Connection().GetConnection();

            PartyListFrame.Navigate(typeof(PartyList));
            PartyLedgerFrame.Navigate(typeof(LedgerParty));
            PartyAddFrame.Navigate(typeof(PartyAdd));
            PartySettingsFrame.Navigate(typeof(PartySettings));
        }

        private void PartyPivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PartyListFrame.Navigate(typeof(PartyList));
            PartyLedgerFrame.Navigate(typeof(LedgerParty));
            PartyAddFrame.Navigate(typeof(PartyAdd));
            PartySettingsFrame.Navigate(typeof(PartySettings));
        }
    }
}
