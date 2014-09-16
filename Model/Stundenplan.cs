using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Groll.Schule.Model
{
    public enum Wochentag { Montag, Dienstag, Mittwoch, Donnerstag, Freitag, Samstag, Sonntag }
    public class Stundenplan
    {
        private const int StundenProTag = 10;

        public int StundenplanId { get; set; }
        public virtual Klasse Klasse { get; set; }
        public virtual List<Unterrichtsstunde> Stunden { get; set; }

        public List<Fach> Montag { get { return Fächer(Wochentag.Montag); } }
        public List<Fach> Dienstag { get { return Fächer(Wochentag.Dienstag); } }
        public List<Fach> Mittwoch { get { return Fächer(Wochentag.Mittwoch); } }
        public List<Fach> Donnerstag { get { return Fächer(Wochentag.Donnerstag); } }
        public List<Fach> Freitag { get { return Fächer(Wochentag.Freitag); } }
        public List<Fach> Samstag { get { return Fächer(Wochentag.Samstag); } }

        public List<Fach> Fächer(Wochentag Tag)
        {
            return (
                from s in Stunden
                where s.Tag == Tag
                orderby s.Stunde
                select s.Fach).ToList();                
        }
    }

    public class Unterrichtsstunde
    {
        public int StundenplanId { get; set; }
        public Wochentag Tag { get; set; }
        public int Stunde { get; set; }
        public virtual Fach Fach { get; set; }
        public virtual Stundenplan Stundenplan { get; set; }
       
        public override string ToString()
        {
            return Tag.ToString() + ": (" + Stunde + ") " + Fach.Name;
        }
    }
}
