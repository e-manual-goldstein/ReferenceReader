using Microsoft.Build.Construction;
using System.Linq;

namespace ReferenceReader
{
    public class ProjectReference : AbstractReference
    {
        public ProjectReference(ProjectItemElement item)
            : base(item)
        {
            // Additional constructor logic specific to ProjectReference
        }

        protected override void SetProperties(ProjectItemElement item)
        {
            Project = item.Metadata.FirstOrDefault(m => m.Name == "Project")?.Value;
            PrivateAssets = item.Metadata.FirstOrDefault(m => m.Name == "PrivateAssets")?.Value;
            // Set specific properties for ProjectReference
        }
    }
}