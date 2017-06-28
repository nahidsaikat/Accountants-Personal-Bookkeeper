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
    public sealed partial class PartyList : Page
    {

        List<Party> parties;
        SQLite.Net.SQLiteConnection conn;
        PartyViewModel viewModel;

        public PartyList()
        {
            this.InitializeComponent();
            conn = new Connection().GetConnection();
            viewModel = new PartyViewModel();
            parties = viewModel.PartyList();
        }

        private async void ViewPartyButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int id = (Int32)((Button)sender).Tag;
                var party = (from par in conn.Table<Party>()
                               where par.id == id
                               select par
                                  ).First<Party>();
                string message = "Name : " + party.name + "\nCode : " + party.code +
                    "\nSubtype : " + viewModel.GetSubtype(party.subtype_id).name +
                    "\nPhone : " + party.phone + "\nEmail : " + party.email + 
                    "\nAddress : " + party.address + "\nCompany : " + party.company_name + 
                    "\nOpening Balance : " + party.opening_balance.ToString() +
                    "\nOpening Date : " + party.entry_date.ToString();

                var dialog = new Windows.UI.Popups.MessageDialog(message, party.name);
                var result = await dialog.ShowAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Processor Usage" + ex.Message);
            }
        }

        private void EditPartyButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int id = (Int32)((Button)sender).Tag;
                var account = viewModel.Get(id);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Processor Usage" + ex.Message);
            }
        }
    }
}
