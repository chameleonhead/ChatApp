using ChatApp.DataStores;
using ChatApp.Models;
using ChatApp.Tasks;

namespace ChatApp.AppServices
{
    class ChatEntryManagementService
    {
        private ChatEntryRepository _repository;

        public ChatEntryManagementService(ChatEntryRepository repository)
        {
            _repository = repository;
        }

        public void DeleteChatEntry(ChatEntry entry)
        {
            ChatTaskManager.EnqueueTask(() => _repository.Delete(entry));
        }
    }
}
