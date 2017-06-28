using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net.Attributes;

namespace Accountants_Personal_Bookkeeper.Model
{
    enum AccountType { Assets, Liability, Expense, Income, Equity };

    class Account
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public int type { get; set; }
        public int subtype { get; set; }
        public int parent_id { get; set; }
        public int depth { get; set; }
        public DateTime entry_date { get; set; }
        public string note { get; set; }
        public double opening_balance { get; set; }
        public int deleted { get; set; }
    }
}
