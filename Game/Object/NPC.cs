using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Digimon_Project.Game
{
    public abstract class NPC
    {
        public abstract void INPC(Client sender, int npcOp);
        public abstract void INPC(Client sender);
        public abstract void INPC(Client sender, int npcId, int id, int quant);

    }
}
