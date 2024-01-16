using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;

namespace Digimon_Project.Network
{
    public abstract class Packet
    {
        protected MemoryStream stream { get; private set; }

        public Packet(byte[] buffer)
        {
            stream = new MemoryStream(buffer);
        }

        public Packet()
        {
            stream = new MemoryStream();
        }

        ~Packet()
        {
            stream.Dispose();
        }

        public virtual byte[] GetBuffer()
        {
            return stream.ToArray();
        }
    }
}
