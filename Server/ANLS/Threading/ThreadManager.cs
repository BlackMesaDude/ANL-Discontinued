using System;
using System.Threading;
using System.Collections.Generic;

namespace ANL.Server.Threading 
{
    /// <summary>
    /// Allows to assign asynchronous work to a <see cref="ANL.Threading.ManagedWork" /> with the help of the <see cref="System.Threading.ThreadPool" />
    /// </summary>
    public class ThreadManager 
    {
        private static ThreadManager _instance; // defines the singleton instance for this class

        /// <summary>
        /// Gets the shared singleton instance of this class
        /// </summary>
        /// <value>Returns a instance for this class, if null a new one will be created</value>
        public static ThreadManager SharedInstance 
        {
            get 
            {
                // if the instance is null then create a new one based on the maximum thread count
                if(_instance == null)
                    _instance = new ThreadManager(MaximumThreads);
                return _instance; // return the instance
            }
        }

        private List<ManagedWork> _availableManagers = new List<ManagedWork>(MaximumThreads); // creates a list of ManagedWork(s) based on a maximum capacity that is defined by the maximum threads assigned

        private static int _threadCount; // defines the maximum thread count

        /// <summary>
        /// Gets the available thread holders for the individual threads that are hold by the ThreadPool
        /// </summary>
        /// <value></value>
        public List<ManagedWork> AvailableManagers { get => _availableManagers; }

        /// <summary>
        /// Gets or Sets the maximum threads usable by the manager
        /// </summary>
        /// <value>Returns a integer value that defines the maximum usable threads</value>
        public static int MaximumThreads { get => _threadCount; set => _threadCount = value; }

        /// <summary>
        /// Creates a ThreadManager based on the maximum thread count
        /// </summary>
        /// <param name="maxThreads">Maximum usable threads by the pool (default is 2)</param>
        private ThreadManager(int maxThreads = 2)
        {            
            _threadCount = maxThreads;
            
            ThreadPool.SetMaxThreads(MaximumThreads, MaximumThreads); // sets the ThreadPool to use the defined maximum of threads          

            // based on the maximum threads value define a new ManagedWork on the list
            for(int i = 0; i < maxThreads - 1; i++)
            {
                _availableManagers.Add(new ManagedWork());
            }  
        }

        /// <summary>
        /// Requests (queues) work for a specific work manager
        /// </summary>
        /// <param name="index">Index of the manager that should queue the work</param>
        /// <param name="work">Action that needs to be performed by the manager</param>
        public void RequestWorkFor(int index, Action work)
        {
            _availableManagers[index].QueueWork(work); // queues the specified action to the manager holder
        }
    }
}