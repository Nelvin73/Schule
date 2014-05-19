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
    public class TextbausteineConfig : EntityTypeConfiguration<Textbaustein>
    {
        public TextbausteineConfig()
        {
            // Set table name 
            this.ToTable("Textbausteine");            
        }
    }
   
}
