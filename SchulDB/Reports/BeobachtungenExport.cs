using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Groll.Schule.Model;
using Microsoft.Office.Interop.Word;

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
 * ACHTUNG: Die Klasse in Schüler und Beobachtung stimmt nicht überein !!!!
 * Kein Abgleich zwischen SchuljahrId und Schuljahr in Beobachtung !!!
 * */




namespace Groll.Schule.SchulDB.Reports
{
    public class BeobachtungenExport
    {
        public enum TextBreakType {Paragraph, Page, None}
        
        private bool useTemplate = false;
	    public bool UseTemplate
	    {
		    get { return useTemplate;}
		    set { useTemplate = value;}
	    }
	
        private string templatePath = "";
	    public string TemplatePath
	    {
		    get { return templatePath;}
		    set { templatePath = value;}
	    }
	
        private TextBreakType breakNewKlasse = TextBreakType.Page;

	public TextBreakType BreakOnNewKlasse
	{
		get { return breakNewKlasse;}
		set { breakNewKlasse = value;}
	}
	
        private TextBreakType breakNewSchüler = TextBreakType.Paragraph;

	public TextBreakType BreakOnNewSchüler
	{
		get { return breakNewSchüler;}
		set { breakNewSchüler = value;}
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
            if (Beobachtungen.Count() == 0)
                return;

            // diverse Objekte für Word initialisieren (wegen REF Parameterübergabe)
            object missing = Type.Missing;
            object stypeTypeParagraph = WdStyleType.wdStyleTypeParagraph;
            //object TemplateName = (File.Exists(Parameter.TemplateName) ? Parameter.TemplateName : "");
            object AbsatzUmbruch = WdBreakType.wdSectionBreakNextPage;
            object SeitenUmbruch = WdBreakType.wdPageBreak;
            object CollapseEnd = WdCollapseDirection.wdCollapseEnd;
            object FormatStandard = "Standard";
            object FormatSchuelerName = "Beo_Schülername";
            object FormatOhneDatum = "Beo_Liste_ohne_Datum";
            object FormatmitDatum = "Beo_Liste_mit_Datum";
            // object FormatEnde = "Beo_Bemerkung_Ende";
            object ZahlEins = 1;
            object FieldEmpty = WdFieldType.wdFieldEmpty;
            object autoTextSEITE = "PAGE";
            object autoTextSEITEN = "NUMPAGES";
            object preserveFormatting = true;
            object TABleft = WdTabAlignment.wdAlignTabLeft;
            object TABcenter = WdTabAlignment.wdAlignTabCenter;
            object TABright = WdTabAlignment.wdAlignTabRight;

            // Word-Dokument öffnen 
            var app = new ApplicationClass();
            app.Visible = true;


            // Document öffnen
            Document doc = app.Documents.Add(ref missing, ref missing, ref missing, ref missing);

            #region Formatvorlagen prüfen und notfalls anlegen
            try
            {
                // Standard-Format für Schülername
                Style s = doc.Styles.Add(FormatSchuelerName.ToString(), ref stypeTypeParagraph);
                s.Font.Size = 14;
                s.Font.Bold = -1;
                s.Font.Name = "Arial";
            }
            catch
            { }

            try
            {
                // Standard-Format für Liste (ohne Datum)
                Style s = doc.Styles.Add(FormatOhneDatum.ToString(), ref stypeTypeParagraph);
                s.Font.Name = "Arial";
                s.Font.Size = 10;
                s.Font.Bold = 0;
                ListTemplate lTempl = app.ListGalleries[WdListGalleryType.wdBulletGallery].ListTemplates.get_Item(ref ZahlEins);
                s.LinkToListTemplate(lTempl, ref ZahlEins);
            }
            catch
            { }

            try
            {
                // Standard-Format für Datumseinträge
                Style s = doc.Styles.Add(FormatmitDatum.ToString(), ref stypeTypeParagraph);
                s.Font.Name = "Arial";
                s.Font.Size = 10;
                s.Font.Bold = 0;
                s.ParagraphFormat.TabStops.Add(70F, ref missing, ref missing);
                s.ParagraphFormat.LeftIndent = 70F;
                s.ParagraphFormat.FirstLineIndent = -70F;
            }
            catch
            { }

            /*try
            {
                // Standard-Format für End-Notizen
                Style s = doc.Styles.Add(FormatEnde.ToString(), ref x);
                s.Font.Name = "Arial";
                s.Font.Size = 10;
                s.Font.Italic = -1;
            }
            catch
            { }
            */
            #endregion

            #region Kopf und Fußzeile
            // Kopf- und Fußzeile anlegen
            HeaderFooter footer = doc.Sections[1].Footers[WdHeaderFooterIndex.wdHeaderFooterPrimary];
            Range r = footer.Range;
            //r.ParagraphFormat.TabStops.Add(200, ref TABleft, ref missing);
            //r.ParagraphFormat.TabStops.Add(300, ref TABcenter, ref missing);
            //r.ParagraphFormat.TabStops.Add(550, ref TABright, ref missing);
            r.Font.Size = 10;
            r.Text = "\t- Seite ";
            r.Collapse(ref CollapseEnd);
            r.Fields.Add(r, ref missing, ref autoTextSEITE, ref preserveFormatting);
            r.MoveEnd(ref missing, ref missing);
            r.InsertAfter("/");
            r.Collapse(ref CollapseEnd);
            r.Fields.Add(r, ref missing, ref autoTextSEITEN, ref preserveFormatting);
            r.MoveEnd(ref missing, ref missing);
            r.InsertAfter(" -");

            HeaderFooter header = doc.Sections[1].Headers[WdHeaderFooterIndex.wdHeaderFooterPrimary];
            r = header.Range;
            //r.ParagraphFormat.TabStops.Add(200, ref TABleft, ref missing);
            //r.ParagraphFormat.TabStops.Add(300, ref TABcenter, ref missing);
            //r.ParagraphFormat.TabStops.Add(550, ref TABright, ref missing);
            r.Font.Size = 12;
            r.Text = "\tSchülerbeobachtungen\t" + DateTime.Now.ToShortDateString();
            #endregion

            #region Datensätze ausgeben
            // Datensätze durchgehen 
            Schueler lastSchueler = null;
            Klasse lastKlasse = null;

            foreach (Beobachtung beo in Beobachtungen)
            {
                // Neue Klasse ? --> Kopfzeile bzw. neue Seite 
                var k = beo.Schueler.Klassen.FirstOrDefault(x => x.SchuljahrId == beo.SchuljahrId);
                if (k != null && k != lastKlasse)
                {
                    if (lastKlasse != null && breakNewKlasse != TextBreakType.None)
                    {
                        // Umbruch nach jeder folgenden Klasse (außer der ersten)
                        if (breakNewKlasse == TextBreakType.Page)
                            app.Selection.InsertBreak(ref SeitenUmbruch);
                        else
                            app.Selection.InsertBreak(ref AbsatzUmbruch);
                    }

                    // Kopfzeile anpassen    
                    header = app.Selection.Sections[1].Headers[WdHeaderFooterIndex.wdHeaderFooterPrimary];
                    header.LinkToPrevious = false;
                    r = header.Range;
                    if (r.Words[3].Text == "\t")
                        r.Words[2].InsertAfter(" (Klasse: " + k.Name + ")");
                    else
                        r.Words[6].Text = k.Name;
                    lastKlasse = k;
                }

                // Neuer Schüler ? --> Header bzw. neue Seite 
                if (beo.Schueler != null && lastSchueler != beo.Schueler)
                {
                    if (lastSchueler != null && breakNewSchüler != TextBreakType.None)
                    {
                        // Umbruch Bei jedem weiteren Schüler (außer dem ersten)
                        if (breakNewSchüler == TextBreakType.Page)
                            app.Selection.InsertBreak(ref SeitenUmbruch);
                        else
                            app.Selection.InsertBreak(ref AbsatzUmbruch);
                        // app.Selection.TypeParagraph();
                    }


                    // Schülername ausgeben
                    app.Selection.set_Style(ref FormatSchuelerName);
                    app.Selection.TypeText(beo.Schueler.DisplayName);
                    app.Selection.TypeParagraph();

                    lastSchueler = beo.Schueler;
                }

                // Format je nach Beobachtungsart
                if (beo.Datum == null)
                    app.Selection.set_Style(ref FormatOhneDatum);

                else
                {
                    app.Selection.set_Style(ref FormatmitDatum);
                    app.Selection.TypeText(beo.Datum.Value.ToShortDateString() + "\t");
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
        /// Exportiert Beobachtungen in Word, basierend auf die aktuellen Settings
        /// </summary>
        public void ExportToWork()
        {

        }


        /*
        
        private void ExportToWord(ExportParameter Parameter)
        {
            #region SQL String aufgrund der Parameter generieren
        

         

            // Datensätze durchgehen 
            string lastSchueler = "";
            string lastKlasse = "";

            foreach (DataRow row in data.Rows)
            {
                // Neue Klasse ? --> Kopfzeile bzw. neue Seite 
                if (SQLselect.Contains("Klasse"))
                {
                    if (lastKlasse != row["Klasse"].ToString())
                    {
                        if (lastKlasse != "")
                        {
                            app.Selection.InsertBreak(ref AbsatzUmbruch);        
                        }
                        header = app.Selection.Sections[1].Headers[WdHeaderFooterIndex.wdHeaderFooterPrimary];
                        header.LinkToPrevious = false;
                        r = header.Range;
                        if (r.Words[3].Text == "\t")
                            r.Words[2].InsertAfter(" (Klasse: " + row["Klasse"].ToString() + ")");
                        else
                            r.Words[6].Text = row["Klasse"].ToString();
                    }
                }
                // Neuer Schüler ? --> Header bzw. neue Seite 
                if (lastSchueler != row["DisplayName"].ToString())
                {
                    if (Parameter.NeueSeiteProSchueler && lastSchueler != "" & lastKlasse == row["Klasse"].ToString())
                        app.Selection.InsertBreak(ref SeitenUmbruch);
                    else if (!Parameter.NeueSeiteProSchueler)
                        app.Selection.TypeParagraph();

                    if (Parameter.TemplateName != null)
                        app.Selection.set_Style(ref FormatSchuelerName);
                    app.Selection.TypeText(row["DisplayName"].ToString());
                    app.Selection.TypeParagraph();
                }

                // Beobachtungsart
                switch (row["Art"].ToString())
                {
                    case "Anfang":
                        if (Parameter.TemplateName != null)
                            app.Selection.set_Style(ref FormatAnfangsListe);
                        break;
                    case "Datum":
                        if (Parameter.TemplateName != null)
                            app.Selection.set_Style(ref FormatDatum);
                        app.Selection.TypeText(Convert.ToDateTime(row["Datum"]).ToShortDateString() + "\t");
                        break;
                    case "Ende":
                        if (Parameter.TemplateName != null)
                            app.Selection.set_Style(ref FormatEnde);
                        break;
                    default:
                        app.Selection.set_Style(ref FormatStandard);
                        break;
                }

                // Beobachtung formatieren
                string beoText = row["BeobachtungsText"].ToString();
                beoText = beoText.Replace("\r", "");
                beoText = beoText.Replace("\n", "\v") + "\r";
                app.Selection.TypeText(beoText);

                lastSchueler = row["DisplayName"].ToString();
                if (SQLselect.Contains("Klasse"))
                    lastKlasse = row["Klasse"].ToString();
            }
        } 
         * */

    }
}
