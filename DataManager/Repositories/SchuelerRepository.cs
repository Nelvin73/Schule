using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Collections.ObjectModel;
using Groll.Schule.Model;
using Groll.Schule.Datenbank;


namespace Groll.Schule.DataManager.Repositories
{
    // Öffentliche Schnittstelle zu der Schüler-Tabelle
    public class SchuelerRepository : RepositoryBase<Schueler>
    {
        public SchuelerRepository(SchuleContext context) : base(context) { }

        public Schueler Get(string Nachname, string Vorname)
        {
            return Get(x => x.Nachname == Nachname && x.Vorname == Vorname);
        }

        
      
      
        
    }
}
