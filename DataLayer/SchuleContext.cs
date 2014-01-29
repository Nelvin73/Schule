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
using Groll.Schule.DataAccess.Configuration;


namespace Groll.Schule.DataAccess
{
    public partial class SchuleContext : DbContext
    {
        private SchuleContext() : this("Groll.SchulDB") {    }

        private SchuleContext(string DBfile)
            : base(DBfile)
        {
            Database.DefaultConnectionFactory = new SqlCeConnectionFactory("System.Data.SqlServerCe.4.0");
            Database.SetInitializer(new SchuleContextInitializer());                 
        }

        public DbSet<Setting> Settings { get; set; }
        public DbSet<Schueler> Schueler { get; set; }
        public DbSet<Klasse> Klassen { get; set; }
        public DbSet<Schuljahr> Schuljahre { get; set; }
        public DbSet<Fach> Fächer { get; set; }
        // public DbSet<Stundenplan> Stundenpläne { get; set; }
        public DbSet<Beobachtung> Beobachtungen { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Setting>().HasKey(x=> x.Key);
            modelBuilder.Configurations.Add(new SchuelerConfiguration());
            modelBuilder.Configurations.Add(new KlasseConfiguration());
            modelBuilder.Configurations.Add(new SchuljahrConfiguration());
            modelBuilder.Configurations.Add(new FachConfiguration());
            //modelBuilder.Configurations.Add(new UnterrichtsstundeConfiguration());
            modelBuilder.Configurations.Add(new BeobachtungConfiguration());
            //modelBuilder.Configurations.Add(new StundenplanConfiguration());
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
        public static SchuleContext Open(string File)
        {
            return new SchuleContext(File);
        }

    }


}
