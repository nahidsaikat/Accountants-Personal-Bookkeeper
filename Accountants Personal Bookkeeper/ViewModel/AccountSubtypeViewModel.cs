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
    class AccountSubtypeViewModel
    {
        SQLite.Net.SQLiteConnection conn;

        public AccountSubtypeViewModel()
        {
            conn = new Connection().GetConnection();
            conn.CreateTable<AccountSubtype>();
        }

        public int Add(string name, string code, string typeString)
        {
            int add = -1;
            try
            {
                int type = int.Parse(typeString);
                AccountSubtype subtype = new AccountSubtype()
                {
                    name = name,
                    code = code,
                    type = type,
                    deleted = 0
                };
                conn.Insert(subtype);
                add = subtype.id;
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.ToString());
                add = -1;
            }
            return add;
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
