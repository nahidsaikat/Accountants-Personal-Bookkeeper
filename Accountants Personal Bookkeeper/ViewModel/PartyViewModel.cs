using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Accountants_Personal_Bookkeeper.DB;
using Accountants_Personal_Bookkeeper.Model;
using Accountants_Personal_Bookkeeper.ViewModel;

namespace Accountants_Personal_Bookkeeper.ViewModel
{
    class PartyViewModel
    {
        SQLite.Net.SQLiteConnection conn;
        SettingsViewModel settings;
        AccountViewModel accountVM;

        public PartyViewModel()
        {
            conn = new Connection().GetConnection();
            conn.CreateTable<Party>();
            settings = new SettingsViewModel();
            accountVM = new AccountViewModel();
        }

        public int Add(string name, string code, string subtypeString, string phone, string email,
             string address, string company_name, string entry_date, string opening_balance)
        {
            int add = -1;
            try
            {
                add = CreateAccount(name, code, entry_date, opening_balance);
                if (add > 0)
                {
                    int account_id = add;
                    int subtype = int.Parse(subtypeString);
                    Party party = new Party()
                    {
                        name = name,
                        code = code,
                        subtype_id = subtype,
                        phone = phone,
                        email = email,
                        address = address,
                        company_name = company_name,
                        entry_date = DateTime.Parse(entry_date),
                        opening_balance = Double.Parse(opening_balance),
                        account_id = account_id,
                        deleted = 0
                    };
                    conn.Insert(party);
                    add = party.id;
                }
                else { add = -2; }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                add = -1;
            }
            return add;
        }

        public int CreateAccount(string name, string code, string entry_date, string opening_balance)
        {
            Windows.Storage.ApplicationDataCompositeValue composite = settings.GetSettingsComposite();
            int ar_id = int.Parse(composite["AccountReceivableId"].ToString());
            
            Account AR = accountVM.Get(ar_id);
            string acc_name = "AR " + name, 
                acc_code = AR.code + "." + code, 
                note = "Account for " + name,
                type = AR.type.ToString(),
                subtype = AR.subtype.ToString(),
                parent_id = ar_id.ToString();
            int success = accountVM.Add(acc_name, acc_code, type, subtype, parent_id, opening_balance, entry_date, note);
            return success;
        }

        public List<Party> PartyList()
        {
            List<Party> partyList = new List<Party>();
            foreach (var party in conn.Table<Party>())
            {
                partyList.Add(party);
            }
            return partyList;
        }

        public Party Get(int id)
        {
            Party party = (from acc in conn.Table<Party>()
                               where acc.id == id
                               select acc
                                  ).First<Party>();
            return party;
        }

        public PartySubtype GetSubtype(int subtype_id)
        {
            PartySubtypeViewModel subtypevM = new PartySubtypeViewModel();
            return subtypevM.Get(subtype_id);
        }

        public List<PartyLedger> PartyLedgerList()
        {
            List<PartyLedger> ledgers = new List<PartyLedger>();
            int counter = 0;
            foreach (Party party in PartyList())
            {
                ledgers.Add(new PartyLedger()
                {
                    id = ++counter,
                    party_id = party.id,
                    party_name = party.name,
                    balance = accountVM.GetBalance(party.account_id)
                });
            }
            return ledgers;
        }
    }
}
