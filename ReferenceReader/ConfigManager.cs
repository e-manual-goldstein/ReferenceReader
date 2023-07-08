using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

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
            var jsonConfig = JsonDocument.Parse(configContent);
            var projectFilePath = jsonConfig.RootElement.GetProperty("ProjectFilePath").GetString();

            return File.Exists(projectFilePath) ? projectFilePath : OnSettingNotFound();
        }

        private string OnSettingNotFound()
        {
            if (SettingNotFound != null)
            {
                return SettingNotFound.Invoke();
            }

            throw new InvalidOperationException("Setting not found in the config file and no event handler is assigned.");
        }

        private void CreateDefaultConfig()
        {
            var defaultConfig = new { ProjectFilePath = "Path/To/YourProject.csproj" };
            var configContent = JsonSerializer.Serialize(defaultConfig, jsonOptions);
            File.WriteAllText(ConfigFilePath, configContent);
        }
    }
}
