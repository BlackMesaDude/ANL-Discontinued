namespace ANL.Server.Threading.Loops
{
    /// <summary>
    /// Base loop that manipulates and\or updates each thread and entities
    /// </summary>
    public class NetworkLoop : Interfaces.ILoop
    {
        private static NetworkLoop _instance; // defines the singleton instance for this class

        /// <summary>
        /// Gets the shared instance of this class
        /// </summary>
        /// <value>Returns the current instance for this class, if null a new one will be created</value>
        public static NetworkLoop SharedInstance 
        {
            get 
            {
                if(_instance == null)
                    _instance = new NetworkLoop();
                return _instance;
            }
        }

        private NetworkLoop() { /* TODO: needs implementation */ }

        /// <summary>
        /// Updates everything (threads and entities)
        /// </summary>
        public virtual void Update()
        {
            // for each client inside the available clients
            foreach(Networking.Client client in Networking.Server.AvailableUsers.Values)
            {
                // if the current client user entity isn't null
                if(client.user != null)
                    client.user.Update(); // updates the client user entity
            }
            
            // for each manager inside the available managers
            for(int i = 0; i < ThreadManager.SharedInstance.AvailableManagers.Count; i++)
            {
                ThreadManager.SharedInstance.AvailableManagers[i].Update(); // updates the current manager
            }
        }
    }
}