using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net.Attributes;

namespace Accountants_Personal_Bookkeeper.Model
{
    class JournalType
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string name { get; set; }
        public string prefix { get; set; }
        public string start_number { get; set; }
        public int debit_account_id { get; set; }
        public int credit_account_id { get; set; }
        public int deleted { get; set; }
    }
}
