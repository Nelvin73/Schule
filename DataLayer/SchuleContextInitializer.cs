using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Groll.Schule.Model;

namespace Groll.Schule.DataAccess
{
    public class SchuleContextInitializer : DropCreateDatabaseIfModelChanges<SchuleContext>
    {
        public bool CreateEmptyDB = true;

        protected override void Seed(SchuleContext context)
        {
            // Create default settings
            var SJ = Schuljahr.GetCurrent();
            context.Settings.Add(new Setting("Global.AktuellesSchuljahr", SJ.Startjahr));
            context.Schuljahre.Add(SJ);
            
            if (!CreateEmptyDB)
            {
                var Zuhause = new Adresse { Strasse = "Vogtstrasse 1", Ort = "Friedberg", PLZ = 86316 };

                var Tamara = new Schueler { Nachname = "Biebricher", Vorname = "Tamara", Geschlecht = Model.Geschlecht.weiblich, Adresse = Zuhause };
                var Fenja = new Schueler { Nachname = "Biebricher", Vorname = "Fenja", Geschlecht = Model.Geschlecht.weiblich, Adresse = Zuhause };
                var Cedric = new Schueler { Nachname = "Biebricher", Vorname = "Cedric", Geschlecht = Model.Geschlecht.männlich, Adresse = Zuhause };
                var Felian = new Schueler { Nachname = "Biebricher", Vorname = "Felian", Geschlecht = Model.Geschlecht.männlich, Adresse = Zuhause };
                var Christian = new Schueler { Inaktiv = true, Nachname = "Groll", Vorname = "Christian", Geschlecht = Model.Geschlecht.männlich, Adresse = Zuhause };
                var Moni = new Schueler { Nachname = "Biebricher", Vorname = "Monica", Geschlecht = Model.Geschlecht.weiblich, Adresse = Zuhause, Geburtsdatum = DateTime.Parse("21.8.1979") };
                
                var oldSJ = new Schuljahr(SJ.Startjahr - 1);

                new List<Klasse> {
                new Klasse { Schuljahr = SJ, Name = "1a", Schueler = new List<Schueler> { Tamara, Fenja, Cedric }},
                new Klasse { Schuljahr = SJ, Name = "2a", Schueler = new List<Schueler> { Felian, Christian, Moni }},
                new Klasse { Schuljahr = oldSJ, Name = "2b", Schueler = new List<Schueler> { Moni, Tamara }}
                }.ForEach(x => context.Klassen.Add(x));

                var Mathe = new Fach("Mathe");
                var Deutsch = new Fach("Deutsch");

                new List<Fach> {
                Mathe,
                Deutsch,
                new Fach("HSU"),
                new Fach("Kunst"),
                new Fach("Sport") }.ForEach(x => context.Fächer.Add(x));

                new List<Beobachtung> {
                new Beobachtung { Datum = DateTime.Now, Schueler = Tamara, Schuljahr = SJ, Fach = Deutsch, Text = "Beobachtung 1" },
                new Beobachtung { Datum = DateTime.Now, Schueler = Fenja, Schuljahr = SJ, Fach = Mathe, Text = "Beobachtung 2" },
                new Beobachtung { Datum = DateTime.Now.AddDays(-1), Schueler = Cedric, Schuljahr = SJ, Text = "Beobachtung ohne Fach" },
                new Beobachtung { Schueler = Tamara, Schuljahr = oldSJ, Fach = Deutsch, Text = "Beobachtung ohne Datum" }
                 }.ForEach(x => context.Beobachtungen.Add(x));

            }            
            
        }

    }
}
