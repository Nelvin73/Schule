using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Groll.Schule.OutputTools
{

    
    /// <summary>
    /// Interface für Tools, die einen Output generieren, z.B. Arbeitsblätter
    /// </summary>
    public interface IOutputTemplate
    {
        string Name { get; }
        string Description { get; }
        string Group { get; }
        string SubGroup { get; }

        bool HasConfig { get; }

        void ShowConfig(Schule.DataManager.UowSchuleDB uow = null);
        void Start(Schule.DataManager.UowSchuleDB uow = null);

    }
}
