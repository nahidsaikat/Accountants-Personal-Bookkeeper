using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accountants_Personal_Bookkeeper.Model
{
    class AccountLedger
    {
        public int id { get; set; }
        public int account_id { get; set; }
        public string account_name { get; set; }
        public double balance { get; set; }
    }
}
