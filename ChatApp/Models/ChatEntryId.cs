using System;

namespace ChatApp.Models
{
    public class ChatEntryId
    {
        public Guid Id { get; set; }

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
