namespace Digimon_Project.Enums
{
    public enum PacketType : int
    {
        Unknown = -1,
        // Login
        PACKET_MAP_CONNECT = 0x1CC, // CC 01 Map Login ?
        PACKET_972 = 5, // LoginServer Login
        PACKET_TAMER_LIST = 0x4CC, // CC 04 Verificando o ID e Passowrd antes de entrar na Tamer List
        LOGIN_TAMERLIST = 0x12CC, // CC 12 Enviando a Tamer List (Tela de Seleção)
        PACKET_CREATE_TAMER = 0x10CC, // CC 10 Create Tamer
        PACKET_DELETE_TAMER = 0x11CC, // CC 11 Delete Tamer
        PACKET_CHECK_CONNECTION = 0x14CC, // CC 14 - Check Connection
        PACKET_18892 = 0x49CC, // 0x49CC
        PACKET_TELEPORT_CONFIRM = 0xDDCC, // Pacote recebito quando o Client confirma teleporte

        // Login no Map
        PACKET_MAP_TAMER_AND_DIGIMONS = 0x02CC, // CC 02 Tamer Informations & Digimon in Map
        PACKET_INVENTARIO = 0x03CC, // Inventory load
        PACKET_CONECT = 0x03CC, // CC 03 Autorizando conexão (Selecionando o Servidor)
        PACKET_NEW_TRADE_PASS = 0x15CC, // CC 15 Nova warehouepassword
        PACKET_MAP_LOGIN = 0x16CC, // CC 16 Select Character
        PACKET_18CC = 0x18CC, // MAC Data.

        // Map
        PACKET_INVENTARIO_ATT = 0xFCCC, // Atualização de Inventário Durante o jogo
        PACKET_CHECK_TRADE_PASS = 0xCACC, // Verificando Tradepassword Durante o Jogo
        PACKET_20CC = 0x20CC, // Movement
        PACKET_CHAT = 0x21CC, // Chat
        PACKET_CHAT_WHISPER = 0xC8CC, // Chat Whisper PV Private
        PACKET_CHAT_SHOUT = 0xCACC, // Chat Global (verde)
        PACKET_CHAT_SPEAKER = 0x4FCC, // Chat Global (verde)
        PACKET_CHAT_SYSTEM = 0x4FCC, // Chat de systema
        PACKET_CHAT_GM = 0xE6CC, // Chat de systema
        PACKET_CHAT_SYSTEM2 = 0x66CC, // Chat de systema
        PACKET_NPC = 0x56CC, // NPC (NPC DA KATE? opcao de pegar itens iniciais)
        //PACKET_NPC_BUFF_DAY = 0xF9CC,   //npc da kate, opcao do buff de um dia
        PACKET_NPC_RESET_STATUS = 0xF2CC, // NPC Status Reset
        PACKET_NPC_DIGIMON = 0x54CC, // NPC Digimon ("D": Revive, Warehouse, Heal, etc)
        PACKET_NPC_ITEM_SHOP = 0x52CC, // NPC vendedor de itens
        PACKET_NPC_CARD_SHOP = 0x50CC, // NPC vendedor de cards
        PACKET_NPC_ITEM_SHOP_OP = 0x53CC, // Comprando ou vendendo itens de NPC
        PACKET_NPC_CARD_SHOP_OP = 0x51CC, // Comprando ou vendendo cards de NPC
        PACKET_NPC_SKY = 0x78CC, // Comprando ou vendendo cards de NPC
        PACKET_NPC_OMEGAX = 0x59CC, // Comprando ou vendendo cards de NPC
        //PACKET_NPC_OMEGAX_TRADE = 0x7DCC, // Comprando ou vendendo cards de NPC
        //PACKET_NPC_OMEGAX_TRADE = 0x96CC, // Comprando ou vendendo cards de NPC
        PACKET_NPC_OMEGAX_TRADE = 0x1DCC, // Comprando ou vendendo cards de NPC

