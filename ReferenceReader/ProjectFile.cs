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

        public ProjectFile(string projectFilePath)
        {
            _projectFilePath = projectFilePath;
        }

        Dictionary<string, DllReference> _dllReferences = new Dictionary<string, DllReference>();
        Dictionary<string, ProjectReference> _projectReferences = new Dictionary<string, ProjectReference>();
        Dictionary<string, PackageReference> _packageReferences = new Dictionary<string, PackageReference>();
        Dictionary<string, WebTestFile> _webTests = new Dictionary<string, WebTestFile>();

        public void GetReferences()
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

        public void ParseWebTests()
        {
            // Get all web test files in the project directory
            string projectDirectory = System.IO.Path.GetDirectoryName(_projectFilePath);
            string[] webTestFiles = System.IO.Directory.GetFiles(projectDirectory, "*.webtest", System.IO.SearchOption.AllDirectories);

            foreach (string webTestFile in webTestFiles)
            {
                WebTestFile webTest = ParseWebTestFile(webTestFile);
                _webTests[webTest.Name] = webTest;
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