using System;
using System.Collections.Generic;

namespace ChatApp.Models
{
    public class ChatEntry
    {
        public ChatEntryId Id { get; set; }
        public ChatTopic Topic { get; set; }
        public DateTime SendAt { get; set; }
        public User Sender { get; set; }
        public string Content { get; set; }

        internal ChatEntry()
        {
        }

        public ChatEntry(ChatEntryId id, DateTime sendAt, User sender, string content) : this(id, sendAt, sender, content, null)
        {
        }

        public ChatEntry(ChatEntryId id, DateTime sendAt, User sender, string content, ChatTopic topic)
        {
            Id = id;
            SendAt = sendAt;
            Sender = sender;
            Content = content;
            Topic = topic;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ChatEntry)) return false;
            return Id.Equals(((ChatEntry)obj).Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
