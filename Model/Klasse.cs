﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Groll.Schule.Model
{
    public class Klasse
    {
        public int KlasseId { get; set; }
        public string Name { get; set; }
        public int SchuljahrId { get; set; }

        public virtual ObservableCollection<Schueler> Schueler { get; set; }        
        public virtual Schuljahr Schuljahr { get; set; }

        public override string ToString()
        {
            if ((Name ?? "") == "")
                return "<Keine Klasse>";
            return "Klasse " + (Name ?? "")  + " in Schuljahr " + (Schuljahr == null ? "" : Schuljahr.ToString());
        }
    }
}
