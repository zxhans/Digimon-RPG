using System;
using System.Diagnostics;
using System.IO;
using Digimon_Project.Enums;
using Digimon_Project.Game;
using Digimon_Project.Game.Entities;

namespace Digimon_Project.Network.Packets
{
    // Este é apenas um Writer de pacotes com estrutura de digimons, só para não ficar repetindo a estrutura.
    public class PACKET_DIGIMON_WRITER
    {
        public void WriteDigimon(Digimon d, OutPacket p)
        {
            WriteDigimon(d, d.BattleId, d.BattleSufix, d.estage, d.Id, p);
        }
        public void WriteDigimon(Digimon d, long BattleId, string BattleSufix, OutPacket p)
        {
            WriteDigimon(d, BattleId, BattleSufix, d.estage, d.Id, p);
        }
        public void WriteNullId(Digimon d, int id, OutPacket p)
        {
            p.Write(id);
            p.Write(new byte[516]);
        }
        public void WriteDigimon(Digimon d, int fase, OutPacket p)
        {
            WriteDigimon(d, d.BattleId, d.BattleSufix, fase, d.Id, p);
        }
        public void WriteDigimon(Digimon d, long BattleId, string BattleSufix, int fase, int id, OutPacket p)
        {
            int Model = d.Model;
            double STR = d.Strength;
            double DEX = d.Dexterity;
            double CON = d.Constitution;
            double INT = d.Intelligence;
            int type = d.type;
            int Health = d.Health;
            int VP = d.VP;
            int MaxHealth = d.MaxHealth;
            int MaxVP = d.MaxVP;
            int Attack = d.Attack;
            int Defence = d.Defence;
            int BattleLevel = d.BattleLevel;
            int skill1lvl = d.skill1lvl;
            int skill2lvl = d.skill2lvl;
            Skill skill1 = d.skill1;
            Skill skill2 = d.skill2;
            string OriName = d.OriName;

            // Pontos a distribuir
            int Points = d.MyPoints;
            switch (fase)
            {
                case 3:
                    Model = d.CModel;
                    STR = d.CStrength;
                    DEX = d.CDexterity;
                    CON = d.CConstitution;
                    INT = d.CIntelligence;
                    type = d.Ctype;
                    Health = d.C_Health;
                    VP = d.C_VP;
                    MaxHealth = d.C_MaxHealth;
                    MaxVP = d.C_MaxVP;
                    Attack = d.CAttack;
                    Defence = d.CDefence;
                    BattleLevel = d.CBattleLevel;
                    skill1 = d.Cskill1;
                    skill2 = d.Cskill2;
                    skill1lvl = d.Cskill1lvl;
                    skill2lvl = d.Cskill2lvl;
                    Points = d.MyCPoints;
                    OriName = d.COriName;
                    break;
                case 4:
                    Model = d.UModel;
                    STR = d.UStrength;
                    DEX = d.UDexterity;
                    CON = d.UConstitution;
                    INT = d.UIntelligence;
                    type = d.Utype;
                    Health = d.U_Health;
                    VP = d.U_VP;
                    MaxHealth = d.U_MaxHealth;
                    MaxVP = d.U_MaxVP;
                    Attack = d.UAttack;
                    Defence = d.UDefence;
                    BattleLevel = d.UBattleLevel;
                    skill1 = d.Uskill1;
                    skill2 = d.Uskill2;
                    skill1lvl = d.Uskill1lvl;
                    skill2lvl = d.Uskill2lvl;
                    Points = d.MyUPoints;
                    OriName = d.UOriName;
                    break;
                case 5:
                    Model = d.MModel;
                    STR = d.MStrength;
                    DEX = d.MDexterity;
                    CON = d.MConstitution;
                    INT = d.MIntelligence;
                    type = d.Mtype;
                    Health = d.M_Health;
                    VP = d.M_VP;
                    MaxHealth = d.M_MaxHealth;
                    MaxVP = d.M_MaxVP;
                    Attack = d.MAttack;
                    Defence = d.MDefence;
                    BattleLevel = d.MBattleLevel;
                    skill1 = d.Mskill1;
                    skill2 = d.Mskill2;
                    skill1lvl = d.Mskill1lvl;
                    skill2lvl = d.Mskill2lvl;
                    Points = d.MyMPoints;
                    OriName = d.MOriName;
                    break;
            }

            p.Write(id); // Digimon_id
            p.Write(BattleId); // Id em batalha (vamos usar o mesmo Id)
            p.Write(BattleSufix, 8); // Preenchimento
            p.Write(Model); // Digimon_model
            p.Write((byte)d.EvolutionQuant); // Quantidade de Evoluções liberadas
            p.Write(d.Name != "noname" ? d.Name : OriName, 21); // Nome
            p.Write(d.Level); // Level
            p.Write((int)STR);
            p.Write((int)DEX);
            p.Write((int)CON);
            p.Write((int)INT);
            p.Write(Utils.StringHex.Hex2Binary("00 00 00 00 00 00 00 00"));
            p.Write(Utils.StringHex.Hex2Binary("00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00"));
            p.Write(Utils.StringHex.Hex2Binary("00 00 0F 00"));
            p.Write((short)(d.Tamer != null ? d.Tamer.STRBonus : 0)); // STR Bônus
            p.Write((short)(d.Tamer != null ? d.Tamer.DEXBonus : 0)); // DEX Bônus
            p.Write((short)(d.Tamer != null ? d.Tamer.CONBonus : 0)); // CON Bônus
            p.Write((short)(d.Tamer != null ? d.Tamer.INTBonus : 0)); // INT Bônus
            p.Write(Utils.StringHex.Hex2Binary("00 00 00 00 00 00 00 00"));
            p.Write((short)(d.Tamer != null ? d.Tamer.AttackBonus : 0)); // Attack Bônus (%)
            p.Write((short)(d.Tamer != null ? d.Tamer.DefenseBonus : 0)); // Defense Bônus (%)
            p.Write((short)(d.Tamer != null ? d.Tamer.BLevelBonus : 0)); // Battle Level Bônus (%)
            p.Write(Utils.StringHex.Hex2Binary("00 00 00 00 00 00"));
            p.Write(Utils.StringHex.Hex2Binary("00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00"));
            p.Write(Utils.StringHex.Hex2Binary("00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00"));
            p.Write(Utils.StringHex.Hex2Binary("00 00 00 00 00 00 00 00"));
            p.Write((byte)fase); // Nível (01-Training, 02-Rookie, 03-Champ, 04-Ult, 05-Mega)
            p.Write((byte)type); // Type (1 Vacine, 2 Data, 3 Virus)
            p.Write(Utils.StringHex.Hex2Binary("00 00 00 00 00 00"));
            p.Write(Health); // HP
            p.Write(VP); // VP
            p.Write(d.EV); // EVP
            p.Write(Attack); // Damage
            p.Write(Defence); // Defense
            p.Write(BattleLevel); // Battle Level
            p.Write(Utils.StringHex.Hex2Binary("00 00 00 00 00 00 00 00"));
            p.Write((long)d.XP); // Exp
            p.Write(Utils.StringHex.Hex2Binary("F4 01 00 00"));
            p.Write(MaxHealth); // HP Máximo
            p.Write(MaxVP); // VP Máximo
            p.Write(d.MaxEV); // EVP Máximo
            p.Write(Utils.StringHex.Hex2Binary("00 00 00 00 A0 86 01 00"));
            p.Write((long)d.MaxXP); // Exp Máxima
            p.Write(d.BattleWins > 0 ? d.BattleWins : 0); // Battles Win
            p.Write(d.Battles > 0 ? d.Battles : 0); // Battles Total
            p.Write((ushort)(Points > 0 ? Points : 0)); // Pontos a serem distribuidos nos atributos
            p.Write((ushort)(d.MySkillPoints > 0 ? d.MySkillPoints : 0)); // Pontos a serem distribuidos nas skills
            p.Write(Utils.StringHex.Hex2Binary("00 00 00 00"));
            p.Write(skill1.Id); // Skill 1 ID
            p.Write((short)skill1.Lvl); // Skill 1 lvl required
            p.Write(Utils.StringHex.Hex2Binary("20 00"));
            p.Write((byte)skill1.Range); // Skill 1 Range (01 = Near, 02 = Far)
            p.Write((byte)skill1.Units); // Skill 1 Unidades atingidas
            p.Write(Utils.StringHex.Hex2Binary("04 00 20 00 04 00"));
            p.Write(skill1lvl); // Skill 1 lvl
            p.Write(skill2.Id); // Skill 2 ID
            p.Write((short)(skill2.Id != 0 ? skill2.Lvl : 0)); // Skill 2 lvl required
            p.Write((short)(skill2.Id != 0 ? 20 : 0));
            p.Write((byte)(skill2.Id != 0 ? skill2.Range : 0)); // Skill 2 Range (01 = Near, 02 = Far)
            p.Write((byte)(skill2.Id != 0 ? skill2.Units : 0)); // Skill 2 Unidades atingidas
            p.Write((skill2.Id != 0 ? Utils.StringHex.Hex2Binary("04 00 20 00 04 00") : 
                Utils.StringHex.Hex2Binary("00 00 00 00 00 00")));
            p.Write(skill2.Id != 0 ? (d.Level >= skill2.Lvl ? skill2lvl : 0) : 0); // Skill 2 lvl
            p.Write(Utils.StringHex.Hex2Binary("00 00 00 00 00 00 00 00"));
            p.Write((byte)(d.iSpawn ? d.rank : 0));
            p.Write(Utils.StringHex.Hex2Binary("00 00 00 00 00 00 00"));
            p.Write(Utils.StringHex.Hex2Binary("00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00"));
            p.Write(Utils.StringHex.Hex2Binary("00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00"));
            p.Write(new byte[176]);

        }
    }
}
