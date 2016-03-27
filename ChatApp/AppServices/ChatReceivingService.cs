using ChatApp.AppServices.AppEvents;
using ChatApp.DataStores;
using ChatApp.Models;

using System.Collections.Generic;
using System.Timers;
using System.Linq;

namespace ChatApp.AppServices
{
    class ChatReceivingService
    {
        private class ReceivedMessageStore : Dictionary<ChatSource, IEnumerable<ChatEntry>>
        {
            public new IEnumerable<ChatEntry> this[ChatSource key]
            {
                get
                {
                    if (!base.ContainsKey(key))
                        base.Add(key, new HashSet<ChatEntry>(new ChatEntryEqualsComparer()));
                    return base[key];
                }
            }

            public bool Add(ChatSource source, ChatEntry entry)
            {
                return (this[source] as HashSet<ChatEntry>).Add(entry);
            }

            public new void Add(ChatSource source, IEnumerable<ChatEntry> entries)
            {
                var set = (this[source] as HashSet<ChatEntry>);
                entries.ToList().ForEach(e => Add(source, e));
            }
        }

        public event ChatMessageReceivedHandler ChatMessageReceived;

        private ChatEntryRepository _repository;
        private ReceivedMessageStore _localEntries;
        private Timer _timer;

        public IDictionary<ChatSource, IEnumerable<ChatEntry>> ReceivedMessages 
        { 
            get 
            {
                return _localEntries;
            } 
        }

        public ChatReceivingService(ChatEntryRepository repository)
        {
            _repository = repository;

            _localEntries = new ReceivedMessageStore();
            foreach (var kv in repository.FindAll())
            {
                _localEntries.Add(kv.Key, kv.Value);
            }

            _timer = new Timer(Properties.Settings.Default.ReloadTimeInMillis);
            _timer.Elapsed += FetchChatEntry;
            _timer.Enabled = true;
        }

        ~ChatReceivingService()
        {
            _timer.Elapsed += FetchChatEntry;
        }

        private void FetchChatEntry(object sender, ElapsedEventArgs e)
        {
            var allEntries = _repository.FindAll();
            
            foreach (var source in allEntries.Keys)
            {
                foreach (var entry in allEntries[source].Except(_localEntries[source]))
                {
                    if (_localEntries.Add(source, entry))
                    {
                        OnChatEntryReceived(source, entry);
                    }
                }
            }
        }

        private void OnChatEntryReceived(ChatSource source, ChatEntry entry)
        {
            if (ChatMessageReceived != null)
                ChatMessageReceived(this, new ChatMessageReceivedEventArgs(source, entry));
        }
    }
}
