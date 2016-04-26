using ChatApp.DataStores;
using ChatApp.Models;
using ChatApp.Tasks;

namespace ChatApp.AppServices
{
    class ChatEntryManagementService
    {
        private ChatEntryRepository _repository;
        private ChatTaskManager _taskManager;

        public ChatEntryManagementService(ChatEntryRepository repository, ChatTaskManager taskManager)
        {
            _repository = repository;
            _taskManager = taskManager;
        }

        public void DeleteChatEntry(ChatEntry entry)
        {
            _taskManager.EnqueueTask(() => _repository.Delete(entry));
        }
    }
}
