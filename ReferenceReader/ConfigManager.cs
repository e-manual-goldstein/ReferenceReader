using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;


namespace ReferenceReader
{
    public class ConfigManager
    {
        private const string ConfigFilePath = "config.json";
        private readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions { WriteIndented = true };

        public event Func<string> SettingNotFound;

        public string GetProjectFilePath()
        {
            if (!File.Exists(ConfigFilePath))
            {
                CreateDefaultConfig();
                Console.WriteLine($"A new config.json file has been created at: {Path.GetFullPath(ConfigFilePath)}");
            }

            var configContent = File.ReadAllText(ConfigFilePath);
            var configDictionary = JsonSerializer.Deserialize<Dictionary<string, string>>(configContent, jsonOptions);

            if (configDictionary.TryGetValue("ProjectFilePath", out var projectFilePath) && File.Exists(projectFilePath))
            {
                Console.WriteLine("Using project file path from the config.");
                return projectFilePath;
            }

            return OnSettingNotFound();
        }

        private string OnSettingNotFound()
        {
            if (SettingNotFound != null)
            {
                Console.WriteLine("Setting not found in the config file. Invoking event handler to get the value.");
                var settingValue = SettingNotFound.Invoke();
                UpdateConfigFile("ProjectFilePath", settingValue);
                Console.WriteLine("Updated config file with the provided value.");
                return settingValue;
            }

            throw new InvalidOperationException("Setting not found in the config file and no event handler is assigned.");
        }


        private void CreateDefaultConfig()
        {
            Console.WriteLine("Config file not found. Creating default config.");

            var defaultConfig = new Dictionary<string, string> { { "ProjectFilePath", "Path/To/YourProject.csproj" } };

            var configContent = JsonSerializer.Serialize(defaultConfig, jsonOptions);
            File.WriteAllText(ConfigFilePath, configContent);
        }

        private void UpdateConfigFile(string settingName, string settingValue)
        {
            Console.WriteLine($"Updating config file. Setting name: {settingName}, Setting value: {settingValue}");

            var configContent = File.ReadAllText(ConfigFilePath);
            var configDictionary = JsonSerializer.Deserialize<Dictionary<string, string>>(configContent, jsonOptions);

            if (configDictionary.ContainsKey(settingName))
            {
                configDictionary[settingName] = settingValue;
            }
            else
            {
                configDictionary.Add(settingName, settingValue);
            }

            var updatedContent = JsonSerializer.Serialize(configDictionary, jsonOptions);
            File.WriteAllText(ConfigFilePath, updatedContent);
        }
    }
}
