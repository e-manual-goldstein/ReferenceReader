﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Text.Json;
using System.Xml.Linq;
using Microsoft.Build.Construction;

namespace ReferenceReader
{
    public class Program
    {
        static Dictionary<string, DllReference> dllReferences = new Dictionary<string, DllReference>();
        static Dictionary<string, ProjectReference> projectReferences = new Dictionary<string, ProjectReference>();
        static Dictionary<string, PackageReference> packageReferences = new Dictionary<string, PackageReference>();

        private static ConfigManager configManager;

        public static void Main()
        {
            configManager = new ConfigManager();
            configManager.SettingNotFound += HandleSettingNotFound;

            string projectFilePath = configManager.GetConfigSetting("ProjectFilePath");

            if (projectFilePath == null)
            {
                Console.WriteLine($"Project file not found at the specified location. {projectFilePath}");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
                return; // Exit the program
            }


            ProjectRootElement projectRootElement = ProjectRootElement.Open(projectFilePath);
            var itemGroups = projectRootElement.ItemGroups;
            string packageDirectoryRelativePath = configManager.GetConfigSetting("PackageDirectory");
            foreach (var itemGroup in itemGroups)
            {
                foreach (var item in itemGroup.Items)
                {
                    if (item.ItemType == "Reference")
                    {
                        DllReference dllReference = new DllReference(item);
                        dllReferences[dllReference.Name] = dllReference;
                    }
                    else if (item.ItemType == "ProjectReference")
                    {
                        ProjectReference projectReference = new ProjectReference(item);
                        projectReferences[projectReference.Name] = projectReference;
                    }
                    else if (item.ItemType == "PackageReference")
                    {
                        PackageReference packageReference = new PackageReference(item, packageDirectoryRelativePath);
                        packageReferences[packageReference.Name] = packageReference;
                    }
                }
            }

            // Use the dictionaries as needed
            Console.WriteLine("DLL References:");
            foreach (var dllReference in dllReferences.Values)
            {
                Console.WriteLine($"{dllReference.Name} - {dllReference.ActualPath}");
                // Print other properties specific to DLL references
            }

            Console.WriteLine("\nProject References:");
            foreach (var projectReference in projectReferences.Values)
            {
                Console.WriteLine($"{projectReference.Name} - {projectReference.ActualPath}");
                // Print other properties specific to project references
            }

            Console.WriteLine("\nPackage References:");
            foreach (var packageReference in packageReferences.Values)
            {
                Console.WriteLine($"{packageReference.Name}  -  {packageReference.ActualPath}");               
                // Print other properties specific to package references
            }
        }

        //private static string GetProjectFilePath()
        //{
        //    var configPath = "config.json";
        //    string configContent;

        //    if (!File.Exists(configPath))
        //    {
        //        var defaultConfig = new { ProjectFilePath = "Path/To/YourProject.csproj" };
        //        configContent = JsonSerializer.Serialize(defaultConfig, new JsonSerializerOptions { WriteIndented = true });
        //        File.WriteAllText(configPath, configContent);
        //        Console.WriteLine($"A new config.json file has been created at: {Path.GetFullPath(configPath)}");
        //    }
        //    else
        //    {
        //        configContent = File.ReadAllText(configPath);
        //    }

        //    var jsonConfig = JsonDocument.Parse(configContent);
        //    var projectFilePath = jsonConfig.RootElement.GetProperty("ProjectFilePath").GetString();
        //    return projectFilePath;
        //}

        private static string HandleSettingNotFound()
        {
            Console.WriteLine("Please input a value for the following config setting: ProjectFilePath");
            return Console.ReadLine();
        }
    }
}