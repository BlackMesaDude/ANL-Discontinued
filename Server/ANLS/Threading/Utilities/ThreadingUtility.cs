using System.Threading;

namespace ANL.Server.Threading.Utilities 
{
    /// <summary>
    /// Utility class that contains helpful methods for threading purposes
    /// </summary>
    public static class ThreadingUtility 
    {
        /// <summary>
        /// Thread-safe lock, it uses a single atomic operation with the help of <see cref="System.Threading.Interlocked" />
        /// </summary>
        /// <param name="holder">Integer which holds the state of the lock</param>
        /// <param name="@Action">Action that should be executed inside the lock</param>
        /// <returns></returns>
        public static bool AtomicLock(ref int holder, System.Action @Action) 
        {
            if(0 == Interlocked.Exchange(ref holder, 1))
            {
                @Action();

                Interlocked.Exchange(ref holder, 0);

                return true;
            }

            return false;
        }
    }
}