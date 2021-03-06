﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net.Attributes;

namespace Accountants_Personal_Bookkeeper.Model
{
    class Journal
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string number { get; set; }
        public DateTime journal_date { get; set; }
        public double amount { get; set; }
        public int type { get; set; }
        public int party_id { get; set; }
        public int ref_journal_id { get; set; }
        public string description { get; set; }
        public string account_info { get; set; }
        public int deleted { get; set; }
    }
}
