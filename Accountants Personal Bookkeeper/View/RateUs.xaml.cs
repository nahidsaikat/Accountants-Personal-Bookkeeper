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
using Windows.ApplicationModel;
using Windows.UI.Popups;
using Windows.System;

namespace Accountants_Personal_Bookkeeper.View
{
    public sealed partial class RateUs : Page
    {
        public RateUs()
        {
            this.InitializeComponent();
        }

        private async void RateUsButton_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog RateDialog = new MessageDialog("Would you like to rate this app?");
            RateDialog.Commands.Add(new UICommand("Rate now", async (command) =>
            {
                await Launcher.LaunchUriAsync( new Uri($"ms-windows-store://review/?PFN={Package.Current.Id.FamilyName}") );
            }));
            RateDialog.Commands.Add(new UICommand("Not now"));
            RateDialog.DefaultCommandIndex = 0;
            RateDialog.CancelCommandIndex = 1;
            await RateDialog.ShowAsync();
        }
    }
}
