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
    public class RibbonTabVM : RibbonBaseVM
    {        
        private RibbonVM ribbonVM;
        private string contextualTabGroupHeader;
        protected Dictionary<string, RibbonBaseVM> elementCache = new Dictionary<string,RibbonBaseVM>();

        protected RibbonBaseVM GetElement(string Key)
        {
            if (Key != null && elementCache.ContainsKey(Key))
                return elementCache[Key];
            else
                return null;            
        }

        protected UowSchuleDB UnitOfWork
        {
            get { return ribbonVM.UnitOfWork; }
         }

        protected string ContextualTabGroupHeader
        {
            get
            {
                return contextualTabGroupHeader;
            }
            set
            {
                if (contextualTabGroupHeader != value)
                {
                    contextualTabGroupHeader = value;
                    OnPropertyChanged();
                }
            }
        }

        public RibbonTabVM(RibbonVM RibbonVM)
        {
            ribbonVM = RibbonVM;
            
            // Im Designermode anzeigen
            if (App.Current.MainWindow == null)
            {
                IsVisible = true;
                IsSelected = true;
            }
        }
       
        /// <summary>
        /// Initialisierung wenn Datenbank sich geändert hat
        /// </summary>
        public virtual void OnDatabaseChanged()
        {

        }      
    }
}
