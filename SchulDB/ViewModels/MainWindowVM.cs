using Groll.Schule.DataManager;
using Groll.Schule.SchulDB.Commands;
using Groll.Schule.SchulDB.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Groll.Schule.SchulDB.Pages;

namespace Groll.Schule.SchulDB.ViewModels
{
    /// <summary>
    /// ViewModel für das Hauptfenster
    /// </summary>
    public class MainWindowVM : SchuleViewModelBase
    {
       
        #region Properties

        //public Schuljahr SelectedSchuljahr
        //{
        //    get { return selectedSchuljahr; }
        //    set
        //    {
        //        if (selectedSchuljahr == value)
        //            return;
        //        selectedSchuljahr = value; OnPropertyChanged(); OnSelectedSchuljahrChanged();
        //    }
        //}

        private ISchulDBPage currentPage;

        public ISchulDBPage CurrentPage
        {
            get { return currentPage; }
            set { currentPage = value;
            OnPropertyChanged();
            }
        }
        

     
        #endregion

        //  Konstructor
        public MainWindowVM()
        {   

            // Define Commands
            NavigateTo = new DelegateCommand((object p) => ShowPage(p.ToString()), (object p) => CanNavigateTo(p.ToString()));
            CurrentPage = new WelcomePage();
        }

        #region Verhalten bei Änderungen der Daten              

        protected override void RefreshData()
        {
            // Datenbank wurde geändert
          
        }

      


        #endregion


        // Commands

        #region Commands
        public DelegateCommand NavigateTo {get; private set;}        

        public void ShowPage(string p)
        {


        }

        public bool CanNavigateTo(string p)
        {
            return true;
        }

        #endregion
    }
}

    
