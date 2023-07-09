using Microsoft.Build.Construction;
using Microsoft.Build.Evaluation;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ReferenceReader
{
    public class DllReference : AbstractReference
    {
        public string HintPath { get; }
        
        public DllReference(ProjectItemElement item)
            : base(item)
        {
            HintPath = item.GetMetadataValue("HintPath");
            ActualPath = ResolveActualPath();
            Name = Regex.Match(item.Include, @"(?'Name'^[^\,]+)(,|$)").Groups["Name"].Value;
        }

        protected override string ResolveActualPath()
        {
            if (!string.IsNullOrEmpty(ContainingProjectPath) && !string.IsNullOrEmpty(HintPath))
            {
                string directory = Path.GetDirectoryName(ContainingProjectPath);
                return Path.GetFullPath(Path.Combine(directory, HintPath));
            }
            return null;
        }

        public override IEnumerable<ITransitiveDependency> GetTransitiveDependencies(ProjectFile rootProject)
        {
            // Logic to determine transitive dependencies specific to PackageReference
            // Return the transitive dependencies as IEnumerable<ITransitiveDependency>
            yield break;
        }
    }
}
