﻿using Groll.Schule.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Groll.Schule.DataAccess.Configuration
{  
    public class BeobachtungConfiguration : EntityTypeConfiguration<Beobachtung>
    {
        public BeobachtungConfiguration()
        {
            // Set table name 
            this.ToTable("Beobachtungen");
            this.HasRequired(c => c.Schuljahr).WithMany().HasForeignKey(k => k.SchuljahrId);
        }
    }
   
}
