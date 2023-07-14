using Microsoft.Build.Construction;
using ReferenceReader.Files;
using ReferenceReader.References;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace ReferenceReader
{
    public class ProjectFile
    {
        bool _isSDK;
        string _projectFilePath;
        string _packagePath;

        public ProjectFile(string projectFilePath, string packagePath = default)
        {
            _projectFilePath = projectFilePath;
            _isSDK = CheckIfSDKFormat();
            _packagePath = packagePath;
        }

        private bool CheckIfSDKFormat()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(_projectFilePath);

            XmlElement rootElement = xmlDoc.DocumentElement;
            return rootElement != null && rootElement.HasAttribute("Sdk");
        }

        Dictionary<string, DllReference> _dllReferences = new Dictionary<string, DllReference>();
        Dictionary<string, ProjectReference> _projectReferences = new Dictionary<string, ProjectReference>();
        Dictionary<string, PackageReference> _packageReferences = new Dictionary<string, PackageReference>();
        Dictionary<string, WebTestFile> _webTests = new Dictionary<string, WebTestFile>();

        public void GetReferences()
        {
            if (_isSDK)
            {
                ProjectRootElement root = ProjectRootElement.Open(_projectFilePath);

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
                                var packageReference = new PackageReference(item, _packagePath);
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
        }

        public void ParseWebTestFiles()
        {
            foreach (string webTestFile in GetWebTestPaths())
            {
                if (File.Exists(webTestFile) && !_webTests.ContainsKey(webTestFile))
                {
                    WebTestFile webTest = ParseWebTestFile(webTestFile);
                    _webTests.Add(webTest.FilePath, webTest);
                }
            }           
        }

        private IEnumerable<string> GetWebTestPaths()
        {
            if (_isSDK)
            {
                string projectDirectory = Path.GetDirectoryName(_projectFilePath);
                return Directory.GetFiles(projectDirectory, "*.webtest", SearchOption.AllDirectories);
            }
            ProjectRootElement root = ProjectRootElement.Open(_projectFilePath);

            // Find all web test files referenced in the project file
            IEnumerable<ProjectItemElement> webTestItemElements = root.Items
                .Where(item => item.ItemType == "None" && item.Include.EndsWith(".webtest"));

            return webTestItemElements.Select(e => Path.Combine(Path.GetDirectoryName(_projectFilePath), e.Include));
        }

        private WebTestFile ParseWebTestFile(string filePath)
        {
            WebTestFile webTest = new WebTestFile();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath);
            webTest.Name = Path.GetFileNameWithoutExtension(filePath);
            webTest.FilePath = filePath;
            XmlNodeList requestNodes = xmlDoc.DocumentElement.SelectNodes("//*[local-name()='Request']");
            foreach (XmlNode requestNode in requestNodes)
            {
                Request request = new Request(requestNode.Attributes["Url"].Value);
                request.TryParseUrl();
                request.Method = requestNode.Attributes["Method"].Value;
                // Parse other properties of the request node and add them to the request object
                webTest.Requests.Add(request);
            }

            // Parse DataSources and ValidationRules in a similar manner

            return webTest;
        }

        public WebTestFile[] WebTests => _webTests.Values.ToArray();
    }
}