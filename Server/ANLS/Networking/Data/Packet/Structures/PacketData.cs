using System.Collections.Generic;

namespace ANL.Server.Networking.Data.Packet.Structures
{
    /// <summary>
    /// Defines the main packet data that contains buffers and other information related to it
    /// </summary>
    public class PacketData
    {
        /// <summary>
        /// The main packet buffer
        /// </summary>
        public List<byte> MainBuffer;

        /// <summary>
        /// Readable buffer, differs from the main buffer, allows to read each byte of the packet easly after the processing is complete
        /// </summary>
        public byte[] ReadableBuffer;

        /// <summary>
        /// Current reading cursor position inside the packet buffer
        /// </summary>
        public int CurrentReadingPosition;

        /// <summary>
        /// Sets the packet bytes based on a predefined buffer
        /// </summary>
        /// <param name="packet">Packet that needs to get the new buffer</param>
        /// <param name="data">Buffer that needs to be wrote inside the pakcet</param>
        public void SetBytes(Networking.Packet packet, byte[] data)
        {
            PacketSerializer.Write(packet, data); // writes the buffer to the packet
            
            ReadableBuffer = MainBuffer.ToArray(); // sets the readable buffer to the main buffer as array
        }

        /// <summary>
        /// Writes the length of the packet
        /// </summary>
        public void WriteLength()
        {
            MainBuffer.InsertRange(0, System.BitConverter.GetBytes(MainBuffer.Count));
        }

        /// <summary>
        /// Inserts an integer value in the packet
        /// </summary>
        /// <param name="value">Integer that needs to be inserted</param>
        public void InsertInt(int value)
        {
            MainBuffer.InsertRange(0, System.BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Converts main buffer and assigns the output to the readable buffer
        /// </summary>
        /// <returns></returns>
        public byte[] ToArray()
        {
            ReadableBuffer = MainBuffer.ToArray();

            return ReadableBuffer;
        }

        /// <summary>
        /// Length of the MainBuffer
        /// </summary>
        /// <returns></returns>
        public int Length()
        {
            return MainBuffer.Count; 
        }

        /// <summary>
        /// Length that is still needed to be read\passed by the cursor
        /// </summary>
        /// <returns></returns>
        public int UnreadLength()
        {
            return Length() - CurrentReadingPosition; 
        }
    }
}