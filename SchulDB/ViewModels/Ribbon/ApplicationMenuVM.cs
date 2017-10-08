using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Groll.Schule.SchulDB.Helper;
using Groll.Schule.DataManager;
using Groll.Schule.Model;
using System.Collections.ObjectModel;
using System.Windows.Controls.Ribbon;
using System.Windows.Media;
using Groll.Schule.SchulDB.Commands;
using System.Windows;


namespace Groll.Schule.SchulDB.ViewModels
{
    public class ApplicationMenuVM : RibbonTabViewModel
    {        
        #region Properties für Bindings

        public string CurrentDbType
        {
            get
            {
                return UnitOfWork == null ? "" : UnitOfWork.ConnectionProvider.ToString();
            }
        }

        public string CurrentDbFile
        {
            get
            {
                return UnitOfWork == null ? "" : UnitOfWork.CurrentDbFilename.ToString();
            }
        }
        #endregion
      
        #region Konstruktor
        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="RibbonViewModel">Root Element</param>
        public ApplicationMenuVM()
        {
            Label = "";
            ChangeDatabaseCommand = new DelegateCommand((object p) => ChangeDatabase((p ?? "").ToString()));
            OnDatabaseChanged();             
        }

        
        #endregion

        #region Database-Handling
        public override void OnDatabaseChanged()
        {
            // invalidate all database relevant properties            
            OnPropertyChanged("CurrentDbType");
            OnPropertyChanged("CurrentDbFile");            
            base.OnDatabaseChanged();
        }
      
        #endregion

        #region Commands

        public DelegateCommand ChangeDatabaseCommand {get; private set;}

        // Führt das Command "ChangeDatabase" aus
        public void ChangeDatabase(string p = "")
        {
            switch (p.ToLower())
            {
                case "custom":
                    // User custom database selected
                    MessageBox.Show("Funktion noch nicht implementiert!");
                    break;

                case "dev":
                    UnitOfWork.ConnectDatabase("Groll.SchulDB_dev");
                    Properties.Settings.Default.UsedDatabase = "<Dev>";
                    Properties.Settings.Default.Save();
                    break;

                default:
                    UnitOfWork.ConnectDatabase("Groll.SchulDB");
                    Properties.Settings.Default.UsedDatabase = "<Default>";
                    Properties.Settings.Default.Save();
                    break;
            }
        }
       
        #endregion
    }
}
