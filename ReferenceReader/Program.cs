using Microsoft.Build.Construction;
using Microsoft.Build.Evaluation;
using ReferenceReader.Dependencies;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ReferenceReader
{
    public class Program
    {

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

            ProjectFile rootProject = new ProjectFile(projectFilePath);
            rootProject.GetReferences();

            foreach (var @ref in rootProject.AllReferences())
            {
                Console.WriteLine(@ref.Name);
                foreach (var dependency in @ref.GetTransitiveDependencies(rootProject))
                {
                    Console.WriteLine($"\t{dependency.Name}");
                }
                
                // Process the transitive dependencies as needed
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }


       
        private static string HandleSettingNotFound()
        {
            Console.WriteLine("Please input a value for the following config setting: ProjectFilePath");
            return Console.ReadLine();
        }
    }

    // Rest of the classes and interfaces...
}
