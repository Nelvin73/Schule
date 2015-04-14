using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Groll.Schule.OutputTools.Arbeitsblätter
{
    public class GrundrechnenÜbungConfig : IOutputTemplateConfig
    {
        public class Scenario
        {
            public string DisplayName {get; set;}
            public string Code { get; set; }
            public string Description { get; set; }

            public Scenario (string displayName, string code, string description)
            {
                DisplayName = displayName;
                Code = code;
                Description = description;
            }
        }

        // Allgemein
        #region Scenarious
        
        private List<Scenario> scenarious = new List<Scenario>()
        {
            new Scenario("Normal (Kein Scenario)","None", "Kein Scenario ausgewählt"),
            new Scenario("Eine Zahl Mal 10","x10", "Eine Zahl wird mit 10 multipliziert.\nz.B.  23 + 17 => 230 + 17"),
            new Scenario("Eine Zahl Mal 100","x100", "Eine Zahl wird mit 100 multipliziert.\nz.B.  9 x 6 => 900 x 6"),
            new Scenario("nur Aufgaben ohne Zehnerübergang (Addition, Subtraktion)", "-10" , "Rechnungen sind alle ohne Zehnerübergang"),
            new Scenario("nur Aufgaben mit Zehnerübergang (Addition, Subtraktion)", "10+" , "Rechnungen sind alle mit Zehnerübergang")
        };
        
        #endregion

        public string FilePath {get; set;}
        public List<GrundrechenArten> SelectedOperators
        {
            get
            {
                var i = new List<GrundrechenArten>();
                if (IncludePlus)
                    i.Add(GrundrechenArten.Plus);

                if (IncludeMinus)
                    i.Add(GrundrechenArten.Minus);
                
                if (IncludeMal)
                    i.Add(GrundrechenArten.Mal);
                
                if (IncludeGeteilt)
                    i.Add(GrundrechenArten.Geteilt);
                
                return i;

            }
        }
        public AufgabenStellung MissingPart { get; set; }
        public bool OutputToExcelTemplate { get; set; }

        public List<string> Rechenzeichen { get; set; }

        public bool IncludePlus { get; set; }        
        public bool IncludeMinus { get; set; }         
        public bool IncludeMal { get; set; }         
        public bool IncludeGeteilt { get; set; }         


        public bool AllowZero { get; set; }         // erlaubt die Zahl 0 als Operant oder Ergebnis
        public bool AllowOne { get; set; }          // erlaubt die Zahl 1 (nur für Multiplikation oder Division)

        public int MaxResult { get; set; }          // maximale Zahl als Ergebnis

        public int MaxSummant { get; set; }       // maximale Zahl als Operant (Plus / Minus)
        public int MaxFaktor { get; set; }       // maximale Zahl als Faktor (Mal / Geteilt)

        // public bool MixedOperators { get; set; }   // ob Aufgaben nach Operator sortiert sind oder gemischt

        public List<int> AllowedFaktors { get; set; } // Bei Multiplikation erlaubte Faktoren (EinmalEins)

        public List<Scenario> AvailableScenarious
        {
            get
            {
                return scenarious;
            }
        }

        public Scenario SelectedScenariou { get; set; }

        public GrundrechnenÜbungConfig()
        {
            RestoreDefaultValues();
        }


        public void LoadFromDatabase(DataManager.UowSchuleDB UnitofWork)
        {
            throw new NotImplementedException();
        }

        public void SaveToDatabase(DataManager.UowSchuleDB UnitofWork)
        {
            throw new NotImplementedException();
        }
        
        public void RestoreDefaultValues()
        {
            OutputToExcelTemplate = true;
            FilePath = "Arbeitsblätter\\GrundrechenÜbung\\Mathe_Arbeitsblatt1.arb";
            Rechenzeichen = new List<string>()
            {
                "+", "-", "\u00B7", ":" 
            };

            MissingPart = AufgabenStellung.ErgebnisFehlt;
            AllowZero = false;
            AllowOne = false;
            MaxResult = 101;
            MaxFaktor = 10;
            MaxSummant = 50;
            // MixedOperators = true;
            AllowedFaktors = new List<int>() { 2, 4, 5, 10 };// {null;
            IncludeGeteilt = IncludeMal = IncludeMinus = IncludePlus = true;
            SelectedScenariou =  scenarious.First(x => x.Code == "None");

        }

        public object Clone()
        {
            return this.MemberwiseClone();

        }
    }
}
