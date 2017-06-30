using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Accountants_Personal_Bookkeeper.ViewModel
{
    class SettingsViewModel
    {
        Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
        //Windows.Storage.ApplicationDataCompositeValue composite = new Windows.Storage.ApplicationDataCompositeValue();

        public Windows.Storage.ApplicationDataCompositeValue GetSettingsComposite()
        {
            Windows.Storage.ApplicationDataCompositeValue composite =
                (Windows.Storage.ApplicationDataCompositeValue)localSettings.Values["SettingComposite"];
            if (composite == null)
            {
                composite = new Windows.Storage.ApplicationDataCompositeValue();
                composite["NumberOfAccountInJournal"] = "2";
                composite["AccountReceivableId"] = "-1";
                composite["IncomeAccountId"] = "-1";
                localSettings.Values["SettingComposite"] = composite;
            }
            return composite;
        }

        public bool SaveSettingsComposite(Dictionary<string, string> settingData)
        {
            bool success = true;
            try { 
                Windows.Storage.ApplicationDataCompositeValue composite = new Windows.Storage.ApplicationDataCompositeValue();
                foreach (KeyValuePair<string, string> pair in settingData)
                {
                    composite[pair.Key] = pair.Value;
                }
                localSettings.Values["SettingComposite"] = composite;
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.ToString());
                success = false;
            }
            return success;
        }
    }
}
