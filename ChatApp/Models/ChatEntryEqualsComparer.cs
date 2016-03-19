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
}
