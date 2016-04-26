using ChatApp.AppServices.AppEvents;
using ChatApp.DataStores;
using ChatApp.Models;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Timers;

namespace ChatApp.Tasks
{
    class ChatSourceWatchingTask
    {
        public event NewChatEntryFoundEventHandler NewChatEntryFound;

        private HashSet<ChatEntry> _localEntries;
        private ChatEntryRepository _repository;
        private ChatTaskManager _taskManager;

        private SynchronizationContext _context;
        private ChatSource _source;

        public ChatSourceWatchingTask(ChatSource source, ChatEntryRepository repository, ChatTaskManager taskManager)
        {
            _context = SynchronizationContext.Current;

            _repository = repository;
            _source = source;
            _taskManager = taskManager;

            _localEntries = new HashSet<ChatEntry>();
            foreach (var entry in _repository.FindAll())
            {
                _localEntries.Add(entry);
            }

            var fsw = new FileSystemWatcher(Path.GetDirectoryName(source.DocumentUri.LocalPath), Path.GetFileName(source.DocumentUri.LocalPath));
            fsw.Changed += new FileSystemEventHandler((o, e) => {
                _taskManager.EnqueueTask(() => FetchChatEntry(this, e));
            });
            fsw.EnableRaisingEvents = true;
        }

        public IEnumerable<ChatEntry> ReceivedMessages
        {
            get
            {
                return _localEntries;
            }
        }

        private void FetchChatEntry(object sender, EventArgs e)
        {
            var allEntries = _repository.FindAll().ToArray();

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
