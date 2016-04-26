using ChatApp.AppServices.AppEvents;
using ChatApp.DataStores;
using ChatApp.Models;
using ChatApp.Tasks;

using System.Collections.Generic;

namespace ChatApp.AppServices
{
    class ChatReceivingService
    {
        public event ChatMessageReceivedHandler ChatMessageReceived;

        private ChatSourceWatchingTask _task;

        public IEnumerable<ChatEntry> ReceivedMessages 
        { 
            get 
            {
                return _task.ReceivedMessages;
            } 
        }

        public ChatReceivingService(ChatSource source, ChatEntryRepository repository, ChatTaskManager taskManager)
        {
            _task = new ChatSourceWatchingTask(source, repository, taskManager);
            _task.NewChatEntryFound += NewChatEntryFoundHandler;
        }

        ~ChatReceivingService()
        {
            if (_task != null)
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
