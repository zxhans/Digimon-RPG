using System;
using System.Diagnostics;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;
using Digimon_Project.Utils;

namespace Digimon_Project.Network.Packets
{
    // Exibindo listagem de digimons no Digistore
    public class PACKET_DIGISTORE_LIST : OutPacket
    {
        public PACKET_DIGISTORE_LIST(Tamer t, short pagina)
            : base(PacketType.PACKET_DIGISTORE_LIST)
        {
            Write(new byte[6]);
            
            // Página atual
            Write(pagina);

            // Total de páginas disponíveis
            int total_paginas = (int)Math.Ceiling(t.Digistore / 10.0);
            Write((short)total_paginas);

            // Não vamos prosseguir se a pagina atual for maior que o total de páginas
            if (pagina <= total_paginas)
            {
                // Número de slots liberados
                int slots = 0;
                if (t.Digistore <= 10)
                    slots = t.Digistore;
                else if (pagina < total_paginas)
                    slots = 10;
                else if (pagina == total_paginas)
                    slots = t.Digistore % 10;
                Write(slots);

                // Digimons
                int start = (pagina - 1) * 10; // Slot inicial
                /**
                Debug.Print("pagina: {0}, total: {1}, Digistore: {2}, start: {3}", pagina, total_paginas
                    , t.Digistore, start);
                /**/
                for (int i = start; i <= start + 9; i++)
                {
                    if (t.Digistorage.Count > i && t.Digistorage[i] != null
                        && t.Digistorage[i].Digistore == 1)
                    {
                        Write(t.Digistorage[i].Id);
                        Write((int)t.Digistorage[i].Model);
                        Write(t.Digistorage[i].Name != "noname" ?
                            t.Digistorage[i].Name : t.Digistorage[i].OriName, 21); // Digimon name
                        Write((byte)1);
                        Write((short)t.Digistorage[i].Level);
                        Write((byte)t.Digistorage[i].estage);
                        Write((byte)t.Digistorage[i].type);
                        Write((short)0);
                        Write(t.Digistorage[i].Health);
                        Write(new byte[40]);
                    }
                    else
                        Write(new byte[80]);
                }
            }
        }
    }
}
