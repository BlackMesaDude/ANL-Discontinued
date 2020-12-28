using System.Threading;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace ANL.Server.Threading.Generics 
{
    /// <summary>
    /// Defines a thread-safe FIFO list that implements the basics of a ConcurrentQueue. Similiar to a <see cref="LinkedList<T>">, this list is doubly-linked but thread-safe.
    /// </summary>
    /// <typeparam name="T">The type of elements held by the connections</typeparam>
    public class ConcurrentLink<T> : ConcurrentQueue<T> 
    {
        private int _lock = 0; // defines a integer value that will be used to define the lock state of this list

        private readonly ConcurrentQueue<T> _consecutiveQueue; // defines the consecutive list of the double connection 

        /// <summary>
        /// Gets the current lock state for this list
        /// </summary>
        /// <value>Returns 0 if not locking otherwise 1 for locking</value>
        public int Locking
        {
            get => _lock;
        }

        /// <summary>
        /// Gets secundary list connected to the primary one. (readonly)
        /// </summary>
        /// <value></value>
        public ConcurrentQueue<T> ConsecutiveQueue
        {
            get => _consecutiveQueue;
        }

        /// <summary>
        /// Creates a ConcurrentLink and initializes the concurrent link
        /// </summary>
        /// <returns></returns>
        public ConcurrentLink() : base() 
        {
            // if the consecutive list is null then create a new one
            if (this._consecutiveQueue == null)
                this._consecutiveQueue = new ConcurrentQueue<T>();
        }

        /// <summary>
        /// Creates a ConcurrentLink based on the given enumerables (applied only for the primary) and initializes the concurrent link
        /// </summary>
        /// <param name="enumerables"></param>
        /// <returns></returns>
        public ConcurrentLink(IEnumerable<T> enumerables) : base(enumerables) 
        { 
            // if the consecutive list is null then create a new one
            if (this._consecutiveQueue == null)
                this._consecutiveQueue = new ConcurrentQueue<T>();
        }

        /// <summary>
        /// Updates the connected lists by porting the latest content from the primary list to the secundary afterwards clearing the primary to leave open space.
        /// </summary>
        public void Update()
        {
            _consecutiveQueue.Clear(); // clears the consecutive list

            // atomic operation that allows to lock (changes the lock state to 1 if is 0)
            if(0 == Interlocked.Exchange(ref _lock, 1))
            {
                // for every element in the primary list
                for(int i = 0; i < base.Count; i++)
                {
                    T cElement; // defines a temporary type element that will store the current item

                    base.TryDequeue(out cElement); // dequeues (adds) the current element from the primary list to the temporary element and removes it
                    _consecutiveQueue.Enqueue(cElement); // adds the current temporary element to the consecutive list
                }

                base.Clear(); // clears the primary list

                Interlocked.Exchange(ref _lock, 0); // releases the lock state (from 1 to 0)
            }
        }

        /// <summary>
        /// Adds a element to the primary list that later will be ported to the secundary by updating
        /// </summary>
        /// <param name="element">Element that should be added to the primary list</param>
        public void Add(T element)
        {
            // if the element is null do nothing and return
            if (element == null)
                return;

            // atomic operation that allows to lock (changes the lock state to 1 if is 0)
            if(0 == Interlocked.Exchange(ref _lock, 1))
            {
                base.Enqueue(element); // adds the element to the primary list

                Interlocked.Exchange(ref _lock, 0); // releases the lock state (from 1 to 0)
            }
        }
    }
}