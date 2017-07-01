﻿using System;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Accountants_Personal_Bookkeeper.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class JournalFrame : Page
    {
        public JournalFrame()
        {
            this.InitializeComponent();
            JournalListThisFrame.Navigate(typeof(JournalListThis));
            JournalListLastFrame.Navigate(typeof(JournalListLast));
            JournalListFrame.Navigate(typeof(JournalList));
            JournalAddFrame.Navigate(typeof(JournalAdd));
            JournalSettingsFrame.Navigate(typeof(JournalSettings));
        }

        private void JournalPivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            JournalListThisFrame.Navigate(typeof(JournalListThis));
            JournalListLastFrame.Navigate(typeof(JournalListLast));
            JournalListFrame.Navigate(typeof(JournalList));
            JournalAddFrame.Navigate(typeof(JournalAdd));
            JournalSettingsFrame.Navigate(typeof(JournalSettings));
        }
    }
}
