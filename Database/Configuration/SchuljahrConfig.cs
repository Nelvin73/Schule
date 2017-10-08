using Groll.Schule.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Groll.Schule.Datenbank.Configuration
{  
    public class SchuljahrConfiguration : EntityTypeConfiguration<Schuljahr>
    {
        public SchuljahrConfiguration()
        {
            // Set table name 
            this.ToTable("Schuljahre");

            // set primary key
            this.HasKey(x => x.Startjahr);

            // disable value generation for key
            this.Property(x => x.Startjahr).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
        }
    }
}
