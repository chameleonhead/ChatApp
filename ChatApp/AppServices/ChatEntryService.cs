using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChatApp.DataStores;
using ChatApp.Models;

namespace ChatApp.AppServices
{
    class ChatEntryService
    {
        private ChatEntryRepository _Repository;

        public ChatEntryService(ChatEntryRepository repository)
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
