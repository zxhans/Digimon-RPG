namespace Digimon_Project.Game
{
    public class SlotEntity : Entity
    {
        public int Slot { get; set; }
        public int XP { get; set; }
        public int MaxXP { get; set; }

        public SlotEntity(int slot, int id)
            : base (id)
        {
            this.Slot = slot;
        }
    }
}
