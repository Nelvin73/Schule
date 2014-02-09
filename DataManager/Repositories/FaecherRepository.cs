using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Collections.ObjectModel;
using Groll.Schule.Model;
using Groll.Schule.DataAccess;


namespace Groll.Schule.DataManager.Repositories
{
    // Öffentliche Schnittstelle zu der Schüler-Tabelle
    public class FaecherRepository : RepositoryBase<Fach>
    {
        public FaecherRepository(SchuleContext context) : base(context) { }

        public Fach Get(string Name)
        {
            return Get(x => x.Name == Name);
        }

        public List<Fach> GetActiveFächer()
        {
            return GetList(x => !x.Inaktiv);
        }
        
      
      
        
    }
}
