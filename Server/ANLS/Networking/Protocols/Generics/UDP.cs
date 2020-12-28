using System;
using System.Net;
using System.Net.Sockets;

using ANL.Server.Threading;
using ANL.Server.Networking.Data.Packet;

namespace ANL.Server.Networking.Protocols.Generics 
{
    /// <summary>
    /// Simple UDP protocol definition <b>ready-to-use</b>
    /// </summary>
    public class UDP : Interfaces.IProtocol 
    {
        /// <summary>
        /// Current aiming udp endpoint
        /// </summary>
        public IPEndPoint endPoint;

        /// <summary>
        /// Defines the UDP client
        /// </summary>
        public static UdpClient UdpClient;

        private int _id; // id to check wich client is requesting a packet

        private static UDP _instance; // defines the instance of this protocol

        /// <summary>
        /// Gets an IProtocol value that defines the instance of this class
        /// </summary>
        /// <value></value>
        public static Interfaces.IProtocol Instance
        {
            get 
            {
                // if the instance definition is null then create a new one
                if(_instance == null)
                    _instance = new UDP();
                return _instance; // returns the instance of this protocol
            }
        }

        private UDP() { /* only for instance purposes */ }

        /// <summary>
        /// Creates a UDP connection to the defined client id
        /// </summary>
        /// <param name="id">Client id that needs a udp connection</param>
        public UDP(int id) 
        {
            this._id = id;
        }

        /// <summary>
        /// Sends a udp packet 
        /// </summary>
        /// <param name="packet"></param>
        public void SendData(Packet packet)
        {
            if(endPoint != null)
            {
                UdpClient.BeginSend(packet.Data.ToArray(), packet.Data.Length(), endPoint, null, null);
            }
        }

        /// <summary>
        /// Handles the incoming packet(s)
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public void HandleData(Packet packet)
        {
            int length = PacketSerializer.ReadInt(packet); // reads the packet length
            byte[] data = PacketSerializer.ReadBytes(packet, length); // converts the packet to a sequence of bytes and assigns it to the buffer

            // executes readings to get the current buffer packet id and sends it to the specific client that is requesting it
            ThreadManager.SharedInstance.RequestWorkFor(0, () => 
            {
                using (Packet intPacket = new Packet(data))
                {
                    int id = PacketSerializer.ReadInt(intPacket);
                    
                    Handling.ServerHandler.SharedInstance.RegisteredHandlers[id](_id, packet);
                }
            });
        }

        /// <summary>
        /// Main callback for this protocol, connects new clients and handles their incoming data costantly
        /// </summary>
        /// <param name="result">Status of this callback</param>
        public void ReceiverCallback(IAsyncResult result)
        {
            try
            {
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 0); // defines an endpoint aiming at any ip at port 0
                byte[] data = UdpClient.EndReceive(result, ref endPoint); // gets the latest data buffer from the endpoint and stops the receiving

                UdpClient.BeginReceive(ReceiverCallback, null); // starts it back based on this method

                // if the buffer length is short than 4 then return and retry
                if(data.Length < 4)
                    return;

                // creates a packet based on the current data buffer
                using (Packet packet = new Packet(data))
                {
                    int id = PacketSerializer.ReadInt(packet); // reads the client id from the incoming packet

                    // if the user id is less than 0 (only clients with an id greater than 0 can join) then return
                    if(id < 0)
                        return;
                    
                    // if the available client endpoint is null
                    if(Server.AvailableUsers[id].udp.endPoint == null)
                    {
                        Server.AvailableUsers[id].udp.Connect(endPoint); // connect based on this protocol to the needed endpoint

                        return;
                    }

                    // if the available client endpoint is equal to this protocol endpoint then start handling his data
                    if(Server.AvailableUsers[id].udp.endPoint.ToString() == endPoint.ToString())
                        Server.AvailableUsers[id].udp.HandleData(packet);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine($"Error while receiving UDP data due to: {e}");
            }
        }

        /// <summary>
        /// Handles a buffer of data that resides inside a packet
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool HandleData(byte[] data)
        {
            try 
            {
                using(Packet packet = new Packet(data))
                {
                    int len = PacketSerializer.ReadInt(packet);
                    data = PacketSerializer.ReadBytes(packet, len);
                }

                ThreadManager.SharedInstance.RequestWorkFor(0, () => 
                {
                    using(Packet packet = new Packet(data))
                    {
                        int pID = PacketSerializer.ReadInt(packet);

                        Handling.ServerHandler.SharedInstance.RegisteredHandlers[pID](_id, packet);
                    }
                });

                return true;
            }
            catch(Exception e) 
            {
                Console.WriteLine($"Unable to handle incoming buffer with udp due to: {e}");

                return false;
            }
        }

        /// <summary>
        /// Connets a endpoint to the other endpoint
        /// </summary>
        /// <param name="endPoint">EndPoint that udp needs to connect to</param>
        public void Connect(IPEndPoint endPoint) => this.endPoint = endPoint;

        public void Start(IPAddress address, int port) 
        {
            try 
            {
                UdpClient = new UdpClient(Server.Data.AimingPort);

                UdpClient.BeginReceive(ReceiverCallback, null);
            }
            catch(Exception e) 
            {
                Console.WriteLine($"Unable to initialize UDP protocol due to: {e}");
            }
        }

        /// <summary>
        /// Disconnects the udp listener
        /// </summary>
        /// <returns>Returns true if disconnection was succesfull</returns>
        public void Disconnect()
        { 
            try 
            {
                endPoint = null; 
            }
            catch(Exception e) 
            {
                Console.WriteLine($"Unable to close udp connection due to: {e}");
            }
        }
    }
}