using Microsoft.Build.Construction;
using Mono.Cecil;
using ReferenceReader.Dependencies;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace ReferenceReader.References
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

        public override IEnumerable<ITransitiveDependency> GetTransitiveDependencies(ProjectFile projectFile)
        {
            // Load the DLL using Cecil
            AssemblyDefinition assembly = AssemblyDefinition.ReadAssembly(ActualPath);

            // Iterate through each referenced assembly
            foreach (AssemblyNameReference reference in assembly.MainModule.AssemblyReferences)
            {
                // Create a TransitiveDependency instance with the assembly name as the name
                TransitiveDependency dependency = new DllDependency(reference.Name);

                // Yield the dependency
                yield return dependency;
            }
        }
    }
}
