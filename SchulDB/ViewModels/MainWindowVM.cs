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

            // Define Commands
            NavigateToCommand = new DelegateCommand((object p) => ShowPage(p.ToString()), (object p) => CanNavigateTo(p.ToString()));
            ShowPage("welcome"); 
        }

        #region Verhalten bei Änderungen der Daten              

        protected override void RefreshData()
        {
            // Datenbank wurde geändert
          
        }

      


        #endregion


        // Commands

        #region Commands
        public DelegateCommand NavigateToCommand {get; private set;}
        public DelegateCommand SaveCommand { get; private set; }                

        public bool CanNavigateTo(string p)
        {
            return true;
        }

        #endregion
  
    
    
    
        /*
                this.CommandBindings.AddRange( new List<CommandBinding>
                {
                    new CommandBinding(BasicCommands.NavigateTo, Executed_NavigateTo, CanExecute_TRUE),
                    new CommandBinding(ApplicationCommands.Save, Executed_Save, CanExecute_TRUE),
                    new CommandBinding(BasicCommands.DumpContext, Executed_DumpContext, CanExecute_TRUE),
                    new CommandBinding(BasicCommands.ChangeDatabase, Executed_ChangeDatabase, CanExecute_TRUE)

                });

                // Initialisiere Datenbank
                UnitOfWork = this.FindResource("UnitOfWork") as Groll.Schule.DataManager.UowSchuleDB;
                ConnectDatabase();
               
                ShowPage("welcome");   
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                throw;
            }
           

                     */
       
        /*
        private void ConnectDatabase()
        {
            string db = Properties.Settings.Default.UsedDatabase;
            if (db == "<Default>")
                ConnectDatabase( DataManager.UowSchuleDB.DatabaseType.Standard);
            else if (db == "<Dev>")
                ConnectDatabase( DataManager.UowSchuleDB.DatabaseType.Development);
            else
                ConnectDatabase(  DataManager.UowSchuleDB.DatabaseType.Custom, db);
        }
       

        private void ConnectDatabase(DataManager.UowSchuleDB.DatabaseType DBtype, string Filename = "")
        {
            if (UnitOfWork.CurrentDbType != DBtype || Filename != UnitOfWork.CurrentDbFilename)            
                UnitOfWork.ConnectDatabase(DBtype, Filename);                           
        }
         */

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

     /*

        /// <summary>
        /// Führt das Command "ChangeDatabase" aus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Executed_ChangeDatabase(object sender, ExecutedRoutedEventArgs e)
        {            
            switch ((e.Parameter ?? "").ToString())
            {
                case "custom":
                    // User custom database selected
                    MessageBox.Show("Funktion noch nicht implementiert!");
                    break;

                case "dev":
                    ConnectDatabase(DataManager.UowSchuleDB.DatabaseType.Development);
                    Properties.Settings.Default.UsedDatabase = "<Dev>";
                    Properties.Settings.Default.Save();
                    break;

                default:
                    ConnectDatabase(DataManager.UowSchuleDB.DatabaseType.Standard);
                    Properties.Settings.Default.UsedDatabase = "<Default>";
                    Properties.Settings.Default.Save();
                    break;
            }
        }

        private void Executed_DumpContext(object sender, ExecutedRoutedEventArgs e)
        {
            UnitOfWork.DumpContext();
        }


        private void CanExecute_TRUE(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        */
    }
}

    
