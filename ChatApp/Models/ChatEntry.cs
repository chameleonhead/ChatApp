using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChatApp.Models
{
    public class ChatEntry
    {
        public ChatEntryId Id { get; set; }
        public DateTime SendAt { get; set; }
        public User Sender { get; set; }
        public string Content { get; set; }

        internal ChatEntry()
        {
        }

        public ChatEntry(ChatEntryId id, DateTime sendAt, User sender, string content)
        {
            Id = id;
            SendAt = sendAt;
            Sender = sender;
            Content = content;
        }
    }
}
