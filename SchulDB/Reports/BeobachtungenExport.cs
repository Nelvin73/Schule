using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Groll.Schule.Model;
using Word = Microsoft.Office.Interop.Word;
using System.ComponentModel;

/*
 * TODO:
 * 
 * - Template verwenden
 * - Sortierung korrekt machen
 * - schöneres Standardformat
 * - Ausgabe gruppiert nach ...
 *     o Klasse -> Schüler -> Datum
 *     o Klasse -> Datum -> Schüler
 * - Auswahl der Daten (Klassen, Schuljahr ...)
 * 
 * 
 * 
 * */

// Schuljahr -> Klasse -> Schüler -> Datum
// Schuljahr -> Klasse -> Datum -> Schüler


namespace Groll.Schule.SchulDB.Reports
{
    public class BeobachtungenExport
    {
        public enum TextBreakType { None, Paragraph, Page }
        public enum GroupByType { GroupByDatum, GroupBySchüler }
        
        private bool useTemplate = false;
        private string templatePath = "";
        private TextBreakType breakNewKlasse = TextBreakType.Page;
        private TextBreakType breakNewSchüler = TextBreakType.Paragraph;
        private TextBreakType breakNewDate = TextBreakType.None;
        private ListSortDirection dateSortDirection = ListSortDirection.Ascending;
        private string reportHeader = "Schülerbeobachtungen";
        private GroupByType groupBy = GroupByType.GroupBySchüler;

       
        

        /// <summary>
        /// Legt fest, ob ein vorhandenes Template für den Export verwendet werden soll
        /// </summary>
        public bool UseTemplate
        {
            get { return useTemplate; }
            set { useTemplate = value; }
        }

        /// <summary>
        /// Legt den Pfad des Templates fest, das verwendet werden soll
        /// </summary>
        public string TemplatePath
        {
            get { return templatePath; }
            set { templatePath = value; }
        }

        /// <summary>
        /// Legt fest, ob nach einer neuen Klasse umgebrochen werden soll
        /// </summary>
        public TextBreakType BreakOnNewKlasse
        {
            get { return breakNewKlasse; }
            set { breakNewKlasse = value; }
        }

        /// <summary>
        /// Legt fest, ob nach einem neuen Schüler umgebrochen werden soll
        /// </summary>        
        public TextBreakType BreakOnNewSchüler
        {
            get { return breakNewSchüler; }
            set { breakNewSchüler = value; }
        }

        /// <summary>
        /// Legt fest, ob nach einem neuen Datum umgebrochen werden soll
        /// </summary>
        public TextBreakType BreakOnNewDate
        {
            get { return breakNewDate; }
            set { breakNewDate = value; }
        }

        /// <summary>
        /// Reihenfolge nach dem das Datum sortiert werden soll
        /// </summary>
        public ListSortDirection DateSortDirection
        {
            get { return dateSortDirection; }
            set { dateSortDirection = value; }
        }

       /// <summary>
       /// Text der als Kopfzeilen-Überschrift verwendet werden soll
       /// </summary>
        public string ReportHeader
        {
            get { return reportHeader; }
            set { reportHeader = value; }
        }

        /// <summary>
        /// Legt fest, ob nach Schüler oder Datum gruppiert werden soll
        /// </summary>
        public GroupByType GroupBy
        {
            get { return groupBy; }
            set { groupBy = value; }
        }
        


        public BeobachtungenExport()
        {
            // Default value
        }

