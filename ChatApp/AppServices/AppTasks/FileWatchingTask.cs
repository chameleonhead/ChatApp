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
        public event NewChatEntryFoundEventHandler NewChatEntryFound;

        private HashSet<ChatEntry> _localEntries;
        private System.Timers.Timer _timer;
        private ChatEntryRepository _repository;

        private SynchronizationContext _context;
        private ChatSource _source;

        public ChatSourceWatchingTask(ChatSource source, ChatEntryRepository repository, int reloadTimeInMillis)
        {
            _context = SynchronizationContext.Current;

            _repository = repository;
            _source = source;

            _localEntries = new HashSet<ChatEntry>();
            foreach (var entry in _repository.FindAll())
            {
                _localEntries.Add(entry);
            }

            _timer = new System.Timers.Timer(reloadTimeInMillis);
            _timer.Elapsed += FetchChatEntry;
            _timer.Enabled = true;
        }

        ~ChatSourceWatchingTask()
        {
            _timer.Elapsed += FetchChatEntry;
        }

        public IEnumerable<ChatEntry> ReceivedMessages
        {
            get
            {
                return _localEntries;
            }
        }

        private void FetchChatEntry(object sender, ElapsedEventArgs e)
        {
            var allEntries = _repository.FindAll();

            foreach (var entry in allEntries.Except(_localEntries))
            {
                if (_localEntries.Add(entry))
                {
                    OnNewEntryFound(entry);
                }
            }
        }

        private void OnNewEntryFound(ChatEntry entry)
        {
            _context.Post(new SendOrPostCallback(o =>
                {
                    if (NewChatEntryFound != null)
                        NewChatEntryFound(o, new NewChatEntryFoundEventArgs(_source, entry));
                }), this);
        }
    }
}
