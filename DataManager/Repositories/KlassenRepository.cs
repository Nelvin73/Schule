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
    public class KlassenRepository : RepositoryBase<Klasse>
    {
        public KlassenRepository(SchuleContext context) : base(context) { }

        public Klasse Get(string Name, int Schuljahr)
        {
            return Get(x => x.Name == Name && x.SchuljahrId == Schuljahr);
        }

        public Klasse Get(string Name, Schuljahr Schuljahr)
        {
            return Get(Name, Schuljahr.Startjahr);
        }

        public Klasse Add(string Name, Schuljahr Schuljahr)
        {
            return Add(new Klasse() {Name = Name, Schuljahr = Schuljahr, Schueler = new ObservableCollection<Schueler>()});
        }

        public Klasse Add(string Name, int Schuljahr)
        {
            return Add(new Klasse() { Name = Name, SchuljahrId = Schuljahr, Schueler = new ObservableCollection<Schueler>() });
        }

      
        
    }
}
