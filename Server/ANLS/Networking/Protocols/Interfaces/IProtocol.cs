using System.Net;

namespace ANL.Server.Networking.Protocols.Interfaces 
{
    /// <summary>
    /// Defines basic protocol methods
    /// </summary>
    public interface IProtocol
    {
        /// <summary>
        /// Behaviour of how this protocol will send data
        /// </summary>
        /// <param name="packet">Packet to be sent</param>
        void SendData(Packet packet);
        /// <summary>
        /// Behaviour of how this protocol will handle the incoming data
        /// </summary>
        /// <param name="data">Incoming buffer</param>
        bool HandleData(byte[] data);

        /// <summary>
        /// Defines the protocol callback
        /// </summary>
        /// <param name="result"></param>
        void ReceiverCallback(System.IAsyncResult result);

        /// <summary>
        /// Defines how this protocol should start and\or connect
        /// </summary>
        /// <param name="address"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        void Start(IPAddress address, int port);

        /// <summary>
        /// Defines how the connection will end between the server with this protocol
        /// </summary>
        /// <returns>returns true if disconnection was succesful</returns>
        void Disconnect();
    }
}