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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Accountants_Personal_Bookkeeper.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PartySubtypeList : Page
    {
        List<PartySubtype> partySubtype;
        SQLite.Net.SQLiteConnection conn;

        public PartySubtypeList()
        {
            this.InitializeComponent();
            conn = new Connection().GetConnection();
            PartySubtypeViewModel viewModel = new PartySubtypeViewModel();
            partySubtype = viewModel.PartySubtypeList();
        }

        private void EditPartySubtypeButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int id = (Int32)((Button)sender).Tag;
                var accountSubtype = (from sub in conn.Table<PartySubtype>()
                                      where sub.id == id
                                      select sub
                                  ).First<PartySubtype>();

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Processor Usage" + ex.Message);
            }
        }

        private async void ViewPartySubtypeButton_ClickAsync(object sender, RoutedEventArgs e)
        {
            try
            {
                int id = (Int32)((Button)sender).Tag;
                var accountSubtype = (from sub in conn.Table<PartySubtype>()
                                      where sub.id == id
                                      select sub
                                  ).First<PartySubtype>();
                string message = "Name : " + accountSubtype.name + "\nCode : " + accountSubtype.code;

                var dialog = new Windows.UI.Popups.MessageDialog(message, accountSubtype.name);
                var result = await dialog.ShowAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Processor Usage" + ex.Message);
            }
        }
    }
}
