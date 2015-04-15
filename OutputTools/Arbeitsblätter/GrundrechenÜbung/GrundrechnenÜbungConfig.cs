using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Groll.Schule.OutputTools.Arbeitsblätter
{
    public class GrundrechnenÜbungConfig : OutputTemplateConfigBase
    {
        string Seperator = "&";

        // Allgemein
        #region Scenarious
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

        private List<Scenario> scenarious = new List<Scenario>()
        {
            new Scenario("Normal (Kein Scenario)","None", "Kein Scenario ausgewählt"),
            new Scenario("Eine Zahl Mal 10","x10", "Eine Zahl wird mit 10 multipliziert.\nz.B.  23 + 17 => 230 + 17"),
            new Scenario("Eine Zahl Mal 100","x100", "Eine Zahl wird mit 100 multipliziert.\nz.B.  9 x 6 => 900 x 6"),
            new Scenario("nur Aufgaben ohne Zehnerübergang (Addition, Subtraktion)", "-10" , "Rechnungen sind alle ohne Zehnerübergang"),
            new Scenario("nur Aufgaben mit Zehnerübergang (Addition, Subtraktion)", "10+" , "Rechnungen sind alle mit Zehnerübergang")
        };

        public List<Scenario> AvailableScenarious
        {
            get
            {
                return scenarious;
            }
        }

        #endregion

        #region Settings 
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

        private List<string> rechenzeichen = new List<string>() {"+", "-", "\u00B7", ":" };
        public List<string> Rechenzeichen { get {return rechenzeichen;}}

        public bool IncludePlus { get; set; }        
        public bool IncludeMinus { get; set; }         
        public bool IncludeMal { get; set; }         
        public bool IncludeGeteilt { get; set; }         


        public bool AllowZero { get; set; }         // erlaubt die Zahl 0 als Operant oder Ergebnis
        public bool AllowOne { get; set; }          // erlaubt die Zahl 1 (nur für Multiplikation oder Division)
        public int MaxResult { get; set; }          // maximale Zahl als Ergebnis
        public int MaxSummand { get; set; }       // maximale Zahl als Operant (Plus / Minus)
        public int MaxFaktor { get; set; }       // maximale Zahl als Faktor (Mal / Geteilt)        

        public List<int> AllowedFaktors { get; set; } // Bei Multiplikation erlaubte Faktoren (EinmalEins)
        
        public Scenario SelectedScenariou { get; set; }

        #endregion
        
      
        public override void RestoreDefaultValues()
        {
            OutputToExcelTemplate = true;
            FilePath = "Arbeitsblätter\\GrundrechenÜbung\\Mathe_Arbeitsblatt1.arb";
            MissingPart = AufgabenStellung.ErgebnisFehlt;
            AllowZero = false;
            AllowOne = false;
            MaxResult = 101;
            MaxFaktor = 10;
            MaxSummand = 50;
            // MixedOperators = true;
            AllowedFaktors = new List<int>() { 2, 4, 5, 10 };// {null;
            IncludeGeteilt = IncludeMal = IncludeMinus = IncludePlus = true;
            SelectedScenariou =  scenarious.First(x => x.Code == "None");

        }

        public override string GetSettingsAsString()
        {
            return String.Join(
                Seperator, FilePath, IncludePlus, IncludeMinus, IncludeMal, IncludeGeteilt, MissingPart, OutputToExcelTemplate, AllowZero,
                AllowOne, MaxResult, MaxSummand, String.Join(",", AllowedFaktors), SelectedScenariou.Code);                       
        }

        /// <summary>
        /// Try to load Settings from String; on error reset to default
        /// </summary>
        /// <param name="Settings"></param>
        public override void SetSettingsFromString(string Settings)
        {            
            var s = Settings.Split(new string[] {Seperator},  StringSplitOptions.None);
            try
            {
                FilePath = s[0];
                IncludePlus = bool.Parse(s[1]);
                IncludeMinus = bool.Parse(s[2]);
                IncludeMal = bool.Parse(s[3]);
                IncludeGeteilt = bool.Parse(s[4]);
                MissingPart = (AufgabenStellung)Enum.Parse(typeof(AufgabenStellung), s[5]);
                OutputToExcelTemplate = bool.Parse(s[6]);
                AllowZero = bool.Parse(s[7]);
                AllowOne = bool.Parse(s[8]);
                MaxResult = int.Parse(s[9]);
                MaxSummand = int.Parse(s[10]);
                AllowedFaktors = s[11].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x)).ToList();
                if (scenarious.Exists(x => x.Code == s[12]))
                    SelectedScenariou = scenarious.First(x => x.Code == s[12]);
            }
            catch
            { 
                // On error reset to default
                RestoreDefaultValues();
            }             
        }
    }
}
