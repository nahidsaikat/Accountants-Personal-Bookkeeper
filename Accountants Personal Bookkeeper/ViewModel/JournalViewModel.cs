using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Diagnostics;
using Accountants_Personal_Bookkeeper.DB;
using Accountants_Personal_Bookkeeper.Model;

namespace Accountants_Personal_Bookkeeper.ViewModel
{
    class JournalViewModel
    {

        SQLite.Net.SQLiteConnection conn;

        public JournalViewModel()
        {
            conn = new Connection().GetConnection();
            conn.CreateTable<Journal>();
            conn.CreateTable<NumberGenerator>();
            conn.CreateTable<Ledger>();
        }

        public string Add(string journalDate, string typeString, string partyString, string referenceString, string note, 
            Dictionary<int, double> accountInfo, double totalDebit, double totalCredit)
        {
            string message = "";
            if (accountInfo.Count < 2) return "Two Account Required!";
            if (totalDebit != totalCredit) return "Debit Credit Mismatch!";
            try
            {
                int type = int.Parse(typeString), party_id = int.Parse(partyString),
                    ref_journal_id = int.Parse(referenceString);
                
                string number = GetJournalNumber(type);
                //string acc_info = "";
                //foreach(KeyValuePair<int, double> pair in accountInfo)
                //{
                //    acc_info += pair.Key.ToString() + ":" + pair.Value.ToString() + " ";
                //}
                string json = JsonConvert.SerializeObject(accountInfo, Formatting.Indented).ToString()
                    .Replace("\r", "").Replace("\n", "").Replace(" ", "");
                var add = conn.Insert(new Journal()
                {
                    number = number,
                    journal_date = DateTime.Parse(journalDate),
                    amount = totalDebit,
                    type = type,
                    party_id = party_id,
                    ref_journal_id = ref_journal_id,
                    description = note,
                    account_info = json.Trim(),
                    deleted = 0
                });
                if (add != -1)
                {
                    foreach(KeyValuePair<int, double> pair in accountInfo)
                    {
                        conn.Insert(new Ledger()
                        {
                            journal_id = add,
                            account_id = pair.Key,
                            amount = pair.Value,
                            party_id = party_id,
                            entry_date = DateTime.Parse(journalDate),
                            description = note,
                            other_accounts = json
                        });
                    }
                }
                message = "Success";
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                message = "Something went wrong!";
            }
            return message;
        }

        private string GetJournalNumber(int type)
        {
            int next_number = 0;
            JournalType jType = (from jt in conn.Table<JournalType>()
                                 where jt.id == type
                                 select jt
                                 ).First<JournalType>();
            string prefix = jType.prefix;
            bool NGTableEntry = (from generator in conn.Table<NumberGenerator>()
                                 where generator.prefix == prefix
                                 select generator
                                 ).Any<NumberGenerator>();

            if (NGTableEntry)
            {
                NumberGenerator Gen = (from generator in conn.Table<NumberGenerator>()
                               where generator.prefix == prefix
                               select generator
                               ).First<NumberGenerator>();
                next_number = Gen.value + 1;
                Gen.value += 1;
                conn.Update(Gen);
            }
            else
            {
                next_number = int.Parse(jType.start_number);
                conn.Insert(new NumberGenerator()
                {
                    prefix = prefix,
                    value = next_number
                });
            }

            return prefix + " - " + next_number;
        }

        public List<Journal> JournalList()
        {
            List<Journal> journalList = new List<Journal>();
            foreach (var journal in conn.Table<Journal>())
            {
                journalList.Add(journal);
            }
            return journalList;
        }

        public Journal Get(int id)
        {
            Journal journal = (from jour in conn.Table<Journal>()
                                where jour.id == id
                                select jour
                                  ).First<Journal>();
            return journal;
        }

        public JournalType GetType(int subtype_id)
        {
            JournalTypeViewModel typeVM = new JournalTypeViewModel();
            return typeVM.Get(subtype_id);
        }

        public Party GetParty(int party_id)
        {
            PartyViewModel partyVM = new PartyViewModel();
            return partyVM.Get(party_id);
        }

        public string GetAccountsName(string accountInfo)
        {
            string names = "";
            AccountViewModel accountVM = new AccountViewModel();
            Dictionary<int, double> accountData = JsonConvert.DeserializeObject<Dictionary<int, double>>(accountInfo);
            foreach(KeyValuePair<int, double> pair in accountData)
            {
                Account account = accountVM.Get(pair.Key);
                names += account.name + " : " + pair.Value.ToString() + "\n ";
            }
            return names.Trim();
        }
    }
}
