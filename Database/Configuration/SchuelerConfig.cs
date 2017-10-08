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

    public class SchuelerConfiguration : EntityTypeConfiguration<Schueler>
    {
        public SchuelerConfiguration()
        {
            // Set table name 
            this.ToTable("Schüler");
            Property(x => x.Foto).HasColumnType("Image");
        }
    }
}
