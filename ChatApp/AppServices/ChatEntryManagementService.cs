using ChatApp.DataStores;
using ChatApp.Models;

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
            _repository.Delete(entry);
        }
    }
}
