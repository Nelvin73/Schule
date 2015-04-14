using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Groll.Schule.OutputTools.Arbeitsblätter
{
    public enum GrundrechenArten { Plus, Minus, Mal, Geteilt }
    
  
    public class GrundrechnenÜbungOutput : OutputTemplate
    {        

        
        
        public GrundrechnenÜbungOutput()
        {
            Name = "Grundrechenarten-Übungsblatt";
            Description = "Arbeitsblatt mit vielen Grundrechenarten.\nKann konfiguriert werden.\n\nAnwendung: z.B. Blitzrechnen";
            Group = "Arbeitsblätter";
            SubGroup = "Mathe";
            HasConfig = true;
        }

        public GrundrechnenÜbungOutput(string name, string description, string group, string subgroup, bool hasConfig = true)
        {
            if (name != "")
                Name = name;
            Description = description;
            if (group != "")
                Group = group;
            SubGroup = subgroup;
            HasConfig = hasConfig;
        }



        public override void ShowConfig()
        {
            System.Windows.MessageBox.Show(Name + "\nConfig");
        }

        public override void Start()
        {
            System.Windows.MessageBox.Show(Name + "\nStart");
        }
    }
}
