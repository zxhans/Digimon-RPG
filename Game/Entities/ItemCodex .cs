using Digimon_Project.Game.Data;
using System.Collections.Generic;

namespace Digimon_Project.Game.Entities
{
    // Classe que guarda as informações de um tamer
    public class ItemCodex : Entity
    {
        public int ItemId { get; set; }
        public byte ItemTag { get; set; }
        public byte ItemType { get; set; }
        public byte ItemUseOn { get; set; }
        public int ItemQuantMax { get; set; }
        public int ItemtamerLvl { get; set; }
        public int ItemEffect1 { get; set; }
        public int ItemEffect1Value { get; set; }
        public int ItemEffect2 { get; set; }
        public int ItemEffect2Value { get; set; }
        public int ItemEffect3 { get; set; }
        public int ItemEffect3Value { get; set; }
        public int ItemEffect4 { get; set; }
        public int ItemEffect4Value { get; set; }
        public int Custo { get; set; }
        public string ItemName { get; set; }
        public int ItemTab = 0;

        public ItemCodex(int id)
            : base(id)
        {
            
        }

        // Função que retorna um objeto Item baseado neste Codex
        public Item GetItem(int quant, int id)
        {
            return new Item(0, id) {
                ItemId = ItemId,
                ItemTag = ItemTag,
                ItemType = ItemType,
                ItemUseOn = ItemUseOn,
                ItemQuant = quant,
                ItemQuantMax = ItemQuantMax,
                ItemtamerLvl = ItemtamerLvl,
                ItemEffect1 = ItemEffect1,
                ItemEffect2 = ItemEffect2,
                ItemEffect3 = ItemEffect3,
                ItemEffect4 = ItemEffect4,
                ItemEffect1Value = ItemEffect1Value,
                ItemEffect2Value = ItemEffect2Value,
                ItemEffect3Value = ItemEffect3Value,
                ItemEffect4Value = ItemEffect4Value,
                ItemName = ItemName,
                ItemTab = ItemTab,
                Custo = Custo,
            };
        }
        public Item GetItem(int id)
        {
            return GetItem(ItemQuantMax, id);
        }
        public Item GetItem()
        {
            return GetItem(ItemQuantMax, Id);
        }
    }
}
