using Microsoft.Build.Construction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReferenceReader
{
    public class PackageReference : AbstractReference
    {
        // Add specific properties for package references

        public PackageReference(ProjectItemElement item) : base(item)
        {
            // Assign additional properties for package references
        }
    }
}
