using ChatApp.Models;

using System.Collections.Generic;

namespace ChatApp.Helpers
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
