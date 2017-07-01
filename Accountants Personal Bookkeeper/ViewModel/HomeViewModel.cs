using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accountants_Personal_Bookkeeper.Model;
using Accountants_Personal_Bookkeeper.View;
using Accountants_Personal_Bookkeeper.DB;

namespace Accountants_Personal_Bookkeeper.ViewModel
{
    public class Val
    {
        public int type { get; set; }
        public double total { get; set; }
    }

    class HomeViewModel
    {
        SQLite.Net.SQLiteConnection conn;
        AccountViewModel accountVM;
        AccountSubtypeViewModel subtypeVM;
        JournalViewModel journalVM;
        JournalTypeViewModel journalTypeVM;

        public HomeViewModel()
        {
            conn = new Connection().GetConnection();
            accountVM = new AccountViewModel();
            subtypeVM = new AccountSubtypeViewModel();
            journalVM = new JournalViewModel();
            journalTypeVM = new JournalTypeViewModel();
        }

        public List<Home> GetAccounts()
        {
            List<Account> accounts = accountVM.AccountList();
            int length = accounts.Count;
            List<Home> homes = new List<Home>();
            for (int index=0; index<5; index++)
            {
                if(index < length)
                {
                    homes.Add(new Home() {
                        name = accounts[index].name,
                        amount = int.Parse(accountVM.GetBalance(accounts[index].id).ToString())
                    });
                }
            }
            return homes;
        }

        public List<Home> GetAccountsByTypes()
        {
            List<Home> homes = new List<Home>();
            List<Account> accounts = accountVM.AccountList();
            double asset = 0.0, liability = 0.0, income = 0.0, expense = 0.0, equity = 0.0, balance = 0.0;
            foreach(Account account in accounts)
            {
                balance = accountVM.GetBalance(account.id);
                if (account.type == (int)AccountType.Assets)
                {
                    asset += balance;
                }
                else if (account.type == (int)AccountType.Liability)
                {
                    liability += balance;
                }
                else if (account.type == (int)AccountType.Expense)
                {
                    income += balance;
                }
                else if (account.type == (int)AccountType.Income)
                {
                    expense += balance;
                }
                else if (account.type == (int)AccountType.Equity)
                {
                    equity += balance;
                }
            }
            homes.Add(new Home() {
                name = "Assets",
                amount = int.Parse(asset.ToString())
            });
            homes.Add(new Home()
            {
                name = "Liability",
                amount = int.Parse(liability.ToString())
            });
            homes.Add(new Home()
            {
                name = "Expense",
                amount = int.Parse(expense.ToString())
            });
            homes.Add(new Home()
            {
                name = "Income",
                amount = int.Parse(income.ToString())
            });
            homes.Add(new Home()
            {
                name = "Equity",
                amount = int.Parse(equity.ToString())
            });

            return homes;
        }

        public List<Home> GetAccountsSubtypeWise()
        {
            List<Home> homes = new List<Home>();
            List<Account> accounts = accountVM.AccountList();
            double balance = 0.0;
            Dictionary<int, double> subtype = new Dictionary<int, double>();
            foreach (Account account in accounts)
            {
                balance = accountVM.GetBalance(account.id);
                try
                {
                    subtype[account.subtype] += balance;
                }
                catch (Exception e)
                {
                    subtype.Add(account.subtype, balance);
                }
            }
            foreach(KeyValuePair<int, double> pair in subtype)
            {
                string name = subtypeVM.Get(pair.Key).name;
                homes.Add(new Home() {
                    name = name,
                    amount = int.Parse(pair.Value.ToString())
                });
            }
            return homes;
        }

        public List<Home> GetTransactions()
        {
            List<Home> homes = new List<Home>();

            double amount = 0.0;
            DateTime today = DateTime.Today;
            DateTime yesterday = DateTime.Today.AddDays(-1);
            DateTime seventhDate = DateTime.Today.AddDays(-7);
            DateTime thirtythDate = DateTime.Today.AddDays(-30);

            List<Journal> journals = (from jour in conn.Table<Journal>()
                                        where jour.journal_date == today.Date
                                        select jour
                                        ).ToList<Journal>();
            foreach(Journal journal in journals)
            {
                amount += journal.amount;
            }
            homes.Add(new Home() {
                name = "Today",
                amount = int.Parse(amount.ToString())
            });

            amount = 0.0;
            journals = (from jour in conn.Table<Journal>()
                        where jour.journal_date == yesterday
                        select jour
                        ).ToList<Journal>();
            foreach (Journal journal in journals)
            {
                amount += journal.amount;
            }
            homes.Add(new Home()
            {
                name = "Yeaterday",
                amount = int.Parse(amount.ToString())
            });

            amount = 0.0;
            journals = (from jour in conn.Table<Journal>()
                        where jour.journal_date >= today.Date && jour.journal_date <= seventhDate.Date
                        select jour
                        ).ToList<Journal>();
            foreach (Journal journal in journals)
            {
                amount += journal.amount;
            }
            homes.Add(new Home()
            {
                name = "Last Week",
                amount = int.Parse(amount.ToString())
            });

            amount = 0.0;
            journals = (from jour in conn.Table<Journal>()
                        where jour.journal_date >= today.Date && jour.journal_date <= thirtythDate.Date
                        select jour
                        ).ToList<Journal>();
            foreach (Journal journal in journals)
            {
                amount += journal.amount;
            }
            homes.Add(new Home()
            {
                name = "Last Month",
                amount = int.Parse(amount.ToString())
            });

            return homes;
        }

        public List<Home> GetTransactionsTypeWise()
        {
            List<Home> homes = new List<Home>();
            string query = "SELECT type, SUM(amount) AS total FROM Journal GROUP BY type";
            string name = "";
            var journals = conn.Query<Val>(query).ToList<Val>();
            foreach(var val in journals)
            {
                name = journalTypeVM.Get(val.type).name;
                homes.Add(new Home() {
                    name = name,
                    amount = int.Parse(val.total.ToString())
                });
            }

            return homes;
        }
    }
}
