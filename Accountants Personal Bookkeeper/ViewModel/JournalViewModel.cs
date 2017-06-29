using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accountants_Personal_Bookkeeper.DB;
using Accountants_Personal_Bookkeeper.Model;

namespace Accountants_Personal_Bookkeeper.ViewModel
{
    class JournalViewModel
    {

        SQLite.Net.SQLiteConnection conn;

        public JournalViewModel()
        {
            conn = new Connection().GetConnection();
            conn.CreateTable<Journal>();
        }
    }
}
