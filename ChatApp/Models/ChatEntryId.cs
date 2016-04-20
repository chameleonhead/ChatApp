using System;

namespace ChatApp.Models
{
    public class ChatEntryId
    {
        public ChatEntryId(string guid)
        {
            Id = new Guid(guid);
        }

        public ChatEntryId(Guid guid)
        {
            Id = guid;
        }

        public Guid Id { get; private set; }

        public override string ToString()
        {
            return Id.ToString();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ChatEntryId)) return false;
            return Id.Equals(((ChatEntryId)obj).Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
