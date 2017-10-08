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
    public class KlassenarbeitenNotenConfig : EntityTypeConfiguration<KlassenarbeitsNote>
    {
        public KlassenarbeitenNotenConfig()
        {
            // Set table name 
            this.HasKey(x => new { x.KlassenarbeitId, x.SchülerId} );            
            this.ToTable("KlassenarbeitsNoten");
            this.Ignore(x => x.HatMitgeschrieben);            
       //     this.HasMany( x=>x.Schueler).WithMany( y => y.Klassen).Map(v => v.ToTable("Klassen_Schüler"));
        //    this.HasRequired(x => x.Schuljahr).WithMany().HasForeignKey(c => c.SchuljahrId);
        }
    }
}
