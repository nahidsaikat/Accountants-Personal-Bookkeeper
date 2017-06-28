using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net.Attributes;

namespace Accountants_Personal_Bookkeeper.Model
{
    class AccountSubtype
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public int type { get; set; }
        public int deleted { get; set; }
    }
}
