using Microsoft.Build.Construction;
using Microsoft.Build.Evaluation;
using System.Collections.Generic;
using System.Linq;

namespace ReferenceReader
{
    public class PackageReference : AbstractReference
    {
        public PackageReference(ProjectItemElement item)
            : base(item)
        {
            // Additional constructor logic specific to PackageReference
        }

        protected override void SetProperties(ProjectItemElement item)
        {
            // Set specific properties for PackageReference
        }
    }
}
