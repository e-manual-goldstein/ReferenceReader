using ReferenceReader;
using System;
using System.IO;
using System.Linq;

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
        string packagePath = configManager.GetConfigSetting("PackageDirectory");
        ProjectFile rootProject = new ProjectFile(projectFilePath, packagePath);
        rootProject.GetReferences();
        rootProject.ParseWebTestFiles();

        var allRequests = rootProject.WebTests.SelectMany(r => r.Requests).ToList();
        var requestsByApp = allRequests.GroupBy(r => r.ApplicationName);
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }

    private static string HandleSettingNotFound()
    {
        Console.WriteLine("Please input a value for the following config setting: ProjectFilePath");
        return Console.ReadLine();
    }
}