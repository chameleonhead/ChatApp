using ChatApp.Models;

using System;

namespace ChatApp.AppServices.AppEvents
{
    delegate void NewChatEntryFoundEventHandler(object sender, NewChatEntryFoundEventArgs entry);

    class NewChatEntryFoundEventArgs : EventArgs
    {
        public ChatEntry Entry { get; set; }
        public ChatSource Source { get; set; }

        public NewChatEntryFoundEventArgs(ChatSource source, ChatEntry entry)
            : base()
        {
            Source = source;
            Entry = entry;
        }
    }
}
