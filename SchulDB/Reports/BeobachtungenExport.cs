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
 * 
 * */



namespace Groll.Schule.SchulDB.Reports
{
    public class BeobachtungenExport
    {
        // Settings
        public class BeobachtungenExportSettings
        {
            public TextBreakType TextBreakNewSchüler { get; set; }
            public TextBreakType TextBreakNewKlasse { get; set; }
            public TextBreakType TextBreakNewDatum { get; set; }
            public ListSortDirection DateSortDirection { get; set; }    // Default = Ascending
            public GroupByType GroupBy { get; set; }    // Default = Schüler 
            public string TemplateFile { get; set; }
            public string Header { get; set; }
            public bool ParagraphAfterEveryEntry { get; set; }
            public bool RepeatSameName { get; set; }

            public BeobachtungenExportSettings()
            {
                Header = "Schülerbeobachtungen";
            }
        }

        public enum TextBreakType { None, Paragraph, Page }
        public enum GroupByType { GroupBySchüler, GroupByDatum  }
        
   
        public BeobachtungenExportSettings ExportSettings { get; set; }
      
        public BeobachtungenExport(BeobachtungenExportSettings Settings = null)
        {
            ExportSettings = Settings ?? new BeobachtungenExportSettings();
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
            IOrderedEnumerable<Beobachtung> beos = ExportSettings.DateSortDirection == ListSortDirection.Ascending ?
                Beobachtungen.OrderBy(x => x.SchuljahrId) : Beobachtungen.OrderByDescending(x => x.SchuljahrId);

            // ... dann nach Klasse
            beos = beos.ThenBy(x => x.Schueler.Klassen.FirstOrDefault(y => y.SchuljahrId == x.SchuljahrId).Name);

            if (ExportSettings.GroupBy == GroupByType.GroupBySchüler)
            {
                // ... anschließend nach Schülername
                beos = beos.ThenBy(x => x.Schueler.DisplayName);

                // ... und zuletzt nach Datum (null -> Anfang)
                if (ExportSettings.DateSortDirection == ListSortDirection.Ascending)
                    beos = beos.ThenBy(x => x.Datum.HasValue ? x.Datum.Value : DateTime.MinValue);
                else
                    beos = beos.ThenByDescending(x => x.Datum.HasValue ? x.Datum.Value : DateTime.MaxValue);
            }
            else
            {
                // ... oder erst nach Datum (null -> Anfang)
                if (ExportSettings.DateSortDirection == ListSortDirection.Ascending)
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
                float Indent = ExportSettings.GroupBy == GroupByType.GroupBySchüler ? 70F : 120F;
                s.ParagraphFormat.TabStops.Add(Indent);
                s.ParagraphFormat.LeftIndent = Indent;
                s.ParagraphFormat.FirstLineIndent = -Indent;
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
            r.Text = "\t" + ExportSettings.Header + "\t" + DateTime.Now.ToShortDateString();
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
                    // Neue Klasse
                    if (lastKlasse != null && ExportSettings.TextBreakNewKlasse != TextBreakType.None && ExportSettings.TextBreakNewKlasse > breakDone)
                    {
                        // Umbruch nach jeder folgenden Klasse (außer der ersten)
                        if (ExportSettings.TextBreakNewKlasse == TextBreakType.Page)
                            app.Selection.InsertBreak(Word.WdBreakType.wdSectionBreakNextPage);
                        else
                            app.Selection.TypeParagraph();

                        breakDone = ExportSettings.TextBreakNewKlasse;
                    }

                    // Header ausgeben
                    app.Selection.set_Style(FormatKlassenHeader);
                    app.Selection.TypeText(currKlasse.ToString());
                    app.Selection.TypeParagraph();
                }                               
               
                // Neuer Schüler ? --> Header bzw. neue Seite 
                if (lastSchueler != beo.Schueler) // && groupBy == GroupByType.GroupBySchüler)
                {
                    // Neuer Schüler
                    if (lastSchueler != null && ExportSettings.TextBreakNewSchüler != TextBreakType.None && ExportSettings.TextBreakNewSchüler > breakDone)
                    {
                        // Umbruch Bei jedem weiteren Schüler (außer dem ersten)
                        if (ExportSettings.TextBreakNewSchüler == TextBreakType.Page)
                            app.Selection.InsertBreak(Word.WdBreakType.wdSectionBreakNextPage);
                        else
                            app.Selection.TypeParagraph();

                        breakDone = ExportSettings.TextBreakNewSchüler;
                    }

                    // Schülername ausgeben
                    if (ExportSettings.GroupBy == GroupByType.GroupBySchüler)
                    {
                        app.Selection.set_Style(FormatGruppenHeader);
                        app.Selection.TypeText(beo.Schueler.DisplayName);
                        app.Selection.TypeParagraph();
                        lastDatum = null;
                    }
                }

                // Neues Datum? --> Header bzw. neue Seite 
                if ((lastDatum == null || lastDatum.Value.Date != beo.Datum.Value.Date)) // && groupBy == GroupByType.GroupByDatum)
                {
                    // Neues Datum
                    if (lastDatum != null && ExportSettings.TextBreakNewDatum != TextBreakType.None && ExportSettings.TextBreakNewDatum > breakDone)
                    {
                        // Umbruch Bei jedem weiteren Datum (außer dem ersten)
                        if (ExportSettings.TextBreakNewDatum == TextBreakType.Page)
                            app.Selection.InsertBreak(Word.WdBreakType.wdSectionBreakNextPage);
                        else
                            app.Selection.TypeParagraph();

                        breakDone = ExportSettings.TextBreakNewDatum;
                    }

                    // Datum ausgeben
                    if (ExportSettings.GroupBy == GroupByType.GroupByDatum)
                    {
                        app.Selection.set_Style(FormatGruppenHeader);
                        app.Selection.TypeText(beo.Datum == null ? "Allgemein" : beo.Datum.Value.ToString("dd.MM.yyyy"));
                        app.Selection.TypeParagraph();
                        lastSchueler = null;
                    }
                }

                // Absatz auf jeden Fall nach Eintrag ?
                if (ExportSettings.ParagraphAfterEveryEntry && breakDone == TextBreakType.None)
                    app.Selection.TypeParagraph();
               
                // Kopfzeile anpassen, wenn auf neuer Seite
                if ((int)app.Selection.get_Information(Word.WdInformation.wdActiveEndPageNumber) != lastPageNumber &&
                    (lastKlasse != currKlasse || (ExportSettings.GroupBy == GroupByType.GroupBySchüler && lastSchueler != beo.Schueler) ||
                    (ExportSettings.GroupBy == GroupByType.GroupByDatum && lastDatum != beo.Datum)))
                {
                    header = app.Selection.Sections[1].Headers[Word.WdHeaderFooterIndex.wdHeaderFooterPrimary];
                    header.LinkToPrevious = false;
                    r = header.Range;
                    r.Text = "\t" + ExportSettings.Header + "\t" + DateTime.Now.ToShortDateString() + "\n\t" + currKlasse.ToString() + " - " +
                        (ExportSettings.GroupBy == GroupByType.GroupBySchüler ? beo.Schueler.DisplayName : (beo.Datum == null ? "Allgemein" : beo.Datum.Value.ToShortDateString()));
                               
                    
                    lastPageNumber = (int)app.Selection.get_Information(Word.WdInformation.wdActiveEndPageNumber);
                }
 #endregion
                // Format je nach Beobachtungsart
                if (beo.Datum == null && ExportSettings.GroupBy == GroupByType.GroupBySchüler)
                    app.Selection.set_Style(FormatDataListe);

                else
                {
                    app.Selection.set_Style(FormatData2Spalten);
                    if (ExportSettings.GroupBy == GroupByType.GroupBySchüler)
                    {
                        if (!lastDatum.HasValue || lastDatum.Value.Date != beo.Datum.Value.Date || ExportSettings.RepeatSameName)
                            app.Selection.TypeText(beo.Datum.Value.ToString("dd.MM.yyyy"));
                        app.Selection.TypeText("\t");
                    }
                    else
                    {
                        if (lastSchueler != beo.Schueler || ExportSettings.RepeatSameName)
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
        }

        /// <summary>
        /// Exportiert alle Beobachtungen in Word
        /// </summary>
        public void ExportToWord(Schule.DataManager.UowSchuleDB UOW = null)
        {
            if (UOW == null)
                UOW = App.Current.FindResource("UnitOfWork") as Groll.Schule.DataManager.UowSchuleDB;
            
            if (UOW != null)   
                ExportToWord(UOW.Beobachtungen.GetList());
        }

        /// <summary>
        /// Exportiert alle Beobachtungen einer Klasse in Word
        /// </summary>
        public void ExportToWord(Klasse Klasse, Schule.DataManager.UowSchuleDB UOW = null)
        {
            if (UOW == null)
                UOW = App.Current.FindResource("UnitOfWork") as Groll.Schule.DataManager.UowSchuleDB;

            if (UOW != null)
            {
                var t = from b in UOW.Beobachtungen.GetList()
                        where (b.Klasse) == Klasse
                        select b;
                            
                ExportToWord(t);
            }
        }

        /// <summary>
        /// Exportiert alle Beobachtungen eines Jahrgangs in Word
        /// </summary>
        public void ExportToWord(Schuljahr Schuljahr, Schule.DataManager.UowSchuleDB UOW = null)
        {
            ExportToWord(Schuljahr.Startjahr, UOW);
        }

        /// <summary>
        /// Exportiert alle Beobachtungen eines Jahrgangs in Word
        /// </summary>
        public void ExportToWord(int Schuljahr, Schule.DataManager.UowSchuleDB UOW = null)
        {
            if (UOW == null)
                UOW = App.Current.FindResource("UnitOfWork") as Groll.Schule.DataManager.UowSchuleDB;

            if (UOW != null)
            {
                ExportToWord(UOW.Beobachtungen.GetList(x => x.SchuljahrId == Schuljahr));
            }
        }

         /// <summary>
        /// Exportiert alle Beobachtungen eines Schülers in Word
        /// </summary>
        public void ExportToWord(Schueler Schüler, int Schuljahr = 0, Schule.DataManager.UowSchuleDB UOW = null)
        {
            if (UOW == null)
                UOW = App.Current.FindResource("UnitOfWork") as Groll.Schule.DataManager.UowSchuleDB;

            if (UOW != null)
            {
                ExportSettings.GroupBy = GroupByType.GroupBySchüler;
                var t = from b in UOW.Beobachtungen.GetList()
                        where (b.Schueler) == Schüler && (b.SchuljahrId == Schuljahr|| Schuljahr == 0)
                        select b;

                ExportToWord(t);
            }
        }

        /// <summary>
        /// Exportiert alle Beobachtungen eines Schülers in Word
        /// </summary>
        public void ExportToWord(Schueler Schüler, Schuljahr Schuljahr, Schule.DataManager.UowSchuleDB UOW = null)
        {
            ExportToWord(Schüler, Schuljahr == null ? 0 : Schuljahr.Startjahr, UOW);
        }

       
                     



    }
}
