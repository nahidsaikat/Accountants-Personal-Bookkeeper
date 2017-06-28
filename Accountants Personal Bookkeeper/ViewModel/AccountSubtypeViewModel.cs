using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Accountants_Personal_Bookkeeper.Model;
using Accountants_Personal_Bookkeeper.DB;

namespace Accountants_Personal_Bookkeeper.ViewModel
{
    class AccountSubtypeViewModel
    {
        SQLite.Net.SQLiteConnection conn;

        public AccountSubtypeViewModel()
        {
            conn = new Connection().GetConnection();
            conn.CreateTable<AccountSubtype>();
        }

        public bool Add(string name, string code, string typeString)
        {
            bool success = false;
            try
            {
                int type = int.Parse(typeString);
                var add = conn.Insert(new AccountSubtype()
                {
                    name = name,
                    code = code,
                    type = type,
                    deleted = 0
                });
                success = true;
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.ToString());
                success = false;
            }
            return success;
        }

        public List<AccountSubtype> AccountSubtypeList()
        {
            List<AccountSubtype> accountList = new List<AccountSubtype>();
            foreach(var subtype in conn.Table<AccountSubtype>())
            {
                accountList.Add(subtype);
            }
            return accountList;
        }

        public AccountSubtype Get(int id)
        {
            AccountSubtype subtype = (from sub in conn.Table<AccountSubtype>()
                                      where sub.id == id
                                      select sub
                                  ).First<AccountSubtype>();
            return subtype;
        }
    }
}
