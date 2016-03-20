using System;
using System.IO;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ChatApp.DataStores
{
    class XDocumentRepository<T>
    {
        private Uri _documentUri;
        private XDocument _doc;

        public XDocumentRepository(Uri documentUri)
        {
            _documentUri = documentUri;
        }

        public void Save(T entry)
        {
            var elem = ToXElement(entry);
            var doc = LoadDocument();
            doc.Root.Add(elem);

            using (var writer = new StreamWriter(_documentUri.AbsolutePath))
            {
                doc.Save(writer);
            }
        }

        public XDocument LoadDocument()
        {
            if (File.Exists(_documentUri.AbsolutePath))
            {
                _doc = XDocument.Load(_documentUri.AbsolutePath);
            }
            else
            {
                _doc = new XDocument();
                _doc.Add(new XElement("ChatEntries"));
            }

            return _doc;
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
