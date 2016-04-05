using ChatApp.Models;

using System;
using System.Collections.Generic;
using System.Linq;

namespace ChatApp.DataStores
{
    class ChatEntryRepository : XDocumentRepository<ChatEntry>
    {
        private ChatSource _Source;

        public ChatEntryRepository(ChatSource source)
            : base(source.DocumentUri, "ChatEntries")
        {
            _Source = source;
        }

        public ChatEntryId NextIdentity()
        {
            return new ChatEntryId() { Id = Guid.NewGuid() };
        }

        public ChatEntry Find(ChatEntryId id)
        {
            var doc = LoadDocument();
            var elem = doc.Root.Elements("ChatEntry")
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
            var doc = LoadDocument();
            var elems = doc.Root.Elements("ChatEntry");
            foreach (var elem in elems)
            {
                var entry = FromXElement(elem);
                yield return entry;
            }
        }

        public new void Save(ChatEntry entry)
        {
            base.Save(entry);
        }
    }
}
