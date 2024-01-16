using System;

namespace Digimon_Project.Game
{
    public class Entity : IDisposable
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public ushort Model { get; set; }
        public ushort Level { get; set; }

        public Entity(int id)
        {
            Id = id;
        }

        public Entity()
        {

        }

        bool is_disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!is_disposed) // only dispose once!
            {
                if (disposing)
                {
                    
                }
            }
            is_disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            // tell the GC not to finalize
            GC.SuppressFinalize(this);
        }

        ~Entity()
        {

        }
    }
}
