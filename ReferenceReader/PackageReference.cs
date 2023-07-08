using Microsoft.Build.Construction;
using Microsoft.Build.Evaluation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ReferenceReader
{
    public class PackageReference : AbstractReference
    {
        public string Version { get; protected set; }

        public PackageReference(ProjectItemElement item)
            : base(item)
        {
        }

        protected override void SetProperties(ProjectItemElement item)
        {
            Version = item.GetMetadataValue("Version");
        }
    }
}
