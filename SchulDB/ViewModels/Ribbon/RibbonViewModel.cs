using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Groll.Schule.SchulDB.Helper;
using Groll.Schule.DataManager;
using Groll.Schule.Model;
using System.Collections.ObjectModel;
using Groll.Schule.SchulDB.Commands;

namespace Groll.Schule.SchulDB.ViewModels
{
    public class RibbonViewModel : SchuleViewModelBase
    {
        #region Static Instance
        static RibbonViewModel staticInstance;

        public static RibbonViewModel Default
        {
            get
            {
                if (staticInstance == null)
                    staticInstance = new RibbonViewModel();
                return staticInstance;
            }
        }
        #endregion
               

        #region Tabs

        private Dictionary<string, object> tabs = new Dictionary<string, object>();

        public ApplicationMenuVM ApplicationMenu
        {
            get
            {
                string Key = "ApplicationMenu";
                ApplicationMenuVM t = GetElement(Key) as ApplicationMenuVM;
                return t ?? SetElement(Key, new ApplicationMenuVM()) as ApplicationMenuVM;
            }
        }

        public RibbonTabStandardVM TabStandard
        {
            get
            {
                string Key = "TabStandard";
                RibbonTabStandardVM t = GetElement(Key) as RibbonTabStandardVM;
                return t ?? SetElement(Key, new RibbonTabStandardVM()) as RibbonTabStandardVM;
            }
        }

        public RibbonTabBeobachtungenVM TabBeobachtungen
        {
            get
            {
                string Key = "TabBeobachtungen";
                RibbonTabBeobachtungenVM t = GetElement(Key) as RibbonTabBeobachtungenVM;
                return t ?? SetElement(Key, new RibbonTabBeobachtungenVM()) as RibbonTabBeobachtungenVM;
            }
        }

        public RibbonTabBeobachtungenViewVM TabBeobachtungenAnsicht
        {
            get
            {
                string Key = "TabBeobachtungenAnsicht";
                RibbonTabBeobachtungenViewVM t = GetElement(Key) as RibbonTabBeobachtungenViewVM;
                return t ?? SetElement(Key, new RibbonTabBeobachtungenViewVM()) as RibbonTabBeobachtungenViewVM;
            }
        }

        private object GetElement(string Key)
        {
            if (Key != null && tabs.ContainsKey(Key))
                return tabs[Key];
            else
                return null;
        }

        private object SetElement(string Key, object Element, bool Overwrite = true)
        {
            if (string.IsNullOrEmpty(Key) || Element == null)
                return null;

            if (Overwrite || !tabs.ContainsKey(Key))
                tabs.Add(Key, Element);

            return Element;
        }       

        #endregion

        #region TabGroups
        private bool isContextTabBeobachtungenVisible = false;

        public bool IsContextTabBeobachtungenVisible
        {
            get { return isContextTabBeobachtungenVisible; }
            set
            {
                if (isContextTabBeobachtungenVisible != value)
                { isContextTabBeobachtungenVisible = value; OnPropertyChanged(); }
            }
        }
        


        #endregion


        public RibbonViewModel()
        {
            // Initialization 

            SaveCommand = new DelegateCommand((x) => Save());
            DumpContextCommand = new DelegateCommand((x) => DumpContext());
        }
/*
        public override void RefreshData()
        {
            base.RefreshData();

            // Inform RibbonTabs
            foreach (object tab in tabs.Values)
                if (tab is RibbonTabViewModel)
                    (tab as RibbonTabViewModel).OnDatabaseChanged();
                else if (tab is RibbonTabVM )
                    (tab as RibbonTabVM).OnDatabaseChanged();
        }
        */

        #region Commands
        public DelegateCommand SaveCommand { get; private set; }
        public DelegateCommand DumpContextCommand { get; private set; }


        public void Save()
        {
            UnitOfWork.Save();
        }

        public void DumpContext()
        {
            UnitOfWork.DumpContext();
        }
        
        #endregion
    }
}
