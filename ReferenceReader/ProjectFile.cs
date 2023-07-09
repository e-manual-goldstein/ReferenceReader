using Microsoft.Build.Construction;
using System.Collections.Generic;
using System.Linq;

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

        public IEnumerable<AbstractReference> AllReferences()
        {
            return _dllReferences.Values
                .Concat<AbstractReference>(_projectReferences.Values)
                .Concat<AbstractReference>(_packageReferences.Values);
        }
    }
}