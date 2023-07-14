using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReferenceReader.Files
{
    public class WebTestFile
    {
        public string FilePath { get; set; }
        public string Name { get; set; }
        public List<Request> Requests { get; set; }
        public List<DataSource> DataSources { get; set; }
        public List<ValidationRule> ValidationRules { get; set; }

        public WebTestFile()
        {
            Requests = new List<Request>();
            DataSources = new List<DataSource>();
            ValidationRules = new List<ValidationRule>();
        }
    }
}
