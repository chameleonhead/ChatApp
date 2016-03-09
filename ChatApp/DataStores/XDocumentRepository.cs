﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.IO;

namespace ChatApp.DataStores
{
    public delegate void EntrySavedHandler<T>(object sender, T entry);

    class XDocumentRepository<T>
    {
        private string _documentPath;

        public EntrySavedHandler<T> EntrySaved;

        public XDocumentRepository(string documentPath)
        {
            _documentPath = documentPath;
        }

        public void Save(T entry)
        {
            var elem = ToXElement(entry);
            var doc = LoadDocument();
            doc.Root.Add(elem);

            doc.Save(_documentPath);

            if (EntrySaved != null)
                EntrySaved(this, entry);
        }

        public XDocument LoadDocument()
        {
            XDocument doc;
            if (File.Exists(_documentPath))
            {
                doc = XDocument.Load(_documentPath);
            }
            else
            {
                doc = new XDocument();
                doc.Add(new XElement("ChatEntries"));
            }
            return doc;
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