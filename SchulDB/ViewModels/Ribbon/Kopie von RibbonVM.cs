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
    public class RibbronVM : ObservableObject
    {
        #region Unit of Work
        private UowSchuleDB unitOfWork;
        public UowSchuleDB UnitOfWork
        {
            get { return unitOfWork; }
            set { unitOfWork = value; OnPropertyChanged(); OnUnitOfWorkChanged(); }
        }
        #endregion

        #region Tabs

        private RibbonTabBeobachtungenVM beobachtungenTabVM;
        public RibbonTabBeobachtungenVM BeobachtungenTabVM
        {
            get { return beobachtungenTabVM; }
            set { beobachtungenTabVM = value; }
        }

        #endregion


        #region CurrentDB info properties
        public string CurrentDbType
        {
            get
            {
                if (unitOfWork == null)
                    return "";

                return unitOfWork.CurrentDbType.ToString();
            }           
        }

        public string CurrentDbFile
        {
            get
            {
                if (unitOfWork == null)
                    return "";

                return unitOfWork.CurrentDbFilename.ToString();
            }
        }
        #endregion
      
          
        public RibbronVM()
        {
            // Initialization
            BeobachtungenTabVM = new RibbonTabBeobachtungenVM(this);
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
            OnPropertyChanged("CurrentDbType");
            OnPropertyChanged("CurrentDbFile");
            
            // Inform RibbonTabs
            BeobachtungenTabVM.OnDatabaseChanged();
        }      
    }
}
