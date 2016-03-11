using ChatApp.DataStores;
using ChatApp.Helpers;
using ChatApp.Models;

using System;
using System.Collections.Generic;
using System.Timers;
using System.Linq;
using System.Windows;

namespace ChatApp.AppServices
{
    public delegate void ChatMessageReceivedHandler(object sender, ChatEntry entry);

    class ChatReceivingService
    {
        public event ChatMessageReceivedHandler ChatMessageReceived;

        private ChatEntryRepository _repository;
        private HashSet<ChatEntry> localEntries;

        public IEnumerable<ChatEntry> ReceivedMessages { get { return localEntries; } }

        public ChatReceivingService(ChatEntryRepository repository)
        {
            _repository = repository;

            localEntries = new HashSet<ChatEntry>(new ChatEntryEqualsComparer());

            Timer t = new Timer(Properties.Settings.Default.ReloadTimeInMillis);
            t.Elapsed += new ElapsedEventHandler(t_Elapsed);
            t.Enabled = true;
        }

        private void t_Elapsed(object sender, ElapsedEventArgs e)
        {
            var allEntries = _repository.FindAll();
            foreach (var entry in allEntries.Except(localEntries))
            {
                if (localEntries.Add(entry))
                {
                    RaiseChatEntryReceived(entry);
                }
            }
        }

        private void RaiseChatEntryReceived(ChatEntry entry)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() => {
                if (ChatMessageReceived != null)
                    ChatMessageReceived(this, entry);
            }));
        }
    }
}
