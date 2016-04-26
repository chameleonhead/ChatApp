using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ChatApp.Tasks
{
    class ChatTaskManager
    {
        private Task _currentTask;

        public ChatTaskManager()
        {
        }

        public void EnqueueTask(Action action)
        {
            var task = new Task(action);
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
