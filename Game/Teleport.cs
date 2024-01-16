﻿using Digimon_Project.Database.Results;
using Digimon_Project.Game.Data;
using Digimon_Project.Game.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace Digimon_Project.Game
{
    // Classe que armazena itens no chão dos mapas
    public class Teleport : Entity
    {
        public Vector2 Location { get; set; }
        public int Alvo { get; set; }
        public int AlvoX { get; set; }
        public int AlvoY { get; set; }

        public Teleport(Vector2 location, int alvo, int alvox, int alvoy)
        {
            Location = location;
            Alvo = alvo;
            AlvoX = alvox;
            AlvoY = alvoy;
        }

        public Teleport(int id)
            : base(id)
        {
            
        }
    }
}
