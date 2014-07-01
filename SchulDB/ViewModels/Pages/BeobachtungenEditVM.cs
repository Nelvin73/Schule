using Groll.Schule.SchulDB.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Groll.Schule.Model;
using Groll.Schule.DataManager;
using System.Collections.ObjectModel;
using Groll.Schule.SchulDB.Commands;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows;

namespace Groll.Schule.SchulDB.ViewModels
{
    /// <summary>
    /// ViewModel für die Beobachtungs-Eingabe-Seite
    /// </summary>
    public class BeobachtungenEditVM : BeobachtungenBaseVM
    {
        // internal Member
        #region Fields
        
        private FlowDocument document;
        private Beobachtung editedBeobachtung;
        private Paragraph editedParagraph;
        private bool isEditMode;

        #endregion

        #region Properties

        public FlowDocument Document
        {
            get
            {
                if (document == null)
                    document = CreateDocument();

                return document; }
            set
            {
                if (document != value)
                    document = value; OnPropertyChanged();
            }
        }

        public Beobachtung EditedBeobachtung
        {
            get { return editedBeobachtung; }
            set
            {
                if (editedBeobachtung != value)
                {
                    editedBeobachtung = value;

                    // Update Values in Editboxes  
                    SelectedSchuljahr = (value == null ? null : value.Schuljahr);
                    SelectedKlasse = (value == null ? null : value.Klasse);
                    SelectedSchüler = (value == null ? null : value.Schueler);
                    SelectedFach = (value == null ? null : (value.Fach ?? Fächerliste.First(x => x.FachId == -1000)));                    
                    BeoDatum = (value == null ? null : value.Datum);
                    BeoText = (value == null ? "" : value.Text);
                    OnPropertyChanged();
                }
            }
        }
    
        // Paragraph für Bearbeitung auswählen
        public Paragraph EditedParagraph
        {
            get
            {
                return editedParagraph;
            }
            private set
            {
                if (editedParagraph != value)
                {
                    // neuer Absatz soll editiert werden (oder keiner) => Farbmarkierung entfernen
                    Beobachtung b = null;
                    IsEditMode = true;

                    if (editedParagraph != null) { editedParagraph.Background = null; editedParagraph = null; }
                    
                    // Edit von Beobachtung aktivieren                        
                    if (value != null && value.Tag != null && value.Tag is int)
                    {
                        b = UnitOfWork.Beobachtungen.GetById((int)value.Tag);
                        if (b != null)
                        {
                            editedParagraph = value;
                            editedParagraph.Background = Brushes.Yellow;                            
                        }
                    }

                    // Set EditedBeobachtung
                    EditedBeobachtung = b;
                    OnPropertyChanged();
                }
            }
        }
        
        public bool IsSchülerChanged
        {
            get { return editedBeobachtung.Schueler != SelectedSchüler; }
        }

        public bool IsSchuljahrChanged
        {
            get { return editedBeobachtung.Schuljahr != SelectedSchuljahr; }
        }
        
        public bool IsEditMode
        {
            get { return isEditMode; }
            set
            {
                if (isEditMode != value)
                {
                    isEditMode = value;
                    Ribbon.TabBeobachtungenAnsicht.EditMode = value;
                    OnPropertyChanged();
                }
            }
        }
        
               
        #endregion


        #region Hilfsroutinen - Dokumenthandling
        
