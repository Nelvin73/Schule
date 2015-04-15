using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Groll.Schule.OutputTools
{
    public abstract class OutputTemplateBase : IOutputTemplate
    {
        public string Name { get; protected set; }
        public string Description { get; protected set; }
        public string Group { get; protected set; }
        public string SubGroup { get; protected set; }

        public OutputTemplateBase()
        {
            Group = "Sonstiges";
        }

        public bool HasConfig
        {
            get; protected set;
        }

        public abstract void ShowConfig(Schule.DataManager.UowSchuleDB uow = null);

        public abstract void Start(Schule.DataManager.UowSchuleDB uow = null);
       
    }
}
