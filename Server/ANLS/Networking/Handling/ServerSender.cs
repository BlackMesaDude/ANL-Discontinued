using System.Collections.Generic;

namespace ANL.Server.Networking.Handling
{
    /// <summary>
    /// Holds and manages outgoing packets from the server
    /// </summary>
    public class ServerSender 
    {
        private ServerSender _instance; // defines the singleton instance of this class

        /// <summary>
        /// Delegate that defines the structure of this 
        /// </summary>
        /// <param name="params"></param>
        public delegate void PacketSender(object[] @params);

        private Dictionary<int, PacketSender> _registeredSenders; // defines the registered\listed sebders that need to manage specific packets sent by the client   

        /// <summary>
        /// Gets the current instance of the ServerSender
        /// </summary>
        /// <value>Returns the current instance of the ServerSender</value>
        public ServerSender SharedInstance 
        {
            get 
            {
                // if the instance is null then create a new one
                if(_instance == null)
                    _instance = new ServerSender();
                return _instance; // return the instance
            }
        }

        /// <summary>
        /// Creates a new handler and defines the registered senders container
        /// </summary>
        private ServerSender() 
        {
            if(_registeredSenders == null)
                _registeredSenders = new Dictionary<int, PacketSender>();
        }

        /// <summary>
        /// Adds a sender to the container
        /// </summary>
        /// <param name="packetID">Id of the packet</param>
        /// <param name="sender">Sender delegate method</param>
        public void RegisterPacket(int packetID, PacketSender sender)
        {
            if(!_registeredSenders.ContainsKey(packetID))
                _registeredSenders.Add(packetID, sender);
        }

        /// <summary>
        /// Removes a sender from the container
        /// </summary>
        /// <param name="packetID">Id of the packet</param>
        public void UnregisterPacket(int packetID)
        {
            if(_registeredSenders.ContainsKey(packetID))
                _registeredSenders.Remove(packetID);
        }
    }
}