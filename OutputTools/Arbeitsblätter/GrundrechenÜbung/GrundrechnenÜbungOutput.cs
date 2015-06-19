using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Excel = Microsoft.Office.Interop.Excel;
using Groll.Schule.OutputTools;
using System.IO;

namespace Groll.Schule.OutputTools.Arbeitsblätter
{
    public enum GrundrechenArten { Plus, Minus, Mal, Geteilt }
    public enum AufgabenStellung { AFehlt, BFehlt, ErgebnisFehlt, Gemischt }
  
    public class GrundrechnenÜbungOutput : OutputTemplateBase
    {
        #region Pos Struct

        private struct ColumnRowTupel
        {
            public int Column;
            public int Row;

            public ColumnRowTupel(int c, int r)
            {
                Column = c;
                Row = r;
            }

            public bool IsZero { get { return Column == 0 && Row == 0; } }
        }

        #endregion
        #region Rechnung class
        public class Rechnung
        {
            private int? r;
            public int A { get; set; }
            public int B { get; set; }
            public int R
            {
                get
                {
                    return r ?? Calculate();
                }
                set { r = value; }
            }
            public GrundrechenArten Operator;

            public Rechnung() { }

            public Rechnung(int a, int b, int r, GrundrechenArten op)
            {
                A = a;
                B = b;
                R = r;
                Operator = op;
            }

            public int Calculate()
            {
                switch (Operator)
                {
                    case GrundrechenArten.Plus: return A + B;
                    case GrundrechenArten.Minus: return A - B;
                    case GrundrechenArten.Mal: return A * B;
                    case GrundrechenArten.Geteilt: return A / B;
                    default: throw new ArgumentException();
                }
            }

            public void SwitchAB()
            {
                int t = A; A = B; B = t;                
            }
        }
        #endregion

        private GrundrechnenÜbungConfig Config = new GrundrechnenÜbungConfig();
        private Random rnd;
        
        public GrundrechnenÜbungOutput()
        {
            Name = "Grundrechenarten-Übungsblatt";
            Description = "Arbeitsblatt mit vielen Grundrechenarten.\nKann konfiguriert werden.\n\nAnwendung: z.B. Blitzrechnen";
            Group = "Arbeitsblätter";
            SubGroup = "Mathe";
            HasConfig = true;
        }

        public GrundrechnenÜbungOutput(string name, string description, string group, string subgroup, bool hasConfig = true)
        {
            if (name != "")
                Name = name;
            Description = description;
            if (group != "")
                Group = group;
            SubGroup = subgroup;
            HasConfig = hasConfig;
        }

        public override void ShowConfig(Schule.DataManager.UowSchuleDB uow = null)
        {
            if (uow != null)
                Config.LoadFromDatabase(uow);
            
            var c = new GrundrechnenÜbungConfigWindow(Config.Clone() as GrundrechnenÜbungConfig);            
            if (c.ShowDialog() == true)
                Config = c.Config;

            if (uow != null)
                Config.SaveToDatabase(uow);
           
        }

        public override void Start(Schule.DataManager.UowSchuleDB uow = null)
        {
            if (uow != null)
                Config.LoadFromDatabase(uow);
            
            if (Config.OutputToExcelTemplate)
                CreateExcel();

            else
                // TODO
                ;
            
        }
  
