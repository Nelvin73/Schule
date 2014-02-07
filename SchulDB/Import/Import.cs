using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Groll.Schule.Model;
using System.Data;

namespace Groll.Schule.SchulDB
{
    public class Import
    {

        public void ImportFromSchuleMDB(string DatabaseFile = "")
        {
            if (DatabaseFile == "")
                return;

            var uow = ViewModels.RibbonVM.Default.UnitOfWork;          
           
            string CON = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=\"" + DatabaseFile + "\";Persist Security Info=True";

            var connection = new OleDbConnection(CON);            
      
            DataTable klassen = new DataTable();
            DataTable schueler = new DataTable();
            DataTable schuelerSJ = new DataTable();
            DataTable beobachtungen = new DataTable();
            DataTable faecher = new DataTable();

            var da = new OleDbDataAdapter("SELECT * FROM tblKlassen", connection);
            da.FillSchema(klassen, SchemaType.Source);
            da.Fill(klassen);

            da = new OleDbDataAdapter("SELECT * FROM tblSchueler", connection);
            da.FillSchema(schueler, SchemaType.Source);
            da.Fill(schueler);

            da = new OleDbDataAdapter("SELECT * FROM tblschuelerSJ", connection);
            da.FillSchema(schuelerSJ, SchemaType.Source);
            da.Fill(schuelerSJ);

            da = new OleDbDataAdapter("SELECT * FROM tblbeobachtungen", connection);
            da.FillSchema(beobachtungen, SchemaType.Source);
            da.Fill(beobachtungen);

            da = new OleDbDataAdapter("SELECT * FROM tblfaecher", connection);
            da.FillSchema(faecher, SchemaType.Source);
            da.Fill(faecher);


            var schuelerSJList = schuelerSJ.Rows.Cast<DataRow>();

             // Klassen importieren
            foreach (DataRow row in klassen.Rows)
            {
                foreach (var SJ in schuelerSJList.Where( x => x["Klasse"].ToString() == row["ID"].ToString()).Select( y => y["Schuljahr"].ToString().Substring(0,4)).Distinct())
                {
                    var K = uow.Klassen.Get(row["Bezeichnung"].ToString(), int.Parse(SJ));
                    if (K == null)
                        K = uow.Klassen.Add(new Klasse() { Name = row["Bezeichnung"].ToString(), SchuljahrId = int.Parse(SJ) }); 
                }
            }                        
            uow.Save();
                       
            // Schüler importieren           
            foreach (DataRow imp in schueler.Rows)
            {
                var loc = uow.Schueler.Get(imp["Nachname"].ToString(), imp["Vorname"].ToString());
                if (loc == null)
                {
                    loc = uow.Schueler.Create();
                    loc.Nachname = imp["Nachname"].ToString();
                    loc.Vorname = imp["Vorname"].ToString();
                    loc.Geschlecht = imp["Geschlecht"].ToString() == "w" ? Geschlecht.weiblich : Geschlecht.männlich;
                    uow.Schueler.Add(loc);
                    uow.Save();

                     foreach (var SJ in schuelerSJList.Where( x => x["Schueler"].ToString() == imp["ID"].ToString()))
                    {
                         var oldK = klassen.Rows.Find(int.Parse(SJ["Klasse"].ToString()));
                        var K = uow.Klassen.Get(oldK["Bezeichnung"].ToString(), int.Parse(SJ["Schuljahr"].ToString().Substring(0,4)));
                        if (K != null)                        
                            loc.Klassen.Add(K);
                     }
                }
            }
            uow.Save();
            
            // Fächer importieren
            foreach (DataRow imp in faecher.Rows)
            {
                var loc = uow.Fächer.Get(imp["Bezeichnung"].ToString());
                if (loc == null)
                {
                    loc = uow.Fächer.Add(new Fach(imp["Bezeichnung"].ToString()));
                }
            }

            // Add Beobachtungen
            foreach (DataRow imp in beobachtungen.Rows)
            {
                var loc = uow.Beobachtungen.Create();
                loc.Datum = imp["Datum"] == null ? null : (DateTime?) imp["Datum"];
                 
                string Fach = (int) imp["Fach"] == -1 ? null : (faecher.Rows.Find((int) imp["Fach"])["Bezeichnung"].ToString());
                loc.Fach = uow.Fächer.Get(Fach);                     
             
                  var SJ = schuelerSJList.Where( x => x["ID"].ToString() == imp["Schueler"].ToString()).FirstOrDefault();
                   
                loc.SchuljahrId = int.Parse(SJ["Schuljahr"].ToString().Substring(0,4));                                   

                var SS = schueler.Rows.Find((int) SJ["Schueler"]);
                if ( SS != null)
                {
                    loc.Schueler = uow.Schueler.Get(SS["Nachname"].ToString(), SS["Vorname"].ToString());                    
                }
                loc.Text = imp["Text"].ToString();
             
               // uow.Beobachtungen.Add(b);
                uow.Beobachtungen.Add(loc);
            }

            uow.Save();
        }
     }
}
