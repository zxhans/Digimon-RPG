using Digimon_Project.Database.Results;
using Digimon_Project.Enums;

namespace Digimon_Project.Network.Packets
{
    // CC 03 Pacote contendo a autorização de conexão para o client
    public class PACKET_CONECT : OutPacket
    {
        public PACKET_CONECT()
            : base(PacketType.PACKET_CONECT)
        {
            int digimon_4 = 255;
            VerificationResult result = Emulator.Enviroment.Database.Select<VerificationResult>(
                                "valor as id", "config", "WHERE config = '4 digimon'");
            if (result.HasRows) digimon_4 = result.Id;
            Write(Utils.StringHex.Hex2Binary("00 00 00 00 92 49"));
            Write(Utils.StringHex.Hex2Binary("02 00 00 00 00 00 00 00"));
            Write(digimon_4); // ID do 4º Digimon no create Tamer.
                              // ATENÇÂO: Este NÃO é o ID do Banco, nem o modelo.
                              // É um ID interno, no client.
            Write(Utils.StringHex.Hex2Binary("D3 F5 D7 A3"));
            Write(Utils.StringHex.Hex2Binary("F1 FD D0 FC E7 E9 CE FA 00 00 00 00 00 00 00 00"));
            Write(Utils.StringHex.Hex2Binary("00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00"));
            Write(Utils.StringHex.Hex2Binary("00 00 00 00"));
        }
    }
}
