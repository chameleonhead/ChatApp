using System;
using System.IO;
using System.Net.Mime;
using System.Runtime.Serialization;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;

namespace ChatApp.Models
{
    public interface IChatContent
    {
        object Value { get; }
    }

    class TextContent : IChatContent
    {
        public TextContent(string value)
        {
            this.Value = value;
        }

        public string Value { get; private set; }

        object IChatContent.Value
        {
            get { return Value; }
        }
    }

    class DataContent : IChatContent
    {
        public DataContent(ContentType mimeType, string fileName, byte[] value)
        {
            this.ContentType = mimeType;
            this.FileName = fileName;
            this.Value = value;
        }

        public ContentType ContentType { get; private set; }
        public string FileName { get; private set; }
        public byte[] Value { get; private set; }

        object IChatContent.Value
        {
            get { return Value; }
        }
    }

    class ImageContent : DataContent
    {
        public ImageContent(DataContent data)
            : base(data.ContentType, data.FileName, data.Value)
        {
            Image = CreateImage(Value);
        }

        private static BitmapImage CreateImage(byte[] data)
        {
            var image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = new MemoryStream(data);
            image.CacheOption = BitmapCacheOption.Default;
            image.EndInit();
            image.Freeze();
            return image;
        }

        public BitmapImage Image { get; private set; }
    }
}
