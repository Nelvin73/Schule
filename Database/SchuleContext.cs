using Groll.Schule.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure;
using Groll.Schule.Datenbank.Configuration;
using System.Data.Common;
using System.Data.SqlClient;

// Update-Database -connectionstring "Data Source=|DataDirectory|..\..\..\SchulDB\bin\debug\Groll.Schuldb.sdf" -connectionprovidername "System.Data.SqlServerCe.4.0" -verbose

namespace Groll.Schule.Datenbank
{
    public enum ConnectionProvider { Default, SqlCe, Sql, LocalDB11, LocalDB, Unknown, None}

    public partial class SchuleContext : DbContext
    {   

        #region Factory Methods
        /// <summary>
        /// Creates Context by attaching a LocalDB File
        /// </summary>
        /// <param name="File"></param>
        /// <param name="provider"></param>
        /// <returns></returns>
        public static SchuleContext AttachLocalDBFile(string File, ConnectionProvider provider = ConnectionProvider.LocalDB)
        {
            StringBuilder ConnectionString = new StringBuilder();
            switch (provider)
            {
                case ConnectionProvider.LocalDB11:
                    ConnectionString.Append("Data Source=(LocalDb)\\v11.0;");
                    break;

                case ConnectionProvider.LocalDB:
                    ConnectionString.Append("Data Source=(LocalDb)\\mssqllocaldb;");
                    break;

                default:
                    throw new ArgumentException("Diese Funktion unterstützt nur LocalDB!");
            }

            ConnectionString.Append("AttachDbFilename=\"" + File + "\";Initial Catalog=\"" + 
                System.IO.Path.GetFileNameWithoutExtension(File.Replace("|", "")) + 
                "\";Integrated Security=True");
            return SchuleContext.OpenDatabase(ConnectionString.ToString(), provider);
        }
        
        /// <summary>
        /// Creates Context using a custom connection String and Connection provider
        /// </summary>
        /// <param name="ConnectionString">Connection String</param>
        /// <param name="ConnectionProvider">Connection Provider </param>       
        public static SchuleContext OpenDatabase(string connectionStringOrName, ConnectionProvider provider = ConnectionProvider.Default)
        {

            if (provider == ConnectionProvider.Default)
                return new SchuleContext(connectionStringOrName);

            // dedizierter Provider
            DbConnection connection = null;
            switch (provider)
            {
                case ConnectionProvider.LocalDB11:
                    connection = new LocalDbConnectionFactory("v11.0").CreateConnection(connectionStringOrName);
                    break;

                case ConnectionProvider.LocalDB:
                    connection = new LocalDbConnectionFactory("mssqllocaldb").CreateConnection(connectionStringOrName);
                    break;

                case ConnectionProvider.SqlCe:
                    connection = new SqlCeConnectionFactory("System.Data.SqlServerCe.4.0").CreateConnection(connectionStringOrName);
                    break;
            }
                        
            return new SchuleContext(connection);                   
        }
       
        #endregion

        private SchuleContext(DbConnection conn)
            : base(conn, true)
        {
            Database.SetInitializer(new SchuleContextInitializer());
            
        }

        public SchuleContext() : this("Groll.SchulDB") {    }

        private SchuleContext(string connectionStringOrName)
            : base(connectionStringOrName)
        {
             Database.SetInitializer(new SchuleContextInitializer());                 
        }

        public DbSet<Setting> Settings { get; set; }
        public DbSet<Schueler> Schueler { get; set; }
        public DbSet<Klasse> Klassen { get; set; }
        public DbSet<Schuljahr> Schuljahre { get; set; }
        public DbSet<Fach> Fächer { get; set; }
        public DbSet<Stundenplan> Stundenpläne { get; set; }
        public DbSet<Textbaustein> Textbausteine { get; set; }
        public DbSet<Beobachtung> Beobachtungen { get; set; }
        public DbSet<Klassenarbeit> Klassenarbeiten { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Setting>().HasKey(x=> x.Key);
            modelBuilder.Configurations.Add(new SchuelerConfiguration());
            modelBuilder.Configurations.Add(new KlasseConfiguration());
            modelBuilder.Configurations.Add(new KlassenarbeitenConfiguration());
            modelBuilder.Configurations.Add(new KlassenarbeitenNotenConfig());
            
            modelBuilder.Configurations.Add(new SchuljahrConfiguration());
            modelBuilder.Configurations.Add(new FachConfiguration());
            modelBuilder.Configurations.Add(new TextbausteineConfig());
            
            modelBuilder.Configurations.Add(new UnterrichtsstundeConfiguration());
            modelBuilder.Configurations.Add(new BeobachtungConfiguration());
            modelBuilder.Configurations.Add(new StundenplanConfiguration());
            //modelBuilder.ComplexType<Adresse>();

            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// Öffnet die Standard-DB (Groll.SchulDB.sdf)
        /// </summary>
        /// <returns>SchuleContext</returns>
        public static SchuleContext Open()
        {
            return new SchuleContext();
        }

        /// <summary>
        /// Öffnet die Developer-DB (SchuleDB_dev.sdf)
        /// </summary>
        /// <returns>SchuleContext</returns>
        public static SchuleContext OpenDev()
        {
            return new SchuleContext("Groll.SchulDB_dev");           
        }


        /// <summary>
        /// Öffnet eine beliebige DB
        /// </summary>
        /// <param name="File">Filename der zu öffnenden DB</param>
        /// <returns>SchuleContext</returns>
        //public static SchuleContext Open(string File)
        //{
        //    return new SchuleContext(File);
        //}
       

    }


}
