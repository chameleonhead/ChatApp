using System;
using System.Net.Mime;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace ChatApp.Models
{
    public partial class ChatEntry : ISerializable, IXmlSerializable
    {
        public ChatEntry(SerializationInfo info, StreamingContext context)
        {
            foreach (SerializationEntry entry in info)
            {
                switch (entry.Name)
                {
                    case "Id":
                        Id = (ChatEntryId)info.GetValue("Id", typeof(ChatEntryId));
                        break;
                    case "SendAt":
                        SendAt = (DateTime)info.GetValue("SendAt", typeof(DateTime));
                        break;
                    case "Sender":
                        Sender = (User)info.GetValue("Sender", typeof(User));
                        break;
                    case "Image":
                        Content = new ImageContent((DataContent)info.GetValue("Image", typeof(DataContent)));
                        break;
                    case "Data":
                        Content = (DataContent)info.GetValue("Data", typeof(DataContent));
                        break;
                    case "Content":
                        Content = new TextContent((string)info.GetValue("Content", typeof(string)));
                        break;
                }
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Id", Id);
            info.AddValue("SendAt", SendAt);
            info.AddValue("Sender", Sender);
            if (Content is TextContent)
            {
                info.AddValue("Content", Content.Value);
            }
            else if (Content is ImageContent)
            {
                info.AddValue("Image", (ImageContent)Content);
            }
            else if (Content is DataContent)
            {
                info.AddValue("Content", (DataContent)Content);
            }
        }

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            reader.Read();

            // IDを取得
            reader.ReadStartElement("Id");
            reader.ReadStartElement("Id");
            var id = new ChatEntryId();
            id.Id = new Guid(reader.ReadContentAsString());
            Id = id;
            reader.ReadEndElement();
            reader.ReadEndElement();

            // SendAtを取得
            reader.ReadStartElement("SendAt");
            SendAt = reader.ReadContentAsDateTime();
            reader.ReadEndElement();

            // Senderを取得
            reader.ReadStartElement("Sender");

            var sender = new User();
            reader.ReadStartElement("Name");
            sender.Name = reader.ReadContentAsString();
            reader.ReadEndElement();

            reader.ReadStartElement("EmailAddress");
            sender.EmailAddress = reader.ReadContentAsString();
            reader.ReadEndElement();

            Sender = sender;
            reader.ReadEndElement();

            // Contentを取得
            reader.ReadStartElement("Content");
            if (reader.HasAttributes)
            {
                reader.MoveToFirstAttribute();
                reader.ReadAttributeValue();
                var mime = reader.Value.ToString();
                var data = Convert.FromBase64String(reader.ReadContentAsString());

                var dataContent = new DataContent(data, new ContentType(mime));

                if (mime.Contains("image"))
                {
                    Content = new ImageContent(dataContent);
                }
                else
                {
                    Content = dataContent;
                }
            }
            else
            {
                Content = new TextContent(reader.ReadContentAsString());
            }
            reader.ReadEndElement();
        }

        public void WriteXml(XmlWriter writer)
        {
            // IDを取得
            writer.WriteStartElement("Id");
            writer.WriteStartElement("Id");
            writer.WriteString(Id.Id.ToString());
            writer.WriteEndElement();
            writer.WriteEndElement();

            // SendAtを取得
            writer.WriteStartElement("SendAt");
            writer.WriteValue(SendAt);
            writer.WriteEndElement();

            // Senderを取得
            writer.WriteStartElement("Sender");

            writer.WriteStartElement("Name");
            writer.WriteString(Sender.Name);
            writer.WriteEndElement();

            writer.WriteStartElement("EmailAddress");
            writer.WriteString(Sender.EmailAddress);
            writer.WriteEndElement();

            writer.WriteEndElement();

            // Contentを取得
            writer.WriteStartElement("Content");
            if (Content.GetType() == typeof(TextContent))
            {
                var tc = (TextContent)Content;
                writer.WriteString(tc.Value);
            }
            else
            {
                var dc = (DataContent)Content;
                writer.WriteStartAttribute("ContentType");
                writer.WriteString(dc.ContentType.Name);
                writer.WriteEndAttribute();

                writer.WriteString(Convert.ToBase64String(dc.Value));
            }
            writer.WriteEndElement();
        }
    }
}