        private void CreateExcel()
        {
            // Get Excel Config File
            XElement doc = null;
            string Filename = "";
            try
            {
                doc = XElement.Load(Config.FilePath);
                if (doc.Name.LocalName != "OutputTemplate" || doc.AttributeValue("Class") != GetType().Name)
                    throw new Exception("Falscher Template-Typ");

                // Check file
                Filename = doc.Attribute("File").Value;
                if (!Path.IsPathRooted(Filename))                
                    Filename = Path.Combine(Environment.CurrentDirectory, Path.GetDirectoryName(Config.FilePath), Filename);
                
                if (!File.Exists(Filename))
                    throw new FileNotFoundException("Vorlage nicht gefunden:", Filename);

            }

            catch (Exception e)
            {
                System.Windows.MessageBox.Show("Fehler beim Öffnen des Templates:\n" + e.Message);
                return;
            }

            // create Positions-Cache 
            var positions = new List<ColumnRowTupel>();
            var xml = doc.Element("Rechnungen");
            if (xml != null)
            {
                // Auto-Einträge durchgehen
                foreach (XElement auto in xml.Elements("Auto"))
                {
                    var t = auto.Element("Anfang");
                    if (t == null)
                        continue;

                    var StartPos = new ColumnRowTupel(t.AttributeValue<int>("Spalte"), t.AttributeValue<int>("Zeile"));

                    t = auto.Element("Abstand");
                    if (t == null)
                        continue;

                    var Abstand = new ColumnRowTupel(t.AttributeValue<int>("Spalte"), t.AttributeValue<int>("Zeile"));

                    t = auto.Element("Anzahl");
                    if (t == null)
                        continue;

                    var Anzahl = new ColumnRowTupel(t.AttributeValue<int>("Spalten"), t.AttributeValue<int>("Zeilen"));

                    if (StartPos.Column == 0 || StartPos.Row == 0 || Abstand.Column == 0 || Abstand.Row == 0 || Anzahl.Column == 0 || Anzahl.Row == 0)
                        continue;

                    for (int c = 0; c < Anzahl.Column; c++)
                        for (int r = 0; r < Anzahl.Row; r++)
                        {
                            positions.Add(new ColumnRowTupel(StartPos.Column + Abstand.Column * c, StartPos.Row + Abstand.Row * r));                            
                        }                                       
                }

                // Manuelle Einträge durchgehen
                foreach (XElement man in xml.Elements("Manuell"))
                {                   
                    var m = new ColumnRowTupel(man.AttributeValue<int>("Spalte"), man.AttributeValue<int>("Zeile"));
                    if (m.IsZero)
                        continue;
                    positions.Add(m);                       
                }
            }

            if (positions.Count == 0)
            {
                System.Windows.MessageBox.Show("Keine gültigen Felder für Rechnungen im Template gefunden.");
                return;
            }

            
            // Open Excel file
            Excel.Application app = null;
            Excel.Workbook wb;
            Excel.Worksheet ws;
            Excel.Worksheet wsL;   // Lösungs-Sheet
            try
            {               
                app = new Excel.Application();
                wb = app.Workbooks.Add(Filename);
                app.Visible = true;
                app.ScreenUpdating = false;

                // Get  sheets
                ws = (Excel.Worksheet)wb.Worksheets[1];
                wsL = (Excel.Worksheet)wb.Worksheets[2];

                
                // Einträge durchgehen
                foreach (var pos in positions)
                {                                    
                    var rech = GenerateRechnung();

                    var missingValue = Config.MissingPart == AufgabenStellung.Gemischt ? (AufgabenStellung) rnd.Next(0, 3) : Config.MissingPart;            
                    WriteRechnung((Excel.Range) ws.Cells[pos.Row, pos.Column], rech, missingValue);
                    WriteRechnung((Excel.Range) wsL.Cells[pos.Row, pos.Column], rech, missingValue, true);                                       
                }
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show("Fehler beim Anzeigen der Excel-Vorlage\n" + e.Message);                
            }

            finally
            {
                if (app != null)
                {
                    app.Visible = true;
                    app.ScreenUpdating = true;
                }
            }
           
        }


        /// <summary>
        /// Ausgabe der Rechnung in Excel Bereich
        /// </summary>
        /// <param name="range">Start-Zelle</param>
        /// <param name="rechnung">Rechnung</param>
        /// <param name="missingValue">Welcher Wert soll leer bleiben</param>
        /// <param name="WithSolution">Lösung ausgeben ?</param>
        private void WriteRechnung(Excel.Range range, Rechnung rechnung, AufgabenStellung missingValue = AufgabenStellung.ErgebnisFehlt, bool ShowSolution = false)
        {
            try
            {
                FormatNumber(range, rechnung.A, missingValue == AufgabenStellung.AFehlt, ShowSolution);
                range.Offset[0, 1].Value = Config.Rechenzeichen[(int)rechnung.Operator];
                FormatNumber(range.Offset[0, 2], rechnung.B, missingValue == AufgabenStellung.BFehlt, ShowSolution);
                range.Offset[0, 3].Value = "=";
                FormatNumber(range.Offset[0, 4], rechnung.R, missingValue == AufgabenStellung.ErgebnisFehlt, ShowSolution);              
            }
            catch 
            {
                
                // Ignore
            }
        }

