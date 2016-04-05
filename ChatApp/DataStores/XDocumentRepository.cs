using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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

        FileStream WaitForFile(string fullPath, FileMode mode, FileAccess access, FileShare share)
        {
            for (int numTries = 0; numTries < 10; numTries++)
            {
                try
                {
                    FileStream fs = new FileStream(fullPath, mode, access, share);

                    fs.ReadByte();
                    fs.Seek(0, SeekOrigin.Begin);

                    return fs;
                }
                catch (IOException)
                {
                    Thread.Sleep(50);
                }
            }

            return null;
        }

        protected XDocument LoadDocument()
        {
            XDocument doc;
            if (File.Exists(_documentUri.LocalPath))
            {
                using (var fs = WaitForFile(_documentUri.LocalPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    doc = XDocument.Load(_documentUri.LocalPath);
                }
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

            using (var fs = WaitForFile(_documentUri.LocalPath, FileMode.Open, FileAccess.ReadWrite, FileShare.Read))
            {
                using (var writer = new StreamWriter(fs))
                {
                    doc.Save(writer);
                }
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
