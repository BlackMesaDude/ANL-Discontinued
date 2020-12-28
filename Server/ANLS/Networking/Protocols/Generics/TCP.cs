using System;
using System.Net;
using System.Net.Sockets;

using ANL.Server.Threading;
using ANL.Server.Networking.Data.Packet;

namespace ANL.Server.Networking.Protocols.Generics 
{
    using Packet = Networking.Packet;

    /// <summary>
    /// Simple TCP protocol definition <b>ready-to-use</b>
    /// </summary>
    public class TCP : Interfaces.IProtocol 
    {
        /// <summary>
        /// Defines the main TcpClient
        /// </summary>
        public TcpClient tcpClient;

        /// <summary>
        /// Defines the TCP listener
        /// </summary>
        public static TcpListener TcpListener;

        private NetworkStream _nStream; // defines a network stream for this protocol
        
        private Packet _packet; // defines the current received packet 
        private byte[] _buffer; // defines the buffer of the packet

        private int _bufferSize; // defines the default size of the network stream buffer

        private readonly int _id; // readonly id to check wich client is requesting a packet

        private static TCP _instance; // defines the instance of this protocol

        /// <summary>
        /// Gets the instance of this protocol
        /// </summary>
        /// <value>Returns a IProtocol that defines the instance of this class</value>
        public static Interfaces.IProtocol Instance 
        {
            get 
            {
                if(_instance == null)
                    _instance = new TCP();
                return _instance;
            }
        }

        private TCP() { /* only for instance purposes */ }

        /// <summary>
        /// Creates a handler for the TCP protocol based on the target user id and buffer size
        /// </summary>
        /// <param name="id">The user id to be handled, must be available and connected</param>
        /// <param name="maximumBufferSize">Maximum size for the incoming and sending buffer</param>
        public TCP(int id, int maximumBufferSize) 
        {
            this._id = id;
            this._bufferSize = maximumBufferSize;
        }

        /// <summary>
        /// Allows to connect to a specific client
        /// </summary>
        /// <param name="client">TcpClient to be connected at</param>
        public void Connect(TcpClient client)
        {
            this.tcpClient = client;

            /* defines the default sizes of the tcp client buffer */
            client.ReceiveBufferSize = _bufferSize; 
            client.SendBufferSize = _bufferSize;

            _nStream = client.GetStream(); // initializes the network stream based on the client stream

            /* initializes incoming packet and buffer definition */
            _packet = new Packet();
            _buffer = new byte[_bufferSize];

            _nStream.BeginRead(_buffer, 0, _bufferSize, ReceiverCallback, null); // starts to read at offset 0 on the defined buffer with the appropriate callback
        }

        /// <summary>
        /// Sends a packet to the client
        /// </summary>
        /// <param name="packet">Packet to be sent</param>
        public void SendData(Packet packet)
        {
            try 
            {
                if(tcpClient != null) _nStream.BeginWrite(packet.Data.ToArray(), 0, packet.Data.Length(), null, null); // if the tcp client isn't null then start writing the packet contents
            }
            catch(Exception e)
            {
                Console.WriteLine($"Unable to write packet contents for tcp due to: {e}");
            }
        }

        /// <summary>
        /// Handles the incoming data as a buffer, checks it and executes the handler
        /// </summary>
        /// <param name="data">Data to be handled</param>
        /// <returns>Returns true if the operation was succesful</returns>
        public bool HandleData(byte[] data)
        {
            int len = 0; // length of the packet

            _packet.Data.SetBytes(_packet, data); // sets the packet buffer as the data parameter

            // if the length is > of 4 re-read and try again
            if(_packet.Data.UnreadLength() >= 4)
            {
                len = PacketSerializer.ReadInt(_packet);
                if(len <= 0)
                    return true;
            }

            while(len > 0 && len <= _packet.Data.UnreadLength())
            {
                byte[] pBytes = PacketSerializer.ReadBytes(_packet, len); // defines a temporary buffer that contains the packet contents

                // reads the id of the specific packet and sends it to the client that is needing it
                ThreadManager.SharedInstance.RequestWorkFor(0, () => 
                {
                    using(Packet packet = new Packet(pBytes))
                    {
                        int id = PacketSerializer.ReadInt(packet);

                        Handling.ServerHandler.SharedInstance.RegisteredHandlers[id](_id, packet);
                    }
                });

                // if the length is still > than 4 re-read and try again
                len = 0;
                if(_packet.Data.UnreadLength() >= 4)
                {
                    len = PacketSerializer.ReadInt(_packet);
                    if(len <= 0)
                        return true;
                }
            }

            // if the packet has contents then return true otherwise false
            if(len <= 1)
                return true;
            return false;
        }

        /// <summary>
        /// Callback that allows to fastly handle the incoming results, doesn't have specific purposes outside this class. Use this only when needed!
        /// </summary>
        /// <param name="result"></param>
        public void ReceiverCallback(IAsyncResult result)
        {
            // tries to copy the buffer over, reset the packet it-self and read new data again
            try 
            {
                int len = _nStream.EndRead(result); // ends the stream reading process
                if(len <= 0)
                {
                    // if the received length is under 0 then re-try or disconnect
                    Server.AvailableUsers[_id].Disconnect();
                    return;
                }

                byte[] data = new byte[len]; // creates a empty buffer based on the final length from the stream 
                Array.Copy(_buffer, data, len); // copies the current buffer to the new buffer with the specified length

                _packet.Reset(HandleData(data)); // resets the main packet
                _nStream.BeginRead(_buffer, 0, _bufferSize, ReceiverCallback, null); // starts the stream reading back again
            }
            catch(Exception e) 
            {
                Console.WriteLine($"Unable to gather tcp data due to: {e}");
                Server.AvailableUsers[_id].Disconnect();
            }
        }

        public void AcceptanceCallback(IAsyncResult result)
        {
            TcpClient client = TcpListener.EndAcceptTcpClient(result);
            TcpListener.BeginAcceptTcpClient(new AsyncCallback(AcceptanceCallback), null);

            Console.WriteLine($"Client {client.Client.RemoteEndPoint} is trying to connect to the server.");

            for(int i = 0; i < Server.Data.MaximumUsers; i++)
            {
                if(Server.AvailableUsers[i].tcp.tcpClient == null)
                {
                    Server.AvailableUsers[i].tcp.Connect(client);

                    Console.WriteLine($"{client.Client.RemoteEndPoint} has been allowed to the server.");
                    return;
                }
            }

            Console.WriteLine($"{client.Client.RemoteEndPoint} failed to connect: server is full!.");
        }

        public void Start(IPAddress address, int port) 
        {
            try 
            {
                TcpListener = new TcpListener(address, port);

                TcpListener.Start();            
                TcpListener.BeginAcceptTcpClient(new AsyncCallback(AcceptanceCallback), null);
            }
            catch(Exception e) 
            {
                Console.WriteLine($"Unable to initialize TCP protocol due to: {e}");
            }
        }

        /// <summary>
        /// Tries to disconnect the tcp listener from the server
        /// </summary>
        /// <returns>returns true if the stream closed and the listener terminated it-self</returns>
        public void Disconnect() 
        {
            try 
            {
                _nStream.Close(); // closes the stream

                _nStream = null;

                _packet = null;
                _buffer = null;

                tcpClient = null;
            }
            catch(Exception e) 
            {
                Console.WriteLine($"Unable to disconnect from tcp due to: {e}");
            }          
        }
    }
}