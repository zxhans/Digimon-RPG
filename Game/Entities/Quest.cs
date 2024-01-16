using Digimon_Project.Game.Data;
using System.Collections.Generic;

namespace Digimon_Project.Game.Entities
{
    // Classe que guarda as informações de um tamer
    public class Quest : Entity
    {
        public string QuestName { get; set; }
        public int Andamento { get; set; }
        public string Tipo { get; set; }
        public string Objetivo { get; set; }

        public Quest(int id)
            : base(id)
        {
            
        }

        public Quest()
        {
            Andamento = 1;
        }
    }
}
