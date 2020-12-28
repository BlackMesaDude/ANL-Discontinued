using System;
using System.Threading;
using System.Threading.Tasks;

using ANL.Server.Threading.Generics;

namespace ANL.Server.Threading 
{
    /// <summary>
    /// Represents a controller for a <see cref="ANL.Threading.Generics.ConcurrentLink{T}" /> that allows to queue one or more <see cref="System.Action" /> that later will be executed during runtime
    /// </summary>
    public class ManagedWork 
    {
        private ConcurrentLink<Action> _threadActionsQueue = new ConcurrentLink<Action>(); // defines a queue (linked) of actions that the manager needs to execute

        private int _isExecuting = 0; // defines a locking state that is changed during action execution or more likely when the action dequeueing and invoking is being executed

        /// <summary>
        /// Gets the action queue which stores all the listed action(s) that need to be executed by the manager
        /// </summary>
        /// <value></value>
        public ConcurrentLink<Action> ActionsQueue { get => _threadActionsQueue; }

        /// <summary>
        /// Adds an action to be executed to the queue that later will be executed within the update process
        /// </summary>
        /// <param name="toExecute">Action to be executed</param>
        public void QueueWork(Action work) => _threadActionsQueue.Add(work); // adds the action to the list

        /// <summary>
        /// Updates this manager queue by firstly updating the queue and then executing the remaining work
        /// </summary>
        public void Update()
        {
            // temporarly executes the update on an available thread
            ThreadPool.QueueUserWorkItem(new WaitCallback((sender) => 
            {
                // for each Action on the base list
                for(int i = 0; i < _threadActionsQueue.Count; i++)
                    _threadActionsQueue.Update(); // update the queue

                // atomically locks on _isExecuting
                Utilities.ThreadingUtility.AtomicLock(ref _isExecuting, () => 
                {
                    Action cAction; // temporary action that will store the n action from the queue
                    _threadActionsQueue.ConsecutiveQueue.TryDequeue(out cAction); // dequeues (gets and removes) the action from the queue and assigns it to the temporary variable

                    // if the temporary action is null then return to avoid any NullPointerException
                    if(cAction == null)
                        return;
                                        
                    Task.Factory.StartNew(cAction); // starts an asynchronous task based on the current temporary action
                });
            }));
        }
    }
}