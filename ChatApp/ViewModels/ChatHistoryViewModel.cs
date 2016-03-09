using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChatApp.Models;
using System.Collections.ObjectModel;
using System.Timers;
using ChatApp.DataStores;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;

namespace ChatApp.ViewModels
{
    class ChatHistoryViewModel : AbstractViewModel
    {
        private ChatEntryRepository _repository;
        public ObservableCollection<ChatEntry> Entries { get; private set; }

        public ChatHistoryViewModel(ChatEntryRepository repo, double reloatTimeInMillis)
        {
            Entries = new ObservableCollection<ChatEntry>();
            _repository = repo;

            _repository.EntrySaved += (s, e) =>
            {
                ReloadAllEntries();
            };

            System.Timers.Timer t = new System.Timers.Timer(reloatTimeInMillis);
            t.Elapsed += new ElapsedEventHandler(t_Elapsed);
            t.Enabled = true;

            ReloadAllEntries();
        }

        void t_Elapsed(object sender, EventArgs e)
        {
            ReloadAllEntries();
        }

        public void ReloadAllEntries()
        {
            Application.Current.Dispatcher.Invoke(new Action(() => 
                {
                    Entries.Clear();
                    _repository.FindAll()
                        .OrderByDescending(e => e.SendAt)
                        .ToList()
                        .ForEach((e) => Entries.Add(e));
                }));
        }
    }
}
