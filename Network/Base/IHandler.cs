namespace Digimon_Project.Network
{
    public interface IHandler
    {
        void Handle(object sender, InPacket packet);
    }
}
