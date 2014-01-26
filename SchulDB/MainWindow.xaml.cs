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

namespace Groll.Schule.SchulDB
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : RibbonWindow
    {
        private Dictionary<string, ISchulDBPage> pages = new Dictionary<string, ISchulDBPage>();
        private Groll.Schule.DataManager.UowSchuleDB UnitOfWork;

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
                    new CommandBinding(BasicCommands.DumpContext, Executed_DumpContext, CanExecute_TRUE)
                });


                UnitOfWork = this.FindResource("UnitOfWork") as Groll.Schule.DataManager.UowSchuleDB;
                UnitOfWork.Schueler.GetList();
                ShowPage("welcome");   
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                throw;
            }
           

                     
        }

        private void Executed_DumpContext(object sender, ExecutedRoutedEventArgs e)
        {
            UnitOfWork.DumpContext();
        }


        private RibbonVM ribbonVM;
        public RibbonVM RibbonVM
        {
            get
            {
                if (ribbonVM == null)
                    ribbonVM = FindResource("RibbonVM") as RibbonVM;

                return ribbonVM;
            }
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
                }

                // Save page in cache
                if (page != null)
                {
                    pages[p] = page;
                    page.SetMainWindow(this);
                }
            }

            // Show page
            if (page != null)
                ContentFrame.Navigate(page);
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

        private void CanExecute_TRUE(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }


        #endregion


    }
}
