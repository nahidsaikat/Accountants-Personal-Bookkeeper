using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Accountants_Personal_Bookkeeper.DB;
using Accountants_Personal_Bookkeeper.Model;

namespace Accountants_Personal_Bookkeeper.ViewModel
{
    class PartySubtypeViewModel
    {
        SQLite.Net.SQLiteConnection conn;

        public PartySubtypeViewModel()
        {
            conn = new Connection().GetConnection();
            conn.CreateTable<PartySubtype>();
        }

        public bool Add(string name, string code)
        {
            bool success = false;
            try
            {
                var add = conn.Insert(new PartySubtype()
                {
                    name = name,
                    code = code,
                    deleted = 0
                });
                success = true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                success = false;
            }
            return success;
        }

        public List<PartySubtype> PartySubtypeList()
        {
            List<PartySubtype> accountList = new List<PartySubtype>();
            foreach (var subtype in conn.Table<PartySubtype>())
            {
                accountList.Add(subtype);
            }
            return accountList;
        }

        public PartySubtype Get(int id)
        {
            PartySubtype subtype = (from sub in conn.Table<PartySubtype>()
                                      where sub.id == id
                                      select sub
                                  ).First<PartySubtype>();
            return subtype;
        }
    }
}
