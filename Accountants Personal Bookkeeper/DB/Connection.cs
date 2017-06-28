using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace Accountants_Personal_Bookkeeper.DB
{
    class Connection
    {
        string path;
        SQLite.Net.SQLiteConnection conn;

        public SQLite.Net.SQLiteConnection GetConnection()
        {
            path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path,
               "db.sqlite");
            Debug.WriteLine("**************Database Path**************");
            Debug.WriteLine(path);
            conn = new SQLite.Net.SQLiteConnection(new
               SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), path);
            //conn.CreateTable<CustomerModel>();
            return conn;
        }
    }
}
