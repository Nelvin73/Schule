using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Groll.Schule.Model
{
    public class Schuljahr
    {
        private const int StartMonat = 9;

        public int Startjahr { get; set; }
        //public virtual List<Klasse> Klassen { get; set; }

        public Schuljahr(int startjahr) { Startjahr = startjahr; }
        public Schuljahr() { }

        public DateTime GetStart() { return new DateTime(Startjahr, StartMonat, 1); }
        public DateTime GetEnd() { return new DateTime(Startjahr + 1, StartMonat, 1).AddDays(-1); }
        
        public static Schuljahr GetCurrent()
        {
            return new Schuljahr( DateTime.Now.Year - (DateTime.Now.Month >= StartMonat ? 0 : 1));
        }

        public override string ToString()
        {
            return Startjahr + "/" + (Startjahr + 1);
        }

       public override bool Equals(object obj)
        {
            var j = obj as Schuljahr;
            return (j != null && j.Startjahr == this.Startjahr);            
        }
       
        public override int GetHashCode()
        {
            return Startjahr;
        }
         

    }
}
