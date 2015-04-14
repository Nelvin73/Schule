using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Groll.Schule.OutputTools
{
    public abstract class OutputTemplate : IOutputTemplate
    {
        public string Name { get; protected set; }
        public string Description { get; protected set; }
        public string Group { get; protected set; }
        public string SubGroup { get; protected set; }

        public OutputTemplate()
        {
            Group = "Sonstiges";
        }

        public bool HasConfig
        {
            get; protected set;
        }

        public abstract void ShowConfig();

        public abstract void Start();
       
    }
}
