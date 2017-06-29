using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net.Attributes;

namespace Accountants_Personal_Bookkeeper.Model
{
    class Ledger
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public int journal_id { get; set; }
        public int account_id { get; set; }
        public double amount { get; set; }
        public int party_id { get; set; }
        public DateTime entry_date { get; set; }
        public string description { get; set; }
        public string other_accounts { get; set; }
        public int deleted { get; set; }
    }
}
