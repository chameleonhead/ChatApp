using ChatApp.DataStores;
using ChatApp.Models;

using System;

namespace ChatApp.AppServices
{
    class ChatSendingService
    {
        private ChatEntryRepository _Repository;

        public ChatSendingService(ChatEntryRepository repository)
        {
            _Repository = repository;
        }

        public void CreateChatEntry(DateTime time, User sender, string content)
        {
            var id = _Repository.NextIdentity();
            var entry = new ChatEntry(id, time, sender, new TextContent(content));
            _Repository.Save(entry);
        }

        public void CreateChatEntry(DateTime time, User sender, ChatContentType contentType, string fileName, byte[] data)
        {
            var id = _Repository.NextIdentity();
            var dc = new DataContent(contentType, fileName, data);
            var entry = new ChatEntry(id, time, sender, dc);
            _Repository.Save(entry);
        }
    }
}
