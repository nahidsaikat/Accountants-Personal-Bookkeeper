using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accountants_Personal_Bookkeeper.Model
{
    class PartyLedger
    {
        public int id { get; set; }
        public int party_id { get; set; }
        public string party_name { get; set; }
        public double balance { get; set; }
    }
}
