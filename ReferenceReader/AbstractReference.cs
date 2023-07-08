using System.Linq;
using Microsoft.Build.Construction;

namespace ReferenceReader
{
    public abstract class AbstractReference
    {
        public string Include { get; set; }
        public string Condition { get; set; }
        public string Alias { get; set; }
        public string Update { get; set; }
        public string Name { get; set; }
        public string Project { get; set; }
        public string Package { get; set; }
        public string PrivateAssets { get; set; }
        // Add more properties as needed

        protected AbstractReference(ProjectItemElement item)
        {
            Include = item.Include;
            Condition = item.Condition;
            Alias = item.Metadata.FirstOrDefault(m => m.Name == "Alias")?.Value;
            Update = item.Metadata.FirstOrDefault(m => m.Name == "Update")?.Value;
            // Assign other common properties

            // Specific properties for different reference types
            if (item.ItemType == "Reference")
            {
                Name = item.Metadata.FirstOrDefault(m => m.Name == "Name")?.Value;
            }
            else if (item.ItemType == "ProjectReference")
            {
                Name = item.Metadata.FirstOrDefault(m => m.Name == "Name")?.Value;
                Project = item.Metadata.FirstOrDefault(m => m.Name == "Project")?.Value;
                PrivateAssets = item.Metadata.FirstOrDefault(m => m.Name == "PrivateAssets")?.Value;
            }
            else if (item.ItemType == "PackageReference")
            {
                Name = item.Metadata.FirstOrDefault(m => m.Name == "Name")?.Value;
                Package = item.Metadata.FirstOrDefault(m => m.Name == "Package")?.Value;
            }
            // Add more properties for other reference types
        }
    }
}