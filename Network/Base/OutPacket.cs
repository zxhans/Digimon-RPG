using Digimon_Project.Enums;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digimon_Project.Network
{
    public class OutPacket : Packet
    {
        private int type = 0;
        public BinaryWriter writer = null;

        // Classe responsável pelo envio de pacotes (Saída)

        public OutPacket(PacketType type)
            : base()
        {
            this.type = (int)type;
            writer = new BinaryWriter(this.stream);
            Write((int)type); // Packet Id
            Write((int)0); // Total Length, Replace later.
        }

        public OutPacket(int type, byte[] buffer)
            : base()
        {
            this.type = type;
            writer = new BinaryWriter(this.stream);
            Write((int)type); // Packet Id
            Write((int)0); // Total Length, Replace later.
            Write(buffer);
        }

        public OutPacket(InPacket p)
            : base()
        {
            writer = new BinaryWriter(this.stream);
            writer.Write(p.GetBuffer());
        }

        public OutPacket()
            : base()
        {
            writer = new BinaryWriter(this.stream);
        }

        ~OutPacket()
        {
            writer.Dispose();
        }

        public void Seek(int offset, SeekOrigin orgin)
        {
            this.writer.Seek(offset + 8, orgin); // We skip the header.
        }

        public void Write(bool value)
        {
            this.writer.Write((byte)(value ? 1 : 0));
        }

        public void Write(byte value)
        {
            this.writer.Write(value);
        }

        public void Write(byte[] value)
        {
            this.writer.Write(value);
        }

        public void Write(short value)
        {
            this.writer.Write(value);
        }

        public void Write(ushort value)
        {
            this.writer.Write(value);
        }

        public void Write(int value)
        {
            this.writer.Write(value);
        }

        public void Write(uint value)
        {
            this.writer.Write(value);
        }

        public void Write(long value)
        {
            this.writer.Write(value);
        }

        public void Write(ulong value)
        {
            this.writer.Write(value);
        }

        public void Write(double value)
        {
            this.writer.Write(value);
        }

        public void Fill(int length)
        {
            if (length > 0)
                for (int i = 0; i < length; i++)
                    Write((byte)0);
        }

        public void Write(string value, int length)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(value);

            if (buffer.Length < length)
            {
                Write(buffer); // Write buffer.
                Write(new byte[length - buffer.Length]); // Fill the rest with nulls.
            }
            else if (buffer.Length >= length)
            {
                writer.Write(buffer, 0, length); // Cut off the buffer.
            }
        }

        public override byte[] GetBuffer()
        {
            int length = (int)writer.BaseStream.Length;

            writer.Seek(4, SeekOrigin.Begin);
            writer.Write(length);

            byte[] buffer = base.GetBuffer();
            if (Emulator.Enviroment.Teste)
            {
                System.Console.WriteLine("SEND -> {0}  LENGTH: {1}", type, buffer.Length);
                System.Console.WriteLine(Utils.Dump.HexDump(buffer).Skip(8).ToArray());
                System.Console.WriteLine("==============================================================================");
            }

            return buffer;
        }
    }
}
