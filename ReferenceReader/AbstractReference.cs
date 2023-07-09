using Microsoft.Build.Construction;
using Microsoft.Build.Evaluation;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ReferenceReader
{
    public abstract class AbstractReference
    {
        public string Include { get; set; }
        public string Condition { get; set; }
        public string Alias { get; set; }
        public string Update { get; set; }
        public string Name { get; protected set; }
        public string Project { get; protected set; }
        public string Package { get; protected set; }
        public string PrivateAssets { get; protected set; }
        public string ContainingProjectPath { get; protected set; }
        public string ActualPath { get; protected set; }

        public AbstractReference(ProjectItemElement item)
        {
            Include = item.Include;
            Condition = item.Condition;
            Alias = item.GetMetadataValue("Alias");
            Update = item.GetMetadataValue("Update");
            // Assign other common properties
            Name = item.GetMetadataValue("Name") ?? item.Include;            
            ContainingProjectPath = item.ContainingProject.FullPath;

        }

        protected abstract string ResolveActualPath();
    }
}
