using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChatApp.Models;
using System.Xml.Linq;

namespace ChatApp.DataStores
{
    class ChatEntryRepository : XDocumentRepository<ChatEntry>
    {

        public ChatEntryRepository(string documentPath)
            : base(documentPath)
        {
        }

        public ChatEntryId NextIdentity()
        {
            return new ChatEntryId() { Id = Guid.NewGuid() };
        }

        public ChatEntry Find(ChatEntryId id)
        {
            var elem = LoadDocument().Root.Elements("ChatEntry")
                .Where(e => e.Attributes("Id").Any(a => a.Value.Equals(id.Id)))
                .SingleOrDefault();
            if (elem != null)
            {
                var entry = FromXElement(elem);
                return entry;
            }
            return null;
        }

        public IEnumerable<ChatEntry> FindAll()
        {
            var elems = LoadDocument().Root.Elements("ChatEntry");
            foreach (var elem in elems)
            {
                var entry = FromXElement(elem);
                yield return entry;
            }
        }
    }
}
