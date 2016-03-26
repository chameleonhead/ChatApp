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
        private IEnumerable<Uri> _documentUris;
        private IDictionary<Uri, XDocument> _docs;
        protected IDictionary<Uri, XDocument> Docs
        {
            get { return _docs; }
        }

        protected XDocumentRepository(IEnumerable<Uri> documentUris)
        {
            _documentUris = documentUris;
            Refresh();
        }

        protected void Save(Uri uri, T entry)
        {
            var elem = ToXElement(entry);
            var doc = LoadDocument(uri);
            doc.Root.Add(elem);

            using (var writer = new StreamWriter(uri.AbsolutePath))
            {
                doc.Save(writer);
            }
        }

        protected XDocument LoadDocument(Uri uri)
        {
            XDocument doc;
            if (File.Exists(uri.AbsolutePath))
            {
                doc = XDocument.Load(uri.AbsolutePath);
            }
            else
            {
                doc = new XDocument();
                doc.Add(new XElement("ChatEntries"));
            }

            return doc;
        }

        protected IDictionary<Uri, XDocument> LoadDocuments()
        {
            return _documentUris.ToDictionary(u => u, u => LoadDocument(u));
        }

        protected void Refresh()
        {
            _docs = LoadDocuments();
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
