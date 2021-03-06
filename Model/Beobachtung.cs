﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Groll.Schule.Model
{
    public class Beobachtung
    {
        public int BeobachtungId { get; set; }               
        public DateTime? Datum { get; set; }
        public string Text { get; set; }
        public virtual Fach Fach { get; set; }
        public int SchuljahrId { get; set; }
        public virtual Schuljahr Schuljahr { get; set; }
        public virtual Schueler Schueler { get; set; }

        public Beobachtung()
        {
            Text = "";

        }

        public override string ToString()
        {
            return string.Format("Schüler: {0}, Klasse: {1} - {2}",
                Schueler == null ? "???" : Schueler.ToString(),
                Klasse == null ? "???" : Klasse.ToString(),
                Text.Length > 20 ? Text.Substring(0, 20) + "..." : Text);
        }

        public Klasse Klasse
        {
            get
            {
                if (Schueler != null)
                    return Schueler.GetKlasse(SchuljahrId);
                else
                    return null;
            }
        }
    }
}

