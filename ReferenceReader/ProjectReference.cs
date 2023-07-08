using Microsoft.Build.Construction;

namespace ReferenceReader
{
    public class ProjectReference : AbstractReference
    {
        // Add specific properties for project references

        public ProjectReference(ProjectItemElement item) : base(item)
        {
            // Assign additional properties for project references
        }
    }
}