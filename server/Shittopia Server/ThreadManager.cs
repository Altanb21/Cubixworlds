
using System;
using System.Collections.Generic;
using System.Threading;

namespace Shittopia_Server
{
    internal class ThreadManager
    {
        private static readonly List<Action> executeOnMainThread = new List<Action>();
        private static readonly List<Action> executeCopiedOnMainThread = new List<Action>();
        private static bool actionToExecuteOnMainThread = false;

        public static void ExecuteOnMainThread(Action _action)
        {
            if (_action == null)
            {
                Console.WriteLine("No action to execute on main thread!");
            }
            else
            {
                List<Action> executeOnMainThread = ThreadManager.executeOnMainThread;
                bool lockTaken = false;
                try
                {
                    Monitor.Enter((object)executeOnMainThread, ref lockTaken);
                    ThreadManager.executeOnMainThread.Add(_action);
                    ThreadManager.actionToExecuteOnMainThread = true;
                }
                finally
                {
                    if (lockTaken)
                        Monitor.Exit((object)executeOnMainThread);
                }
            }
        }

        public static void UpdateMain()
        {
            if (!ThreadManager.actionToExecuteOnMainThread)
                return;
            ThreadManager.executeCopiedOnMainThread.Clear();
            List<Action> executeOnMainThread = ThreadManager.executeOnMainThread;
            bool lockTaken = false;
            try
            {
                Monitor.Enter((object)executeOnMainThread, ref lockTaken);
                ThreadManager.executeCopiedOnMainThread.AddRange((IEnumerable<Action>)ThreadManager.executeOnMainThread);
                ThreadManager.executeOnMainThread.Clear();
                ThreadManager.actionToExecuteOnMainThread = false;
            }
            finally
            {
                if (lockTaken)
                    Monitor.Exit((object)executeOnMainThread);
            }
            for (int index = 0; index < ThreadManager.executeCopiedOnMainThread.Count; ++index)
                ThreadManager.executeCopiedOnMainThread[index]();
        }
    }
}
