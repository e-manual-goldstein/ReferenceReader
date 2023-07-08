using Microsoft.Build.Construction;
using Microsoft.Build.Evaluation;
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

        public AbstractReference(ProjectItemElement item)
        {
            Include = item.Include;
            Condition = item.Condition;
            Alias = item.Metadata.FirstOrDefault(m => m.Name == "Alias")?.Value;
            Update = item.Metadata.FirstOrDefault(m => m.Name == "Update")?.Value;
            // Assign other common properties
            Name = item.Metadata.FirstOrDefault(m => m.Name == "Name")?.Value;

            SetProperties(item);
        }

        protected abstract void SetProperties(ProjectItemElement item);
    }
}
