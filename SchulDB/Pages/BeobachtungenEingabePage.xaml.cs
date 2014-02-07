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
            this.CommandBindings.AddRange(new List<CommandBinding>
                {
                    new CommandBinding(BeobachtungenCommands.ClearInput, Executed_ClearInput, BasicCommands.CanExecute_TRUE),
                    new CommandBinding(BeobachtungenCommands.Add, Executed_Add, CanExecute_Add),
                    new CommandBinding(BeobachtungenCommands.InsertText, Executed_InsertText, BasicCommands.CanExecute_TRUE),
                    new CommandBinding(BeobachtungenCommands.ExportBeobachtungen, Executed_Export, BasicCommands.CanExecute_TRUE),
                    new CommandBinding(BeobachtungenCommands.HistoryViewChanged, Executed_HistoryViewChanged, BasicCommands.CanExecute_TRUE)
                });
        }

        
       
       
       



        #region ICommand implementierungen
        private void Executed_HistoryViewChanged(object sender, ExecutedRoutedEventArgs e)
        {
            var type = ViewModels.BeobachtungenEingabeVM.BeoHistoryMode.LastEntered;
            switch (e.Parameter.ToString())
            {
                case "Schüler":
                    type = ViewModels.BeobachtungenEingabeVM.BeoHistoryMode.CurrentSchueler;
                    break;
                case "Datum":
                    type = ViewModels.BeobachtungenEingabeVM.BeoHistoryMode.SortDate;
                    break;
                default: // "ID"
                    break;
            }
            ViewModel.BeobachtungenHistoryType = type;
        }


        private void Executed_InsertText(object sender, ExecutedRoutedEventArgs e)
        {
            // Text aus History einfügen
            if (txtBeoText.IsSelectionActive)
            {
                string x = txtBeoText.Text;
                x = x.Remove(txtBeoText.SelectionStart, txtBeoText.SelectionLength).Insert(txtBeoText.SelectionStart, e.Parameter.ToString());
                txtBeoText.Text = x;
            }   
        }

        private void Executed_ClearInput(object sender, ExecutedRoutedEventArgs e)
        {
            // Eingabe im Model löschen
            ViewModel.ClearInput();

            // Anzeigefilter zurücksetzen
            Filter.Text = "";

            // Falls Textfeld Fokus hat, auch den angezeigten Text löschen
            if (txtBeoText.IsFocused)
                txtBeoText.Text = "";
        }

        private Reports.BeobachtungenExport.TextBreakType ConvertToTextBreakType(object o)
        {
            string tag = "";
            if (o is RibbonBaseVM)
                tag = ((RibbonBaseVM)o).Tag.ToString();
            else
                tag = o.ToString();

            switch (tag)
            {
                case "Seite":
                    return Reports.BeobachtungenExport.TextBreakType.Page;                    
                case "Absatz":
                    return Reports.BeobachtungenExport.TextBreakType.Paragraph;                   
                default:
                    return Reports.BeobachtungenExport.TextBreakType.None;
            }
        }

        private void Executed_Export(object sender, ExecutedRoutedEventArgs e)
        {          
            // Get settings from Ribbon               
            var vm = RibbonVM.Default.TabBeobachtungen;
            var exp = new Reports.BeobachtungenExport();

            exp.DateSortDirection = vm.SelectedSorting.Tag == null || vm.SelectedSorting.Tag.ToString() == "ASC" ? System.ComponentModel.ListSortDirection.Ascending : System.ComponentModel.ListSortDirection.Descending;
            exp.BreakOnNewKlasse = ConvertToTextBreakType(vm.SelectedTextBreakKlasse);
            exp.BreakOnNewSchüler = ConvertToTextBreakType(vm.SelectedTextBreakSchueler);
            exp.BreakOnNewDate = ConvertToTextBreakType(vm.SelectedTextBreakDatum);
            exp.GroupBy = vm.SelectedSorting.Tag == null || vm.SelectedGrouping.Tag.ToString() == "S" ? Reports.BeobachtungenExport.GroupByType.GroupBySchüler : Reports.BeobachtungenExport.GroupByType.GroupByDatum;
            exp.ParagraphAfterEveryEntry = vm.ParagraphAfterEveryEntry;
            exp.RepeatSameName = vm.RepeatSameName;

            switch (vm.FilterMenuButton.Tag.ToString())
            {
                case "ALL":   // Alle
                    exp.ExportToWord();
                    break;
                case "SJ": // Aktuelles Schuljahr
                    exp.ExportToWord(ViewModel.CurrentSJ);
                    break;
                case "KL": // Aktuelle Klasse
                    exp.ExportToWord(ViewModel.SelectedKlasse);
                    break;
                case "SSJ":  // Aktueller Schüler (nur dieses Schuljahr)
                    exp.ExportToWord(ViewModel.SelectedSchüler, ViewModel.CurrentSJ);
                    break;
                case "SCH":  // Aktueller Schüler (Komplett)                
                    exp.ExportToWord(ViewModel.SelectedSchüler);
                    break;
            }

       
        }

        private void Executed_Add(object sender, ExecutedRoutedEventArgs e)
        {            
            ViewModel.AddCurrentComment((e.Parameter ?? "").ToString() != "noClear");            
        }

        private void CanExecute_Add(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ViewModel.ValidateCurrent();
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ViewModel.AddCurrentComment();
        }

        // Liste neuladen beim Ändern des Filters
        private void Filter_TextChanged(object sender, TextChangedEventArgs e)
        {
            var s = TryFindResource("SchülerListeViewSource") as CollectionViewSource;
            s.View.Refresh();
        }

        // Filter
        void s_Filter(object sender, FilterEventArgs e)
        {
            var i = e.Item as Schueler;
            e.Accepted = i != null && (Filter.Text == "" || i.DisplayName.ToLower().Contains(Filter.Text.ToLower()));
        }


        #region Navigation / Initialization
        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            // Navigated away from Page
            // Hide Context Tab
            var mw = Tag as MainWindow;
            if (mw != null && mw.RibbonVM != null)            
                mw.RibbonVM.TabBeobachtungen.IsVisible = false;                                       
        }

        private void Page_Initialized(object sender, EventArgs e)
        {
            // Page is first time initialized
            
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Navigated toward Page
            var mw = Tag as MainWindow;
            if (mw != null && mw.RibbonVM != null)
            {
                ViewModels.RibbonVM.Default.TabBeobachtungen.IsSelected = ViewModels.RibbonVM.Default.TabBeobachtungen.IsVisible = true;
                mw.RibbonVM.TabBeobachtungen.IsSelected = mw.RibbonVM.TabBeobachtungen.IsVisible = true;
                txtBeoText.Focus();
            }
        }
        #endregion



       
    }
}
