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
using System.Windows;

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

        #region Central Commands
        
        // zentral Commands deklarieren, damit sie von hier gebunden, aber in anderen VMs definiert werden können.  
        private DelegateCommand command_BeoClearInput;
        private DelegateCommand command_BeoAdd;
        private DelegateCommand command_BeoHistoryViewChanged;
        private DelegateCommand command_BeoStartExport;
        private DelegateCommand command_BeoInsertText;
        private DelegateCommand command_BeoInsertTextbaustein;


        public DelegateCommand Command_BeoClearInput
        {
            get { return command_BeoClearInput; }
            set { command_BeoClearInput = value; OnPropertyChanged(); }
        }        
        public DelegateCommand Command_BeoAdd
        {
            get { return command_BeoAdd; }
            set { command_BeoAdd = value; OnPropertyChanged(); }
        }

        public DelegateCommand Command_BeoHistoryViewChanged
        {
            get { return command_BeoHistoryViewChanged; }
            set { command_BeoHistoryViewChanged = value; OnPropertyChanged(); }
        }
        public DelegateCommand Command_BeoStartExport
        {
            get { return command_BeoStartExport; }
            set { command_BeoStartExport = value; OnPropertyChanged(); }
        }
        public DelegateCommand Command_BeoInsertText
        {
            get { return command_BeoInsertText; }
            set { command_BeoInsertText = value; OnPropertyChanged(); }
        }
        public DelegateCommand Command_BeoInsertTextbaustein
        {
            get { return command_BeoInsertTextbaustein; }
            set { command_BeoInsertTextbaustein = value; OnPropertyChanged(); }
        }    
        #endregion
    }
}

    
