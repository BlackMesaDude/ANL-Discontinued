namespace ANL.Server.Networking.Data.Packet 
{
    using Packet = Networking.Packet;

    /// <summary>
    /// Utility class that allows to serialize and deserialize data that resides in the packet
    /// </summary>
    public static class PacketSerializer 
    {
        #region Packet Serialization
        
        /// <summary>
        /// Writes a byte to the packet buffer
        /// </summary>
        /// <param name="packet">Target packet where the byte should be added</param>
        /// <param name="value">Byte that needs to be wrote inside the packet</param>
        public static void Write(this Packet packet, byte value)
        {
            packet.Data.MainBuffer.Add(value);
        }

        /// <summary>
        /// Writes a byte array to the packet buffer
        /// </summary>
        /// <param name="packet">Target packet where the byte array should be added</param>
        /// <param name="value">Byte array that needs to be wrote inside the packet</param>
        public static void Write(this Packet packet, byte[] value)
        {
            packet.Data.MainBuffer.AddRange(value);
        }

        /// <summary>
        /// Writes a short to the packet buffer
        /// </summary>
        /// <param name="packet">Target packet where the short should be added</param>
        /// <param name="value">Short that needs to be wrote inside the packet</param>
        public static void Write(this Packet packet, short value)
        {
            packet.Data.MainBuffer.AddRange(System.BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Writes a 32bit signed integer to the packet buffer
        /// </summary>
        /// <param name="packet">Target packet where the 32bit signed integer should be added should be added</param>
        /// <param name="value">32bit signed integer value that needs to be wrote inside the packet</param>
        public static void Write(this Packet packet, int value)
        {
            packet.Data.MainBuffer.AddRange(System.BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Writes a 64bit signed integer to the packet buffer
        /// </summary>
        /// <param name="packet">Target packet where the 64bit signed integer should be added should be added</param>
        /// <param name="value">32bit signed integer value that needs to be wrote inside the packet</param>
        public static void Write(this Packet packet, long value)
        {
            packet.Data.MainBuffer.AddRange(System.BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Writes a single-precision floating-point to the packet buffer
        /// </summary>
        /// <param name="packet">Target packet where the floating-point should be added should be added</param>
        /// <param name="value">Floating-point value that needs to be wrote inside the packet</param>
        public static void Write(this Packet packet, float value)
        {
            packet.Data.MainBuffer.AddRange(System.BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Writes a boolean to the packet buffer
        /// </summary>
        /// <param name="packet">Target packet where the boolean should be added should be added</param>
        /// <param name="value">Boolean value that needs to be wrote inside the packet</param>
        public static void Write(this Packet packet, bool value)
        {
            packet.Data.MainBuffer.AddRange(System.BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Writes a string value to the packet buffer
        /// </summary>
        /// <param name="packet">Target packet where the string should be added should be added</param>
        /// <param name="value">String value that needs to be wrote inside the packet</param>
        public static void Write(this Packet packet, string value)
        {
            Write(packet, value.Length);

            packet.Data.MainBuffer.AddRange(System.Text.Encoding.ASCII.GetBytes(value));
        }

        #endregion

        #region Data Deserialization

        /// <summary>
        /// Reads a byte value inside the buffer at the given offset
        /// </summary>
        /// <param name="packet">Target packet where the byte should be read</param>
        /// <param name="moveReadPos">Should we move the offset after the reading?</param>
        /// <returns>Returns the readden byte value</returns>
        public static byte ReadByte(this Packet packet, bool moveReadPos = true)
        {
            if (packet.Data.MainBuffer.Count > packet.Data.CurrentReadingPosition)
            {
                byte value = packet.Data.ReadableBuffer[packet.Data.CurrentReadingPosition];
                if (moveReadPos)
                {
                    packet.Data.CurrentReadingPosition += 1;
                }

                return value;
            }
            else
            {
                throw new System.Exception($"Couldn't read the given value as 'byte'.");
            }
        }

        /// <summary>
        /// Reads a byte array value inside the buffer at the given offset
        /// </summary>
        /// <param name="packet">Target packet where the byte array should be read</param>
        /// <param name="moveReadPos">Should we move the offset after the reading?</param>
        /// <returns>Returns the readden byte[] value</returns>
        public static byte[] ReadBytes(this Packet packet, int length, bool moveReadPos = true)
        {
            if (packet.Data.MainBuffer.Count > packet.Data.CurrentReadingPosition)
            {
                byte[] value = packet.Data.MainBuffer.GetRange(packet.Data.CurrentReadingPosition, length).ToArray(); 
                if (moveReadPos)
                {
                    packet.Data.CurrentReadingPosition += length; 
                }

                return value; 
            }
            else
            {
                throw new System.Exception("Couldn't read the given value as 'byte[]'.");
            }
        }

        /// <summary>
        /// Reads a short value inside the buffer at the given offset
        /// </summary>
        /// <param name="packet">Target packet where the short should be read</param>
        /// <param name="moveReadPos">Should we move the offset after the reading?</param>
        /// <returns>Returns the readden short value</returns>
        public static short ReadShort(this Packet packet, bool moveReadPos = true)
        {
            if (packet.Data.MainBuffer.Count > packet.Data.CurrentReadingPosition)
            {
                short value = System.BitConverter.ToInt16(packet.Data.ReadableBuffer, packet.Data.CurrentReadingPosition);
                if (moveReadPos)
                {
                    packet.Data.CurrentReadingPosition += 2;
                }

                return value;
            }
            else
            {
                throw new System.Exception("Couldn't read the given value as 'short'.");
            }
        }

        /// <summary>
        /// Reads a int value inside the buffer at the given offset
        /// </summary>
        /// <param name="packet">Target packet where the int should be read</param>
        /// <param name="moveReadPos">Should we move the offset after the reading?</param>
        /// <returns>Returns the readden int value</returns>
        public static int ReadInt(this Packet packet, bool moveReadPos = true)
        {
            if (packet.Data.MainBuffer.Count > packet.Data.CurrentReadingPosition)
            {
                int value = System.BitConverter.ToInt32(packet.Data.ReadableBuffer, packet.Data.CurrentReadingPosition);
                if (moveReadPos)
                {
                    packet.Data.CurrentReadingPosition += 4; 
                }

                return value;
            }
            else
            {
                throw new System.Exception("Couldn't read the given value as 'int'.");
            }
        }

        /// <summary>
        /// Reads a long value inside the buffer at the given offset
        /// </summary>
        /// <param name="packet">Target packet where the long should be read</param>
        /// <param name="moveReadPos">Should we move the offset after the reading?</param>
        /// <returns>Returns the readden long value</returns>
        public static long ReadLong(this Packet packet, bool moveReadPos = true)
        {
            if (packet.Data.MainBuffer.Count > packet.Data.CurrentReadingPosition)
            {
                long value = System.BitConverter.ToInt64(packet.Data.ReadableBuffer, packet.Data.CurrentReadingPosition);
                if (moveReadPos)
                {
                    packet.Data.CurrentReadingPosition += 8; 
                }

                return value;
            }
            else
            {
                throw new System.Exception("Couldn't read the given value as 'long'.");
            }
        }

        /// <summary>
        /// Reads a float value inside the buffer at the given offset
        /// </summary>
        /// <param name="packet">Target packet where the float should be read</param>
        /// <param name="moveReadPos">Should we move the offset after the reading?</param>
        /// <returns>Returns the readden float value</returns>
        public static float ReadFloat(this Packet packet, bool _moveReadPos = true)
        {
            if (packet.Data.MainBuffer.Count > packet.Data.CurrentReadingPosition)
            {
                float value = System.BitConverter.ToSingle(packet.Data.ReadableBuffer, packet.Data.CurrentReadingPosition);
                if (_moveReadPos)
                {
                    packet.Data.CurrentReadingPosition += 4; 
                }

                return value;
            }
            else
            {
                throw new System.Exception("Couldn't read the given value as 'float'.");
            }
        }

        /// <summary>
        /// Reads a bool value inside the buffer at the given offset
        /// </summary>
        /// <param name="packet">Target packet where the bool should be read</param>
        /// <param name="moveReadPos">Should we move the offset after the reading?</param>
        /// <returns>Returns the readden bool value</returns>
        public static bool ReadBool(this Packet packet, bool moveReadPos = true)
        {
            if (packet.Data.MainBuffer.Count > packet.Data.CurrentReadingPosition)
            {
                bool value = System.BitConverter.ToBoolean(packet.Data.ReadableBuffer, packet.Data.CurrentReadingPosition);
                if (moveReadPos)
                {
                    packet.Data.CurrentReadingPosition += 1;
                }

                return value;
            }
            else
            {
                throw new System.Exception("Couldn't read the given value as 'bool'.");
            }
        }

        /// <summary>
        /// Reads a string value inside the buffer at the given offset
        /// </summary>
        /// <param name="packet">Target packet where the string should be read</param>
        /// <param name="moveReadPos">Should we move the offset after the reading?</param>
        /// <returns>Returns the readden string value</returns>
        public static string ReadString(this Packet packet, bool moveReadPos = true)
        {
            try
            {
                int len = ReadInt(packet); 

                string value = System.Text.Encoding.ASCII.GetString(packet.Data.ReadableBuffer, packet.Data.CurrentReadingPosition, len); 
                if (moveReadPos && value.Length > 0)
                {
                    packet.Data.CurrentReadingPosition += len;
                }

                return value;
            }
            catch
            {
                throw new System.Exception("Couldn't read the given value as 'string'.");
            }
        }

        #endregion
    }
}