        PACKET_DIGIMON_CHANGETYPE = 0xF3CC, // tfu
        PACKET_DIGIMON_INDIVIDUAL = 0xB4CC, // Pacote que envia informação individual de cada digimon
        PACKET_DIGIMON_ATT = 0xB2CC, // Pacote que envia informação individual de cada digimon
        PACKET_DIGIMON_LIDER = 0xF1CC, // Pacote que envia mudança de Digimon Líder
        PACKET_DIGIMON_ITEM_CHANGE_NAME = 0xF5CC, // Pacote que envia mudança de Nome od Digimon ao usar o item NameSwapX
        PACKET_DIGIMON_CATCHED_LOPEN = 0xEFCC, // Pacote que envia novo digimon ao Player (Pós captura, abre o L)
        PACKET_DIGIMON_EVO_RETURN = 0xF7CC, // Capsule Return (Evo Return)

        PACKET_D_INFO = 0x82CC, // Pacote com informações do Digimon por evolução (press D)
        PACKET_DIGIMON_INCR_ATRIBUTE = 0x71CC, // Client inseriu pontos em algum atributo do Digimon
        PACKET_DIGIMON_INCR_SKILL = 0x72CC, // Client inseriu pontos em alguma skill do Digimon
        PACKET_QUIT = 0x2BCC, // Pacote recebido ao Quitar do jogo
        PACKET_SPAWN = 0x11CC, // Pacote que spawna digimons no mapa
        PACKET_CC09 = 0x09CC,
        PACKET_FFCC = 0xFFCC,
        PACKET_CC64 = 0x64CC,
        PACKET_CONFIRM_LOCATION = 0xDECC, // Tamer confirmando sua posição
                                          //0xDCCC  [220] LENGTH: 142 -> muito parecido com localizacao e login. Aparece quando teleporta pra underground
        PACKET_CCC4 = 0xC4CC,
        PACKET_CC68 = 0x68CC,
        PACKET_CC55 = 0x55CC,
        PACKET_DIGIMON_NAME = 0xF0CC,
        PACKET_TAMER_XP = 0xB1CC, // Pacote que envia XP, XPMax, Level e Rank do Tamer
        PACKET_TELEPORT_GATE = 0xDFCC, // Pacote recebito quando o Client confirma teleporte
        
        //SERA QUE ESSE PACOTE NÃO É DO LOGIN??? O 0XDDCC É O MESMO USADO PARA PEGAR ITENS DO CASH SHOP WAREHOUSE
        //PACKET_TELEPORT_CONFIRM = 0xDDCC, // Pacote recebito quando o Client confirma teleporte
        
        
        PACKET_TELEPORT = 0xB0CC, // Pacote que envia teleporte
        PACKET_BIT_ATT = 0xCBCC, // Pacote que atualiza os Bits do Tamer
                                 // 0xA8CC - No MAPID 85 108x183, tem a RB com o NPC Dourgora, quando fala com ele da esse pacote

        PACKET_OPEN_SHOPWAREHOUSE = 0xDECC, // Tamer confirmando sua posição
        PACKET_SHOPWAREHOUSE_OP = 0xDDCC, // Tamer confirmando sua posição

        //ROBUST QUEST
        PACKET_QUEST_ROBUST = 0x92CC,

        // Party
        PACKET_PARTY_INVITY = 0x30CC, // Um Client convidou outro para Party
        PACKET_PARTY_LIST = 0x31CC, // Integrantes da Party
        PACKET_PARTY_EXIT = 0x32CC, // Membro da Party saiu do jogo
        PACKET_PARTY_CHECK = 0xDFCC, // Confirmação recebida durante a Party
        PACKET_PARTY_KICK = 0x40CC, // Líder kickou um membro da Party

        // Itens
        PACKET_GAIN_ITEM_INFO = 0xC9CC, // Pacote que envia uma descrição no chat quanto a item recebido
        PACKET_ITEM_DROP = 0x13CC, // Pacote que envia item no chão
        PACKET_GET_ITEM = 0x91CC, // Pacote que envia operação realizada com itens
        PACKET_GET_CARD = 0x90CC, // Pacote que envia operação realizada com Cards
        PACKET_USE_ITEM_DIGIMON = 0x41CC, // Pacote com o uso de um Item em um Digimon (Consumíveis)
        PACKET_USE_ITEM_BOX = 0xB6CC, // Pacote com o uso de um Item em um Digimon (Consumíveis)
        PACKET_USE_ITEM_TP_MINIMAP = 0x85CC, // Pacote do uso do item de Teleport no Minimap
        PACKET_DELETE_ITEM = 0xBDCC, // Client descartou um item
        PACKET_DELETE_CARD = 0xBECC, // Client descartou um Card
        PACKET_CARD_ATT = 0xC8CC, // Atualizando card
        PACKET_HATCH_PET = 0xDACC, // Client está chocando um Pet Egg
        PACKET_PET_ATT = 0xDBCC, // Atualizando Pet

