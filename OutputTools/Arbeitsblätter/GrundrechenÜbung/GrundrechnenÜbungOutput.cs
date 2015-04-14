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
  
    public class GrundrechnenÜbungOutput : OutputTemplate
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



        public override void ShowConfig()
        {
            var c = new GrundrechnenÜbungConfigWindow(Config.Clone() as GrundrechnenÜbungConfig);            
            if (c.ShowDialog() == true)
                Config = c.Config;            
        }

        public override void Start()
        {
            if (Config.OutputToExcelTemplate)
                CreateExcel();

            else
                // TODO
                ;
            
        }
        private void CreateExcel()
        {
            // Get Config File
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

                // Get first sheet
                ws = (Excel.Worksheet)wb.Worksheets[1];
                wsL = (Excel.Worksheet)wb.Worksheets[2];

                var xml = doc.Element("Rechnungen");
                if (xml != null)
                {
                    // Auto-Einträge durchgehen
                    foreach (XElement auto in xml.Elements("Auto"))
                    {

                        try
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
                                    var rech = GenerateRechnung();
                                    WriteRechnung(ws, wsL, new ColumnRowTupel(StartPos.Row +Abstand.Row * r, StartPos.Column + Abstand.Column * c), rech, Config.MissingPart);                            
                                    
                                }
                        }

                        catch (Exception)
                        {
                            // Ignore error; continue                           
                        }

                    }

                    foreach (XElement man in xml.Elements("Manuell"))
                    {
                        try
                        {
                            var m = new ColumnRowTupel (man.AttributeValue<int>("Spalte"), man.AttributeValue<int>("Zeile"));
                            if (m.Column == 0 || m.Row == 0)
                                continue;

                            var rech = GenerateRechnung();
                            WriteRechnung( ws, wsL, m, rech, Config.MissingPart);                            
                        }

                        catch (Exception)
                        {
                            // Ignore error; continue      
                        }

                    }
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
        private void WriteRechnung(Excel.Worksheet Aufgabe, Excel.Worksheet Lösung,  ColumnRowTupel position, Rechnung rechnung, AufgabenStellung missingValue = AufgabenStellung.ErgebnisFehlt)
        {
            // Wenn Aufgabenstellung gemischt, dann konkrete Aufgabenstellung herleiten
            if (missingValue == AufgabenStellung.Gemischt)
            {
                missingValue = (AufgabenStellung)rnd.Next(0, 2);
            }

            Excel.Range range = (Excel.Range) Aufgabe.Cells[position.Row, position.Column];

            // Write A
            FormatNumber(range, rechnung.A, missingValue == AufgabenStellung.AFehlt);
            range.Offset[0, 1].Value = Config.Rechenzeichen[(int)rechnung.Operator];
            FormatNumber(range.Offset[0, 2], rechnung.B, missingValue == AufgabenStellung.BFehlt);
            range.Offset[0, 3].Value = "=";
            FormatNumber(range.Offset[0, 4], rechnung.R, missingValue == AufgabenStellung.ErgebnisFehlt);            

            range = (Excel.Range) Lösung.Cells[position.Row, position.Column];
            FormatNumber(range, rechnung.A, missingValue == AufgabenStellung.AFehlt, true);
            range.Offset[0, 1].Value = Config.Rechenzeichen[(int)rechnung.Operator];
            FormatNumber(range.Offset[0, 2], rechnung.B, missingValue == AufgabenStellung.BFehlt, true);
            range.Offset[0, 3].Value = "=";
            FormatNumber(range.Offset[0, 4], rechnung.R, missingValue == AufgabenStellung.ErgebnisFehlt, true); 
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
                    cell.Interior.Color = 65535;
                }
            }
        }

        
        public Rechnung GenerateRechnung()
        {
            if (rnd == null)
                rnd = new Random();

            Rechnung r = new Rechnung();                       
            r.Operator = (GrundrechenArten) rnd.Next(Config.SelectedOperators.Count);
            int min = 0, max = 0;

            switch (r.Operator)
            {
                case GrundrechenArten.Plus:
                    min = !Config.AllowZero ? 1 : 0;
                    max = Math.Min(Config.MaxResult - min, Config.MaxSummant);
                    r.A = rnd.Next(min, max + 1);
                    max = Math.Min(Config.MaxResult - r.A, Config.MaxSummant);
                    r.B = rnd.Next(min, max + 1);                    
                    break;

                case GrundrechenArten.Minus:
                    min = !Config.AllowZero ? 2 : 0;
                    max = Math.Min(Config.MaxResult, Config.MaxSummant);
                    r.A = rnd.Next(min, max + 1);
                    min = !Config.AllowZero ? 1 : 0;
                    max = Math.Min(r.A - min, Config.MaxSummant);
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
