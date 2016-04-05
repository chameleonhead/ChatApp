using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ChatApp.DataStores
{
    class XDocumentRepository<T>
    {
        private Uri _documentUri;
        private string _rootElemName;

        protected XDocumentRepository(Uri documentUri, string rootElemName)
        {
            _documentUri = documentUri;
            _rootElemName = rootElemName;
        }

        protected XDocument LoadDocument()
        {
            XDocument doc;
            if (File.Exists(_documentUri.LocalPath))
            {
                doc = XDocument.Load(_documentUri.LocalPath);
            }
            else
            {
                doc = new XDocument();
                doc.Add(new XElement(_rootElemName));
            }

            return doc;
        }

        protected void Save(T entry)
        {
            var elem = ToXElement(entry);
            var doc = LoadDocument();
            doc.Root.Add(elem);

            using (var writer = new StreamWriter(_documentUri.LocalPath))
            {
                doc.Save(writer);
            }
        }

        protected static XElement ToXElement(T obj)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (TextWriter streamWriter = new StreamWriter(memoryStream))
                {
                    var xmlSerializer = new XmlSerializer(typeof(T));
                    xmlSerializer.Serialize(streamWriter, obj);
                    return XElement.Parse(Encoding.UTF8.GetString(memoryStream.ToArray()));
                }
            }
        }

        protected static T FromXElement(XElement xElement)
        {
            var xmlSerializer = new XmlSerializer(typeof(T));
            return (T)xmlSerializer.Deserialize(xElement.CreateReader());
        }
    }
}
