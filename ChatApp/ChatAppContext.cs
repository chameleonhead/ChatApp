using ChatApp.AppServices;
using ChatApp.DataStores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChatApp.Models;

namespace ChatApp
{
    class ChatAppContext
    {
        private static ChatAppContext _Context;
        public static ChatAppContext Context
        {
            get
            {
                if (_Context == null)
                    _Context = new ChatAppContext();
                return _Context;
            }
        }

        public ChatReceivingService ChatReceivingService { get; private set; }
        public ChatSendingService ChatSendingService { get; private set; }

        private ChatEntryRepository _ChatEntryRepository;

        public ChatAppContext()
        {
            _ChatEntryRepository = new ChatEntryRepository(new ChatSource("DefaulutSource", Properties.Settings.Default.ChatHistoryFilePath));
            ChatReceivingService = new ChatReceivingService(_ChatEntryRepository);
            ChatSendingService = new ChatSendingService(_ChatEntryRepository);
        }

    }
}
