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
