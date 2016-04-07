using System;
using System.Collections.Generic;

namespace ChatApp.Models
{
    public partial class ChatEntry
    {
        public ChatEntryId Id { get; set; }
        public DateTime SendAt { get; set; }
        public User Sender { get; set; }
        public IChatContent Content { get; set; }

        internal ChatEntry()
        {
        }

        public ChatEntry(ChatEntryId id, DateTime sendAt, User sender, IChatContent content)
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