        private void FormatNumber(Excel.Range cell, double Number, bool IsMissing = false, bool ShowSolution = false)
        {
            // Write number
            if (!IsMissing || ShowSolution)
                cell.Value = Number;

            // Format Cell
            if (IsMissing)
            {
                cell.BorderAround2(Excel.XlLineStyle.xlContinuous, Excel.XlBorderWeight.xlThin);

                if (ShowSolution)
                {
                    cell.Font.Bold = true;
                    cell.Interior.Color = 65535;
                }
            }
        }

        
        public Rechnung GenerateRechnung()
        {
            if (rnd == null)
                rnd = new Random();

            Rechnung r = new Rechnung();                       
            r.Operator = Config.SelectedOperators[rnd.Next(Config.SelectedOperators.Count)];
            int min = 0, max = 0;

            switch (r.Operator)
            {
                case GrundrechenArten.Plus:
                    min = !Config.AllowZero ? 1 : 0;
                    max = Math.Min(Config.MaxResult - min, Config.MaxSummand);
                    r.A = rnd.Next(min, max + 1);
                    max = Math.Min(Config.MaxResult - r.A, Config.MaxSummand);
                    r.B = rnd.Next(min, max + 1);                    
                    break;

                case GrundrechenArten.Minus:
                    min = !Config.AllowZero ? 2 : 0;
                    max = Math.Min(Config.MaxResult, Config.MaxSummand);
                    r.A = rnd.Next(min, max + 1);
                    min = !Config.AllowZero ? 1 : 0;
                    max = Math.Min(r.A - min, Config.MaxSummand);
                    r.B = rnd.Next(min, max + 1);                    
                    break;

                case GrundrechenArten.Mal:
                    if (Config.AllowedFaktors == null || Config.AllowedFaktors.Count == 0)
                    {
                        // Faktor A  (0/1/2) - Wurzel(MAX)
                        min = !Config.AllowOne ? 2 : (!Config.AllowZero ? 1 : 0);
                        max = Math.Min((int)Math.Sqrt(Config.MaxResult), Config.MaxFaktor);
                        r.A = rnd.Next(min, max + 1);
                        max = Math.Min(Config.MaxResult / r.A, Config.MaxFaktor);
                        r.B = rnd.Next(min, max + 1);

                        // Zufällig Operanten tauschen, damit auch große Zahlen vorne sein können
                        if (rnd.Next() > 0.5)
                            r.SwitchAB();
                    }
                    else
                    {
                        // nur bestimmte Einmal-Eins sind erlaubt
                        r.B = Config.AllowedFaktors[rnd.Next(0, Config.AllowedFaktors.Count)];
                        min = !Config.AllowOne ? 2 : (!Config.AllowZero ? 1 : 0);
                        max = Math.Min(Config.MaxResult / r.B, Config.MaxFaktor);
                        r.A = rnd.Next(min, max + 1);
                    }
                    break;

                case GrundrechenArten.Geteilt:                    
                    min = !Config.AllowOne ? 2 : (!Config.AllowZero ? 1 : 0);
                    max = Math.Min((int)Math.Sqrt(Config.MaxResult), Config.MaxFaktor);
                    r.B = rnd.Next(min, max);
                    max = Math.Min(Config.MaxResult / r.B, Config.MaxFaktor);                                        
                    r.R = rnd.Next(min, max);   
                    
                    // Zufällig Operanten tauschen, damit auch große Zahlen vorne sein können
                    if (rnd.Next() > 0.5) 
                    {
                        int t = r.R;
                        r.R = r.B;
                        r.B = t;
                    }

                    r.A = r.R * r.B;
                    break;

            }                       

            return r;
        }
    }
}
