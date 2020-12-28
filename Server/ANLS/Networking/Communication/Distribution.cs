namespace ANL.Server.Networking.Communication
{
    /// <summary>
    /// Contains static methods that allow to send any packet to the client using specific casting(s)
    /// </summary>
    public class Distribution
    {
        /// <summary>
        /// Contains available casting methods for the TCP protocol
        /// </summary>
        public class TCP
        {
            /// <summary>
            /// Unicasts a packet trough TCP using to the specified ID
            /// </summary>
            /// <param name="packet">Packet that contains the data to be sended</param>
            /// <param name="id">Id of the client that needs to receive the packet</param>
            public static void Unicast(Packet packet, int id) 
            {
                packet.Data.WriteLength();

                Server.AvailableUsers[id].tcp.SendData(packet);
            }

            /// <summary>
            /// Broadcasts a packet trough TCP
            /// </summary>
            /// <param name="packet">Packet that contains the data to be sended</param>
            public static void Broadcast(Packet packet)
            {
                packet.Data.WriteLength();
                for(int i = 0; i < Server.Data.MaximumUsers; i++)
                {
                    Server.AvailableUsers[i].tcp.SendData(packet);
                }
            }

            /// <summary>
            /// Multicasts a packet trough TCP with an exception
            /// </summary>
            /// <param name="packet">Packet that contains the data to be sended</param>
            /// <param name="exception">Client id that should be avoided while sending the packet</param>
            public static void Multicast(Packet packet, int exception)
            {
                packet.Data.WriteLength();
                for (int i = 0; i < Server.Data.MaximumUsers; i++)
                {
                    if(i != exception)
                        Server.AvailableUsers[i].tcp.SendData(packet);
                }
            }

            /// <summary>
            /// Multicasts a packet trough TCP with multiple exceptions
            /// </summary>
            /// <param name="packet">Packet that contains the data to be sended</param>
            /// <param name="exceptions">Client ids that should be avoided while sending the packet</param>
            public static void Multicast(Packet packet, params int[] exceptions) 
            {
                packet.Data.WriteLength();
                for (int i = 0; i < Server.Data.MaximumUsers; i++)
                {
                    if(i != exceptions[i])
                        Server.AvailableUsers[i].tcp.SendData(packet);
                }
            }
        }

        /// <summary>
        /// Contains available casting methods for the UDP protocol
        /// </summary>
        public class UDP
        {
            /// <summary>
            /// Unicasts a packet trough UDP using to the specified ID
            /// </summary>
            /// <param name="packet">Packet that contains the data to be sended</param>
            /// <param name="id">Id of the client that needs to receive the packet</param>
            private static void Unicast(Packet packet, int id) 
            {
                packet.Data.WriteLength(); 

                Server.AvailableUsers[id].udp.SendData(packet);
            }

            /// <summary>
            /// Broadcasts a packet trough UDP
            /// </summary>
            /// <param name="packet">Packet that contains the data to be sended</param>
            private static void Broadcast(Packet packet)
            {
                packet.Data.WriteLength();
                for (int i = 0; i < Server.Data.MaximumUsers; i++)
                {
                    Server.AvailableUsers[i].udp.SendData(packet);
                }
            }

            /// <summary>
            /// Multicasts a packet trough UDP with an exception
            /// </summary>
            /// <param name="packet">Packet that contains the data to be sended</param>
            /// <param name="exception">Client id that should be avoided while sending the packet</param>
            private static void Multicast(Packet packet, int exception)
            {
                packet.Data.WriteLength();
                for (int i = 0; i < Server.Data.MaximumUsers; i++)
                {
                    if (i != exception)
                        Server.AvailableUsers[i].udp.SendData(packet);
                }
            } 

            /// <summary>
            /// Multicasts a packet trough UDP with multiple exceptions
            /// </summary>
            /// <param name="packet">Packet that contains the data to be sended</param>
            /// <param name="exceptions">Client ids that should be avoided while sending the packet</param>
            public static void Multicast(Packet packet, params int[] exceptions) 
            {
                packet.Data.WriteLength();
                for (int i = 0; i < Server.Data.MaximumUsers; i++)
                {
                    if(i != exceptions[i])
                        Server.AvailableUsers[i].udp.SendData(packet);
                }
            }
        }
    }
}