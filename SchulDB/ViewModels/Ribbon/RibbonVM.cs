using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Groll.Schule.SchulDB.Helper;
using Groll.Schule.DataManager;
using Groll.Schule.Model;
using System.Collections.ObjectModel;


namespace Groll.Schule.SchulDB.ViewModels
{
    public class RibbonVM : ObservableObject
    {
        #region Static Instance
        static RibbonVM staticInstance;

        public static RibbonVM Default
        {
            get
            {
                if (staticInstance == null)
                    staticInstance = new RibbonVM();
                return staticInstance;
            }
        }
        #endregion
        
        #region Unit of Work
        private UowSchuleDB unitOfWork;
        public UowSchuleDB UnitOfWork
        {
            get {
                if (unitOfWork == null)                
                    // Try to get UnitOfWork Global Ressource; if not successful, it stays <null>
                    UnitOfWork = System.Windows.Application.Current.TryFindResource("UnitOfWork") as UowSchuleDB;                    
                
                return unitOfWork; }

            set { unitOfWork = value; OnPropertyChanged(); OnUnitOfWorkChanged(); }
        }
        #endregion

        #region Tabs

        private Dictionary<string, RibbonTabVM> tabs = new Dictionary<string, RibbonTabVM>();

        public ApplicationMenuVM ApplicationMenu
        {
            get
            {
                string Key = "ApplicationMenu";
                ApplicationMenuVM t = GetElement(Key) as ApplicationMenuVM;
                return t ?? SetElement(Key, new ApplicationMenuVM(this)) as ApplicationMenuVM;
            }
        }

        public RibbonTabStandardVM TabStandard
        {
            get
            {
                string Key = "TabStandard";
                RibbonTabStandardVM t = GetElement(Key) as RibbonTabStandardVM;
                return t ?? SetElement(Key, new RibbonTabStandardVM(this)) as RibbonTabStandardVM;
            }
        }

        public RibbonTabBeobachtungenVM TabBeobachtungen
        {
            get
            {
                string Key = "TabBeobachtungen";
                RibbonTabBeobachtungenVM t = GetElement(Key) as RibbonTabBeobachtungenVM;
                return t ?? SetElement(Key, new RibbonTabBeobachtungenVM(this)) as RibbonTabBeobachtungenVM;
            }
        }

        private RibbonTabVM GetElement(string Key)
        {
            if (Key != null && tabs.ContainsKey(Key))
                return tabs[Key];
            else
                return null;
        }

        private RibbonTabVM SetElement(string Key, RibbonTabVM Element, bool Overwrite = true)
        {
            if (string.IsNullOrEmpty(Key) || Element == null)
                return null;

            if (Overwrite || !tabs.ContainsKey(Key))
                tabs.Add(Key, Element);

            return Element;
        }


        private RibbonTabBeobachtungenVM beobachtungenTabVM;
        public RibbonTabBeobachtungenVM aBeobachtungenTabVM
        {
            get { return beobachtungenTabVM; }
            set { beobachtungenTabVM = value; }
        }

        #endregion


    
        public RibbonVM()
        {
            // Initialization            
        }


        private void OnUnitOfWorkChanged()
        {
            // Initialize Database parameters
            unitOfWork.DatabaseChanged += unitOfWork_DatabaseChanged;
            unitOfWork_DatabaseChanged(this, null);
        }

        void unitOfWork_DatabaseChanged(object sender, EventArgs e)
        {
            // Invalidate database dependent properties
            
            // Inform RibbonTabs
            foreach (RibbonTabVM tab in tabs.Values)
                tab.OnDatabaseChanged();
          
        }      
    }
}
