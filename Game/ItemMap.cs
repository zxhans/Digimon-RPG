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
    // Classe que armazena itens no chão dos mapas
    public class ItemMap
    {
        public Item Item { get; set; }
        public Vector2 Location{ get; set; }
        private Timer aTimer;
        public MapZone Zone { get; set; }
        public int Dono { get; set; }
        public bool Livre = false;

        public ItemMap(Item i, Vector2 vec, MapZone map, int id)
        {
            Item = i;
            Location = vec;
            Zone = map;
            Dono = id;
            SetTimer();
        }

        // Startando temporizador, que vai liberar e eliminar o Item depois de um tempo
        private void SetTimer()
        {
            // Create a timer with a two second interval.
            aTimer = new Timer(60000); // 1000 = 1 segundo
            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += NextTime; 
            aTimer.AutoReset = false;
            aTimer.Enabled = true;
        }
        // Função que libera o item para outros Players depois do primeiro temporizador
        private void NextTime(Object source, ElapsedEventArgs e)
        {
            Livre = true;
            aTimer = new Timer(30000); // 1000 = 1 segundo
            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += Destroy;
            aTimer.AutoReset = false;
            aTimer.Enabled = true;
        }
        // Função que destroi o item depois do segundo temporizador
        private void Destroy(Object source, ElapsedEventArgs e)
        {
            aTimer.Close();
            Zone.Items.Remove(this);
            Zone.removeItem(Location);
        }
    }
}
