using Digimon_Project.Game.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Digimon_Project.Game
{
    public class WorldEntity : Entity
    {
        public WorldEntity(int id)
            : base(id)
        {

        }
        public Vector2 Location { get; set; }
    }
}
