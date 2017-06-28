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

        public PartyViewModel()
        {
            conn = new Connection().GetConnection();
            conn.CreateTable<Party>();
        }

        public bool Add(string name, string code, string subtypeString, string phone, string email,
             string address, string company_name, string entry_date, string opening_balance)
        {
            bool success = false;
            try
            {
                int subtype = int.Parse(subtypeString);
                var add = conn.Insert(new Party()
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
    }
}
