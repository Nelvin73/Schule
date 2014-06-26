using Groll.Schule.Model;
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
using Groll.Schule.SchulDB.Commands;
using Groll.Schule.SchulDB.ViewModels;

namespace Groll.Schule.SchulDB.Pages
{
    /// <summary>
    /// Interaktionslogik für UserDetailsPage.xaml
    /// </summary>

    public partial class BeobachtungenEingabePage : Page, ISchulDBPage
    {

        #region ViewModel
        private Groll.Schule.SchulDB.ViewModels.BeobachtungenEingabeVM viewModel;

        public Groll.Schule.SchulDB.ViewModels.BeobachtungenEingabeVM ViewModel
        {
            get
            {
                if (viewModel == null)
                {
                    viewModel = this.FindResource("ViewModel") as Groll.Schule.SchulDB.ViewModels.BeobachtungenEingabeVM;
                    if (viewModel == null)
                        throw new ResourceReferenceKeyNotFoundException();
                }
                return viewModel;
            }
        }
        #endregion

        public BeobachtungenEingabePage()
        {
            InitializeComponent();
            
            // Command Bindings
            SchuleCommands.Beobachtungen.InsertText = new DelegateCommand((a) => InsertText(a));
            SchuleCommands.Beobachtungen.InsertTextbaustein = new DelegateCommand((a) => InsertTextbaustein(a));            
        }

        #region ICommand implementierungen
        
        private void InsertText(object a)
        {
            string t = (a ?? "").ToString();

            if (txtBeoText.IsSelectionActive)
            {
                txtBeoText.SelectedText = t;               
            }
        }

        private void InsertTextbaustein(object a)
        {
            if (a == null)
                return;

            int i = (int)a;

            if (txtBeoText.IsSelectionActive)
            {
                Textbaustein t = ViewModel.UnitOfWork.Textbausteine.GetById(i);
                if (t != null)
                {
                    t.UsageCount++;
                    InsertText(t.Text);
                }

            }
        }                        
      
        #endregion

        #region ISchulDBPage Implementierung
        public void SetMainWindow(MainWindow x)
        {
            // Save MainWindow handle
            this.Tag = x;
        }

        public void OnDatabaseChanged()
        {
            viewModel.Refresh();
        }
        #endregion

      
        #region Filter in Schüler-Liste
        // Liste neuladen beim Ändern des Filters
        private void Filter_TextChanged(object sender, TextChangedEventArgs e)
        {
            var s = FindResource("SchülerListeViewSource") as CollectionViewSource;
            s.View.Refresh();
            if (cbSchülerListe.Items.Count > 0)
                cbSchülerListe.SelectedIndex = 0;
        }

        // Filter
        void s_Filter(object sender, FilterEventArgs e)
        {
            var i = e.Item as Schueler;
            e.Accepted = i != null && (Filter.Text == "" || i.DisplayName.ToLower().Contains(Filter.Text.ToLower()));
        }

        #endregion

        #region Navigation / Initialization
        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            // Navigated away from Page
            // Hide Context Tab
            if (ViewModel.Ribbon != null)
            {
                ViewModel.Ribbon.IsContextTabBeobachtungenVisible = false;
                ViewModel.Ribbon.TabBeobachtungen.IsVisible = false;
            }
        }

        private void Page_Initialized(object sender, EventArgs e)
        {
            // Page is first time initialized
            
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Navigated toward Page
            if (ViewModel.Ribbon != null)
            {
                ViewModel.Ribbon.IsContextTabBeobachtungenVisible = true;
                ViewModel.Ribbon.TabBeobachtungen.IsSelected = true;
                ViewModel.Ribbon.TabBeobachtungen.IsVisible = true;
                txtBeoText.Focus();
            }
        }
        #endregion

       /* private void txtBeoText_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            var i = e.Source as TextBox;

        } */

        private void cbSchülerListe_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // SelectedItems kann nicht gebunden werden => Auswahl geändert ... SelectedSchülerList manuell anpassen
            ViewModel.SelectedSchülerList = cbSchülerListe.SelectedItems.Cast<Schueler>().ToList();
        }

       



       
    }
}
