using Microsoft.Build.Construction;
using System.Collections.Generic;
using System.Linq;

namespace ReferenceReader
{
    public static class Extensions
    {
        public static string GetMetadataValue(this ProjectItemElement item, string metadataName)
        {
            return item.Metadata.FirstOrDefault(m => m.Name == metadataName)?.Value;
        }
    }
}
