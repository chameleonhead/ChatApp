using ChatApp.Models;

using System;
using System.Collections.Generic;
using System.Linq;

namespace ChatApp.DataStores
{
    class ChatEntryRepository : XDocumentRepository<ChatEntry>
    {
        private IEnumerable<ChatSource> _Sources;

        public ChatEntryRepository(IEnumerable<ChatSource> source)
            : base(source.Select(s => s.DocumentUri))
        {
            _Sources = source;
        }

        public ChatEntryId NextIdentity()
        {
            return new ChatEntryId() { Id = Guid.NewGuid() };
        }

        public ChatEntry Find(ChatSource source, ChatEntryId id)
        {
            var elem = Docs[source.DocumentUri].Root.Elements("ChatEntry")
                .Where(e => e.Attributes("Id").Any(a => a.Value.Equals(id.Id)))
                .SingleOrDefault();
            if (elem != null)
            {
                var entry = FromXElement(elem);

                return entry;
            }
            return null;
        }

        public IEnumerable<ChatEntry> FindAll(ChatSource source)
        {
            var elems = Docs[source.DocumentUri].Root.Elements("ChatEntry");
            foreach (var elem in elems)
            {
                var entry = FromXElement(elem);
                yield return entry;
            }
        }

        public IDictionary<ChatSource, IEnumerable<ChatEntry>> FindAll()
        {
            Refresh(); // TODO:ちょっとかっこ悪いので、キャッシュ処理について本気で考える
            return _Sources.ToDictionary(s => s, s => FindAll(s));
        }

        public void Save(ChatSource source, ChatEntry entry)
        {
            base.Save(source.DocumentUri, entry);
        }
    }
}