        private FlowDocument CreateDocument()
        {
            // Edited löschen            
            EditedParagraph = null;
            
            // Dokument konfigurieren
            FlowDocument doc = new FlowDocument();
            doc.ColumnWidth = 600;
            doc.ColumnGap = 50;
            doc.ColumnRuleBrush = Brushes.Red;
            doc.ColumnRuleWidth = 2;
            doc.IsColumnWidthFlexible = true;
            doc.FontSize = 10;
            doc.FontFamily = new FontFamily("Verdana");

            var beos = UnitOfWork.Beobachtungen.GetList().
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

                // wenn neue Klasse, dann Kopfzeile schreiben; Seitenumbruch ab der zweiten Klasse
                var currKlasse = beo.Klasse ?? nullKlasse;
                if (currKlasse != lastKlasse)
                {                    
                    p = new Paragraph() { FontSize = 16, Foreground = Brushes.Red };

                    if (lastKlasse != null)
                        newPage = p.BreakPageBefore = true;

                    p.Inlines.Add(new Bold(new Run(currKlasse.ToString())));
                    doc.Blocks.Add(p);
                }

                if (lastSchueler != beo.Schueler)
                {
                    // wenn neuer Schüler, dann Kopfzeile schreiben (und evtl. Seitenumbruch)
                    p = new Paragraph() { FontSize = 14, Foreground = Brushes.Blue };
                    if (lastSchueler != null && Ribbon.TabBeobachtungenAnsicht.NewPageOnSchüler && !newPage)
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

                // Tooltip setzen 
                p.ToolTip =  beo.Fach == null ? "<kein Fach>" : beo.Fach.Name;
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

            return doc;
        }

        #endregion


        


        //  Konstructor
        public BeobachtungenEditVM()
        {
            // Initiere Commands
            Command_UpdateBeobachtung = new DelegateCommand((o) => UpdateBeobachtung(o));
            Command_CancelEdit = new DelegateCommand((o) => CancelEdit());
            SchuleCommands.Beobachtungen.UpdateBeobachtungenView = new DelegateCommand((o) => RefreshDocument());
            SchuleCommands.Beobachtungen.EditModeChanged = new DelegateCommand((o) => IsEditMode = (bool) o);            
        }

       


        #region Commands
        public DelegateCommand Command_UpdateBeobachtung {get; set;}
        public DelegateCommand Command_CancelEdit { get; set; }

        #endregion
        #region Implementation für Commands

        // Einsprungpunkt für DelegateCommand
        public void StartEdit(object p = null) { EditedParagraph = (p as Paragraph); }       

        public void CancelEdit()
        {
            // EditedParagraph auf null setzen (löscht Markierung und EditedBeobachtung)
            EditedParagraph = null;

            // Edit Mode AUS
            IsEditMode = false;                       
        }


        public void UpdateParagraph(bool Reload = false)
        {
            if (editedParagraph == null || editedBeobachtung == null)
                // nothing to update
                return;

            if (Reload)
            {
                // Komplett neu laden
                Beobachtung b = editedBeobachtung;
                RefreshDocument();

                // Beobachtung wieder editieren
                Paragraph p = GetParagraphOfBeobachtung(b);
                EditedParagraph = p;                
            }
            else
            {
                // Inline ändern
                string Text = (BeoDatum.HasValue ? BeoDatum.Value.ToString("dd.MM.yyyy") : "Kein Datum") + "\t";
                if (editedParagraph.Inlines.Count == 2)
                    ((Run)editedParagraph.Inlines.FirstInline).Text = Text;
                else
                    editedParagraph.Inlines.InsertBefore(editedParagraph.Inlines.FirstInline, new Run(Text));

                editedParagraph.TextIndent = -70F;
                ((Run)editedParagraph.Inlines.LastInline).Text = BeoText;
            }

            editedParagraph.BringIntoView();
        }

        private Paragraph GetParagraphOfBeobachtung(Beobachtung b)
        {
            foreach (var i in Document.Blocks)
            {
                if (i is Paragraph && i.Tag is int && ((int) i.Tag) == b.BeobachtungId)                
                    return (Paragraph) i;                
            }
            return null;
        }

        private void JumpToEditedBeobachtung(Beobachtung b = null)
        {
            // Springt bei Neuorganisation zum editierten Paragraph
            if (b == null)
                return;


        }

        public void RefreshDocument()
        {
            // Document neu erstellen
            document = null;
            OnPropertyChanged("Document");
        }

        public void UpdateBeobachtung(object o)
        {
            // Update Beobachtung
            if (editedParagraph == null || editedBeobachtung == null)
                // nothing to update
                return;
            bool x = IsSchülerChanged || IsSchuljahrChanged;
            UpdateDBRecord();
            UpdateParagraph(x);
        }

        public bool UpdateDBRecord()
        {
            // Wenn beo = null, wird EditedBeobachtung genommen
            Beobachtung b = EditedBeobachtung;
            if (b == null)
                return false;

            b.Datum = BeoDatum;
            b.Fach = (SelectedFach.FachId == -1000) ? null : SelectedFach;            
            b.Schueler = SelectedSchüler;
            b.Schuljahr = SelectedSchuljahr;
            b.Text = BeoText;

            UnitOfWork.Beobachtungen.Save();

            return true;
        }
        #endregion
    }
}
