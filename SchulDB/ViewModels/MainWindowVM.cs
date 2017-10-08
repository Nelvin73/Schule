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
        public string MainWindowTitle
        {
            get { return "Schule DB - Aktuelles Schuljahr: " + Settings.ActiveSchuljahr; }
           
        }       

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
            // Connect to database (if not in Designer mode)
            try
            {
                if (Application.Current.MainWindow != null)
                ConnectDatabase();

            }
            catch (Exception e)
            {
                MessageBox.Show("Database error: " + e.Message);
                throw;
            } 
            // Define Commands
            Command_Navigate = new DelegateCommand((object p) => ShowPage(p.ToString()), (object p) => CanNavigateTo((p ?? "").ToString()));

            // ShowPage("welcome"); 
            ShowPage("welcome"); 
        }

        #region Verhalten bei Änderungen der Daten              

        public override void RefreshData()
        {
            // Datenbank wurde geändert
            OnPropertyChanged("MainWindowTitle");
        }

        #endregion


        private void ConnectDatabase()
        {
            string db = Properties.Settings.Default.UsedDatabase;
            if (db == "<Default>")
                UnitOfWork.ConnectDatabase("Groll.SchulDB");
            else if (db == "<Dev>")
                UnitOfWork.ConnectDatabase("Groll.SchulDB_dev");
            else
                UnitOfWork.ConnectDatabase(db);
        }


        // Commands

        #region Commands

        public DelegateCommand Command_Navigate { get; private set; }
      
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

                    case "klassenedit":
                        page = new KlassenEditPage();
                        break;

                    case "klassenarbeitedit":
                        page = new KlassenarbeitEditPage();
                        break;

                    case "vorlagen":
                        page = new VorlagenPage();
                        break;
                    
                    case "stundenplanedit":
                        page = new StundenplanEditPage();
                        break;

                    case "schuljahredetails":
                        var dlg = new ChangeSchuljahr();
                        dlg.Owner = Application.Current.MainWindow;
                        dlg.ShowDialog();
                        return;                      

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

        #region Common Commands
        
        // zentral Commands deklarieren, damit sie von hier gebunden, aber in anderen VMs definiert werden können.  
        private DelegateCommand command_BeoStartExport;
      

        public DelegateCommand Command_BeoStartExport
        {
            get { return command_BeoStartExport; }
            set { command_BeoStartExport = value; OnPropertyChanged(); }
        }
       
        #endregion
    }
}

    
