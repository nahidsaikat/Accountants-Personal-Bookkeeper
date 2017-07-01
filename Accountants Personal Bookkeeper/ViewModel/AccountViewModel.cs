﻿using System;
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
            bool asset = true, liability = true, expense = true, income = true, equity = true;
            foreach (Account account in conn.Table<Account>().OrderBy(acc => acc.type))
            {
                if (account.type == (int)AccountType.Assets && asset)
                {
                    ledgers.Add(new AccountLedger()
                    {
                        id = ++counter,
                        account_id = -1,
                        account_name = "ASSET ACCOUNTS"
                    });
                    asset = false;
                }
                else if (account.type == (int)AccountType.Liability && liability)
                {
                    ledgers.Add(new AccountLedger()
                    {
                        id = ++counter,
                        account_id = -1,
                        account_name = "LIABILITY ACCOUNTS"
                    });
                    liability = false;
                }
                else if (account.type == (int)AccountType.Expense && expense)
                {
                    ledgers.Add(new AccountLedger()
                    {
                        id = ++counter,
                        account_id = -1,
                        account_name = "EXPENSE ACCOUNTS"
                    });
                    expense = false;
                }
                else if (account.type == (int)AccountType.Income && income)
                {
                    ledgers.Add(new AccountLedger()
                    {
                        id = ++counter,
                        account_id = -1,
                        account_name = "INCOME ACCOUNTS"
                    });
                    income = false;
                }
                else if (account.type == (int)AccountType.Equity && equity)
                {
                    ledgers.Add(new AccountLedger()
                    {
                        id = ++counter,
                        account_id = -1,
                        account_name = "EQUITY ACCOUNTS"
                    });
                    equity = false;
                }

                ledgers.Add(new AccountLedger()
                {
                    id = ++counter,
                    account_id = account.id,
                    account_name = "    " + account.name,
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

        public List<Account> AccountChart()
        {
            List<Account> accountNameList = new List<Account>();
            bool asset = true, liability = true, expense = true, income = true, equity = true;
            foreach (var account in conn.Table<Account>().OrderBy( acc => acc.type ))
            {
                if (account.type == (int)AccountType.Assets && asset)
                {
                    accountNameList.Add(new Account() {
                        id = -1,
                        name = "ASSET ACCOUNTS"
                    });
                    asset = false;
                }
                else if (account.type == (int)AccountType.Liability && liability)
                {
                    accountNameList.Add(new Account()
                    {
                        id = -1,
                        name = "LIABILITY ACCOUNTS"
                    });
                    liability = false;
                }
                else if (account.type == (int)AccountType.Expense && expense)
                {
                    accountNameList.Add(new Account()
                    {
                        id = -1,
                        name = "EXPENSE ACCOUNTS"
                    });
                    expense = false;
                }
                else if (account.type == (int)AccountType.Income && income)
                {
                    accountNameList.Add(new Account()
                    {
                        id = -1,
                        name = "INCOME ACCOUNTS"
                    });
                    income = false;
                }
                else if (account.type == (int)AccountType.Equity && equity)
                {
                    accountNameList.Add(new Account()
                    {
                        id = -1,
                        name = "EQUITY ACCOUNTS"
                    });
                    equity = false;
                }

                accountNameList.Add(new Account() {
                    id = account.id,
                    name = "    " + account.name
                });
            }
            return accountNameList;
        }
    }
}
