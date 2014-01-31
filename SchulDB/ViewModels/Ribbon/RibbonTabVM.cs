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
    public class RibbonTabVM : ObservableObject
    {        
        private RibbonVM ribbonVM;                
        private bool isSelected = false;
        private bool isVisible = true;
        private string header = "";

        protected UowSchuleDB UnitOfWork
        {
            get { return ribbonVM.UnitOfWork; }
         }
                

        public bool IsSelected
        {
            get { return isSelected; }
            set { isSelected = value; OnPropertyChanged(); }
        }
        
        public bool IsVisible
        {
            get { return isVisible; }
            set { isVisible = value; OnPropertyChanged(); }
        }

        public string Header
        {
            get { return header; }
            set { header = value; OnPropertyChanged(); }
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
