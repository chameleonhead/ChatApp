using ChatApp.AppServices.AppEvents;
using ChatApp.AppServices.AppTasks;
using ChatApp.DataStores;
using ChatApp.Models;

using System.Collections.Generic;

namespace ChatApp.AppServices
{
    class ChatReceivingService
    {
        public event ChatMessageReceivedHandler ChatMessageReceived;

        private ChatSourceWatchingTask _task;

        public IDictionary<ChatSource, IEnumerable<ChatEntry>> ReceivedMessages 
        { 
            get 
            {
                return _task.ReceivedMessages;
            } 
        }

        public ChatReceivingService(ChatEntryRepository repository)
        {
            _task = new ChatSourceWatchingTask(repository, Properties.Settings.Default.ReloadTimeInMillis);
            _task.NewChatEntryFound += NewChatEntryFoundHandler;
        }

        ~ChatReceivingService()
        {
            _task.NewChatEntryFound -= NewChatEntryFoundHandler;
        }

        private void NewChatEntryFoundHandler(object sender, NewChatEntryFoundEventArgs e)
        {
            OnChatEntryReceived(e.Source, e.Entry);
        }

        private void OnChatEntryReceived(ChatSource source, ChatEntry entry)
        {
            if (ChatMessageReceived != null)
                ChatMessageReceived(this, new ChatMessageReceivedEventArgs(source, entry));
        }
    }
}
