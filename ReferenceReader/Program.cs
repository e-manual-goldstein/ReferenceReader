using Microsoft.Build.Construction;
using Microsoft.Build.Evaluation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ReferenceReader
{
    public class Program
    {
        static Dictionary<string, DllReference> _dllReferences = new Dictionary<string, DllReference>();
        static Dictionary<string, ProjectReference> _projectReferences = new Dictionary<string, ProjectReference>();
        static Dictionary<string, PackageReference> _packageReferences = new Dictionary<string, PackageReference>();

        private static ConfigManager configManager;

        public static void Main()
        {
            configManager = new ConfigManager();
            configManager.SettingNotFound += HandleSettingNotFound;

            string projectFilePath = configManager.GetConfigSetting("ProjectFilePath");

            if (!File.Exists(projectFilePath))
            {
                Console.WriteLine($"Project file not found at the specified location. {projectFilePath}");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
                return; // Exit the program
            }

            RootProject rootProject = new RootProject(projectFilePath);
            GetReferences(projectFilePath);

            foreach (var (key, @ref) in _dllReferences)
            {
                IEnumerable<ITransitiveDependency> transitiveDependencies = @ref.GetTransitiveDependencies(rootProject).ToList();
                // Process the transitive dependencies as needed
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }


        static void GetReferences(string projectFilePath)
        {
            ProjectRootElement root = ProjectRootElement.Open(projectFilePath);
            
            foreach (var itemGroup in root.ItemGroups)
            {
                foreach (var item in itemGroup.Items)
                {
                    switch (item.ItemType)
                    {
                        case "Reference":
                            var dllReference = new DllReference(item);
                            _dllReferences[dllReference.Name] = dllReference;
                            break;
                        case "PackageReference":
                            var packageReference = new PackageReference(item);
                            _packageReferences[packageReference.Name] = packageReference;
                            break;
                        case "ProjectReference":
                            var projectReference = new ProjectReference(item);
                            _projectReferences[projectReference.Name] = projectReference;
                            break;
                        default:
                            continue;
                    }
                }
            }
        }

        static AbstractReference CreateReference(ProjectItemElement item)
        {
            switch (item.ItemType)
            {
                case "Reference":
                    return new DllReference(item);
                case "PackageReference":
                    return new PackageReference(item);
                case "ProjectReference":
                    return new ProjectReference(item);
                default:
                    return null;
            }


        }
        private static string HandleSettingNotFound()
        {
            Console.WriteLine("Please input a value for the following config setting: ProjectFilePath");
            return Console.ReadLine();
        }
    }

    // Rest of the classes and interfaces...
}
