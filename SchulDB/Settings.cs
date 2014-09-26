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
                int i = Convert.ToInt16(Get("Global.AktivesSchuljahr", typeof(int), Schuljahr.GetCurrent().Startjahr, true));
                return DB.Schuljahre.GetById(i);                
            }

            set
            {
                int i = value.Startjahr;
                if (Set("Global.AktivesSchuljahr", typeof(int), i))
                {
                    Instance.OnPropertyChanged("ActiveSchuljahr");
                    DB.TriggerDatabaseChangedEvent();
                }
            }
       }
                
        
        /// <summary>
        /// Liest ein Setting aus der DB (oder dem cache)
        /// </summary>
        /// <param name="Key">Setting-Key</param>
        /// <param name="ValueType">Value Type</param>
        /// <param name="Default">Default-Wert falls nicht vorhanden</param>
        /// <param name="CreateIfNotExist">Initialisiert die DB mit dem Defaultwert, falls nicht vorhanden</param>
        /// <returns>Value der Setting</returns>
        public static object Get(string Key, Type ValueType, object Default = null, bool CreateIfNotExist = false)
        {
            // ist bereits im Cache eingelesen ?
            if (!settingsCache.ContainsKey(Key))
            {
                // nicht im Cache => Wert aus der Datenbank holen
                var s = DB.Settings[Key];
                if (s == null || s.GetValue() == null)
                {
                    // auch nicht in der Datenbank => Defaultwert nehmen
                    // soll Default wert in DB geschrieben werden ?
                    if (CreateIfNotExist && Default != null)
                    {
                        DB.Settings[Key] = new Setting(Key, "");
                        DB.Settings[Key].SetValue(Default);
                        DB.Save();               
                    }
                    return Default;
                }
                // im Cache zwischenspeichern
                settingsCache[Key] = s.GetValue();
            }

            return settingsCache[Key];
        }

        /// <summary>
        /// Speichert ein Setting in der DB (und im Cache)
        /// </summary>
        /// <param name="Key">Setting-Key</param>
        /// <param name="ValueType">Value Type</param>
        /// <param name="Value">neuer Wert</param>
        /// <returns>true, wenn Value geändert wurde</returns>
        public static bool Set(string Key, Type ValueType, object Value)
        {
            // Prüfen, ob sich der Wert wirklich geändert hat
            if (settingsCache.ContainsKey(Key))
            {
                var s = DB.Settings[Key];
                if (s.GetValue().Equals(Value))
                    return false;
            }
            // Save new value in cache and DB
            settingsCache[Key] = Value;

            DB.Settings[Key] = new Setting(Key, "");
            DB.Settings[Key].SetValue(Value);
            DB.Save();           
            return true;
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
