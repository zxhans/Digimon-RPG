using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Digimon_Project.Enums;

namespace Digimon_Project.Network
{
    public class InPacket : Packet
    {
        public readonly int PacketId;
        public readonly int PacketLength;

        private BinaryReader reader;

        public int Remaining { get { return (int)(stream.Length - stream.Position); } }

        public InPacket(byte[] packetBuffer)
            : base(packetBuffer)
        {

            reader = new BinaryReader(this.stream);

            PacketId = reader.ReadInt32();
            PacketLength = reader.ReadInt32();
            if (Emulator.Enviroment.Teste)
            {
                Console.WriteLine("RECV -> 0x{0:X2}  [{1}] LENGTH: {2}", PacketId, (PacketId - 0xCC) / 0x100, PacketLength);
                Console.WriteLine(Utils.Dump.HexDump(packetBuffer.Skip(8).ToArray()));
                Console.WriteLine("==============================================================================");
            }
        }

        ~InPacket()
        {
            reader.Close();
            reader.Dispose();
        }

        public byte ReadByte()
        {
            if (Remaining < 1)
                throw new IndexOutOfRangeException();
            return reader.ReadByte();
        }

        public byte[] ReadBytes(int length)
        {
            if (Remaining < length)
                throw new IndexOutOfRangeException();
            return reader.ReadBytes(length);
        }

        public sbyte ReadSByte()
        {
            if (Remaining < 1)
                throw new IndexOutOfRangeException();
            return reader.ReadSByte();
        }

        public ushort ReadUShort()
        {
            if (Remaining < 2)
                throw new IndexOutOfRangeException();
            return reader.ReadUInt16();
        }

        public short ReadShort()
        {
            if (Remaining < 2)
                throw new IndexOutOfRangeException();
            return reader.ReadInt16();
        }

        public uint ReadUInt()
        {
            if (Remaining < 4)
                throw new IndexOutOfRangeException();
            return reader.ReadUInt32();
        }

        public int ReadInt()
        {
            if (Remaining < 4)
                throw new IndexOutOfRangeException();
            return reader.ReadInt32();
        }

        public ulong ReadULong()
        {
            if (Remaining < 8)
                throw new IndexOutOfRangeException();
            return reader.ReadUInt64();
        }

        public double ReadDouble()
        {
            if (Remaining < 8)
                throw new IndexOutOfRangeException();
            return reader.ReadDouble();
        }

        public float ReadFloat()
        {
            if (Remaining < 8)
                throw new IndexOutOfRangeException();
            return (float)reader.ReadDouble();
        }

        public long ReadLong()
        {
            if (Remaining < 8)
                throw new IndexOutOfRangeException();
            return reader.ReadInt64();
        }

        public String ReadString(int length)
        {
            byte[] buffer = ReadBytes(length);
            return System.Text.Encoding.UTF8.GetString(buffer).Replace("\0", string.Empty);
        }

        public void Skip(int size)
        {
            if (Remaining < size)
                throw new IndexOutOfRangeException();
            reader.ReadBytes(size); // Skip
        }
    }
}
