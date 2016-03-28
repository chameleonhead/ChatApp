using ChatApp.DataStores;
using ChatApp.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Timers;

using ChatApp.AppServices.AppEvents;

namespace ChatApp.AppServices.AppTasks
{
    class ChatSourceWatchingTask
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

        public event NewChatEntryFoundEventHandler NewChatEntryFound;

        private ReceivedMessageStore _localEntries;
        private System.Timers.Timer _timer;
        private ChatEntryRepository _repository;

        private SynchronizationContext _context;

        public ChatSourceWatchingTask(ChatEntryRepository repository, int reloadTimeInMillis)
        {
            _context = SynchronizationContext.Current;

            _repository = repository;

            _localEntries = new ReceivedMessageStore();
            foreach (var kv in _repository.FindAll())
            {
                _localEntries.Add(kv.Key, kv.Value);
            }

            _timer = new System.Timers.Timer(reloadTimeInMillis);
            _timer.Elapsed += FetchChatEntry;
            _timer.Enabled = true;
        }

        ~ChatSourceWatchingTask()
        {
            _timer.Elapsed += FetchChatEntry;
        }

        public IDictionary<ChatSource, IEnumerable<ChatEntry>> ReceivedMessages
        {
            get
            {
                return _localEntries;
            }
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
                        OnNewEntryFound(source, entry);
                    }
                }
            }
        }

        private void OnNewEntryFound(ChatSource source, ChatEntry entry)
        {
            _context.Post(new SendOrPostCallback(o =>
                {
                    if (NewChatEntryFound != null)
                        NewChatEntryFound(o, new NewChatEntryFoundEventArgs(source, entry));
                }), this);
        }
    }
}
