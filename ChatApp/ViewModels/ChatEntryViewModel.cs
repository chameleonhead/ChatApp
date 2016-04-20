using ChatApp.Models;

using System;

namespace ChatApp.ViewModels
{
    class ChatEntryViewModel : AbstractViewModel
    {
        private ChatEntry _entry;

        public ChatEntryViewModel(ChatEntry entry)
        {
            _entry = entry;

            SendAt = entry.SendAt;
            SenderName = entry.Sender.Name;
            SenderEmailAddress = string.Format("mailto:{0}", entry.Sender.EmailAddress);

            if (entry.Content is TextContent)
                Content = new TextContentViewModel(entry.Content as TextContent);
            else if (entry.Content is ImageContent)
                Content = new ImageContentViewModel(entry.Content as ImageContent);
            else if (entry.Content is DataContent)
                Content = new DataContentViewModel(entry.Content as DataContent);
        }

        public ChatEntry Entry { get { return _entry; } }

        public DateTime SendAt { get; private set; }
        public string SenderName { get; private set; }
        public string SenderEmailAddress { get; private set; }
        public IChatContentViewModel Content { get; private set; }
    }
}
