using ChatApp.AppServices;
using ChatApp.DataStores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChatApp
{
    class ChatAppContext
    {
        public static ChatAppContext Context { get; private set; } = new ChatAppContext();

        public ChatReceivingService ChatReceivingService { get; private set; }
        public ChatSendingService ChatSendingService { get; private set; }

        private ChatEntryRepository _ChatEntryRepository;

        public ChatAppContext()
        {
            _ChatEntryRepository = new ChatEntryRepository(Properties.Settings.Default.ChatHistoryFilePath);
            ChatReceivingService = new ChatReceivingService(_ChatEntryRepository);
            ChatSendingService = new ChatSendingService(_ChatEntryRepository);
        }

    }
}
