using Microsoft.Build.Construction;
using Microsoft.Build.Evaluation;
using ReferenceReader.Dependencies;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ReferenceReader.References
{
    public class ProjectReference : AbstractReference
    {
        public string RelativePath { get; protected set; }

        public ProjectReference(ProjectItemElement item)
            : base(item)
        {
            // Additional constructor logic specific to ProjectReference
            Project = item.GetMetadataValue("Project");
            PrivateAssets = item.GetMetadataValue("PrivateAssets");

            RelativePath = item.Include;
            Name = Path.GetFileNameWithoutExtension(RelativePath);
            ActualPath = ResolveActualPath();
        }


        ProjectFile _projectFile;
        public ProjectFile ProjectFile => _projectFile ?? LoadProjectFileFromPath();

        private ProjectFile LoadProjectFileFromPath()
        {
            if (!string.IsNullOrEmpty(ActualPath) && File.Exists(ActualPath))
            {
                return new ProjectFile(ActualPath);
            }
            return null;
        }

        protected override string ResolveActualPath()
        {
            if (!string.IsNullOrEmpty(ContainingProjectPath))
            {
                string directory = Path.GetDirectoryName(ContainingProjectPath);
                return Path.GetFullPath(Path.Combine(directory, Include));
            }
            return null;
        }

        public override IEnumerable<ITransitiveDependency> GetTransitiveDependencies(ProjectFile projectFile)
        {
            //foreach (var reference in ProjectFile.AllReferences())
            //{
                yield return null;
            //}
        }
    }
}
