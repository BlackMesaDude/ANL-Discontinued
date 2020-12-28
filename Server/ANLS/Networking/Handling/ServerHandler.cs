using System.Collections.Generic;

namespace ANL.Server.Networking.Handling 
{
    /// <summary>
    /// Holds and manages references to packets that come from the client
    /// </summary>
    public class ServerHandler 
    {
        private static ServerHandler _instance; // defines the singleton instance for this class

        /// <summary>
        /// Delegate that defines the structure of a handler
        /// </summary>
        /// <param name="id">Id of the client</param>
        /// <param name="packet">Packet that contains the data from the client</param>
        public delegate void PacketHandler(int id, Packet packet);

        private Dictionary<int, PacketHandler> _registeredHandlers; // defines the registered\listed handlers that need to manage specific packets sent by the client    

        /// <summary>
        /// Gets the current instance of the ServerHandler
        /// </summary>
        /// <value>Returns the current instance of the ServerHandler</value>
        public static ServerHandler SharedInstance 
        {
            get 
            {
                // if the instance is null then create a new one
                if(_instance == null)
                    _instance = new ServerHandler();
                return _instance; // returns the instance
            }
        }

        /// <summary>
        /// Gets a dictionary of registered\listed handlers that allow to manage income packets from the client
        /// </summary>
        /// <value>Returns a list that contains every handler and it's ID</value>
        public Dictionary<int, PacketHandler> RegisteredHandlers { get => _registeredHandlers; }

        /// <summary>
        /// Creates a new handler and defines the registered handlers container
        /// </summary>
        private ServerHandler() 
        {
            if(_registeredHandlers == null)
                _registeredHandlers = new Dictionary<int, PacketHandler>();
        }
    }
}