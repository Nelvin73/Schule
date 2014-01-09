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
    // Öffentliche Schnittstelle zu der Schuljahr-Tabelle
    public class SchuljahrRepository : RepositoryBase<Schuljahr>
    {
        private SchuleContext context = null;
    //    private const int SchuljahrStartMonth = 9;

        public SchuljahrRepository(SchuleContext Context) : base(Context) { }
        
        /// <summary>
        /// Gibt das Schuljahr-Objekt des aktuellen Schuljahres zurück
        /// </summary>
        /// <returns>Aktuelles Schuljahr</returns>
        public Schuljahr GetCurrent()
        {            
            return GetByYear(GetStartJahr(DateTime.Now));
        }

        public bool Exists(int startYear)
        {
            return GetByYear(startYear) != null;
        }      

        public Schuljahr GetByYear(int startYear)
        {
            if (startYear < 2000)
                throw new ArgumentOutOfRangeException("wrong year");

            return context.Schuljahre.Find(startYear);
        }
              
        
        public ObservableCollection<Schuljahr> GetList()
        {
            return new ObservableCollection<Schuljahr>(context.Schuljahre.OrderBy(x => x.Startjahr));
        }

        public void Remove(int startYear)
        {
            Remove(GetByYear(startYear));           
        }

        public void Remove(Schuljahr schuljahr)
        {        
            if (schuljahr != null)
                context.Schuljahre.Remove(schuljahr);

        }

        public Schuljahr Add(int startYear)
        {
            if (!Exists(startYear))
                try
                {
                    Schuljahr neu = context.Schuljahre.Add(new Schuljahr(startYear));
                    context.SaveChanges();
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
                    context.Schuljahre.Add(schuljahr);
                    context.SaveChanges();                  
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("Exception in AddSchuljahr: " + e.Message);
                }        
        }
  
        public Schuljahr Add(DateTime datum)
        {
            return Add(GetStartJahr(datum));
        }

        public Schuljahr AddCurrent()
        {
            return Add(GetStartJahr(DateTime.Now));
        }


        #region private helper
        //private int GetStartJahr(DateTime datum)
        //{
        //    int currentYear = DateTime.Now.Year;
        //    int currentMonth = DateTime.Now.Month;
        //    return currentYear - (currentMonth < SchuljahrStartMonth ? 1 : 0);
        //}
        #endregion

    }
}
