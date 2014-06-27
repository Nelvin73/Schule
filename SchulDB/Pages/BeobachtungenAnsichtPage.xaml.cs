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


/*
 * TODO: 
 * Ribbon: aufhübschen, Button zum Refresh, Sortierung, Filterung
 * Bei Änderung und Neu erstellen zurückspringen zur BEO.
 * 
 * */


namespace Groll.Schule.SchulDB.Pages
{
    /// <summary>
    /// Interaktionslogik für UserDetailsPage.xaml
    /// </summary>

    public partial class BeobachtungenAnsichtPage : Page, ISchulDBPage
    {

        #region ViewModel
        private Groll.Schule.SchulDB.ViewModels.BeobachtungenEditVM viewModel;

        public Groll.Schule.SchulDB.ViewModels.BeobachtungenEditVM ViewModel
        {
            get
            {
                if (viewModel == null)
                {
                    viewModel = this.FindResource("ViewModel") as Groll.Schule.SchulDB.ViewModels.BeobachtungenEditVM;
                    if (viewModel == null)
                        throw new ResourceReferenceKeyNotFoundException();
                }
                return viewModel;
            }
        }
        #endregion

        public BeobachtungenAnsichtPage()
        {
            InitializeComponent();

            // Command Bindings
            
        }
        

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


        #region Navigation / Initialization
        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            // Navigated away from Page
            ViewModel.Ribbon.IsContextTabBeobachtungenVisible = false;
            ViewModel.Ribbon.TabBeobachtungenAnsicht.IsVisible = false;                     
        }

        private void Page_Initialized(object sender, EventArgs e)
        {
            // Page is first time initialized

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Navigated toward Page
            // Load FlowDocument
            ViewModel.Ribbon.IsContextTabBeobachtungenVisible = true;
            ViewModel.Ribbon.TabBeobachtungenAnsicht.IsVisible = true;
            ViewModel.Ribbon.TabBeobachtungenAnsicht.IsSelected = true;
            EditBox.Focus();         
        }
        #endregion

        private void Reader_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // activate edit mode for selected paragraph
            ViewModel.StartEdit(Reader.Selection.Start.Paragraph);
        }
       

        private void Document_Click(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Debug.Print(e.OriginalSource.GetType().ToString());
            
            // Try to get paragraph
            Paragraph p = e.OriginalSource as Paragraph;
            if (p == null)
            {
                // no paragraph clicked .. try to find parent paragraph
                var run = e.OriginalSource as Run;
                if (run != null)
                    p = run.Parent as Paragraph;
            }
            
            if (ViewModel.IsEditMode && p != null)
            {                
                ViewModel.StartEdit(p);
            }            
        }
       
       
       
       
    }
}
