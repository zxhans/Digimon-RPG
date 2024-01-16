using Digimon_Project.Database.Results;
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
    // Classe que armazena o poli-alfabeto do CMO
    public class AlphaMap
    {
        public char[][] Alpha = new char[16][];

        public AlphaMap()
        {
            Alpha[0] = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
            Alpha[1] = new char[] { '1', '0', '3', '2', '5', '4', '7', '6', '9', '8', 'B', 'A', 'D', 'C', 'F', 'E' };
            Alpha[2] = new char[] { '2', '3', '0', '1', '6', '7', '4', '5', 'A', 'B', '8', '9', 'E', 'F', 'C', 'D' };
            Alpha[3] = new char[] { '3', '2', '1', '0', '7', '6', '5', '4', 'B', 'A', '9', '8', 'F', 'E', 'D', 'C' };
            Alpha[4] = new char[] { '4', '5', '6', '7', '0', '1', '2', '3', 'C', 'D', 'E', 'F', '8', '9', 'A', 'B' };
            Alpha[5] = new char[] { '5', '4', '7', '6', '1', '0', '3', '2', 'D', 'C', 'F', 'E', '9', '8', 'B', 'A' };
            Alpha[6] = new char[] { '6', '7', '4', '5', '2', '3', '0', '1', 'E', 'F', 'C', 'D', 'A', 'B', '8', '9' };
            Alpha[7] = new char[] { '7', '6', '5', '4', '3', '2', '1', '0', 'F', 'E', 'D', 'C', 'B', 'A', '9', '8', };
            Alpha[8] = new char[] { '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', '0', '1', '2', '3', '4', '5', '6', '7' };
            Alpha[9] = new char[] { '9', '8', 'B', 'A', 'D', 'C', 'F', 'E', '1', '0', '3', '2', '5', '4', '7', '6' };
            Alpha[0xA] = new char[] { 'A', 'B', '8', '9', 'E', 'F', 'C', 'D', '2', '3', '0', '1', '6', '7', '4', '5' };
            Alpha[0xB] = new char[] { 'B', 'A', '9', '8', 'F', 'E', 'D', 'C', '3', '2', '1', '0', '7', '6', '5', '4' };
            Alpha[0xC] = new char[] { 'C', 'D', 'E', 'F', '8', '9', 'A', 'B', '4', '5', '6', '7', '0', '1', '2', '3' };
            Alpha[0xD] = new char[] { 'D', 'C', 'F', 'E', '9', '8', 'B', 'A', '5', '4', '7', '6', '1', '0', '3', '2' };
            Alpha[0xE] = new char[] { 'E', 'F', 'C', 'D', 'A', 'B', '8', '9', '6', '7', '4', '5', '2', '3', '0', '1' };
            Alpha[0xF] = new char[] { 'F', 'E', 'D', 'C', 'B', 'A', '9', '8', '7', '6', '5', '4', '3', '2', '1', '0' };
        }
    }
}
