using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net.Attributes;

namespace Accountants_Personal_Bookkeeper.Model
{
    class NumberGenerator
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string prefix { get; set; }
        public int value { get; set; }
    }
}
