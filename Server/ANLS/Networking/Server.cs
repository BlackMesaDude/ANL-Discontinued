using System;
using System.Collections.Generic;

using System.Net;
using System.Net.Sockets;

using ANL.Server.Networking.Data.Server.Structures;

namespace ANL.Server.Networking
{
    /// <summary>
    /// Represents the server methods and settings
    /// </summary>
    public class Server 
    {
        /// <summary>
        /// Contains the information about the server
        /// </summary>
        public static ServerData Data;

        /// <summary>
        /// Dictionary containing all the unique users references
        /// </summary>
        /// <typeparam name="int">Id of the user</typeparam>
        /// <typeparam name="Client">Client reference of the user</typeparam>
        /// <returns></returns>
        public static Dictionary<int, Client> AvailableUsers = new Dictionary<int, Client>();

        /// <summary>
        /// Starts the server
        /// </summary>
        /// <param name="data">Starting information about the server</param>
        /// <param name="availableProtocols">Usable protocols for this server</param>
        public static void Start(ServerData data, params Protocols.Interfaces.IProtocol[] availableProtocols)
        {
            Data = data;
            
            InitializeClients();
            InitializeProtocols(availableProtocols);
        }

        /// <summary>
        /// Initializes every needed protocol
        /// </summary>
        /// <param name="usableProtocols">Protocols that are needed</param>
        private static void InitializeProtocols(Protocols.Interfaces.IProtocol[] usableProtocols)
        {
            Threading.ThreadManager.SharedInstance.RequestWorkFor(1, () => 
            {
                for(int i = 0; i < usableProtocols.Length; i++)
                    usableProtocols[i].Start(IPAddress.Any, Data.AimingPort);
            });
        }

        /// <summary>
        /// Initializes every client 
        /// </summary>
        private static void InitializeClients() 
        {
            Threading.ThreadManager.SharedInstance.RequestWorkFor(1, () => 
            {
                for(int i = 0; i < Data.MaximumUsers; i++)
                    AvailableUsers.Add(i, new Client(i));
            });
        }
    }
}