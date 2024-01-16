namespace Digimon_Project.Network
{
    public abstract class Handler<T> : IHandler where T : ISocketSession // Must be a valid connection.
    {
        public abstract void Handle(T sender, InPacket packet);

        public void Handle(object sender, InPacket packet)
        {
            Handle((T)sender, packet);
        }
    }
}
