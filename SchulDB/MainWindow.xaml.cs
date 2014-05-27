using System.Windows.Controls.Ribbon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Groll.Schule.SchulDB.Pages;
using Groll.Schule.SchulDB.Commands;
using Groll.Schule.SchulDB.ViewModels;

namespace Groll.Schule.SchulDB
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : RibbonWindow
    {
        private Dictionary<string, ISchulDBPage> pages = new Dictionary<string, ISchulDBPage>();
        private Groll.Schule.DataManager.UowSchuleDB UnitOfWork;
        private string currentPage;

        public RibbonViewModel RibbonViewModel
        {
            get
            {
                return RibbonViewModel.Default;
            }
        }
        public MainWindow()
        {
            try
            {
                InitializeComponent();

                // Verknüpfe Command Bindings              
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
           

                     
        }

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
        
        public void ShowPage(string p, bool CreateNew = false)
        {
            ISchulDBPage page = null;

            // Try to get existing page
            if (!CreateNew)
            {
                if (pages.ContainsKey(p))
                    page = pages[p];
            }

            // Else create new Page
            if (page == null)
            {
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
                }

                // Save page in cache
                if (page != null)
                {
                    pages[p] = page;
                    page.SetMainWindow(this);
                }
                currentPage = p;
            }

            // Show page
            if (page != null) 
         ;//       ContentFrame.Navigate(page);
            else
                throw new ArgumentException("'" + p.ToString() + "' is no valid page name.", "p");
        }


        #region Command implementations
       

        private void Executed_NavigateTo(object sender, ExecutedRoutedEventArgs e)
        {
            ShowPage(e.Parameter.ToString());
        }

      
        private void Executed_Save(object sender, ExecutedRoutedEventArgs e)
        {
            UnitOfWork.Save();
        }

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


        #endregion


    }
}
