using Microsoft.Build.Construction;
using Microsoft.Build.Evaluation;
using ReferenceReader.Dependencies;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace ReferenceReader.References
{
    public class PackageReference : AbstractReference
    {
        public string Version { get; protected set; }
        public string PackageDirectory { get; }

        public PackageReference(ProjectItemElement item, string packageDirectory = default)
            : base(item)
        {
            Version = item.GetMetadataValue("Version");
            PackageDirectory = packageDirectory;
            ActualPath = ResolveActualPath();
        }

        protected override string ResolveActualPath()
        {
            if (!string.IsNullOrEmpty(PackageDirectory) && !string.IsNullOrEmpty(ContainingProjectPath))
            {
                var packageLocation = Path.GetFullPath(Path.Combine(ContainingProjectPath, PackageDirectory, Name, Version));
                return packageLocation;
            }
            return null;
        }

        public override IEnumerable<ITransitiveDependency> GetTransitiveDependencies(ProjectFile projectFile)
        {
            if (!string.IsNullOrEmpty(ActualPath) && Directory.Exists(ActualPath))
            {
                string nuspecFilePath = Path.Combine(ActualPath, $"{Name}.nuspec");

                if (File.Exists(nuspecFilePath))
                {
                    // Load and parse the .nuspec file
                    XDocument nuspecDocument = XDocument.Load(nuspecFilePath);

                    // Find the <files> or <files> section within the .nuspec file
                    XElement filesElement = nuspecDocument.Root?.Element("files") ?? nuspecDocument.Root?.Element("files");

                    if (filesElement != null)
                    {
                        foreach (XElement fileElement in filesElement.Elements("file"))
                        {
                            string filePath = fileElement.Attribute("src")?.Value;
                            string fileTarget = fileElement.Attribute("target")?.Value;

                            if (!string.IsNullOrEmpty(filePath) && !string.IsNullOrEmpty(fileTarget) && Path.GetExtension(filePath) == ".dll")
                            {
                                string fileName = Path.GetFileName(filePath);
                                string fileFullPath = Path.Combine(ActualPath, fileTarget, fileName);

                                // Create a DllDependency instance for the dll file
                                //DllDependency dependency = new DllDependency(fileName, fileFullPath);
                                
                                yield return null;
                            }
                        }
                    }
                }
            }
        }
    }
}
