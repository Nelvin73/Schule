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
            this.CommandBindings.AddRange(new List<CommandBinding>
            {
                new CommandBinding(BeobachtungenCommands.UpdateBeobachtungenView, Executed_UpdateView, BasicCommands.CanExecute_TRUE),
                new CommandBinding(BeobachtungenCommands.EditModeChanged, Executed_EditModeChanged, BasicCommands.CanExecute_TRUE),            
            });
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
            ViewModels.RibbonVM.Default.IsContextTabBeobachtungenVisible = false;            
            ViewModels.RibbonVM.Default.TabBeobachtungenAnsicht.IsVisible = false;                     
        }

        private void Page_Initialized(object sender, EventArgs e)
        {
            // Page is first time initialized

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Navigated toward Page
            // Load FlowDocument
            ViewModels.RibbonVM.Default.IsContextTabBeobachtungenVisible = true;
            ViewModels.RibbonVM.Default.TabBeobachtungenAnsicht.IsSelected = true;
            ViewModels.RibbonVM.Default.TabBeobachtungenAnsicht.IsVisible = true;
            EditBox.Focus();
            if (Reader.Document == null)
                LoadDocument();
        }
        #endregion

        private void LoadDocument()
        {
            // Dokument konfigurieren
            FlowDocument doc = new FlowDocument();
            doc.ColumnWidth = 600;
            doc.ColumnGap = 50;
            doc.ColumnRuleBrush = Brushes.Red;
            doc.ColumnRuleWidth = 2;
            doc.IsColumnWidthFlexible = true;
            doc.FontSize = 10;
            doc.FontFamily = new FontFamily("Verdana");

            List<Beobachtung> beos = RibbonVM.Default.UnitOfWork.Beobachtungen.GetList().
                OrderBy(x => x.SchuljahrId).
                ThenBy(y => y.Klasse == null ? "" : y.Klasse.Name).
                ThenBy(z => z.Schueler == null ? "" : z.Schueler.DisplayName).
                ThenBy(zz => zz.Datum ?? DateTime.MinValue).ToList();

            Paragraph p;

            // Datensätze durchgehen 
            Schueler lastSchueler = null;
            Klasse lastKlasse = null;
            DateTime? lastDatum = DateTime.MinValue;
            Klasse nullKlasse = new Klasse() { Name = "" };
            

            foreach (Beobachtung beo in beos)
            {
                bool newPage = false;
                if (beo.Schueler == null)
                    throw new ArgumentNullException("Schüler in der Beobachtung darf nicht null sein!");

                // Neue Klasse ? --> Kopfzeile bzw. neue Seite 
                var currKlasse = beo.Klasse ?? nullKlasse;
                if (currKlasse != lastKlasse)
                {
                    // Neue Klasse
                    p = new Paragraph() { FontSize = 16, Foreground = Brushes.Red };

                    if (lastKlasse != null)
                        newPage = p.BreakPageBefore = true;

                    p.Inlines.Add(new Bold(new Run(currKlasse.ToString())));
                    doc.Blocks.Add(p);
                }

                if (lastSchueler != beo.Schueler)
                {
                    // Neuer Schüler
                    p = new Paragraph() { FontSize = 14, Foreground = Brushes.Blue };
                    if (lastSchueler != null && RibbonVM.Default.TabBeobachtungenAnsicht.NewPageOnSchüler && !newPage)
                        p.BreakPageBefore = true;

                    p.Inlines.Add(new Bold(new Run(beo.Schueler.DisplayName)));
                    doc.Blocks.Add(p);
                    lastDatum = null;
                }

                // Text ausgeben
                p = new Paragraph() { Tag = beo.BeobachtungId };
                p.Margin = new Thickness(70F, 0, 0, 0);

                if (!beo.Datum.HasValue && lastDatum.HasValue)
                {
                    p.Inlines.Add(new Run("Kein Datum\t"));
                    p.TextIndent = -70F;
                }
                else if (beo.Datum.HasValue && (!lastDatum.HasValue || lastDatum.Value.Date != beo.Datum.Value.Date))
                {
                    p.Inlines.Add(new Run(beo.Datum.Value.ToString("dd.MM.yyyy") + "\t"));
                    p.TextIndent = -70F;
                }


                p.Inlines.Add(new Run(beo.Text));
                doc.Blocks.Add(p);
                /*// Beobachtung ausgeben
                
                string beoText = beo.Text;
                beoText = beoText.Replace("\r", "");
                beoText = beoText.Replace("\n", "\v") + "\r";
                */
                lastKlasse = currKlasse;
                lastSchueler = beo.Schueler;
                lastDatum = beo.Datum;
            }

            Reader.Document = doc;
        }

        private void StartEdit(Paragraph p = null)
        {
            // Edit Mode EIN
            RibbonVM.Default.TabBeobachtungenAnsicht.EditMode = ViewModel.IsEditMode = true;

            // Markierung von vorherigen Paragraph zurücksetzen            
            var oldP = EditBox.Tag as Paragraph;
            if (oldP == p)
                return;           
            
            if (oldP != null)            
                oldP.Background = null;            

            // Edit von Beobachtung aktivieren
            EditBox.Tag = null;
            if (p == null || p.Tag == null || !(p.Tag is int))
            {
                // Aktive Beobachtung entfernen                
                ViewModel.ResetBeobachtung();
            }
            else
            {
                Beobachtung b = RibbonVM.Default.UnitOfWork.Beobachtungen.GetById((int)p.Tag);
                if (b != null)
                {
                    p.Background = Brushes.Yellow;
                    ViewModel.EditedBeobachtung = b;
                    EditBox.Tag = p;
                }
            }
                   
        }

        private void CancelEdit()
        {            
            // Edit Mode AUS
            RibbonVM.Default.TabBeobachtungenAnsicht.EditMode = ViewModel.IsEditMode = false;
            
            // Evtl. Markierung wegnehmen
            var oldP = EditBox.Tag as Paragraph;           
            if (oldP != null)
                oldP.Background = null; 

            // Aktive Beobachtung entfernen
            EditBox.Tag = null;
            ViewModel.ResetBeobachtung();
        }

        private void Reader_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Get Beobachtungen ID from current Selection             
            StartEdit(Reader.Selection.Start.Paragraph as Paragraph);
        }

        private void btChange_Click(object sender, RoutedEventArgs e)
        {
            // Update View
            var p = EditBox.Tag as Paragraph;
            if (p != null && ViewModel.EditedBeobachtung != null)
            {
                bool Reload = ViewModel.IsSchülerChanged || ViewModel.IsSchuljahrChanged;
                ViewModel.UpdateBeobachtung();
                UpdateDocument(p, Reload);    
                
                            
            }
        }

        private void Document_Click(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Debug.Print(e.OriginalSource.GetType().ToString());
            Paragraph p = e.OriginalSource as Paragraph;
            if (p == null)
            {
                var run = e.OriginalSource as Run;
                if (run != null)
                    p = run.Parent as Paragraph;
            }

            if (ViewModel.IsEditMode && p != null)
            {                
                StartEdit(p);
            }            
        }
        private void UpdateDocument(Paragraph p, bool Reload = false)
        {
            if (p == null || ViewModel.EditedBeobachtung == null)
                // nothing to update
                return;

            if (Reload)
                    // Komplett neu laden
                LoadDocument();
                    
            else
            {
                // Inline ändern
                string Text = (ViewModel.BeoDatum.HasValue ? ViewModel.BeoDatum.Value.ToString("dd.MM.yyyy") : "Kein Datum") + "\t";
                if (p.Inlines.Count == 2)
                    ((Run)p.Inlines.FirstInline).Text = Text;
                else
                    p.Inlines.InsertBefore(p.Inlines.FirstInline, new Run(Text));

                p.TextIndent = -70F;
                ((Run)p.Inlines.LastInline).Text = ViewModel.BeoText;                   
            }               
        }

        private void btCancel_Click(object sender, RoutedEventArgs e)
        {
            CancelEdit();
        }

        
       
        #region Commands
        // View muss aktualisiert werden
        private void Executed_UpdateView(object sender, ExecutedRoutedEventArgs e)
        {
            LoadDocument();
        }

        // Editmode über Ribbon geändert
        private void Executed_EditModeChanged(object sender, ExecutedRoutedEventArgs e)
        {
            if (RibbonVM.Default.TabBeobachtungenAnsicht.EditMode)
                StartEdit();
            else
                CancelEdit();            
        }

       

        #endregion
    }
}
