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

    public partial class BeobachtungenAnsichtPage : Page, ISchulDBPage
    {

        #region ViewModel
        private Groll.Schule.SchulDB.ViewModels.BeobachtungenAnsichtVM viewModel;

        public Groll.Schule.SchulDB.ViewModels.BeobachtungenAnsichtVM ViewModel
        {
            get
            {
                if (viewModel == null)
                {
                    viewModel = this.FindResource("ViewModel") as Groll.Schule.SchulDB.ViewModels.BeobachtungenAnsichtVM;
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

        }

        private void Page_Initialized(object sender, EventArgs e)
        {
            // Page is first time initialized

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Navigated toward Page
            // Load FlowDocument

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

            foreach (Beobachtung beo in beos)
            {
                if (beo.Schueler == null)
                    throw new ArgumentNullException("Schüler in der Beobachtung darf nicht null sein!");

                // Neue Klasse ? --> Kopfzeile bzw. neue Seite 
                var currKlasse = beo.Klasse ?? new Klasse() { Name = "" };
                if (currKlasse != lastKlasse)
                {
                    // Neue Klasse
                    p = new Paragraph() { FontSize = 16, Foreground = Brushes.Red };

                    if (lastKlasse != null)
                        p.BreakPageBefore = true;

                    p.Inlines.Add(new Bold(new Run(currKlasse.ToString())));
                    doc.Blocks.Add(p);
                }

                if (lastSchueler != beo.Schueler)
                {
                    // Neuer Schüler
                    p = new Paragraph() { FontSize = 14, Foreground = Brushes.Blue };
                    if (lastSchueler != null)
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

        private void StartEdit(Paragraph p)
        {
            if (p == null || p.Tag == null || !(p.Tag is int))
                return;
            
            Beobachtung b = RibbonVM.Default.UnitOfWork.Beobachtungen.GetById((int)p.Tag);
            if (b == null)
                return;
            
            ViewModel.EditedBeobachtung = b;
            EditBox.Tag = p;            
        }

        private void Reader_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Get Beobachtungen ID from current Selection
            var i = Reader.Selection.Start.Paragraph as Paragraph;
            StartEdit(i);
        }

        private void btChange_Click(object sender, RoutedEventArgs e)
        {
            // Update View
            var i = EditBox.Tag as Paragraph;
            if (i != null && ViewModel.EditedBeobachtung != null)
            {
                // Schüler geändert ? => Komplett neu laden
                if (ViewModel.EditedBeobachtung.Schueler != ViewModel.SelectedSchüler)
                {
                    ViewModel.UpdateBeobachtung(); 
                    LoadDocument();
                }
                else
                {
                    if (i.Inlines.Count == 2)
                        ((Run)i.Inlines.FirstInline).Text = (ViewModel.BeoDatum.HasValue ? ViewModel.BeoDatum.Value.ToString("dd.MM.yyyy") : "Kein Datum") + "\t";
                    else
                        i.Inlines.InsertBefore(i.Inlines.FirstInline, new Run((ViewModel.BeoDatum.HasValue ? ViewModel.BeoDatum.Value.ToString("dd.MM.yyyy") : "Kein Datum") + "\t"));

                    i.TextIndent = -70F;

                    ((Run)i.Inlines.LastInline).Text = ViewModel.BeoText;
                    // Update DB
                    ViewModel.UpdateBeobachtung();
                }

                // Update DB
                ViewModel.EditedBeobachtung = null;
            }
        }

        private void btCancel_Click(object sender, RoutedEventArgs e)
        {
            EditBox.Tag = null;
            ViewModel.EditedBeobachtung = null;
        }

        private void Reader_MouseUp(object sender, MouseButtonEventArgs e)
        {
            // Wenn bereits edit-Mode an ist, andere Beobachtung selektieren
            if (EditBox.Text != "")
            {
                var i = Reader.Selection.Start.Paragraph as Paragraph;
                StartEdit(i);        
            }
        }

    }
}
