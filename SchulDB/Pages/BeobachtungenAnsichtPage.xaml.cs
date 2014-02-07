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
            FlowDocument doc = new FlowDocument();

            List<Beobachtung> beos = RibbonVM.Default.UnitOfWork.Beobachtungen.GetList().OrderBy ( x=>x.Klasse.Schuljahr.Startjahr).
                ThenBy(y => y.Klasse.Name).ThenBy(z => z.Schueler.DisplayName).ThenBy(zz => zz.Datum ?? DateTime.MinValue).ToList();
            Paragraph p;
                
            // Datensätze durchgehen 
            Schueler lastSchueler = null;
            Klasse lastKlasse = null;
            DateTime? lastDatum = null;

            foreach (Beobachtung beo in beos)
            {
                if (beo.Schueler == null)
                    throw new ArgumentNullException("Schüler in der Beobachtung darf nicht null sein!");

                // Neue Klasse ? --> Kopfzeile bzw. neue Seite 
                var currKlasse = beo.Klasse ?? new Klasse() { Name = "In keiner Klasse" };
                if (currKlasse != lastKlasse)
                {
                    // Neue Klasse
                    if (lastKlasse != null)
                    {
                        // Umbruch nach jeder folgenden Klasse (außer der ersten)

                        // TODO Umbruch ?????
                    }

                    p = new Paragraph() { FontSize = 16, Foreground = Brushes.Red };
                    p.Inlines.Add(new Bold(new Run(currKlasse.ToString())));
                    doc.Blocks.Add(p);
                }

                if (lastSchueler != beo.Schueler)
                {
                    // Neuer Schüler
                    if (lastSchueler != null)
                    {
                        // Umbruch Bei jedem weiteren Schüler (außer dem ersten)

                        // TODO: Umbruch ???
                    }

                    p = new Paragraph() { FontSize = 14, Foreground = Brushes.Blue };
                    p.Inlines.Add(new Bold(new Run(beo.Schueler.DisplayName)));
                    doc.Blocks.Add(p);
                    lastDatum = null;
                }

                // Text ausgeben
                p = new Paragraph() { Tag = beo.BeobachtungId };
                
                if (!lastDatum.HasValue || lastDatum.Value.Date != beo.Datum.Value.Date)
                    p.Inlines.Add( new Run(beo.Datum.Value.ToString("dd.MM.yyyy")));
                
                p.Inlines.Add(new Run("\t" + beo.Text));                
                doc.Blocks.Add(p);
                /*// Beobachtung ausgeben
                
                string beoText = beo.Text;
                beoText = beoText.Replace("\r", "");
                beoText = beoText.Replace("\n", "\v") + "\r";
                */
                lastKlasse = currKlasse;
                lastSchueler = beo.Schueler;
                lastDatum = beo.Datum;

#region Müll
                /*
                TextBreakType breakDone = TextBreakType.None;

               


                // Neues Datum? --> Header bzw. neue Seite 
                if ((lastDatum == null || lastDatum.Value.Date != beo.Datum.Value.Date)) // && groupBy == GroupByType.GroupByDatum)
                {
                    // Neues Datum
                    if (lastDatum != null && breakNewDate != TextBreakType.None && breakNewDate > breakDone)
                    {
                        // Umbruch Bei jedem weiteren Datum (außer dem ersten)
                        if (breakNewDate == TextBreakType.Page)
                            app.Selection.InsertBreak(Word.WdBreakType.wdSectionBreakNextPage);
                        else
                            app.Selection.TypeParagraph();

                        breakDone = breakNewDate;
                    }

                    // Datum ausgeben
                    if (groupBy == GroupByType.GroupByDatum)
                    {
                        app.Selection.set_Style(FormatGruppenHeader);
                        app.Selection.TypeText(beo.Datum == null ? "Allgemein" : beo.Datum.Value.ToString("dd.MM.yyyy"));
                        app.Selection.TypeParagraph();
                    }
                }

                // Absatz auf jeden Fall nach Eintrag ?
                if (paragraphAfterEveryEntry && breakDone == TextBreakType.None)
                    app.Selection.TypeParagraph();

                // Kopfzeile anpassen, wenn auf neuer Seite
                if ((int)app.Selection.get_Information(Word.WdInformation.wdActiveEndPageNumber) != lastPageNumber &&
                    (lastKlasse != currKlasse || (groupBy == GroupByType.GroupBySchüler && lastSchueler != beo.Schueler) ||
                    (groupBy == GroupByType.GroupByDatum && lastDatum != beo.Datum)))
                {
                    header = app.Selection.Sections[1].Headers[Word.WdHeaderFooterIndex.wdHeaderFooterPrimary];
                    header.LinkToPrevious = false;
                    r = header.Range;
                    r.Text = "\t" + reportHeader + "\t" + DateTime.Now.ToShortDateString() + "\n\t" + currKlasse.ToString() + " - " +
                        (groupBy == GroupByType.GroupBySchüler ? beo.Schueler.DisplayName : (beo.Datum == null ? "Allgemein" : beo.Datum.Value.ToShortDateString()));


                    lastPageNumber = (int)app.Selection.get_Information(Word.WdInformation.wdActiveEndPageNumber);
                }
                #endregion
                // Format je nach Beobachtungsart
                if (beo.Datum == null && groupBy == GroupByType.GroupBySchüler)
                    app.Selection.set_Style(FormatDataListe);

                else
                {
                    app.Selection.set_Style(FormatData2Spalten);
                    if (groupBy == GroupByType.GroupBySchüler)
                    {
                        if (lastDatum.Value.Date != beo.Datum.Value.Date || repeatSameName)
                            app.Selection.TypeText(beo.Datum.Value.ToString("dd.MM.yyyy"));
                        app.Selection.TypeText("\t");
                    }
                    else
                    {
                        if (lastSchueler != beo.Schueler || repeatSameName)
                            app.Selection.TypeText(beo.Schueler.DisplayName);
                        app.Selection.TypeText("\t");
                    }
                }

                // Beobachtung ausgeben
                string beoText = beo.Text;
                beoText = beoText.Replace("\r", "");
                beoText = beoText.Replace("\n", "\v") + "\r";
                app.Selection.TypeText(beoText);

                lastKlasse = currKlasse;
                lastSchueler = beo.Schueler;
                lastDatum = beo.Datum;

            }
            #endregion
                */
#endregion
            }

            Reader.Document = doc;
        }

        private void Reader_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var i = Reader.Selection.Start.Paragraph as Paragraph;
            if (i.Tag != null)
                MessageBox.Show(i.Tag.ToString());
            
        }
    }
}
