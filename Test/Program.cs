using Groll.Schule.DataAccess;
using Groll.Schule.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    [TestClass]
    public class Program
    {
        //[TestCategory("Schuljahr")]
        //[TestMethod]
        //public void CurrentSchuljahr()
        //{
        //    using (var context = new SchuleContext())
        //    {
        //        var s = context.Schuljahre.Find(2012);
        //        var k = Schuljahr.GetCurrent();
        //        var l = context.Klassen.Add(new Klasse()
        //        { Name = "Test1",
        //             Jahr = s   });


        //        var o = context.Klassen.Add(new Klasse()
        //        {
        //            Name = "Test2",
        //            Jahr = k
        //        });
        //        context.SaveChanges();
        //        Assert.AreEqual(l.Jahr, o.Jahr);
        //    }
        //}


        //[TestCategory("SampleData")]
        //[TestMethod]
        //public void DeleteAllData()
        //{
        //    using (var context = new SchuleContext())
        //    {
        //        context.Database.ExecuteSqlCommand("DELETE FROM Beobachtungen;");                
        //        context.Database.ExecuteSqlCommand("DELETE FROM Unterrichtsstunden;");
        //        context.Database.ExecuteSqlCommand("DELETE FROM Faches;");
        //        context.Database.ExecuteSqlCommand("DELETE FROM Schüler;");
        //        context.Database.ExecuteSqlCommand("DELETE FROM Klassen_Schüler;");
        //        context.Database.ExecuteSqlCommand("DELETE FROM Stundenpläne;");                
        //        context.Database.ExecuteSqlCommand("DELETE FROM Klassen;");
        //        context.Database.ExecuteSqlCommand("DELETE FROM Schuljahre;");
        //        context.SaveChanges();

        //    }

        //}
        

        //[TestCategory("SampleData")]
        //[TestMethod]
        //public void AddSampleData()
        //{

        //    try
        //    {
        //        using (var context = new SchuleContext())
        //        {
        //            if (context.Schueler.Count() > 0)
        //                return;
                        
        //            Schueler s1 = context.Schueler.Add(new Schueler() { Vorname = "Tamara", Nachname = "Biebricher" });
        //            Schueler s2 = context.Schueler.Add(new Schueler() { Vorname = "Fenja", Nachname = "Biebricher" });
        //            Schueler s3 = context.Schueler.Add(new Schueler() { Vorname = "Cedric", Nachname = "Biebricher" });
        //            Schueler s4 = context.Schueler.Add(new Schueler() { Vorname = "Christian", Nachname = "Groll" });
        //            Schueler s5 = context.Schueler.Add(new Schueler() { Vorname = "Monica", Nachname = "Biebricher" });


        //            Schuljahr sj = context.Schuljahre.Add(
        //                new Schuljahr(2011) 
        //                { 
        //                    Klassen = new List<Klasse>() 
        //                    { 
        //                        new Klasse() {
        //                            Name = "1a",
        //                            Schueler = new List<Schueler>() {s1, s2} },
                                
        //                        new Klasse() {
        //                            Name = "1b",
        //                            Schueler = new List<Schueler>() {s3} },
                                
        //                        new Klasse() {
        //                            Name = "2a",
        //                            Schueler = new List<Schueler>() {s4, s5} }
        //                    }
        //                } );

        //              sj = context.Schuljahre.Add(
        //                new Schuljahr(2012) 
        //                { 
        //                    Klassen = new List<Klasse>() 
        //                    { 
        //                        new Klasse() {
        //                            Name = "2a",
        //                            Schueler = new List<Schueler>() {s1} },
                                
        //                        new Klasse() {
        //                            Name = "2b",
        //                            Schueler = new List<Schueler>() {s2, s3} },
                                
        //                        new Klasse() {
        //                            Name = "3a",
        //                            Schueler = new List<Schueler>() {s4, s5} }
        //                    }
        //                } );


        //              Fach f1 = new Fach() { Name = "Deutsch" };
        //              Fach f2 = new Fach() { Name = "HSU" };
        //              Fach f3 = new Fach() { Name = "Mathe" };
        //              Fach f4 = new Fach() { Name = "Englisch" };
        //              Fach f5 = new Fach() { Name = "Sport" };

        //              Stundenplan pl = context.Stundenpläne.Add(new Stundenplan()
        //              {
        //                  Klasse = sj.Klassen[0],
        //                  Stunden = new List<Unterrichtsstunde>()
        //              {
        //                  new Unterrichtsstunde() { Stunde = 1, Tag = Wochentag.Montag, Fach = f1 },
        //                  new Unterrichtsstunde() { Stunde = 2, Tag = Wochentag.Montag, Fach = f2 },
        //                  new Unterrichtsstunde() { Stunde = 3, Tag = Wochentag.Montag, Fach = f3 },
        //                  new Unterrichtsstunde() { Stunde = 4, Tag = Wochentag.Montag, Fach = f4 },
        //                  new Unterrichtsstunde() { Stunde = 5, Tag = Wochentag.Montag, Fach = f5 },

        //                   new Unterrichtsstunde() { Stunde = 1, Tag = Wochentag.Dienstag, Fach = f5 },
        //                  new Unterrichtsstunde() { Stunde = 2, Tag = Wochentag.Dienstag, Fach = f4 },
        //                  new Unterrichtsstunde() { Stunde = 3, Tag = Wochentag.Dienstag, Fach = f3 },
        //                  new Unterrichtsstunde() { Stunde = 4, Tag = Wochentag.Dienstag, Fach = f2 },
        //                  new Unterrichtsstunde() { Stunde = 5, Tag = Wochentag.Dienstag, Fach = f1 },

        //                   new Unterrichtsstunde() { Stunde = 1, Tag = Wochentag.Mittwoch, Fach = f1 },
        //                  new Unterrichtsstunde() { Stunde = 2, Tag = Wochentag.Mittwoch, Fach = f2 },
        //                  new Unterrichtsstunde() { Stunde = 3, Tag = Wochentag.Mittwoch, Fach = f3 },
        //                  new Unterrichtsstunde() { Stunde = 4, Tag = Wochentag.Mittwoch, Fach = f4 },
        //                  new Unterrichtsstunde() { Stunde = 5, Tag = Wochentag.Mittwoch, Fach = f5 },

        //              }
        //              });

                    
        //            context.SaveChanges();
        //        }

        //    }
        //    catch (Exception e)
        //    {
        //        int i = 0;
        //        throw;
        //    }
        //}

        static void Main(string[] args)
        {
       //     Database.DefaultConnectionFactory = new
       //                 SqlCeConnectionFactory("System.Data.SqlServerCe.4.0");           

          
            var i = SchuleContext.Open();
            
            Console.WriteLine( "\nSchüler: " + i.Schueler.Count());
            foreach (var j in i.Schueler)
                Console.WriteLine(j.ToString());

            Console.WriteLine("\nKlassen: " + i.Klassen.Count());
            foreach (var j in i.Klassen)
            {
                Console.WriteLine("Klasse " + j.Name + " (" + j.Schuljahr.ToString() + ")");
                Console.WriteLine(new string('=',20));
                Console.WriteLine("Schüler: ");
                j.Schueler.ForEach( x=> Console.WriteLine(" - " + x.DisplayName ));                
            }

            Console.WriteLine("\nBeobachtungen: " + i.Beobachtungen.Count());
            foreach (var j in i.Beobachtungen)
            {
                Console.WriteLine("Jahr:{0}, Fach: {1}, Schüler: {2}, Klasse: {3}, Datum: {4}, Text: {5}", j.Schuljahr.ToString(), j.Fach == null ? "" :  j.Fach.Name, j.Schueler.DisplayName, j.Schueler.Klassen.Single(x => x.SchuljahrId == 2013).Name, j.Datum.ToString() ?? "", j.Text);                
            }



            
            Console.ReadLine();
            
        }
    }
    
}
