using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Collections.ObjectModel;
using Groll.Schule.Model;

namespace Groll.Schule.DataManager.Repositories
{
    // Öffentliche Schnittstelle zu der Schuljahr-Tabelle
    public class SchuelerManager : IDisposable
    {
        private SchuleContext connection = new SchuleContext();

        public SchuelerManager()
        {
           // connection.Schuljahre.Load();
        }
        

        public ObservableCollection<Schueler> GetUnmapped(Schuljahr schuljahr)
        {
            var s = from sch in connection.Schueler
                    where sch.Klassen.FirstOrDefault(x => x.Jahr.Startjahr == schuljahr.Startjahr) == null
                    select sch;

            return new ObservableCollection<Schueler>(s);
        }



        /// <summary>
        /// Gibt das Schuljahr-Objekt des aktuellen Schuljahres zurück
        /// </summary>
        /// <returns>Aktuelles Schuljahr</returns>       

        public bool Exists(int startYear)
        {
            return GetByYear(startYear) != null;
        }      

        public Schuljahr GetByYear(int startYear)
        {
            if (startYear < 2000)
                throw new ArgumentOutOfRangeException("wrong year");

            return connection.Schuljahre.Find(startYear);
        }
              
        
        public ObservableCollection<Schuljahr> GetList()
        {
            return new ObservableCollection<Schuljahr>(connection.Schuljahre);
        }

        public void Remove(int startYear)
        {
            Remove(GetByYear(startYear));           
        }

        public void Remove(Schuljahr schuljahr)
        {        
            if (schuljahr != null)
                connection.Schuljahre.Remove(schuljahr);

        }

        public Schuljahr Add(int startYear)
        {
            if (!Exists(startYear))
                try
                {
                    Schuljahr neu = connection.Schuljahre.Add(new Schuljahr(startYear));                                               
                    connection.SaveChanges();
                    return neu;
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("Exception in AddSchuljahr: " + e.Message);                           
                }
            return null;
        }

        public void Add(Schuljahr schuljahr)
        {
            if (!Exists(schuljahr.Startjahr))
                try
                {
                    connection.Schuljahre.Add(schuljahr);
                    connection.SaveChanges();                  
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("Exception in AddSchuljahr: " + e.Message);
                }        
        }
  
    //    public Schuljahr Add(DateTime datum)
      //  {
         //   return Add(GetStartJahr(datum));
    //    }

  //      public Schuljahr AddCurrent()
    //    {
      //      return Add(GetStartJahr(DateTime.Now));
   //     }

      
      
        public void Dispose()
        {
            connection.Dispose();
            
        }
    }
}
