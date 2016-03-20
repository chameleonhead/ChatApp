using ChatApp.Models;

using System;

namespace ChatApp.AppServices.AppEvents
{
    delegate void ChatMessageReceivedHandler(object sender, ChatMessageReceivedEventArgs entry);

    class ChatMessageReceivedEventArgs : EventArgs
    {
        public ChatEntry Entry { get; set; }
        public ChatSource Source { get; set; }

        public ChatMessageReceivedEventArgs(ChatSource source, ChatEntry entry)
            : base()
        {
            Source = source;
            Entry = entry;
        }
    }
}
