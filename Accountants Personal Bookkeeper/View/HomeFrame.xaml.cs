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
using WinRTXamlToolkit.Controls.DataVisualization.Charting;
using Accountants_Personal_Bookkeeper.Model;
using Accountants_Personal_Bookkeeper.ViewModel;


namespace Accountants_Personal_Bookkeeper.View
{

    public class FinancialStuff
    {
        public string Name { get; set; }
        public int Amount { get; set; }
    }

    public sealed partial class HomeFrame : Page
    {
        HomeViewModel viewModel;

        public HomeFrame()
        {
            this.InitializeComponent();
            viewModel = new HomeViewModel();
            LoadChartContents();
        }

        private void LoadChartContents()
        {
            Random rand = new Random();
            List<FinancialStuff> financialStuffList = new List<FinancialStuff>();
            financialStuffList.Add(new FinancialStuff() { Name = "MSFT", Amount = rand.Next(0, 200) });
            financialStuffList.Add(new FinancialStuff() { Name = "AAPL", Amount = rand.Next(0, 200) });
            financialStuffList.Add(new FinancialStuff() { Name = "GOOG", Amount = rand.Next(0, 200) });
            financialStuffList.Add(new FinancialStuff() { Name = "BBRY", Amount = rand.Next(0, 200) });

            (ColumnChart.Series[0] as ColumnSeries).ItemsSource = viewModel.GetAccounts();
            (PieChart.Series[0] as PieSeries).ItemsSource = viewModel.GetAccountsByTypes();
            (BarChart.Series[0] as BarSeries).ItemsSource = viewModel.GetAccountsSubtypeWise();
            (LineChart.Series[0] as LineSeries).ItemsSource = viewModel.GetTransactions();
            (BubbleChart.Series[0] as BubbleSeries).ItemsSource = viewModel.GetTransactionsTypeWise();
        }

        private void ButtonRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadChartContents();
        }
    }
}
