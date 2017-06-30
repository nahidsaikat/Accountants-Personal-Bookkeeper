using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Accountants_Personal_Bookkeeper.DB;
using Accountants_Personal_Bookkeeper.Model;
using Accountants_Personal_Bookkeeper.ViewModel;

namespace Accountants_Personal_Bookkeeper.ViewModel
{
    class AccountViewModel
    {
        SQLite.Net.SQLiteConnection conn;

        public AccountViewModel()
        {
            conn = new Connection().GetConnection();
            conn.CreateTable<Account>();
        }

        public bool Add(string name, string code, string typeString, string subtypeString,
            string parentString, string openingBalance, string openingDate, string note)
        {
            bool success = false;
            try
            {
                int type = int.Parse(typeString),  subtype = int.Parse(subtypeString), parent_id = int.Parse(parentString);
                var add = conn.Insert(new Account()
                {
                    name = name,
                    code = code,
                    type = type,
                    subtype = subtype,
                    parent_id = parent_id,
                    depth = 0,
                    entry_date = DateTime.Parse(openingDate),
                    note = note,
                    opening_balance = Double.Parse(openingBalance),
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

        public List<Account> AccountList()
        {
            List<Account> accountList = new List<Account>();
            foreach (var account in conn.Table<Account>())
            {
                accountList.Add(account);
            }
            return accountList;
        }

        public Account Get(int id)
        {
            Account account = (from acc in conn.Table<Account>()
                                      where acc.id == id
                                      select acc
                                  ).First<Account>();
            return account;
        }

        public AccountSubtype GetSubtype(int subtype_id)
        {
            AccountSubtypeViewModel subtypevM = new AccountSubtypeViewModel();
            return subtypevM.Get(subtype_id);
        }

        public List<AccountLedger> AccountLedgerList()
        {
            List<AccountLedger> ledgers = new List<AccountLedger>();
            int counter = 0;
            foreach(Account account in AccountList())
            {
                ledgers.Add(new AccountLedger()
                {
                    id = ++counter,
                    account_id = account.id,
                    account_name = account.name,
                    balance = GetBalance(account.id)
                });
            }
            return ledgers;
        }

        private double GetBalance(int account_id)
        {
            List<Ledger> allLedgers = (from ledger in conn.Table<Ledger>()
                                         where ledger.account_id == account_id
                                         select ledger
                                         ).ToList<Ledger>();
            double balance = allLedgers.Sum(ledger => ledger.amount);
            return balance;
        }
    }
}
