using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net.Attributes;

namespace Accountants_Personal_Bookkeeper.Model
{
    class Party
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public int subtype_id { get; set; }
        public string phone { get; set; }
        public string address { get; set; }
        public string email { get; set; }
        public string company_name { get; set; }
        public DateTime entry_date { get; set; }
        public double opening_balance { get; set; }
        public int account_id { get; set; }
        public int deleted { get; set; }
    }
}
