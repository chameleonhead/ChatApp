using System;

namespace ChatApp.Models
{
    class ChatSource
    {
        public string Name { get; private set; }
        public Uri DocumentUri { get; private set; }

        public ChatSource(string sourceName, string documentPath)
        {
            Name = sourceName;
            DocumentUri = new Uri(documentPath);
        }

        public ChatSource(Uri documentUri)
        {
            DocumentUri = documentUri;
        }
    }
}
