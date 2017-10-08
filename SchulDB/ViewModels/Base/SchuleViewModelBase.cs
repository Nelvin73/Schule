using Groll.Schule.SchulDB.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Groll.Schule.Model;
using Groll.Schule.DataManager;
using System.Collections.ObjectModel;
using Groll.Schule.SchulDB.Commands;

namespace Groll.Schule.SchulDB.ViewModels
{
    /// <summary>
    /// ViewModel-Basisklasse
    /// 
    /// Biete zentrale UnitOfWork, RibbonVM und MainWindowVM Properties und Database-Change-Handling
    /// </summary>
    public class SchuleViewModelBase : ObservableObject
    {
              
        #region Properties        
       
        // ViewModel von Mainwindow        
        private static MainWindowVM mainWndVM;

        public MainWindowVM MainWindowViewModel 
        {
            get { return mainWndVM; }
            protected set { mainWndVM = value; }
        }        

        #region Unit of Work

        private SchuleUnitOfWork unitOfWork;

        public SchuleUnitOfWork UnitOfWork
        {
            get
            {
                if (unitOfWork == null)
                { 
                    // Try to get UnitOfWork Global Ressource; if not successful, it stays <null>
                    unitOfWork = System.Windows.Application.Current.TryFindResource("UnitOfWork") as SchuleUnitOfWork;
                    unitOfWork.DatabaseChanged += unitOfWork_DatabaseChanged;
                    // OnDatabaseChanged();
                }
                return unitOfWork;
            }          
        }

        
        #endregion       

        #region Ribbon
        private static RibbonViewModel ribbon;

        public RibbonViewModel Ribbon
        {
            get
            {
                if (ribbon == null)
                {
                    // Get global Ribbon Object
                    ribbon = System.Windows.Application.Current.TryFindResource("Ribbon") as RibbonViewModel;
                }
                return ribbon;
            }
        }
        #endregion
        #endregion

        //  Konstructor
        public SchuleViewModelBase()
        {
            // Save ViewModel of MainWindow
            if (this is MainWindowVM)
                MainWindowViewModel = this as MainWindowVM;

            // Initialize data
            RefreshData();
        }
       
        #region Verhalten bei Änderung der Datenbank      

        /// <summary>
        /// EventHandler für das DatabaseChanged event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void unitOfWork_DatabaseChanged(object sender, EventArgs e)
        {
            OnDatabaseChanged();
            
        }

        public virtual void OnDatabaseChanged()
        {
            // Datenbank wurde geändert => Daten neu laden
            RefreshData();
        }

        public virtual void RefreshData() { }

       


        #endregion

      
       
    }
}
