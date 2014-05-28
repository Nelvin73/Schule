using Groll.Schule.DataManager;
using Groll.Schule.SchulDB.Commands;
using Groll.Schule.SchulDB.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Groll.Schule.SchulDB.Pages;
using System.Windows.Controls;

namespace Groll.Schule.SchulDB.ViewModels
{
    /// <summary>
    /// ViewModel für das Hauptfenster
    /// </summary>
    public class MainWindowVM : SchuleViewModelBase
    {
        #region Private Fields
       
        private Dictionary<string, ISchulDBPage> pages = new Dictionary<string, ISchulDBPage>();
        private Page currentPage;
        private static RibbonViewModel RibbonViewModel;


        #endregion

        #region Properties

        
        public Page CurrentPage
        {
            get { return currentPage; }
            set { 
                currentPage = value;
                OnPropertyChanged();
            }
        }

        // Static member to have one common UnitOfWork

        public RibbonViewModel Ribbon
        {
            get
            {
                if (RibbonViewModel == null)
                {
                    // Try to get UnitOfWork Global Ressource; if not successful, it stays <null>
                    RibbonViewModel = RibbonViewModel.Default;
                }
                return RibbonViewModel;
            }          
        }

     
        #endregion

        //  Konstructor
        public MainWindowVM() : base()
        {   
            // Connect to database
            ConnectDatabase();

            // Define Commands
            NavigateToCommand = new DelegateCommand((object p) => ShowPage(p.ToString()), (object p) => CanNavigateTo(p.ToString()));
            ShowPage("welcome"); 
        }

        #region Verhalten bei Änderungen der Daten              

        public override void RefreshData()
        {
            // Datenbank wurde geändert
          
        }

      


        #endregion


        private void ConnectDatabase()
        {
            string db = Properties.Settings.Default.UsedDatabase;
            if (db == "<Default>")
                UnitOfWork.ConnectDatabase(DataManager.UowSchuleDB.DatabaseType.Standard);
            else if (db == "<Dev>")
                UnitOfWork.ConnectDatabase(DataManager.UowSchuleDB.DatabaseType.Development);
            else
                UnitOfWork.ConnectDatabase(DataManager.UowSchuleDB.DatabaseType.Custom, db);
        }


        // Commands

        #region Commands
        public DelegateCommand NavigateToCommand {get; private set;}
      
        public bool CanNavigateTo(string p)
        {
            return true;
        }

      
        private void ShowPage(string p, bool CreateNew = false)
        {
            ISchulDBPage page = null;

            // Try to get existing page
            if (!CreateNew && pages.ContainsKey(p))            
                page = pages[p];
            
            // Else create new Page
            if (page == null)
            {
                #region Create new Page
                switch (p.ToLower())
                {
                    case "welcome":
                        page = new WelcomePage();
                        break;

                    case "schuelerdetails":
                        page = new SchuelerDetailsPage();
                        break;

                    case "faecherdetails":
                        page = new FaecherDetailsPage();
                        break;

                    case "klassendetails":
                        page = new KlassenDetailsPage();
                        break;

                    case "schuljahredetails":
                        page = new SchuljahreDetailsPage();
                        break;

                    case "beobachtungeneingabe":
                        page = new BeobachtungenEingabePage();
                        break;

                    case "beobachtungenansicht":
                        page = new BeobachtungenAnsichtPage();
                        break;

                    default:
                        throw new ArgumentException("'" + p.ToString() + "' is no valid page name.", "p");                
                }
                                
                // Save page in cache
                if (page != null)                
                    pages[p] = page;
                    
                #endregion                
            }

            // Show page
            CurrentPage = page as Page;         
        }

    #endregion                       

    
       
    }
}

    
