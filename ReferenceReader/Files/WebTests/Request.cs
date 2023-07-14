using System;
using System.Configuration;
using System.Text.RegularExpressions;

namespace ReferenceReader.Files
{
    public class Request
    {
        public Request(string url)
        {
            Url = url;
            NormalisedUrl = ResolveUrl(url);
        }

        public string Url { get; }
        public string NormalisedUrl { get; }
        public string HostAndPort { get; set; }
        public string ApplicationName { get; set; }
        public string ObjectName { get; set; }
        public string RequestSubType { get; set; }
        public string ActionName { get; set; }

        public string Method { get; set; }

        public void TryParseUrl()
        {
            try
            {
                var uri = new Uri(NormalisedUrl);

                var hostAndPort = uri.Authority;

                var segments = uri.LocalPath.Trim('/').Split('/');
                if (segments.Length > 4)
                {
                    return; // not interested in those for now
                }

                for (int i = 0; i < segments.Length; i++)
                {
                    var segment = segments[i];
                    switch (i)
                    {
                        case 0:
                            ApplicationName = segments[i];
                            break;
                        case 1:
                            HandleRoot(segment);
                            break;
                        case 2:
                            HandleRequestSubType(segment);
                            break;
                        case 3:
                            HandleActionName(segment);
                            break;
                        default:
                            break;
                    }
                }

            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex);
            }
        }

        private void HandleActionName(string segment)
        {
            ActionName = segment;
        }

        private void HandleRequestSubType(string segment)
        {
            RequestSubType = segment;
        }

        private void HandleRoot(string segment)
        {
            if (segment != "Home" && segment != "Ajax")
            {
                ObjectName = segment;
            }
        }

        private string ResolveUrl(string url)
        {
            var resolved = Regex.Replace(url, @"\{\{(.*?)\}\}", "$1");
            if (!resolved.StartsWith("http://"))
            {
                resolved = $"http://{resolved}";
            }
            return resolved ;
        }
    }

}
