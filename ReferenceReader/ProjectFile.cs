using Microsoft.Build.Construction;
using ReferenceReader.Files;
using ReferenceReader.References;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace ReferenceReader
{
    public class ProjectFile
    {
        private string _projectFilePath;
        private bool _isSDK;

        public ProjectFile(string projectFilePath)
        {
            _projectFilePath = projectFilePath;
            _isSDK = CheckIfSDKFormat();
        }

        private bool CheckIfSDKFormat()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(_projectFilePath);

            XmlElement rootElement = xmlDoc.DocumentElement;
            if (rootElement != null && rootElement.HasAttribute("Sdk"))
            {
                return true; // Project file is in SDK format
            }

            return false; // Project file is not in SDK format
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
        }

        public void ParseWebTestFiles()
        {
            if (_isSDK)
            {
                // Get all web test files in the project directory
                string projectDirectory = System.IO.Path.GetDirectoryName(_projectFilePath);
                string[] webTestFiles = System.IO.Directory.GetFiles(projectDirectory, "*.webtest", System.IO.SearchOption.AllDirectories);

                foreach (string webTestFile in webTestFiles)
                {
                    WebTestFile webTest = ParseWebTestFile(webTestFile);
                    _webTests.Add(webTest.Name, webTest);
                }
            }
            else
            {
                ProjectRootElement root = ProjectRootElement.Open(_projectFilePath);

                // Find all web test files referenced in the project file
                IEnumerable<ProjectItemElement> webTestItemElements = root.Items
                    .Where(item => item.ItemType == "WebTest" && item.Include.EndsWith(".webtest"));

                foreach (ProjectItemElement webTestItemElement in webTestItemElements)
                {
                    string webTestFilePath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(_projectFilePath), webTestItemElement.Include);
                    WebTestFile webTest = ParseWebTestFile(webTestFilePath);
                    _webTests.Add(webTest.Name, webTest);
                }
            }
        }

        private WebTestFile ParseWebTestFile(string filePath)
        {
            WebTestFile webTest = new WebTestFile();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath);

            XmlNodeList requestNodes = xmlDoc.SelectNodes("//WebTest/Items/Request");
            foreach (XmlNode requestNode in requestNodes)
            {
                Request request = new Request();
                request.Url = requestNode.Attributes["Url"].Value;
                // Parse other properties of the request node and add them to the request object
                webTest.Requests.Add(request);
            }

            // Parse DataSources and ValidationRules in a similar manner

            return webTest;
        }
    }
}