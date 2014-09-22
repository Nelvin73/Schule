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
   
    public class StundenplanConfiguration : EntityTypeConfiguration<Stundenplan>
    {
        public StundenplanConfiguration()
        {
            // set table name
            this.ToTable("Stundenpläne");
            this.Ignore(x => x.Montag).Ignore(x => x.Dienstag).Ignore(x => x.Mittwoch).Ignore(x => x.Donnerstag).Ignore(x => x.Freitag).Ignore(x => x.Samstag);
        }
    }
   
}
