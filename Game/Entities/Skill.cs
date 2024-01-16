using Digimon_Project.Database.Results;
using Digimon_Project.Game.Data;
using System.Collections.Generic;

namespace Digimon_Project.Game.Entities
{
    // Classe que guarda as informações de um tamer
    public class Skill : Entity
    {
        public int Lvl { get; set; }
        public int Units { get; set; }
        public int Range { get; set; }
        public int Poder { get; set; }
        public int VP = 31;

        public Skill(int id)
            : base(id)
        {
            
        }
    }
}
