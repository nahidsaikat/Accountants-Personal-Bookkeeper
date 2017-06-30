using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Accountants_Personal_Bookkeeper.Model;
using Accountants_Personal_Bookkeeper.DB;

namespace Accountants_Personal_Bookkeeper.ViewModel
{
    class JournalTypeViewModel
    {
        SQLite.Net.SQLiteConnection conn;

        public JournalTypeViewModel()
        {
            conn = new Connection().GetConnection();
            conn.CreateTable<JournalType>();
            CreateInternalOpeningJournalType();
        }

        private void CreateInternalOpeningJournalType()
        {
            bool already = (from sub in conn.Table<JournalType>()
                                where sub.prefix == "IOJ"
                                select sub
                            ).Any<JournalType>();
            if (!already)
            {
                Add("Internal Opening Journal", "IOJ", "1000", "-1", "-1");
            }
        }

        public int Add(string name, string prefix, string start_number,
            string debit_account_id_str, string credit_account_id_str)
        {
            int add = -1;
            try
            {
                int debit_account_id = int.Parse(debit_account_id_str),
                    credit_account_id = int.Parse(credit_account_id_str);
                JournalType type = new JournalType()
                {
                    name = name,
                    prefix = prefix,
                    start_number = start_number,
                    debit_account_id = debit_account_id,
                    credit_account_id = credit_account_id,
                    deleted = 0
                };
                conn.Insert(type);
                add = type.id;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                add = -1;
            }
            return add;
        }

        public List<JournalType> JournalTypeList()
        {
            List<JournalType> typeList = new List<JournalType>();
            foreach (var subtype in conn.Table<JournalType>())
            {
                typeList.Add(subtype);
            }
            return typeList;
        }

        public JournalType Get(int id)
        {
            JournalType type = (from sub in conn.Table<JournalType>()
                                      where sub.id == id
                                      select sub
                                  ).First<JournalType>();
            return type;
        }

        public JournalType GetHavingPrefix(string prefix)
        {
            JournalType type = (from sub in conn.Table<JournalType>()
                                where sub.prefix == prefix
                                select sub
                                  ).First<JournalType>();
            return type;
        }
    }
}
