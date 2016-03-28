using ChatApp.Models;

using System;
using System.Collections.Generic;
using System.Linq;

namespace ChatApp.AppServices
{
    class ChatSourceLoadService
    {
        public ChatSourceLoadService()
        {
        }

        public IEnumerable<ChatSource> Load()
        {
            return Properties.Settings.Default.ChatHistoryFilePaths
                .Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries)
                .Distinct().OrderBy(s => s)
                .Select(s => new ChatSource(new Uri(s)));
        }

        public void Save(IEnumerable<ChatSource> sources)
        {
            Properties.Settings.Default.ChatHistoryFilePaths
                = string.Join(";", sources.Select(s => s.DocumentUri.LocalPath).ToArray());
            Properties.Settings.Default.Save();
        }
    }
}
