using System;

using ANL.Server.Networking.Entity.Generics;

namespace ANL.Server.Networking
{
    /// <summary>
    /// Represents the basic Client and relevant information to it
    /// </summary>
    public class Client
    {
        /// <summary>
        /// Defines how much data this client can receive from a packet
        /// </summary>
        /// 
        public static int bufferSize = 4096;

        /// <summary>
        /// Base id of this client
        /// </summary>
        public int id;

        /// <summary>
        /// Defines the base User for this client
        /// </summary>
        public User user;

        /// <summary>
        /// Client TCP protocol reference
        /// </summary>
        public Protocols.Generics.TCP tcp;

        /// <summary>
        /// Client UDP protocol reference
        /// </summary>
        public Protocols.Generics.UDP udp;

        /// <summary>
        /// Creates a client based on a ID
        /// </summary>
        /// <param name="uid">Unique ID for this client</param>
        public Client(int uid)
        {
            this.id = uid;

            this.tcp = new Protocols.Generics.TCP(uid, bufferSize);
            this.udp = new Protocols.Generics.UDP(uid);
        }

        /// <summary>
        /// Disconnects this client from the server
        /// </summary>
        public void Disconnect()
        {
            Console.WriteLine($"{tcp.tcpClient.Client.RemoteEndPoint} has disconnected from the server");

            user = null;

            tcp.Disconnect();
            udp.Disconnect();
        }
    }
}