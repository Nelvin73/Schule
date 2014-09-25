using Groll.Schule.Model;
using Groll.Schule.SchulDB.Helper;
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

namespace Groll.Schule.SchulDB.Pages
{
    /// <summary>
    /// Interaktionslogik für UserDetailsPage.xaml
    /// </summary>
    public partial class StundenplanEditPage : Page, ISchulDBPage
    {
        private StundenplanToSchulstundeConverter s2sConv = new StundenplanToSchulstundeConverter();
        private ListBox dragSource = null;
        private Point dragStartPoint = new Point();
        private Schueler currentClickedSchüler = null;
        private bool DragMoveStarted = false;
       

        #region ViewModel
        private Groll.Schule.SchulDB.ViewModels.StundenplanEditVM viewModel;

        public Groll.Schule.SchulDB.ViewModels.StundenplanEditVM ViewModel
        {
            get
            {
                if (viewModel == null)
                {
                    viewModel = this.FindResource("ViewModel") as Groll.Schule.SchulDB.ViewModels.StundenplanEditVM;
                    if (viewModel == null)
                        throw new ResourceReferenceKeyNotFoundException();
                }
                return viewModel;
            }
        }
        #endregion

        public StundenplanEditPage()
        {
            InitializeComponent();
        }

        public void SetMainWindow(MainWindow x)
        {
            // Save MainWindow handle
            this.Tag = x;            
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Initialize Stundenplan
            if (ViewModel.Ribbon != null)
            {
                ViewModel.Ribbon.IsContextTabStundenplanVisible = true;                
                ViewModel.Ribbon.TabStundenplan.IsSelected = true;                                
            }
           

        }
        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            if (ViewModel.Ribbon != null)
            {
                ViewModel.Ribbon.IsContextTabStundenplanVisible = false;                
            }
        }
           

        public void OnDatabaseChanged()
        {
           
        }

       
    }
}
