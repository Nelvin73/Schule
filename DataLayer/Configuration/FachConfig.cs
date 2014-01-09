using Groll.Schule.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Groll.Schule.DataAccess.Configuration
{
    public class FachConfiguration : EntityTypeConfiguration<Fach>
    {
        public FachConfiguration()
        {
            // Set table name 
            this.ToTable("Fächer");
        }
    }
}
