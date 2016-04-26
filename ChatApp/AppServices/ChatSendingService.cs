using ChatApp.DataStores;
using ChatApp.Models;
using ChatApp.Tasks;

using System;

namespace ChatApp.AppServices
{
    class ChatSendingService
    {
        private ChatEntryRepository _repository;
        private ChatTaskManager _taskManager;

        public ChatSendingService(ChatEntryRepository repository, ChatTaskManager taskManager)
        {
            _repository = repository;
            _taskManager = taskManager;
        }

        public void CreateChatEntry(DateTime time, User sender, string content)
        {
            var id = _repository.NextIdentity();
            var entry = new ChatEntry(id, time, sender, new TextContent(content));
            _repository.Save(entry);
        }

        public void CreateChatEntry(DateTime time, User sender, ChatContentType contentType, string fileName, byte[] data)
        {
            var id = _repository.NextIdentity();
            var dc = new DataContent(contentType, fileName, data);
            var entry = new ChatEntry(id, time, sender, dc);
            _taskManager.EnqueueTask(() => _repository.Save(entry));
        }
    }
}