        // Evolution (Card, Digiegg)
        PACKET_INSERT_DIGMON_EVO = 0x9FCC, // Inserindo Digimon no processo de evolução
        PACKET_EVOLUTION = 0x9DCC, // Processo de evolution (Card/Armor)
        PACKET_EVOLUTION_BEASTS = 0x9CCC, // Processo de evolution DAS 4 BESTAS SAGRADAS - BAIHU AZULONG EBON ZHUQI
        PACKET_EVOLUTION_FANG = 0x96CC, // Processo de evolution DAS 4 BESTAS SAGRADAS - BAIHU AZULONG EBON ZHUQI
        PACKET_CARD_COMBINE = 0xAACC, // CRIANDO FUSAO DE CARDS

        // Warehouse
        PACKET_WAREHOUSE_ITENS = 0xF4CC, // Listagem de itens e Cards da Warehouse
        PACKET_WAREHOUSE_BITS = 0x88CC, // Processo de depósito e retirada de bits da Warehouse
        PACKET_DIGISTORE = 0x89CC, // Processo do Digistore
        PACKET_DIGISTORE_LIST = 0x8ACC, // Listagem do Digistore
        PACKET_DIGISTORE_EXPANSION = 0x94CC, // Procedimento de expansão do Digistore

        // Craft
        PACKET_BASIC_CREATE = 0xE9CC, // Processo de criação de Crests e Digieggs básicos
        PACKET_ADVANCED_CREATE = 0xE8CC, // Processo de criação de Crests e Digieggs avançados
        PACKET_UPGRADE_CD = 0xE7CC, // Processo de upgrade de Crests e Digieggs - nao esta implementado

        // PvP
        PACKET_PVP_DESAFIO = 0x60CC, // Um Client desafiou outro Client para batalha

        // Trade
        PACKET_TRADE_REQUEST = 0xFECC, // Client solicitando trade com outro
        PACKET_TRADE = 0xFDCC, // Processo de trade

        // Batalha
        HANDLE_PACKET_START_BATTLE = 0x61CC, // Recebido ao entrar na batalha
        PACKET_BATTLE_START = 0x62CC, // Primeira resposta
        PACKET_BATTLE_CENARY = 0x63CC, // Pacote que envia o cenário da batalha. Digimons no time inimigo e aliado, etc
        PACKET_BATTLE_DIGIEVOLUTION = 0x81CC, // Pacote enviado durante a Digievolução em batalha
        PACKET_BATTLE_RECALL = 0x64CC, // Pacote enviado quando chamamos um Digimon da party em batalha
        PACKET_BATTLE_EVO_LIST = 0x9ECC, // Pacote que envia a lista de evoluções disponíveis em batalha
        PACKET_BATTLE_LVLUP = 0x70CC, // Pacote que envia informação de Level UP durante a batalha
        PACKET_BATTLE_XP = 0x6ECC, // Pacote que envia XP recebida durante batalha
        PACKET_BATTLE_CARD_SLASH = 0xA0CC, // Pacote recebido ao equipar um card no Card Slah em batalha
        PACKET_BATTLE_CARD_SLASH_REMOVE = 0xA1CC, // Pacote recebido ao desequipar um card no Card Slah em batalha
        PACKET_BATTLE_CHECK = 0x69CC, // Pacote enviado e recebido durante a batalha, 
                                      //representa o início e o fim de uma ação.
        PACKET_BATTLE_RESULT = 0x6DCC, // Pacote que envia o resultado da batalha (Win, Lose)
        PACKET_BATTLE_FUGA = 0x65CC, // Pacote enviado para fugir da batalha
        PACKET_BATTLE_EXECUTE_ACTION = 0x67CC, // Pacote que descreve a ação executada em batalha
        PACKET_BATTLE_TP = 0x6BCC, // Pacote enviado para encher a barra TP (Yellow bar) em batalha
        PACKET_BATTLE_ACTION = 0x35CC, // Pacote que determina ações executadas em batalha
        PACKET_BATTLE_CAPTURA = 0xB3CC, // Pacote que envia resultado da captura em batalha

        // Desenvolvimento - pacotes para auxílio ou em andamento
        PACKET_TESTE = 0x0000,
    }
}
