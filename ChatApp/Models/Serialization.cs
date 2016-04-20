using System;
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
            // IDを取得
            ChatEntryId id;
            if (reader.HasAttributes)
            {
                reader.MoveToFirstAttribute();
                if (reader.LocalName == "Id")
                {
                    reader.MoveToAttribute("Id");
                    id = new ChatEntryId(reader.Value);

                    reader.Read();
                }
                else
                {
                    reader.MoveToElement();

                    // コピペコードを後で直す
                    reader.Read();

                    reader.ReadStartElement("Id");
                    reader.ReadStartElement("Id");
                    id = new ChatEntryId(reader.ReadContentAsString());
                    reader.ReadEndElement();
                    reader.ReadEndElement();
                }
            }
            else
            {
                // コピペコードを後で直す
                reader.Read();

                reader.ReadStartElement("Id");
                reader.ReadStartElement("Id");
                id = new ChatEntryId(reader.ReadContentAsString());
                reader.ReadEndElement();
                reader.ReadEndElement();
            }
            Id = id;

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
            if (reader.HasAttributes)
            {
                reader.MoveToAttribute("ContentType");
                reader.ReadAttributeValue();
                var contentType = (ChatContentType)Enum.Parse(typeof(ChatContentType), reader.Value.ToString(), true);

                reader.MoveToAttribute("FileName");
                reader.ReadAttributeValue();
                var fileName = reader.Value.ToString();

                reader.MoveToElement();
                reader.ReadStartElement("Content");
                var data = Convert.FromBase64String(reader.ReadContentAsString());

                var dataContent = new DataContent(contentType, fileName, data);

                if (contentType == ChatContentType.Image)
                {
                    Content = new ImageContent(dataContent);
                }
                else
                {
                    Content = dataContent;
                }
                reader.ReadEndElement();
            }
            else
            {
                reader.ReadStartElement("Content");
                Content = new TextContent(reader.ReadContentAsString());
                reader.ReadEndElement();
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            // IDを書き込み
            writer.WriteStartAttribute("Id");
            writer.WriteString(Id.ToString());
            writer.WriteEndAttribute();

            // SendAtを書き込み
            writer.WriteStartElement("SendAt");
            writer.WriteValue(SendAt);
            writer.WriteEndElement();

            // Senderを書き込み
            writer.WriteStartElement("Sender");

            writer.WriteStartElement("Name");
            writer.WriteString(Sender.Name);
            writer.WriteEndElement();

            writer.WriteStartElement("EmailAddress");
            writer.WriteString(Sender.EmailAddress);
            writer.WriteEndElement();

            writer.WriteEndElement();

            // Contentを書き込み
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
                writer.WriteString(Enum.GetName(typeof(ChatContentType), dc.ContentType));
                writer.WriteEndAttribute();

                writer.WriteStartAttribute("FileName");
                writer.WriteString(dc.FileName);
                writer.WriteEndAttribute();

                writer.WriteString(Convert.ToBase64String(dc.Value));
            }
            writer.WriteEndElement();
        }
    }
}
