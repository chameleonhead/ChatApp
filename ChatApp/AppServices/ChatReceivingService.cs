using ChatApp.AppServices.AppEvents;
using ChatApp.DataStores;
using ChatApp.Models;

using System;
using System.Collections.Generic;
using System.Timers;
using System.Linq;
using System.Windows;

namespace ChatApp.AppServices
{
    class ChatReceivingService
    {
        public event ChatMessageReceivedHandler ChatMessageReceived;

        private ChatEntryRepository _repository;
        private IDictionary<ChatSource, HashSet<ChatEntry>> localEntries;

        public IDictionary<ChatSource, IEnumerable<ChatEntry>> ReceivedMessages 
        { 
            get 
            {
                return localEntries.ToDictionary(kv => kv.Key, kv => kv.Value as IEnumerable<ChatEntry>); 
            } 
        }

        public ChatReceivingService(ChatEntryRepository repository)
        {
            _repository = repository;

            localEntries = new Dictionary<ChatSource, HashSet<ChatEntry>>();
            foreach (var kv in repository.FindAll())
            {
                foreach (var e in kv.Value)
                {
                    AddEntry(kv.Key, e);
                }
            }

            Timer t = new Timer(Properties.Settings.Default.ReloadTimeInMillis);
            t.Elapsed += new ElapsedEventHandler(t_Elapsed);
            t.Enabled = true;
        }

        private void t_Elapsed(object sender, ElapsedEventArgs e)
        {
            var allEntries = _repository.FindAll();
            
            foreach (var source in allEntries.Keys)
            {
                foreach (var entry in allEntries[source].Except(localEntries[source]))
                {
                    if (AddEntry(source, entry))
                    {
                        OnChatEntryReceived(source, entry);
                    }
                }
            }
        }

        private bool AddEntry(ChatSource source, ChatEntry entry)
        {
            if (!localEntries.ContainsKey(source))
            {
                localEntries.Add(source, new HashSet<ChatEntry>(new ChatEntryEqualsComparer()));
            }
            return localEntries[source].Add(entry);
        }

        private void OnChatEntryReceived(ChatSource source, ChatEntry entry)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                if (ChatMessageReceived != null)
                    ChatMessageReceived(this, new ChatMessageReceivedEventArgs(source, entry));
            }));
        }
    }
}
