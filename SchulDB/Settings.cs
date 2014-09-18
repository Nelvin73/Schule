using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Groll.Schule.Model;
using Groll.Schule.DataManager;

namespace Groll.Schule.SchulDB
{
    public class Settings : Schule.SchulDB.ViewModels.SchuleViewModelBase
    {        
        
        // globale Programmeinstellungen        
        private static Dictionary<string, object> settingsCache;
        private static UowSchuleDB DB;

        // Statische Instanz
        public static Settings Instance;



        #region Properties
        
        public static Schuljahr ActiveSchuljahr
        {
            get
            {
                if (!settingsCache.ContainsKey("ActiveSchuljahr"))
                {
                    int sjID = 0;
                    
                    // Aktives Schuljahr aus der Datenbank holen
                    var s = DB.Settings["Global.AktivesSchuljahr"];
                    if (s != null) 
                        sjID = DB.Settings["Global.AktivesSchuljahr"].GetInt(0);

                    Schuljahr sj = DB.Schuljahre.GetById(sjID) ?? Schuljahr.GetCurrent();                   
                    
                    settingsCache["ActiveSchuljahr"] = sj;
                    if (sjID == 0)
                    {    // if not in DB, save it
                        DB.Settings["Global.AktivesSchuljahr"] = new Setting("Global.AktivesSchuljahr", sjID);
                        DB.Save();
                    }

                }

                return settingsCache["ActiveSchuljahr"] as Schuljahr;
            }

            set
            {
                if (settingsCache.ContainsKey("ActiveSchuljahr"))
                {
                    Schuljahr sj = settingsCache["ActiveSchuljahr"] as Schuljahr;
                    if (sj.Startjahr == value.Startjahr)
                        return;
                }

                // Save new value in cache and DB
                settingsCache["ActiveSchuljahr"] = value;
                DB.Settings["Global.AktivesSchuljahr"] = new Setting("Global.AktivesSchuljahr", (Int16)value.Startjahr);
                DB.Save();
                Instance.OnPropertyChanged("ActiveSchuljahr");

                // Trigger DB Update ???
                DB.TriggerDatabaseChangedEvent();
                              
            }
        }

        #endregion

        static Settings()
        {
            settingsCache = new Dictionary<string, object>();
            Instance = new Settings();
        }

        public Settings()
        {
            DB = UnitOfWork;
        }   

        public override void RefreshData()
        {
            // Delete Settings
            if (settingsCache != null)
                settingsCache.Clear();
            base.RefreshData();
        }
    }
}
