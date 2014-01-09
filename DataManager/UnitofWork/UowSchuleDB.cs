using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Groll.Schule.Model;
using Groll.Schule.DataAccess;
using Groll.Schule.DataManager.Repositories;
using System.Data.Entity;

namespace Groll.Schule.DataManager
{
    public class UowSchuleDB : IDisposable
    {
        private SchuleContext context = new SchuleContext();
        
        private RepositoryBase<Schueler> repSchueler;
        private RepositoryBase<Klasse> repKlassen;
        private RepositoryBase<Fach> repFächer;
        private RepositoryBase<Beobachtung> repBeobachtungen;
        private RepositoryBase<Schuljahr> repSchuljahre;
        private SettingsRepository repSettings;

        public SettingsRepository Settings
        {
            get
            {
                if (repSettings == null)
                    repSettings = new SettingsRepository(context);

                return repSettings; ;
            }
        }

        public RepositoryBase<Beobachtung> Beobachtungen
        {
            get
            {
                if (repBeobachtungen == null)
                    repBeobachtungen = new RepositoryBase<Beobachtung>(context);                

                return repBeobachtungen; ;
            }
        }

        public RepositoryBase<Schuljahr> Schuljahre
        {
            get
            {
                if (repSchuljahre == null)
                    repSchuljahre = new RepositoryBase<Schuljahr>(context);

                return repSchuljahre; ;
            }
        } 
      
        public RepositoryBase<Fach> Fächer
        {
            get
            {
                if (repFächer == null)
                    repFächer = new RepositoryBase<Fach>(context);

                return repFächer; ;
            }           
        }
        
        public RepositoryBase<Schueler> Schueler
        {
            get
            {
                if (repSchueler == null)
                    repSchueler = new RepositoryBase<Schueler>(context);

                return repSchueler;
            }
        }

        public RepositoryBase<Klasse> Klassen
        {
            get
            {
                if (repKlassen == null)
                    repKlassen = new RepositoryBase<Klasse>(context);

                return repKlassen;
            }
        }

        public UowSchuleDB()
        {
            context.Schueler.Load();
            context.Fächer.Load();
            context.Klassen.Load();
            context.Schuljahre.Load();            

        }

        public void Save()
        {
            context.SaveChanges();
        }


        public void Dispose()
        {
            context.Dispose();
        }

        public void DumpContext()
        {
            System.Diagnostics.Debug.WriteLine("Dumping Context\n===========================\n");
            System.Diagnostics.Debug.WriteLine("Schueler\n===========================");
            foreach (var s in context.ChangeTracker.Entries<Schueler>())
            {
                System.Diagnostics.Debug.WriteLine("[{0}] {1} ({2})", s.Entity.ID, s.Entity.DisplayName, s.State.ToString());
            }

            System.Diagnostics.Debug.WriteLine("\n\nFächer\n===========================");
            foreach (var s in context.ChangeTracker.Entries<Fach>())
            {
                System.Diagnostics.Debug.WriteLine("[{0}] {1} ({2})", s.Entity.FachId, s.Entity.Name, s.State.ToString());
            }

            System.Diagnostics.Debug.WriteLine("\n\nKlassen\n===========================");
            foreach (var s in context.ChangeTracker.Entries<Klasse>())
            {
                System.Diagnostics.Debug.WriteLine("[{0}] {1} - {2} ({3})", s.Entity.KlasseId, s.Entity.Name, s.Entity.Schuljahr.ToString(), s.State.ToString());
            }

            System.Diagnostics.Debug.WriteLine("\n\nBeobachtungen\n===========================");
            foreach (var s in context.ChangeTracker.Entries<Beobachtung>())
            {
                System.Diagnostics.Debug.WriteLine("[{0}] {1} - {2} ({3})", s.Entity.BeobachtungId, s.Entity.Text, s.Entity.Schueler.DisplayName, s.State.ToString());
            }
        }
    }
}
