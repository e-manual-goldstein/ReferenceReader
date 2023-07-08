using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Xml.Linq;
using Microsoft.Build.Construction;

namespace ReferenceReader
{
    public class Program
    {
        public static void Main()
        {
            string projectFilePath = GetProjectFilePath();

            Dictionary<string, DllReference> dllReferences = new Dictionary<string, DllReference>();
            Dictionary<string, ProjectReference> projectReferences = new Dictionary<string, ProjectReference>();
            Dictionary<string, PackageReference> packageReferences = new Dictionary<string, PackageReference>();

            ProjectRootElement projectRootElement = ProjectRootElement.Open(projectFilePath);
            var itemGroups = projectRootElement.ItemGroups;

            foreach (var itemGroup in itemGroups)
            {
                foreach (var item in itemGroup.Items)
                {
                    if (item.ItemType == "Reference")
                    {
                        DllReference dllReference = new DllReference(item);
                        dllReferences[dllReference.Include] = dllReference;
                    }
                    else if (item.ItemType == "ProjectReference")
                    {
                        ProjectReference projectReference = new ProjectReference(item);
                        projectReferences[projectReference.Include] = projectReference;
                    }
                    else if (item.ItemType == "PackageReference")
                    {
                        PackageReference packageReference = new PackageReference(item);
                        packageReferences[packageReference.Include] = packageReference;
                    }
                }
            }

            // Use the dictionaries as needed
            Console.WriteLine("DLL References:");
            foreach (var dllReference in dllReferences.Values)
            {
                Console.WriteLine($"{dllReference.Include} - {dllReference.Condition} - {dllReference.Name}");
                // Print other properties specific to DLL references
            }

            Console.WriteLine("\nProject References:");
            foreach (var projectReference in projectReferences.Values)
            {
                Console.WriteLine($"{projectReference.Include} - {projectReference.Condition} - {projectReference.Name}");
                Console.WriteLine($"  Project: {projectReference.Project}");
                Console.WriteLine($"  PrivateAssets: {projectReference.PrivateAssets}");
                // Print other properties specific to project references
            }

            Console.WriteLine("\nPackage References:");
            foreach (var packageReference in packageReferences.Values)
            {
                Console.WriteLine($"{packageReference.Include} - {packageReference.Condition} - {packageReference.Name}");
                Console.WriteLine($"  Package: {packageReference.Package}");
                // Print other properties specific to package references
            }
        }

        private static string GetProjectFilePath()
        {
            var configPath = "config.json";
            var configContent = File.ReadAllText(configPath);
            var jsonConfig = JsonDocument.Parse(configContent);
            var projectFilePath = jsonConfig.RootElement.GetProperty("ProjectFilePath").GetString();
            return projectFilePath;
        }
    }
}