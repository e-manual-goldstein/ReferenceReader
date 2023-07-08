using Microsoft.Build.Construction;
using Microsoft.Build.Evaluation;
using System.Collections.Generic;
using System.Linq;

namespace ReferenceReader
{
    public class DllReference : AbstractReference
    {
        public DllReference(ProjectItemElement item)
            : base(item)
        {
            // Additional constructor logic specific to DllReference
        }

    }
}
