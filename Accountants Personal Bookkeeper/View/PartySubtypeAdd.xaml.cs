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
    public sealed partial class PartySubtypeAdd : Page
    {
        SQLite.Net.SQLiteConnection conn;
        PartySubtypeViewModel viewModel;

        public PartySubtypeAdd()
        {
            this.InitializeComponent();
            conn = new Connection().GetConnection();
            conn.CreateTable<PartySubtype>();
            viewModel = new PartySubtypeViewModel();
        }

        private void PartySubtypeSaveButton_Click(object sender, RoutedEventArgs e)
        {
            string name = NameTextBox.Text;
            string code = CodeTextBox.Text;

            // Checking empty value
            if (name == string.Empty || code == string.Empty )
            {
                WarningTextBlock.Visibility = Visibility.Visible;
                WarningTextBlock.Text = "Please fill up all the field.";
                WarningTextBlock.Foreground = new SolidColorBrush(Windows.UI.Colors.Red);
            }
            else
            {
                // Checking duplicate value
                bool same = false;
                var querySubtype = conn.Table<PartySubtype>();
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
                    int success = viewModel.Add(name, code);
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
