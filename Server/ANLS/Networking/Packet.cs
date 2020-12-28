using System;
using System.Collections.Generic;

using ANL.Server.Networking.Data.Packet.Structures;

namespace ANL.Server.Networking
{
    using static Data.Packet.PacketSerializer;

    /// <summary>
    /// Represents a Packet that is <see cref="System.IDisposable" /> 
    /// </summary>
    public class Packet : IDisposable
    {
        /// <summary>
        /// Gets this packet data, such as buffers or pointer position
        /// </summary>
        /// <value>Returns a PacketData that contains this packet buffer contents</value>
        public PacketData Data = new PacketData();

        /// <summary>
        /// Creates a new packet
        /// </summary>
        public Packet()
        {
            Data.MainBuffer = new List<byte>();
            Data.CurrentReadingPosition = 0; 
        }

        /// <summary>
        /// Creates a new packet and writes his id to the buffer
        /// </summary>
        /// <param name="id">ID of the packet</param>
        public Packet(int id)
        {
            Data.MainBuffer = new List<byte>();
            Data.CurrentReadingPosition = 0;

            this.Write(id);
        }

        /// <summary>
        /// Creates a new packet with a starting content for the buffer
        /// </summary>
        /// <param name="data">Starting data for the buffer</param>
        public Packet(byte[] data)
        {
            Data.MainBuffer = new List<byte>();
            Data.CurrentReadingPosition = 0;

            Data.SetBytes(this, data);
        }

        /// <summary>
        /// Resets this packet buffers and any other data contained by it
        /// </summary>
        /// <param name="shouldReset">Should it reset the buffers too?</param>
        public void Reset(bool shouldReset = true)
        {
            if (shouldReset)
            {
                Data.MainBuffer.Clear(); 
                Data.ReadableBuffer = null;
                Data.CurrentReadingPosition = 0; 
            }
            else
            {
                Data.CurrentReadingPosition -= 4;
            }
        }

        private bool disposed = false; // defines if the object is disposed or not

        /// <summary>
        /// Disposes the Packet safely
        /// </summary>
        /// <param name="disposing">Is it disposed?</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    Data.MainBuffer = null;
                    Data.ReadableBuffer = null;
                    Data.CurrentReadingPosition = 0;
                }

                disposed = true;
            }
        }

        /// <summary>
        /// Disposes this packet safely
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}