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
        public enum DatabaseType { Standard, Development, Custom, None }

        #region OnDatabaseChanged-Event
        public event EventHandler<EventArgs> DatabaseChanged;

        private void OnDatabaseChanged()
        {
            if (DatabaseChanged != null)
                foreach (EventHandler<EventArgs> i in DatabaseChanged.GetInvocationList())
                    i(this, new EventArgs());

        }
        #endregion

        #region öffentliche Eigenschaften
        public string CurrentDbFilename
        {
            get
            {               
                if (context == null)
                    return "";
                else
                    return context.Database.Connection.DataSource.Replace("|DataDirectory|", "");
            }
        }

        public DatabaseType CurrentDbType
        {
            get
            {
                return currentDBtype;
            }
        }
        #endregion

        #region interne Felder
        private SchuleContext context;
        private DatabaseType currentDBtype = DatabaseType.None;

        private SchuelerRepository repSchueler;
        private KlassenRepository repKlassen;
        private RepositoryBase<Textbaustein> repTextbausteine;
        private FaecherRepository repFächer;
        private RepositoryBase<Beobachtung> repBeobachtungen;
        private RepositoryBase<Schuljahr> repSchuljahre;
        private RepositoryBase<Stundenplan> repStundenplan;
        private RepositoryBase<Klassenarbeit> repKlassenarbeiten;
        private SettingsRepository repSettings;
        #endregion

        #region Repositories
        

        public RepositoryBase<Klassenarbeit> Klassenarbeiten
        {
            get {
                if (repKlassenarbeiten == null && context != null)
                    repKlassenarbeiten = new RepositoryBase<Klassenarbeit>(context); 
                return repKlassenarbeiten;
            }            
        }
        
        public SettingsRepository Settings
        {
            get
            {
                if (repSettings == null && context != null)
                    repSettings = new SettingsRepository(context);

                return repSettings; ;
            }
        }

        public RepositoryBase<Beobachtung> Beobachtungen
        {
            get
            {
                if (repBeobachtungen == null && context != null)
                    repBeobachtungen = new RepositoryBase<Beobachtung>(context);                

                return repBeobachtungen; ;
            }
        }

        public RepositoryBase<Textbaustein> Textbausteine
        {
            get
            {
                if (repTextbausteine == null && context != null)
                    repTextbausteine = new RepositoryBase<Textbaustein>(context);

                return repTextbausteine; ;
            }
        }
        
        public RepositoryBase<Schuljahr> Schuljahre
        {
            get
            {
                if (repSchuljahre == null && context != null)
                    repSchuljahre = new RepositoryBase<Schuljahr>(context);

                return repSchuljahre; ;
            }
        }

        public FaecherRepository Fächer
        {
            get
            {
                if (repFächer == null && context != null)
                    repFächer = new FaecherRepository(context);

                return repFächer; ;
            }           
        }
        
        public SchuelerRepository Schueler
        {
            get
            {
                if (repSchueler == null && context != null)
                    repSchueler = new SchuelerRepository(context);

                return repSchueler;
            }
        }

        public KlassenRepository Klassen
        {
            get
            {
                if (repKlassen == null && context != null)
                    repKlassen = new KlassenRepository(context);

                return repKlassen;
            }
        }

        public RepositoryBase<Stundenplan> Stundenpläne
        {
            get
            {
                if (repStundenplan == null && context != null)
                    repStundenplan = new RepositoryBase<Stundenplan>(context);

                return repStundenplan;
            }
        }


        #endregion

        public UowSchuleDB()
        {
            // Default Konstruktor verbindet sich noch nicht mit der Datenbank.            
        }

        public UowSchuleDB(DatabaseType DBtype, string Filename = "")
        {
            ConnectDatabase(DBtype, Filename);                     
        }

        #region Public Interface
        public void Save()
        {            
            if (context != null)
                context.SaveChanges();
        }


        public void Dispose()
        {
            if (context != null)
                context.Dispose();
        }

        public enum DumpData { All, Schüler, Fächer, Klassen, Beobachtungen }

        public void DumpContext(DumpData Filter = DumpData.All)
        {
            if (context == null)
                return;

            System.Diagnostics.Debug.WriteLine("Dumping Context\n===========================\n");

            if (Filter == DumpData.Schüler || Filter == DumpData.All)
            {
                System.Diagnostics.Debug.WriteLine("Schueler\n===========================");
                foreach (var s in context.ChangeTracker.Entries<Schueler>())
                {
                    System.Diagnostics.Debug.WriteLine("[{0}] {1} ({2})", s.Entity.ID, s.Entity.DisplayName, s.State.ToString());
                }
            }

            if (Filter == DumpData.Fächer || Filter == DumpData.All)
            {
                System.Diagnostics.Debug.WriteLine("\n\nFächer\n===========================");
                foreach (var s in context.ChangeTracker.Entries<Fach>())
                {
                    System.Diagnostics.Debug.WriteLine("[{0}] {1} ({2})", s.Entity.FachId, s.Entity.Name, s.State.ToString());
                }
            }

            if (Filter == DumpData.Klassen || Filter == DumpData.All)
            {

                System.Diagnostics.Debug.WriteLine("\n\nKlassen\n===========================");
                foreach (var s in context.ChangeTracker.Entries<Klasse>())
                {
                    System.Diagnostics.Debug.WriteLine("[{0}] {1} - {2} ({3})", s.Entity.KlasseId, s.Entity.Name, s.Entity.Schuljahr.ToString(), s.State.ToString());
                }
            }

            if (Filter == DumpData.Beobachtungen || Filter == DumpData.All)
            {
                System.Diagnostics.Debug.WriteLine("\n\nBeobachtungen\n===========================");
                foreach (var s in context.ChangeTracker.Entries<Beobachtung>())
                {
                    System.Diagnostics.Debug.WriteLine("[{0}] {1} ({2})", s.Entity.BeobachtungId, s.Entity.ToString(), s.State.ToString());
                }
            }
        }
        
        public void ConnectDatabase(DatabaseType DBtype, string Filename = "")
        {
                        
            if (CurrentDbType == DBtype && Filename == CurrentDbFilename)
                return;
            
            switch (DBtype)
            {
                case DatabaseType.Standard:
                    context = SchuleContext.Open();
                    break;

                case DatabaseType.Development:
                    context = SchuleContext.OpenDev();
                    break;

                case DatabaseType.Custom:
                    context = SchuleContext.Open(Filename);
                    break;
            }

            currentDBtype = DBtype;
            PreloadDatabase();
            ResetRepositories();

            // Fire event
            OnDatabaseChanged();
            
        }

        public void TriggerDatabaseChangedEvent() { OnDatabaseChanged(); }

        #endregion

        #region interne Hilfsfunktionen
        private void PreloadDatabase()
        {
            if (context == null)
                return;

            context.Schueler.Load();
            context.Fächer.Load();
            context.Klassen.Include("Schueler").Load();
            context.Schuljahre.Load();
            context.Beobachtungen.Load();
            context.Textbausteine.Load();
            context.Stundenpläne.Include("Stunden").Load();
            context.Klassenarbeiten.Include("Noten").Load();
        }

        private void ResetRepositories()
        {
            repBeobachtungen = null;
            repFächer = null;
            repKlassen = null;
            repSchueler = null;
            repSchuljahre = null;
            repSettings = null;
        }

        #endregion
    }
}
