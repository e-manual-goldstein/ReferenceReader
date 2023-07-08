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
            Project = item.GetMetadataValue("Project");
            PrivateAssets = item.GetMetadataValue("PrivateAssets");
            // Set specific properties for ProjectReference
        }
    }
}