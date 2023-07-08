using Microsoft.Build.Construction;
using Microsoft.Build.Evaluation;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ReferenceReader
{
    public class ProjectReference : AbstractReference
    {
        public string RelativePath { get; protected set; }
        public string ActualPath { get; protected set; }

        public ProjectReference(ProjectItemElement item)
            : base(item)
        {
            // Additional constructor logic specific to ProjectReference
            Project = item.GetMetadataValue("Project");
            PrivateAssets = item.GetMetadataValue("PrivateAssets");

            RelativePath = item.Include;
            ActualPath = ResolveActualPath(RelativePath);
            Name = Path.GetFileNameWithoutExtension(RelativePath);
        }

        private string ResolveActualPath(string relativePath)
        {
            if (!string.IsNullOrEmpty(ContainingProjectPath))
            {
                string directory = Path.GetDirectoryName(ContainingProjectPath);
                return Path.GetFullPath( Path.Combine(directory, relativePath));
            }
            return null;
        }
    }
}
