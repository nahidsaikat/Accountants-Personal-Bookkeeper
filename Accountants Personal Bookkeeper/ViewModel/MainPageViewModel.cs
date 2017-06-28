using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Accountants_Personal_Bookkeeper.DB;

namespace Accountants_Personal_Bookkeeper.ViewModel
{
    
    class MainPageViewModel
    {
        SQLite.Net.SQLiteConnection conn;

        public MainPageViewModel()
        {
            conn = new Connection().GetConnection();
        }
    }
}
