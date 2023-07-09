using Microsoft.Build.Construction;
using Microsoft.Build.Evaluation;
using System;
using System.Collections.Generic;
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
        public string ContainedProjectPath { get; protected set; }

        public AbstractReference(ProjectItemElement item)
        {
            Include = item.Include;
            Condition = item.Condition;
            Alias = item.GetMetadataValue("Alias");
            Update = item.GetMetadataValue("Update");
            // Assign other common properties
            Name = item.GetMetadataValue("Name");
            ContainedProjectPath = item.GetMetadataValue("ContainedProject");

        }

        public virtual IEnumerable<ITransitiveDependency> GetTransitiveDependencies(RootProject rootProject)
        {
            // Logic to determine transitive dependencies specific to PackageReference
            // Return the transitive dependencies as IEnumerable<ITransitiveDependency>
            yield break;
        }
    }
}
