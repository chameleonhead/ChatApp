using System;
using System.Collections.Generic;

namespace ChatApp.Models
{
    public class ChatEntryEqualsComparer : IEqualityComparer<ChatEntry>
    {

        public bool Equals(ChatEntry x, ChatEntry y)
        {
            return x.Equals(y);
        }

        public int GetHashCode(ChatEntry obj)
        {
            return obj.GetHashCode();
        }
    }

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
