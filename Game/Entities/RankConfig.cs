using Digimon_Project.Game.Data;
using System.Collections.Generic;

namespace Digimon_Project.Game.Entities
{
    // Classe que guarda as informações de um tamer
    public class RankConfig
    {
        public int TPBarSec = 0;
        public int QuantMax = 0, F2Lvl = 0, F3Lvl = 0, FatorHP = 0;
        public int XPPerc = 0, HPPerc = 0, ATKPerc = 0, DEFPerc = 0
            , BLPerc = 0, BitPerc = 0;

        public RankConfig()
        {
            
        }
    }
}