        /// <summary>
        /// Exportiert die übergebenen Beobachtungen in Word
        /// </summary>
        /// <param name="Beobachtungen"></param>
        public void ExportToWord(IEnumerable<Beobachtung> Beobachtungen)
        {
            // Wenn keine Datensätze vorhanden sind, abbrechen
            if (Beobachtungen.Count() == 0)
                return;

            #region Gruppierung / Sortierung der Daten
            // Sortierung zuerst nach Schuljahr ...
            IOrderedEnumerable<Beobachtung> beos = DateSortDirection == ListSortDirection.Ascending ?
                Beobachtungen.OrderBy(x => x.SchuljahrId) : Beobachtungen.OrderByDescending(x => x.SchuljahrId);

            // ... dann nach Klasse
            beos = beos.ThenBy(x => x.Schueler.Klassen.FirstOrDefault(y => y.SchuljahrId == x.SchuljahrId).Name);

            if (GroupBy == GroupByType.GroupBySchüler)
            {
                // ... anschließend nach Schülername
                beos = beos.ThenBy(x => x.Schueler.DisplayName);

                // ... und zuletzt nach Datum (null -> Anfang)
                if (DateSortDirection == ListSortDirection.Ascending)
                    beos = beos.ThenBy(x => x.Datum.HasValue ? x.Datum.Value : DateTime.MinValue);
                else
                    beos = beos.ThenByDescending(x => x.Datum.HasValue ? x.Datum.Value : DateTime.MaxValue);
            }
            else
            {
                // ... oder erst nach Datum (null -> Anfang)
                if (DateSortDirection == ListSortDirection.Ascending)
                    beos = beos.ThenBy(x => x.Datum);
                else
                    beos = beos.ThenByDescending(x => x.Datum ?? DateTime.MaxValue);

                // ... und danach nach Schülername
                beos = beos.ThenBy(x => x.Schueler.DisplayName);
            }
            #endregion

            #region Word Dokument initialisieren
            string FormatGruppenHeader = "Beo_Gruppenname";
            string FormatKlassenHeader = "Beo_Klasse";
            string FormatDataListe = "Beo_Data_Liste";
            string FormatData2Spalten = "Beo_Data_2Spalten";
            
            // Word-Dokument öffnen 
            var app = new Word.ApplicationClass();
            app.Visible = true;
           
            // Document öffnen
            Word.Document doc = app.Documents.Add();

            #region Formatvorlagen prüfen und notfalls anlegen
            try
            {
                // Standard-Format für Klassen-Überschrift
                Word.Style s = doc.Styles.Add(FormatKlassenHeader, Word.WdStyleType.wdStyleTypeParagraph);
                s.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                s.ParagraphFormat.SpaceAfter = 6;
                s.Shading.ForegroundPatternColor = Word.WdColor.wdColorGray35;
                s.Font.Name = "Calibri";
                s.Font.Size = 14;
                s.Font.Bold = -1;
                s.Borders.InsideLineStyle = Word.WdLineStyle.wdLineStyleDouble;
                
                s.set_NextParagraphStyle(doc.Styles["Normal"]);
            }
            catch
            { } 
            
            try
            {
                // Standard-Format für Schülername / Datum
                Word.Style s = doc.Styles.Add(FormatGruppenHeader, Word.WdStyleType.wdStyleTypeParagraph);
                s.Font.Size = 14;
                s.Font.Bold = -1;
                s.Font.Name = "Calibri";
                s.set_NextParagraphStyle(doc.Styles["Normal"]);
                s.ParagraphFormat.SpaceAfter = 6;
                s.ParagraphFormat.SpaceBefore = 12;
            }
            catch
            { }

            try
            {
                // Standard-Format für Liste (ohne Datum)
                Word.Style s = doc.Styles.Add(FormatDataListe, Word.WdStyleType.wdStyleTypeParagraph);
                s.Font.Name = "Calibri";
                s.Font.Size = 10;
                s.Font.Bold = 0;                
                s.LinkToListTemplate(app.ListGalleries[Word.WdListGalleryType.wdBulletGallery].ListTemplates.get_Item(1), 1);
            }
            catch
            { }

            try
            {
                // Standard-Format für Datumseinträge
                Word.Style s = doc.Styles.Add(FormatData2Spalten, Word.WdStyleType.wdStyleTypeParagraph);
                s.Font.Name = "Calibri";
                s.Font.Size = 10;
                s.Font.Bold = 0;
                s.ParagraphFormat.TabStops.Add(70F);
                s.ParagraphFormat.LeftIndent = 70F;
                s.ParagraphFormat.FirstLineIndent = -70F;
            }
            catch
            { }
          
            #endregion

            #region Kopf und Fußzeile
            // Kopf- und Fußzeile anlegen
            Word.HeaderFooter footer = doc.Sections[1].Footers[Word.WdHeaderFooterIndex.wdHeaderFooterPrimary];
            Word.Range r = footer.Range;
            //r.ParagraphFormat.TabStops.Add(200, ref TABleft, ref missing);
            //r.ParagraphFormat.TabStops.Add(300, ref TABcenter, ref missing);
            //r.ParagraphFormat.TabStops.Add(550, ref TABright, ref missing);
            r.Font.Size = 10;
            r.Text = "\t- Seite ";
            r.Collapse(Word.WdCollapseDirection.wdCollapseEnd);
            r.Fields.Add(Range: r, Text: "PAGE", PreserveFormatting: true);
            r.MoveEnd();
            r.InsertAfter("/");
            r.Collapse(Word.WdCollapseDirection.wdCollapseEnd);
            r.Fields.Add(Range: r, Text: "NUMPAGES", PreserveFormatting: true);
            r.MoveEnd();
            r.InsertAfter(" -");

            Word.HeaderFooter header = doc.Sections[1].Headers[Word.WdHeaderFooterIndex.wdHeaderFooterPrimary];
            r = header.Range;
            //r.ParagraphFormat.TabStops.Add(200, ref TABleft, ref missing);
            //r.ParagraphFormat.TabStops.Add(300, ref TABcenter, ref missing);
            //r.ParagraphFormat.TabStops.Add(550, ref TABright, ref missing);
            r.Font.Size = 12;
            r.Text = "\t" + reportHeader + "\t" + DateTime.Now.ToShortDateString();
            #endregion
        
            #endregion
           
            #region Datensätze ausgeben
            // Datensätze durchgehen 
            Schueler lastSchueler = null;
            Klasse lastKlasse = null;
            DateTime? lastDatum = null;
            int lastPageNumber = 0;

            foreach (Beobachtung beo in beos)
            { 
                if (beo.Schueler == null)
                    throw new ArgumentNullException("Schüler in der Beobachtung darf nicht null sein!");

                #region Gruppierungs-Header
                TextBreakType breakDone = TextBreakType.None;            
                
                // Neue Klasse ? --> Kopfzeile bzw. neue Seite 
                var currKlasse = beo.Schueler.Klassen.FirstOrDefault(x => x.SchuljahrId == beo.SchuljahrId);
                if (currKlasse == null)
                {
                    // Der Schüler wurde in keiner Klasse gefunden ?!
                    currKlasse = new Klasse() { Name = "In keiner Klasse" };
                }

                if (currKlasse != lastKlasse)
                {
                    if (lastKlasse != null && breakNewKlasse != TextBreakType.None && breakNewKlasse > breakDone)
                    {
                        // Umbruch nach jeder folgenden Klasse (außer der ersten)
                        if (breakNewKlasse == TextBreakType.Page)
                            app.Selection.InsertBreak(Word.WdBreakType.wdSectionBreakNextPage);
                        else
                            app.Selection.TypeParagraph();

                        breakDone = breakNewKlasse;
                    }

                    // Header ausgeben
                    app.Selection.set_Style(FormatKlassenHeader);
                    app.Selection.TypeText(currKlasse.ToString());
                    app.Selection.TypeParagraph();
                }                               
               
                // Neuer Schüler ? --> Header bzw. neue Seite 
                if (lastSchueler != beo.Schueler && groupBy == GroupByType.GroupBySchüler)
                {
                    if (lastSchueler != null && breakNewSchüler != TextBreakType.None && breakNewSchüler > breakDone)
                    {
                        // Umbruch Bei jedem weiteren Schüler (außer dem ersten)
                        if (breakNewSchüler == TextBreakType.Page)
                            app.Selection.InsertBreak(Word.WdBreakType.wdSectionBreakNextPage);
                        else
                            app.Selection.TypeParagraph();

                        breakDone = breakNewSchüler;
                    }

                    // Schülername ausgeben
                    app.Selection.set_Style(FormatGruppenHeader);
                    app.Selection.TypeText(beo.Schueler.DisplayName);
                    app.Selection.TypeParagraph();

                }

                // Neues Datum? --> Header bzw. neue Seite 
                if ((lastDatum == null || lastDatum.Value.Date != beo.Datum.Value.Date) && groupBy == GroupByType.GroupByDatum)
                {
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
                    app.Selection.set_Style(FormatGruppenHeader);
                    app.Selection.TypeText(beo.Datum == null ? "Allgemein" : beo.Datum.Value.ToShortDateString());
                    app.Selection.TypeParagraph();
                }
               
                // Kopfzeile anpassen, wenn auf neuer Seite
                if ((int)app.Selection.get_Information(Word.WdInformation.wdActiveEndPageNumber) != lastPageNumber &&
                    (lastKlasse != currKlasse || (groupBy == GroupByType.GroupBySchüler && lastSchueler != beo.Schueler) ||
                    (groupBy == GroupByType.GroupByDatum && lastDatum != beo.Datum)))
                {
                    header = app.Selection.Sections[1].Headers[Word.WdHeaderFooterIndex.wdHeaderFooterPrimary];
                    header.LinkToPrevious = false;
                    r = header.Range;
                    r.Text = "\t" + reportHeader + "\t" + DateTime.Now.ToShortDateString() + "\n\t" + currKlasse.ToString() + " - " + 
                        (groupBy == GroupByType.GroupBySchüler ? beo.Schueler.DisplayName : (beo.Datum  == null ? "Allgemein" : beo.Datum.Value.ToShortDateString())) ;
                               
                    
                    lastPageNumber = (int)app.Selection.get_Information(Word.WdInformation.wdActiveEndPageNumber);
                }
 #endregion
                lastKlasse = currKlasse;
                    lastSchueler = beo.Schueler;
                    lastDatum = beo.Datum;
                // Format je nach Beobachtungsart
                if (beo.Datum == null && groupBy == GroupByType.GroupBySchüler)
                    app.Selection.set_Style(FormatDataListe);

                else
                {
                    app.Selection.set_Style(FormatData2Spalten);
                    app.Selection.TypeText((groupBy == GroupByType.GroupBySchüler ? beo.Datum.Value.ToShortDateString() : beo.Schueler.DisplayName) + "\t");
                }

                // Beobachtung ausgeben
                string beoText = beo.Text;
                beoText = beoText.Replace("\r", "");
                beoText = beoText.Replace("\n", "\v") + "\r";
                app.Selection.TypeText(beoText);

            }
            #endregion
        }

        /// <summary>
        /// Exportiert alle Beobachtungen in Word
        /// </summary>
        public void ExportToWork(Schule.DataManager.UowSchuleDB UOW = null)
        {
            if (UOW == null)
                UOW = App.Current.FindResource("UnitOfWork") as Groll.Schule.DataManager.UowSchuleDB;
            
            if (UOW != null)   
                ExportToWord(UOW.Beobachtungen.GetList());
        }
    }
}
