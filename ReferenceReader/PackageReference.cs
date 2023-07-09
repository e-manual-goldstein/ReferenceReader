using Microsoft.Build.Construction;
using Microsoft.Build.Evaluation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ReferenceReader
{
    public class PackageReference : AbstractReference
    {
        public string Version { get; protected set; }
        public string PackageDirectory { get; }

        public PackageReference(ProjectItemElement item, string packageDirectory = default)
            : base(item)
        {
            Version = item.GetMetadataValue("Version");
            PackageDirectory = packageDirectory;
            ActualPath = ResolveActualPath();
        }

        protected override string ResolveActualPath()
        {
            if (!string.IsNullOrEmpty(PackageDirectory) && !string.IsNullOrEmpty(ContainingProjectPath))
            {
                var packageLocation = Path.GetFullPath(Path.Combine(ContainingProjectPath, PackageDirectory, Name, Version));
                return packageLocation;
            }
            return null;
        }
    }
}
