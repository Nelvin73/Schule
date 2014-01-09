using Groll.Schule.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Groll.Schule.DataAccess.Configuration
{   
    public class KlasseConfiguration : EntityTypeConfiguration<Klasse>
    {
        public KlasseConfiguration()
        {
            // Set table name 
            this.ToTable("Klassen");
            this.HasMany( x=>x.Schueler).WithMany( y => y.Klassen).Map(v => v.ToTable("Klassen_Schüler"));
            this.HasRequired(x => x.Schuljahr).WithMany().HasForeignKey(c => c.SchuljahrId);
        }
    }
}
