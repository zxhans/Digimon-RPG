using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Digimon_Project.Game.Entities
{
    public class Player : WorldEntity
    {
        public Client Owner { get; private set; }
        public Tamer Tamer { get; private set; }

        public Player(Client owner, Tamer tamer)
            : base(0)
        {
            this.Owner = owner;
            this.Tamer = tamer;
            this.Name = tamer.Name; // Copy the name.
        }
    }
}
