using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ChatApp.Tasks
{
    static class ChatTaskManager
    {
        private static ChatTaskManagerCore _manager;

        static ChatTaskManager()
        {
            _manager = new ChatTaskManagerCore();
        }

        public static void EnqueueTask(Action action)
        {
            _manager.EnqueueTask(action);
        }

        public static void EnqueueTask(Action action, Action<Task> continueWith)
        {
            _manager.EnqueueTask(action, continueWith);
        }
    }

    class ChatTaskManagerCore
    {
        private Task _currentTask;

        public ChatTaskManagerCore()
        {
        }

        public Task CurrentTask { get { return _currentTask; } }

        public void EnqueueTask(Action action)
        {
            EnqueueTask(action, null);
        }

        public void EnqueueTask(Action action, Action<Task> continueWith)
        {
            var task = new Task(action);
            if (continueWith != null)
            {
                task.ContinueWith(t => continueWith(t));
            }

            if (_currentTask == null)
            {
                task.Start();
                _currentTask = task;
            }
            else
            {
                _currentTask.ContinueWith(t => task.Start());
                _currentTask = task;
            }
        }
    }
}
