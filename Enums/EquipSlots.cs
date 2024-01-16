namespace Digimon_Project.Enums
{
    public enum EquipSlots : int
    {
        // Os itens equipados nos slots do tamer ainda são itens. Poderíamos fazer uma tabela para eles, mas
        // além de aumentar o trabalho, ficaríamos com um código desnecessariamente grande ao ter que transferir
        // dados entre uma tabela e outra. Então, vamos usar a mesma tabela de itens. Apesar de no inventário 
        // só aparecer os itens até o slot 24, não temos um limite de slots no banco. Vamos usar os slots excedentes
        // como os slots de equipamentos, mantendo os itens na mesma tabela, e tendo que mudar apenas o campo slot
        // ao equipar ou desequipar um item.
        crest1 = 25,
        crest2 = 26,
        crest3 = 27,
        digiegg1 = 28,
        digiegg2 = 29,
        digiegg3 = 30,
        card1 = 31,
        card2 = 32,
        card3 = 33,
        card4 = 34,
        card5 = 35,
        card6 = 36,
        aura = 37,
        digivice = 38,
        sock = 39,
        shoes = 40,
        pants = 41,
        glove = 42,
        tshirt = 43,
        jacket = 44,
        hat = 45,
        customer = 46,
        earring = 47,
        necklace = 48,
        ring = 49,
        bagexp1 = 50,
    }
}
