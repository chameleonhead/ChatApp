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
            var entry = new ChatEntry(id, time, sender, content);
            _Repository.Save(entry);
        }
    }
}
