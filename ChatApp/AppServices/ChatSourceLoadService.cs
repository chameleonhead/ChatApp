using ChatApp.Models;

using System.Collections.Generic;

namespace ChatApp.AppServices
{
    class ChatSourceLoadService
    {
        public IEnumerable<ChatSource> Soucres { get; private set; }

        public ChatSourceLoadService()
        {
            Soucres = new[] { 
                new ChatSource("DefaulutSource", Properties.Settings.Default.ChatHistoryFilePath),
                new ChatSource("AlternativeSource", Properties.Settings.Default.ChatHistoryFilePath2)
            };
        }
    }
}
