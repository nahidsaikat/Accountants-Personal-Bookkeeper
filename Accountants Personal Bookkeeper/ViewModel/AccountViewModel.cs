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
        SettingsViewModel settings;
        JournalTypeViewModel jtVM;
        JournalViewModel journalVM;

        public AccountViewModel()
        {
            conn = new Connection().GetConnection();
            conn.CreateTable<Account>();
            settings = new SettingsViewModel();
            jtVM = new JournalTypeViewModel();
            journalVM = new JournalViewModel();
        }

        public int Add(string name, string code, string typeString, string subtypeString,
            string parentString, string openingBalance, string openingDate, string note)
        {
            int add = -1;
            try
            {
                int type = int.Parse(typeString),  subtype = int.Parse(subtypeString), parent_id = int.Parse(parentString);
                Account account = new Account()
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
                };
                conn.Insert(account);
                add = account.id;
                if (Double.Parse(openingBalance) > 0.0)
                {
                    int flag = CreateJournal(account.id, openingDate, openingBalance, note);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                add = -1;
            }
            return add;
        }

        private int CreateJournal(int account_id, string journalDate, string openingBalance, string note)
        {
            Windows.Storage.ApplicationDataCompositeValue composite = settings.GetSettingsComposite();
            int income_id = int.Parse(composite["IncomeAccountId"].ToString());

            JournalType journalType = jtVM.GetHavingPrefix("IOJ");
            string typeString = journalType.id.ToString(), partyString = "-1", referenceString = "-1";
            double amount = double.Parse(openingBalance);
            Dictionary<int, double> accountInfo = new Dictionary<int, double>()
            {
                { account_id, amount },
                { income_id, amount*-1 }
            };
            return journalVM.Add(journalDate, typeString, partyString, referenceString, note, accountInfo, amount, amount);
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

        public double GetBalance(int account_id)
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
