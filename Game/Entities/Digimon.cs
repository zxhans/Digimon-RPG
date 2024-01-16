using Digimon_Project.Database.Results;
using Digimon_Project.Utils;
using System;
using System.Diagnostics;
using System.Timers;

namespace Digimon_Project.Game.Entities
{
    public class Digimon : SlotEntity
    {
        public Tamer Tamer { get; set; }
        public int DigimonId { get; set; }
        public int RookieForm = 0;
        public int ChampForm = 0;
        public int UltimForm = 0;
        public int MegaForm = 0;
        public int EvolutionQuant { get; set; } // Calculate
        public int RModel { get; set; }
        public int CModel { get; set; }
        public int UModel { get; set; }
        public int MModel { get; set; }
        public string OriName { get; set; }
        public string COriName { get; set; }
        public string UOriName { get; set; }
        public string MOriName { get; set; }
        public int Digistore { get; set; } // Estou no Digistore? 0 - Não, 1 - Sim

        public int Health { get; set; }
        public int VP { get; set; }
        public int EV { get; set; }
        // Champion
        public int C_Health { get; set; }
        public int C_VP { get; set; }
        // Ultimate
        public int U_Health { get; set; }
        public int U_VP { get; set; }
        // Mega
        public int M_Health { get; set; }
        public int M_VP { get; set; }

        // Our custom set points.
        public int MyStrength { get; set; }
        public int MyDexterity { get; set; }
        public int MyConstitution { get; set; }
        public int MyIntelligence { get; set; }
        public int MyPoints { get; set; }
        // Champion Stats
        public int MyCStrength { get; set; }
        public int MyCDexterity { get; set; }
        public int MyCConstitution { get; set; }
        public int MyCIntelligence { get; set; }
        public int MyCPoints { get; set; }
        // Ultimate Stats
        public int MyUStrength { get; set; }
        public int MyUDexterity { get; set; }
        public int MyUConstitution { get; set; }
        public int MyUIntelligence { get; set; }
        public int MyUPoints { get; set; }
        // Mega Stats
        public int MyMStrength { get; set; }
        public int MyMDexterity { get; set; }
        public int MyMConstitution { get; set; }
        public int MyMIntelligence { get; set; }
        public int MyMPoints { get; set; }

        public int MySkillPoints { get; set; }

        public int[] statsLevel = new int[4];
        public int[] statsRLevel = new int[4];
        public int[] statsCLevel = new int[4];
        public int[] statsULevel = new int[4];
        public int[] statsMLevel = new int[4];

        public Skill skill1;
        public Skill skill2;
        public int skill1lvl { get; set; }
        public int skill2lvl { get; set; }
        // Rookie
        public Skill Rskill1;
        public Skill Rskill2;
        public int Rskill1lvl { get; set; }
        public int Rskill2lvl { get; set; }
        // Champion Skills
        public Skill Cskill1;
        public Skill Cskill2;
        public int Cskill1lvl { get; set; }
        public int Cskill2lvl { get; set; }
        // Ultimate skills
        public Skill Uskill1;
        public Skill Uskill2;
        public int Uskill1lvl { get; set; }
        public int Uskill2lvl { get; set; }
        // Mega skills
        public Skill Mskill1;
        public Skill Mskill2;
        public int Mskill1lvl { get; set; }
        public int Mskill2lvl { get; set; }

        public int type { get; set; }
        public int Rtype { get; set; }
        public int Ctype { get; set; }
        public int Utype { get; set; }
        public int Mtype { get; set; }

        public int estage { get; set; }

        public int MaxHealth { get; set; }
        public int MaxVP { get; set; }
        public int MaxEV { get; set; }
        // Rookie
        public int R_MaxHealth { get; set; }
        public int R_MaxVP { get; set; }
        // Champion
        public int C_MaxHealth { get; set; }
        public int C_MaxVP { get; set; }
        // Ultimate
        public int U_MaxHealth { get; set; }
        public int U_MaxVP { get; set; }
        // Mega
        public int M_MaxHealth { get; set; }
        public int M_MaxVP { get; set; }

        // Spawn
        public byte rank { get; set; }

        // Our calculated points.
        public double Strength { get; set; }
        public double Dexterity { get; set; }
        public double Constitution { get; set; }
        public double Intelligence { get; set; }
        // Rookie
        public double RStrength { get; set; }
        public double RDexterity { get; set; }
        public double RConstitution { get; set; }
        public double RIntelligence { get; set; }
        // Champion
        public double CStrength { get; set; }
        public double CDexterity { get; set; }
        public double CConstitution { get; set; }
        public double CIntelligence { get; set; }
        // Ultimate
        public double UStrength { get; set; }
        public double UDexterity { get; set; }
        public double UConstitution { get; set; }
        public double UIntelligence { get; set; }
        // Mega
        public double MStrength { get; set; }
        public double MDexterity { get; set; }
        public double MConstitution { get; set; }
        public double MIntelligence { get; set; }

        public int Attack { get; set; } // Calculate.
        public int Defence { get; set; } // Calculate.
        public int BattleLevel { get; set; } // Calculate.
        // Rookie
        public int RAttack { get; set; } // Calculate.
        public int RDefence { get; set; } // Calculate.
        public int RBattleLevel { get; set; } // Calculate.
        // Champion
        public int CAttack { get; set; } // Calculate.
        public int CDefence { get; set; } // Calculate.
        public int CBattleLevel { get; set; } // Calculate.
        // Ultimate
        public int UAttack { get; set; } // Calculate.
        public int UDefence { get; set; } // Calculate.
        public int UBattleLevel { get; set; } // Calculate.
        // Mega
        public int MAttack { get; set; } // Calculate.
        public int MDefence { get; set; } // Calculate.
        public int MBattleLevel { get; set; } // Calculate.

        public int Battles { get; set; }
        public int BattleWins { get; set; }

        public Digimon(int slot, int id)
            : base(slot, id)
        {
            // Default Values, Level 1.
            Level = 1;
            Health = 100;
            MaxHealth = 100;
            VP = 100;
            MaxVP = 100;
            EV = 0;
            MaxEV = 0;
        }

        // Função para calcular pontos a serem distribuídos nos atributos
        private void CalcRPoints()
        {
            MyPoints = 0;
            int points = MyStrength + MyDexterity + MyConstitution + MyIntelligence;
            int lvl = Level - 1;
            if (lvl > 20) lvl = 19;
            int hPoints = lvl * 2;
            MyPoints = hPoints - points;
            if (MyPoints < 0) MyPoints = 0;
        }
        private void CalcCPoints()
        {
            if (Level >= 21)
            {
                MyCPoints = 0;
                int points = MyCStrength + MyCDexterity + MyCConstitution + MyCIntelligence;
                int lvl = Level;
                if (lvl > 30) lvl = 30;
                lvl -= 20;
                int hPoints = lvl * 2;
                MyCPoints = hPoints - points;
                if (MyCPoints < 0) MyCPoints = 0;
            }
        }
        private void CalcUPoints()
        {
            if (Level >= 31)
            {
                MyUPoints = 0;
                int points = MyUStrength + MyUDexterity + MyUConstitution + MyUIntelligence;
                int lvl = Level;
                if (lvl > 40 && MegaForm != 0) lvl = 40;
                lvl -= 30;
                int hPoints = lvl * 2;
                MyUPoints = hPoints - points;
                if (MyUPoints < 0) MyUPoints = 0;
            }
        }
        private void CalcMPoints()
        {
            if (Level >= 41)
            {
                MyMPoints = 0;
                int points = MyMStrength + MyMDexterity + MyMConstitution + MyMIntelligence;
                int lvl = Level;
                lvl -= 40;
                int hPoints = lvl * 2;
                MyMPoints = hPoints - points;
                if (MyMPoints < 0) MyMPoints = 0;
            }
        }
        public void CalcPoints()
        {
            CalcRPoints();
            CalcCPoints();
            CalcUPoints();
            CalcMPoints();
        }

        // Função para calcular os pontos a serem distribuidos nas skills
        public void CalSkillPoints()
        {
            if (!iSpawn)
            {
                MySkillPoints = 0;

                if (Level >= 6 && (skill1lvl < 10 || skill2lvl < 10 || Cskill1lvl < 10 || Cskill2lvl < 10
                    || Uskill1lvl < 10 || Uskill2lvl < 10 || Mskill1lvl < 10 || Mskill2lvl < 10))
                {
                    int points = 0;

                    if (skill1 != null && Level >= skill1.Lvl)
                    {
                        if (skill1lvl < 1) skill1lvl = 1;
                        points += skill1lvl - 1;
                    }
                    if (skill2 != null && Level >= skill2.Lvl)
                    {
                        if (skill2lvl < 1) skill2lvl = 1;
                        points += skill2lvl - 1;
                    }
                    if (Cskill1 != null && Level >= Cskill1.Lvl)
                    {
                        if (Cskill1lvl < 1) Cskill1lvl = 1;
                        points += Cskill1lvl - 1;
                    }
                    if (Cskill2 != null && Level >= Cskill2.Lvl)
                    {
                        if (Cskill2lvl < 1) Cskill2lvl = 1;
                        points += Cskill2lvl - 1;
                    }
                    if (Uskill1 != null && Level >= Uskill1.Lvl)
                    {
                        if (Uskill1lvl < 1) Uskill1lvl = 1;
                        points += Uskill1lvl - 1;
                    }
                    if (Uskill2 != null && Level >= Uskill2.Lvl)
                    {
                        if (Uskill2lvl < 1) Uskill2lvl = 1;
                        points += Uskill2lvl - 1;
                    }
                    if (Mskill1 != null && Level >= Mskill1.Lvl)
                    {
                        if (Mskill1lvl < 1) Mskill1lvl = 1;
                        points += Mskill1lvl - 1;
                    }
                    if (Mskill2 != null && Level >= Mskill2.Lvl)
                    {
                        if (Mskill2lvl < 1) Mskill2lvl = 1;
                        points += Mskill2lvl - 1;
                    }

                    int lvl = Level - 1;
                    int hPoints = lvl;
                    MySkillPoints = hPoints - points;
                }
            }
        }

        // Calculo de atributos
        private void DefaultStatsForLevel()
        {
            // Até Rookie
            int limit = 20;
            if (Level < 20 || iSpawn) limit = Level;
            Strength = 10 + (statsLevel[0] * limit - 1) + MyStrength + (Math.Floor((limit - 1) / 10.0) * 20);
            Dexterity = 10 + (statsLevel[1] * limit - 1) + MyDexterity + (Math.Floor((limit - 1) / 10.0) * 20);
            Constitution = 10 + (statsLevel[2] * limit - 1) + MyConstitution + (Math.Floor((limit - 1) / 10.0) * 20);
            Intelligence = 10 + (statsLevel[3] * limit - 1) + MyIntelligence + (Math.Floor((limit - 1) / 10.0) * 20);
            RStrength = 10 + (statsRLevel[0] * limit - 1) + MyStrength + (Math.Floor((limit - 1) / 10.0) * 20);
            RDexterity = 10 + (statsRLevel[1] * limit - 1) + MyDexterity + (Math.Floor((limit - 1) / 10.0) * 20);
            RConstitution = 10 + (statsRLevel[2] * limit - 1) + MyConstitution + (Math.Floor((limit - 1) / 10.0) * 20);
            RIntelligence = 10 + (statsRLevel[3] * limit - 1) + MyIntelligence + (Math.Floor((limit - 1) / 10.0) * 20);
            if (Level >= 11 || iSpawn)
            {
                Strength = 10 + (statsRLevel[0] * limit - 1) + MyStrength + (Math.Floor((limit - 1) / 10.0) * 20);
                Dexterity = 10 + (statsRLevel[1] * limit - 1) + MyDexterity + (Math.Floor((limit - 1) / 10.0) * 20);
                Constitution = 10 + (statsRLevel[2] * limit - 1) + MyConstitution + (Math.Floor((limit - 1) / 10.0) * 20);
                Intelligence = 10 + (statsRLevel[3] * limit - 1) + MyIntelligence + (Math.Floor((limit - 1) / 10.0) * 20);
            }

            // Champion
            limit = 30;
            if (Level < 30 || iSpawn) limit = Level;
            CStrength = (statsCLevel[0] * (limit - 20)) + MyCStrength + (Math.Floor((limit - 11) / 10.0) * 20) + Strength;
            CDexterity = (statsCLevel[1] * (limit - 20)) + MyCDexterity + (Math.Floor((limit - 11) / 10.0) * 20) + Dexterity;
            CConstitution = (statsCLevel[2] * (limit - 20)) + MyCConstitution + (Math.Floor((limit - 11) / 10.0) * 20) + Constitution;
            CIntelligence = (statsCLevel[3] * (limit - 20)) + MyCIntelligence + (Math.Floor((limit - 11) / 10.0) * 20) + Intelligence;
            // Ultimate
            limit = 40;
            if (Level < 40 || MegaForm == 0 || iSpawn) limit = Level;
            UStrength = (statsULevel[0] * (limit - 30)) + MyUStrength + (Math.Floor((limit - 21) / 10.0) * 20) + CStrength;
            UDexterity = (statsULevel[1] * (limit - 30)) + MyUDexterity + (Math.Floor((limit - 21) / 10.0) * 20) + CDexterity;
            UConstitution = (statsULevel[2] * (limit - 30)) + MyUConstitution + (Math.Floor((limit - 21) / 10.0) * 20) + CConstitution;
            UIntelligence = (statsULevel[3] * (limit - 30)) + MyUIntelligence + (Math.Floor((limit - 21) / 10.0) * 20) + CIntelligence;
            // Mega
            MStrength = (statsMLevel[0] * (Level - 40)) + MyMStrength + (Math.Floor((Level - 31) / 10.0) * 20) + UStrength;
            MDexterity = (statsMLevel[1] * (Level - 40)) + MyMDexterity + (Math.Floor((Level - 31) / 10.0) * 20) + UDexterity;
            MConstitution = (statsMLevel[2] * (Level - 40)) + MyMConstitution + (Math.Floor((Level - 31) / 10.0) * 20) + UConstitution;
            MIntelligence = (statsMLevel[3] * (Level - 40)) + MyMIntelligence + (Math.Floor((Level - 31) / 10.0) * 20) + UIntelligence;
        }

        // Calculando as bases
        public void Calculate()
        {
            Config c = Emulator.Enviroment.Config;
            // Reset for calculation.
            DefaultStatsForLevel();
            CalcPoints();
            CalSkillPoints();

            // Quantidade de evoluções liberadas
            EvolutionQuant = 1;
            if (Level >= 11) EvolutionQuant = 2;
            if (Level >= 21) EvolutionQuant = 3;
            if (Level >= 31) EvolutionQuant = 4;
            if (Level >= 41 && MegaForm != 0) EvolutionQuant = 5;

            // Attributes
            // In Training Base
            int atk_base = c.ITAttackBase;
            int hp_base = c.ITHPBase;
            int def_base = c.ITDEFBase;
            int bl_base = c.ITBLBase;
            int vp_base = c.ITVPBase;

            // Rookie Base
            int hp_Rbase = c.RokieHPBase;
            int atk_Rbase = c.RokieAttackBase;
            int def_Rbase = c.RokieDEFBase;
            int bl_Rbase = c.RokieBLBase;
            int vp_Rbase = c.RokieVPBase;
            if (estage == 2)
            {
                hp_base = hp_Rbase;
                atk_base = atk_Rbase;
                def_base = def_Rbase;
                bl_base = bl_Rbase;
                vp_base = vp_Rbase;
            }
            // Champion Base
            int atk_Cbase = atk_Rbase + c.ChampAttackBase;
            int hp_Cbase = hp_Rbase + c.ChampHPBase;
            int def_Cbase = def_Rbase + c.ChampDEFBase;
            int bl_Cbase = bl_Rbase + c.ChampBLBase;
            int vp_Cbase = vp_Rbase + c.ChampVPBase;
            // Ultimate Base
            int atk_Ubase = atk_Cbase + c.UltimAttackBase;
            int hp_Ubase = hp_Cbase + c.UltimHPBase;
            int def_Ubase = def_Cbase + c.UltimDEFBase;
            int bl_Ubase = bl_Cbase + c.UltimBLBase;
            int vp_Ubase = vp_Cbase + c.UltimVPBase;
            // Mega Base
            int atk_Mbase = atk_Ubase + c.MegaAttackBase;
            int hp_Mbase = hp_Ubase + c.MegaHPBase;
            int def_Mbase = def_Ubase + c.MegaDEFBase;
            int bl_Mbase = bl_Ubase + c.MegaBLBase;
            int vp_Mbase = vp_Ubase + c.MegaVPBase;

            // Calculo para spawn
            if (iSpawn)
            {
                switch (estage)
                {
                    case 3:
                        hp_base = hp_Cbase;
                        atk_base = atk_Cbase;
                        def_base = def_Cbase;
                        bl_base = bl_Cbase;
                        vp_base = vp_Cbase;
                        break;
                    case 4:
                        hp_base = hp_Ubase;
                        atk_base = atk_Ubase;
                        def_base = def_Ubase;
                        bl_base = bl_Ubase;
                        vp_base = vp_Ubase;
                        break;
                    case 5:
                        hp_base = hp_Mbase;
                        atk_base = atk_Mbase;
                        def_base = def_Mbase;
                        bl_base = bl_Mbase;
                        vp_base = vp_Mbase;
                        break;
                }
            }

            // Bônus de euipamentos
            int STRBonus = 0;
            int DEXBonus = 0;
            int CONBonus = 0;
            int INTBonus = 0;
            int AttackIncr = 0;
            int DefenseIncr = 0;
            int BLevelIncr = 0;
            int AttackBonus = 0;
            int DefenseBonus = 0;
            int BLevelBonus = 0;
            int MaxHPBonus = 0;
            int MaxHPIncr = 0;
            int MaxVPBonus = 0;
            if (!iSpawn && Tamer != null)
            {
                STRBonus = Tamer.STRBonus;
                DEXBonus = Tamer.DEXBonus;
                CONBonus = Tamer.CONBonus;
                INTBonus = Tamer.INTBonus;
                AttackIncr = Tamer.AttackIncr;
                DefenseIncr = Tamer.DefenseIncr;
                BLevelIncr = Tamer.BLevelIncr;
                AttackBonus = Tamer.AttackBonus;
                DefenseBonus = Tamer.DefenseBonus;
                BLevelBonus = Tamer.BLevelBonus;
                MaxHPBonus = Tamer.MaxHP;
                MaxHPIncr = Tamer.MaxHPIncr;
                MaxVPBonus = Tamer.MaxVP;
            }

            MaxHealth = hp_base + MaxHPIncr + Convert.ToInt32(Math.Round(((Constitution + CONBonus) * c.HPMaxConTx) + ((Strength + STRBonus) * c.HPMaxStrTx)));
            MaxHealth += MaxHealth * (MaxHPBonus / 100);
            Attack = atk_base + AttackIncr + Convert.ToInt32(Math.Round(((Strength + STRBonus) * c.AttackStrTx) + ((Dexterity + DEXBonus) * c.AttackDexTx)));
            Defence = def_base + DefenseIncr + Convert.ToInt32(Math.Round(((Constitution + CONBonus) * c.DEFConTx) + ((Intelligence + INTBonus) * c.DEFIntTx)));
            BattleLevel = bl_base + BLevelIncr + Convert.ToInt32(Math.Round(((Dexterity + DEXBonus) * c.BLDexTx) + ((Intelligence + INTBonus) * c.BLIntTx)));
            MaxVP = vp_base + Convert.ToInt32(Math.Round(((Intelligence + INTBonus) * c.VPIntTx) + ((Constitution + CONBonus) * c.VPConTx)));
            MaxVP += MaxVP * (MaxVPBonus / 100);
            MaxEV = c.EVMaxBase;
            for (int i = 1; i <= Level; i++)
                MaxEV = (int)(MaxEV + (i * c.EVMaxMult));
            MaxXP = Utils.XP.MaxForDigimonLevel(Level);

            // Champion
            C_MaxHealth = hp_Cbase + MaxHPIncr + Convert.ToInt32(Math.Round(((CConstitution + CONBonus) * c.HPMaxConTx) + ((CStrength + STRBonus) * c.HPMaxStrTx)));
            C_MaxHealth += C_MaxHealth * (MaxHPBonus / 100);
            CAttack = atk_Cbase + AttackIncr + Convert.ToInt32(Math.Round(((CStrength + STRBonus) * c.AttackStrTx) + ((CDexterity + DEXBonus) * c.AttackDexTx)));
            CDefence = def_Cbase + DefenseIncr + Convert.ToInt32(Math.Round(((CConstitution + CONBonus) * c.DEFConTx) + ((CIntelligence + INTBonus) * c.DEFIntTx)));
            CBattleLevel = bl_Cbase + BLevelIncr + Convert.ToInt32(Math.Round(((CDexterity + DEXBonus) * c.BLDexTx) + ((CIntelligence + INTBonus) * c.BLIntTx)));
            C_MaxVP = vp_Cbase + Convert.ToInt32(Math.Round(((CIntelligence + INTBonus) * c.VPIntTx) + ((CConstitution + CONBonus) * c.VPConTx)));
            C_MaxVP += C_MaxVP * (MaxVPBonus / 100);
            // Ultimate
            U_MaxHealth = hp_Ubase + MaxHPIncr + Convert.ToInt32(Math.Round(((UConstitution + CONBonus) * c.HPMaxConTx) + ((UStrength + STRBonus) * c.HPMaxStrTx)));
            U_MaxHealth += U_MaxHealth * (MaxHPBonus / 100);
            UAttack = atk_Ubase + AttackIncr + Convert.ToInt32(Math.Round(((UStrength + STRBonus) * c.AttackStrTx) + ((UDexterity + DEXBonus) * c.AttackDexTx)));
            UDefence = def_Ubase + DefenseIncr + Convert.ToInt32(Math.Round(((UConstitution + CONBonus) * c.DEFConTx) + ((UIntelligence + INTBonus) * c.DEFIntTx)));
            UBattleLevel = bl_Ubase + BLevelIncr + Convert.ToInt32(Math.Round(((UDexterity + DEXBonus) * c.BLDexTx) + ((UIntelligence + INTBonus) * c.BLIntTx)));
            U_MaxVP = vp_Ubase + Convert.ToInt32(Math.Round(((UIntelligence + INTBonus) * c.VPIntTx) + ((UConstitution + CONBonus) * c.VPConTx)));
            U_MaxVP += U_MaxVP * (MaxVPBonus / 100);
            // Mega
            M_MaxHealth = hp_Mbase + MaxHPIncr + Convert.ToInt32(Math.Round(((MConstitution + CONBonus) * c.HPMaxConTx) + ((MStrength + STRBonus) * c.HPMaxStrTx)));
            M_MaxHealth += M_MaxHealth * (MaxHPBonus / 100);
            MAttack = atk_Mbase + AttackIncr + Convert.ToInt32(Math.Round(((MStrength + STRBonus) * c.AttackStrTx) + ((MDexterity + DEXBonus) * c.AttackDexTx)));
            MDefence = def_Mbase + DefenseIncr + Convert.ToInt32(Math.Round(((MConstitution + CONBonus) * c.DEFConTx) + ((MIntelligence + INTBonus) * c.DEFIntTx)));
            MBattleLevel = bl_Mbase + BLevelIncr + Convert.ToInt32(Math.Round(((MDexterity + DEXBonus) * c.BLDexTx) + ((MIntelligence + INTBonus) * c.BLIntTx)));
            M_MaxVP = vp_Mbase + Convert.ToInt32(Math.Round(((MIntelligence + INTBonus) * c.VPIntTx) + ((MConstitution + CONBonus) * c.VPConTx)));
            M_MaxVP += M_MaxVP * (MaxVPBonus / 100);

            // Calculo para spawn
            if (iSpawn)
            {
                int HPBase = 116;
                int HPSub = 0;
                double HPTx = 6.5;
                switch (rank)
                {
                    case 1:
                        HPBase = c.SpawnHPBaseRank1;
                        HPSub = c.SpawnHPLvSubRank1;
                        HPTx = c.SpawnHPTxRank1;
                        break;
                    case 2:
                        HPBase = c.SpawnHPBaseRank2;
                        HPSub = c.SpawnHPLvSubRank2;
                        HPTx = c.SpawnHPTxRank2;
                        break;
                    case 3:
                        HPBase = c.SpawnHPBaseRank3;
                        HPSub = c.SpawnHPLvSubRank3;
                        HPTx = c.SpawnHPTxRank3;
                        break;
                    case 4:
                        HPBase = c.SpawnHPBaseRank4;
                        HPSub = c.SpawnHPLvSubRank4;
                        HPTx = c.SpawnHPTxRank4;
                        break;
                    case 5:
                        HPBase = c.SpawnHPBaseRank5;
                        HPSub = c.SpawnHPLvSubRank5;
                        HPTx = c.SpawnHPTxRank5;
                        break;
                    case 6:
                        HPBase = c.SpawnHPBaseRank6;
                        HPSub = c.SpawnHPLvSubRank6;
                        HPTx = c.SpawnHPTxRank6;
                        break;
                    case 7:
                        HPBase = c.SpawnHPBaseRank7;
                        HPSub = c.SpawnHPLvSubRank7;
                        HPTx = c.SpawnHPTxRank7;
                        break;
                }
                MaxHealth = (int)(HPBase + ((Level - HPSub) * HPTx));

                int ATKBase = c.SpawnAttackBase;
                double ATKTx = c.SpawnAttackTx;
                Attack = (int)(ATKBase + (Level * ATKTx));

                int DEFBase = c.SpawnDEFBase;
                double DEFTx = c.SpawnDEFTx;
                Defence = (int)(DEFBase + (Level * DEFTx));

                int BLBase = c.SpawnBLBase;
                double BLTx = c.SpawnBLTx;
                BattleLevel = (int)(BLBase + (Level * BLTx));

                C_MaxHealth = MaxHealth;
                CAttack = Attack;
                CDefence = Defence;
                CBattleLevel = BattleLevel;

                U_MaxHealth = MaxHealth;
                UAttack = Attack;
                UDefence = Defence;
                UBattleLevel = BattleLevel;

                M_MaxHealth = MaxHealth;
                MAttack = Attack;
                MDefence = Defence;
                MBattleLevel = BattleLevel;
            }

            // Rookie Original Values
            RStrength = Strength;
            RDexterity = Dexterity;
            RConstitution = Constitution;
            RIntelligence = Intelligence;
            Rtype = type;
            R_MaxHealth = MaxHealth;
            R_MaxVP = MaxVP;
            RAttack = Attack;
            RDefence = Defence;
            RBattleLevel = BattleLevel;
            Rskill1lvl = skill1lvl;
            Rskill2lvl = skill2lvl;

            // Digimon mantendo forma digivoluida
            switch (estage)
            {
                case 3:
                    MaxHealth = C_MaxHealth;
                    Attack = CAttack;
                    Defence = CDefence;
                    BattleLevel = CBattleLevel;
                    MaxVP = C_MaxVP;
                    break;
                case 4:
                    MaxHealth = U_MaxHealth;
                    Attack = UAttack;
                    Defence = UDefence;
                    BattleLevel = UBattleLevel;
                    MaxVP = U_MaxVP;
                    break;
                case 5:
                    MaxHealth = M_MaxHealth;
                    Attack = MAttack;
                    Defence = MDefence;
                    BattleLevel = MBattleLevel;
                    MaxVP = M_MaxVP;
                    break;
            }

            // Configuração de Rank
            if (iSpawn)
            {
                // Configuração Geral
                if (Emulator.Enviroment.RankConfig.ContainsKey(rank))
                {
                    RankConfig Config = Emulator.Enviroment.RankConfig[rank];

                    if (Config.FatorHP != 0) // FatorHP sobrepoe a porcentagem de HP
                        MaxHealth = hp_base + (Config.FatorHP * Level);
                    else if (Config.HPPerc != 0)
                        MaxHealth = MaxHealth * Config.HPPerc / 100;
                    if (Config.ATKPerc != 0)
                        Attack = Attack * Config.ATKPerc / 100;
                    if (Config.DEFPerc != 0)
                        Defence = Defence * Config.DEFPerc / 100;
                    if (Config.BLPerc != 0)
                        BattleLevel = BattleLevel * Config.BLPerc / 100;
                }
                // Configuração por nome
                if (Emulator.Enviroment.RankConfigName.ContainsKey(Name))
                {
                    RankConfig Config = Emulator.Enviroment.RankConfigName[Name];

                    if (Config.FatorHP != 0) // FatorHP sobrepoe a porcentagem de HP
                        MaxHealth = hp_base + (Config.FatorHP * Level);
                    else if (Config.HPPerc != 0)
                        MaxHealth = MaxHealth * Config.HPPerc / 100;
                    if (Config.ATKPerc != 0)
                        Attack = Attack * Config.ATKPerc / 100;
                    if (Config.DEFPerc != 0)
                        Defence = Defence * Config.DEFPerc / 100;
                    if (Config.BLPerc != 0)
                        BattleLevel = BattleLevel * Config.BLPerc / 100;
                }
                // Configuração por Spawn ID
                if (Emulator.Enviroment.RankConfigSpawnId.ContainsKey(Id))
                {
                    RankConfig Config = Emulator.Enviroment.RankConfigSpawnId[Id];

                    if (Config.FatorHP != 0) // FatorHP sobrepoe a porcentagem de HP
                        MaxHealth = hp_base + (Config.FatorHP * Level);
                    else if (Config.HPPerc != 0)
                        MaxHealth = MaxHealth * Config.HPPerc / 100;
                    if (Config.ATKPerc != 0)
                        Attack = Attack * Config.ATKPerc / 100;
                    if (Config.DEFPerc != 0)
                        Defence = Defence * Config.DEFPerc / 100;
                    if (Config.BLPerc != 0)
                        BattleLevel = BattleLevel * Config.BLPerc / 100;
                }
            }

            if (Health > MaxHealth || iSpawn) Health = MaxHealth;
            if (VP > MaxVP) VP = MaxVP;
            //if (C_Health > C_MaxHealth)
            C_Health = C_MaxHealth;
            //if (C_VP > C_MaxVP)
            C_VP = C_MaxVP;
            //if (U_Health > U_MaxHealth)
            U_Health = U_MaxHealth;
            //if (U_VP > U_MaxVP)
            U_VP = U_MaxVP;
            //if (M_Health > M_MaxHealth)
            M_Health = M_MaxHealth;
            //if (M_VP > M_MaxVP)
            M_VP = M_MaxVP;
            if (EV > MaxEV) EV = MaxEV;
        }

        // Função para resetar os atributos (pontos distribuidos)
        public void ResetStatus()
        {
            // Our custom set points.
            MyStrength = 0;
            MyDexterity = 0;
            MyConstitution = 0;
            //MyIntelligence = 0;
            MyPoints = 0;
            skill1lvl = 0;
            skill2lvl = 0;
            // Champion Stats
            MyCStrength = 0;
            MyCDexterity = 0;
            MyCConstitution = 0;
            MyCIntelligence = 0;
            MyCPoints = 0;
            Cskill1lvl = 0;
            Cskill2lvl = 0;
            // Ultimate Stats
            MyUStrength = 0;
            MyUDexterity = 0;
            MyUConstitution = 0;
            MyUIntelligence = 0;
            MyUPoints = 0;
            Uskill1lvl = 0;
            Uskill2lvl = 0;
            // Mega Stats
            MyMStrength = 0;
            MyMDexterity = 0;
            MyMConstitution = 0;
            MyMIntelligence = 0;
            MyMPoints = 0;
            Mskill1lvl = 0;
            Mskill2lvl = 0;

            MySkillPoints = 0;

            // Salvando no banco
            SaveAtributes();
            SaveSkills();
            Calculate();
        }

        // Função para inserir pontos em atributos
        public void AddPoint(byte fase, byte atribute)
        {
            CalcPoints();
            switch (fase)
            {
                case 1:
                case 2:
                    if (MyPoints > 0)
                    {
                        switch (atribute)
                        {
                            case 1:
                                MyStrength++;
                                break;
                            case 2:
                                MyDexterity++;
                                break;
                            case 3:
                                MyConstitution++;
                                break;
                            case 4:
                                MyIntelligence++;
                                break;
                        }
                    }
                    break;
                case 3:
                    if (MyCPoints > 0)
                    {
                        switch (atribute)
                        {
                            case 1:
                                MyCStrength++;
                                break;
                            case 2:
                                MyCDexterity++;
                                break;
                            case 3:
                                MyCConstitution++;
                                break;
                            case 4:
                                MyCIntelligence++;
                                break;
                        }
                    }
                    break;
                case 4:
                    if (MyUPoints > 0)
                    {
                        switch (atribute)
                        {
                            case 1:
                                MyUStrength++;
                                break;
                            case 2:
                                MyUDexterity++;
                                break;
                            case 3:
                                MyUConstitution++;
                                break;
                            case 4:
                                MyUIntelligence++;
                                break;
                        }
                    }
                    break;
                case 5:
                    if (MyMPoints > 0)
                    {
                        switch (atribute)
                        {
                            case 1:
                                MyMStrength++;
                                break;
                            case 2:
                                MyMDexterity++;
                                break;
                            case 3:
                                MyMConstitution++;
                                break;
                            case 4:
                                MyMIntelligence++;
                                break;
                        }
                    }
                    break;
            }
            SaveAtributes();
            Calculate();
        }

        // Função para adicionar pontos em Skill
        public void AddPointSkill(byte fase, byte skill)
        {
            CalSkillPoints();
            switch (fase)
            {
                case 1:
                case 2:
                    if (MySkillPoints > 0)
                    {
                        switch (skill)
                        {
                            case 1:
                                if (skill1lvl < 10)
                                    skill1lvl++;
                                break;
                            case 2:
                                if (skill2lvl < 10)
                                    skill2lvl++;
                                break;
                        }
                    }
                    break;
                case 3:
                    if (MySkillPoints > 0)
                    {
                        switch (skill)
                        {
                            case 1:
                                if (Cskill1lvl < 10)
                                    Cskill1lvl++;
                                break;
                            case 2:
                                if (Cskill2lvl < 10)
                                    Cskill2lvl++;
                                break;
                        }
                    }
                    break;
                case 4:
                    if (MySkillPoints > 0)
                    {
                        switch (skill)
                        {
                            case 1:
                                if (Uskill1lvl < 10)
                                    Uskill1lvl++;
                                break;
                            case 2:
                                if (Uskill2lvl < 10)
                                    Uskill2lvl++;
                                break;
                        }
                    }
                    break;
                case 5:
                    if (MySkillPoints > 0)
                    {
                        switch (skill)
                        {
                            case 1:
                                if (Mskill1lvl < 10)
                                    Mskill1lvl++;
                                break;
                            case 2:
                                if (Mskill2lvl < 10)
                                    Mskill2lvl++;
                                break;
                        }
                    }
                    break;
            }
            CalSkillPoints();
            SaveSkills();

            if (fase > 2 && estage == fase)
                Digivolver(estage);
        }

        // Função para adicionar experiência
        public void GainExp(long xp, double bit)
        {
            if (!iSpawn && Level < Utils.XP.MaxDigimonLevel())
            {
                // Pet
                if (Tamer != null && Tamer.Pet != 0 && Tamer.PetHP > 0)
                {
                    // Pichimon dá mais 25%
                    if (Tamer.Pet == 5)
                    {
                        xp += (xp * 125) / 100;
                        EV += 100;
                        bit += bit / 2;
                    }
                    else
                    {
                        xp *= 2;
                        if (Tamer.Pet == 2) EV += 100;
                        else if (Tamer.Pet == 4) EV += 100;
                        bit += bit / 2;
                    }

                    //FUNCAO QUE TIRA HP DO PET
                    Random r = new Random();
                    int chance = r.Next(100);
                    if (chance >= 0)
                    {
                        Tamer.AddPetHP(-1);
                    }
                }

                XP += xp;

                // Bits
                Tamer.GainBit((int)bit);

                Tamer.Client.Connection.Send(new Network.Packets.PACKET_BATTLE_XP(BattleId, BattleSufix, xp, bit));

                ExecuteExp();
                SaveExp();
            }
            else if (Level >= Utils.XP.MaxDigimonLevel())
            {
                //CASO O DIGIMON ESTEJA NO LEVEL MAX, ELE SÓ GANHARÁ OS BITS

                // Pet
                if (Tamer != null && Tamer.Pet != 0 && Tamer.PetHP > 0)
                {
                    // Pichimon dá mais 25%
                    if (Tamer.Pet == 5)
                    {
                        xp += (xp * 125) / 100;
                        EV += 100;
                        bit += bit / 2;
                    }
                    else
                    {
                        xp *= 2;
                        if (Tamer.Pet == 2) EV += 100;
                        else if (Tamer.Pet == 4) EV += 100;
                        bit += bit / 2;
                    }

                    //FUNCAO QUE TIRA HP DO PET
                    Random r = new Random();
                    int chance = r.Next(100);
                    if (chance >= 0)
                    {
                        Tamer.AddPetHP(-1);
                    }
                }

                // Bits
                Tamer.GainBit((int)bit);

                xp = 0;
                Tamer.Client.Connection.Send(new Network.Packets.PACKET_BATTLE_XP(BattleId, BattleSufix, xp, bit));
            }
        }
        // Auxiliar da função anterior. Se o Digimon digivolveu, devemos enviar o pacote de digievolução em batalha
        // para os Clients
        public void LevelUpDigivolve()
        {
            if (batalha != null)
                foreach (Client c in batalha.Clients)
                    if (c != null)
                        c.Connection.Send(new Network.Packets.PACKET_BATTLE_DIGIEVOLUTION(this));
        }
        public void ExecuteExp()
        {
            if (Level >= Utils.XP.MaxDigimonLevel())
            {
                Level = (ushort)Utils.XP.MaxDigimonLevel();
                XP = 0;
                MaxXP = 0;
                SaveExp();
            }
            else if (XP >= MaxXP)
            {
                // Level up
                Level++;
                XP = 0;
                Tamer.Client.Connection.Send(new Network.Packets.PACKET_BATTLE_LVLUP(this));
                // Digivolveu?
                bool digivolve = false;
                int fase = 2;
                switch (Level)
                {
                    case 11: // Rookie
                        estage = 2;
                        EvolutionQuant = 2;
                        Model = (ushort)RModel;
                        DigimonId = RookieForm;
                        Carregar(DigimonId);
                        skill1lvl = 1;
                        skill1 = Rskill1;
                        skill2 = Rskill2;
                        SaveField("skill1lvl", skill1lvl.ToString());
                        SaveDigimonId();
                        digivolve = true;
                        break;
                    case 21: // Champion
                        CarregarEvolutions();
                        EvolutionQuant = 3;
                        Model = (ushort)CModel;
                        digivolve = true;
                        fase = 0x14;
                        break;
                    case 31: // Ultimate
                        CarregarEvolutions();
                        EvolutionQuant = 4;
                        Model = (ushort)UModel;
                        digivolve = true;
                        fase = 0x15;
                        break;
                    case 41: // Mega
                        CarregarEvolutions();
                        if (MegaForm != 0)
                        {
                            EvolutionQuant = 5;
                            Model = (ushort)MModel;
                            digivolve = true;
                            fase = 0x16;
                        }
                        break;
                }
                SaveLvl();
                Calculate();
                Health = MaxHealth;
                AddHP(MaxHealth);
                VP = MaxVP;
                EV = MaxEV;
                SaveFloppy();
                if (digivolve)
                {
                    Digivolver(fase);
                    LevelUpDigivolve();
                }
                SaveExp();
            }
        }

        // Funções para adicionar HP, VP e EVP
        public void AddHP(int value)
        {
            Health += value;
            if (Health <= 0)
            {
                Health = 0;
                Digivolver(0);
            }
            if (Health > MaxHealth) Health = MaxHealth;

            switch (estage)
            {
                case 3:
                    C_Health = Health;
                    break;
                case 4:
                    U_Health = Health;
                    break;
                case 5:
                    M_Health = Health;
                    break;
            }

            if (!iSpawn)
                SaveFloppy();
        }

        public void setHP(int value)
        {
            Health = value;
            if (Health <= 0)
            {
                Health = 0;
                //Digivolver(0);
            }
            if (Health > MaxHealth) Health = MaxHealth;

            switch (estage)
            {
                case 3:
                    C_Health = Health;
                    break;
                case 4:
                    U_Health = Health;
                    break;
                case 5:
                    M_Health = Health;
                    break;
            }

            if (!iSpawn)
                SaveFloppy();
        }
        public void AddVP(int value)
        {
            VP += value;
            if (VP < 0) VP = 0;
            if (VP > MaxVP) VP = MaxVP;
            if (!iSpawn)
                SaveFloppy();

            switch (estage)
            {
                case 3:
                    C_VP = VP;
                    break;
                case 4:
                    U_VP = VP;
                    break;
                case 5:
                    M_VP = VP;
                    break;
            }
        }
        public void AddEVP(int value)
        {
            EV += value;
            if (EV < 0) EV = 0;
            if (EV > MaxEV) EV = MaxEV;
            if (!iSpawn)
                SaveFloppy();
        }

        // Função para incrementar Número de batalhas e vitórias
        public void IcrBattles(byte result)
        {
            if (Participou)
            {
                Battles++;
                if (result == 1) BattleWins++;
                SaveBattles();
            }
            Participou = false;
        }

        // Função para alterar o Nome
        public void ChangeName(string name)
        {
            Name = name;
            SaveName();
        }

        // Função para alterar alguma das formas deste Digimon
        public void NewDigievolution(byte stage, string form)
        {
            // Procurando o Id do digimon no banco
            VerificationResult result = Emulator.Enviroment.Database.Select<VerificationResult>("id"
                , "digimon", "WHERE nome=@nome", new Database.QueryParameters() { { "nome", form } });

            NewDigievolution(stage, result.Id);
        }
        public void NewDigievolution(byte stage, int EvoId)
        {
            // Nome do campo na tabela que será alterado
            string campo = "digimon_id";

            switch (stage)
            {
                default:
                    RookieForm = EvoId;
                    break;
                case 3:
                    ChampForm = EvoId;
                    campo = "champ";
                    break;
                case 4:
                    UltimForm = EvoId;
                    campo = "ultim";
                    break;
                case 5:
                    MegaForm = EvoId;
                    campo = "mega";
                    break;
            }

            // Salvando alteração
            Emulator.Enviroment.Database.Update("digimons", new Database.QueryParameters() { { campo, EvoId } }
            , "WHERE id=@id", new Database.QueryParameters() { { "id", Id } });

            // Atualizando localmente
            CarregarEvolutions();
            Tamer.CalcularEquips();
        }

        public void NewDigievolutionRookie(byte stage, int EvoId)
        {
            // Nome do campo na tabela que será alterado
            string campo = "rookie";

            switch (stage)
            {
                default:
                    RookieForm = EvoId;
                    break;
                case 3:
                    ChampForm = EvoId;
                    campo = "champ";
                    break;
                case 4:
                    UltimForm = EvoId;
                    campo = "ultim";
                    break;
                case 5:
                    MegaForm = EvoId;
                    campo = "mega";
                    break;
            }

            // Salvando alteração
            Emulator.Enviroment.Database.Update("digimons", new Database.QueryParameters() { { campo, EvoId } }
            , "WHERE id=@id", new Database.QueryParameters() { { "id", Id } });

            // Atualizando localmente
            CarregarEvolutions();
            Tamer.CalcularEquips();
        }

        public void NewDigievolutionForce(byte stage, int EvoId)
        {
            // Nome do campo na tabela que será alterado
            string campo = "digimon_id";

            switch (stage)
            {
                default:
                    RookieForm = EvoId;
                    // Salvando alteração
                    Emulator.Enviroment.Database.Update("digimons", new Database.QueryParameters() { { "rookie", EvoId } }
                    , "WHERE id=@id", new Database.QueryParameters() { { "id", Id } });
                    break;
                case 3:
                    ChampForm = EvoId;
                    campo = "champ";
                    break;
                case 4:
                    UltimForm = EvoId;
                    campo = "ultim";
                    break;
                case 5:
                    MegaForm = EvoId;
                    campo = "mega";
                    break;
            }

            // Salvando alteração
            Emulator.Enviroment.Database.Update("digimons", new Database.QueryParameters() { { campo, EvoId } }
            , "WHERE id=@id", new Database.QueryParameters() { { "id", Id } });

            // Atualizando localmente
            CarregarEvolutions();
            Tamer.CalcularEquips();
        }

        // Função para salvar o número de batalhas
        public void SaveBattles()
        {
            if (!iSpawn)
            {
                Emulator.Enviroment.Database.Update("digimons", new Database.QueryParameters() { { "battles", Battles }
                , { "wins", BattleWins } }
                , "WHERE id=@id", new Database.QueryParameters() { { "id", Id } });
            }
        }

        // Função para salvar Nome no banco
        public void SaveName()
        {
            if (!iSpawn)
            {
                Emulator.Enviroment.Database.Update("digimons", new Database.QueryParameters() { { "name", Name } }
                , "WHERE id=@id", new Database.QueryParameters() { { "id", Id } });
            }
        }

        // Função para salvar experiência no banco
        public void SaveExp()
        {
            if (!iSpawn)
            {
                Emulator.Enviroment.Database.Update("digimons", new Database.QueryParameters() { { "xp", XP } }
                , "WHERE id=@id", new Database.QueryParameters() { { "id", Id } });
            }
        }

        // Função para salvar Level no banco
        public void SaveLvl()
        {
            if (!iSpawn)
            {
                Emulator.Enviroment.Database.Update("digimons", new Database.QueryParameters() { { "level", Level } }
                , "WHERE id=@id", new Database.QueryParameters() { { "id", Id } });
            }
        }

        // Função para salvar DigimonID no banco
        public void SaveDigimonId()
        {
            if (!iSpawn)
            {
                Emulator.Enviroment.Database.Update("digimons", new Database.QueryParameters() {
                    { "digimon_id", DigimonId } }
                , "WHERE id=@id", new Database.QueryParameters() { { "id", Id } });
            }
        }

        // Função para salvar HP, VP e XP atual
        public void SaveFloppy()
        {
            if (!iSpawn)
            {
                Emulator.Enviroment.Database.Update("digimons", new Database.QueryParameters() { { "hp", Health }
                , { "vp", VP }, { "evp", EV } }
                , "WHERE id=@id", new Database.QueryParameters() { { "id", Id } });
            }
        }

        // Função para salvar os atributos no banco
        public void SaveAtributes()
        {
            if (!iSpawn)
            {
                Emulator.Enviroment.Database.Update("digimons", new Database.QueryParameters() {
                    { "strength", MyStrength }, { "dexterity", MyDexterity }, { "constitution", MyConstitution }
                    , { "intelligence", MyIntelligence}, { "c_strength", MyCStrength }, { "c_dexterity", MyCDexterity }
                    , { "c_constitution", MyCConstitution }, { "c_intelligence", MyCIntelligence }
                    , { "U_strength", MyUStrength}
                    , { "u_dexterity", MyUDexterity }, { "U_constitution", MyUConstitution }
                    , { "u_intelligence", MyUIntelligence }, { "m_strength", MyMStrength }, { "m_dexterity", MyMDexterity }
                    , { "m_constitution", MyMConstitution }, { "m_intelligence", MyMIntelligence }}
                , "WHERE id=@id", new Database.QueryParameters() { { "id", Id } });
            }
        }
        public void SaveSkills()
        {
            if (!iSpawn)
            {
                Emulator.Enviroment.Database.Update("digimons", new Database.QueryParameters() {
                    { "skill1lvl", skill1lvl }, { "skill2lvl", skill2lvl }, { "m_skill2lvl", Mskill2lvl }
                    , { "c_skill1lvl", Cskill1lvl }, { "c_skill2lvl", Cskill2lvl }, { "u_skill1lvl", Uskill1lvl }
                    , { "u_skill2lvl", Uskill2lvl }, { "m_skill1lvl", Mskill1lvl} }
                , "WHERE id=@id", new Database.QueryParameters() { { "id", Id } });
            }
        }

        // Função para salvar Slot no banco
        public void SaveSlot()
        {
            if (!iSpawn)
            {
                Emulator.Enviroment.Database.Update("digimons", new Database.QueryParameters() { { "slot", Slot }
                , { "digistore", Digistore } }
                , "WHERE id=@id", new Database.QueryParameters() { { "id", Id } });
            }
        }

        // Função para salvar algum campo na tabela
        public void SaveField(string field, string value)
        {
            if (!iSpawn)
            {
                Emulator.Enviroment.Database.Update("digimons", new Database.QueryParameters() { { field, value } }
                , "WHERE id=@id", new Database.QueryParameters() { { "id", Id } });
            }
        }

        // Função para deletar este Digimon no banco
        public void Delete()
        {
            if (!iSpawn)
            {
                Emulator.Enviroment.Database.Update("digimons", new Database.QueryParameters() {
                    { "is_deleted", true } }
                , "WHERE id=@id", new Database.QueryParameters() { { "id", Id } }
                , "deleted_at=CURRENT_TIMESTAMP()");
            }

        }

        // Função para atualizar as informações deste Digimon
        public void SendInfo()
        {
            if (Tamer != null && Tamer.Client != null)
                Tamer.Client.Connection.Send(new Network.Packets.PACKET_DIGIMON_ATT(this));
        }

        // Processamento
        public TimerPlus recTimer;

        // Temporizador que vai executar a restauração natural do Digimon (HP, VP e EVP regen)
        public void SetRecTimer()
        {
            if (batalha == null && !iSpawn)
            {
                // Create a timer with a two second interval.
                recTimer = new TimerPlus(10000); // 10000 = 10 segundos
                                                 // Hook up the Elapsed event for the timer. 
                recTimer.Elapsed += Regenerar;
                recTimer.AutoReset = true;
                recTimer.Enabled = true;
            }
        }
        private void Regenerar(Object source, ElapsedEventArgs e)
        {
            //TENTATIVA FRUSTA DE TENTAR FAZER FUNCIONAR ITENS DE TEMPO
            /*
            if (Tamer != null && Tamer.Client != null)
            {
                foreach (Item i in Tamer.Items)
                {
                    if (i != null)
                    {
                        if (i.ItemTag == 1)
                        {
                            i.ItemQuant--;
                            i.Save();
                        }
                    }
                    
                }
                Tamer.AtualizarInventario();
            }
            */

            //if(!Emulator.Enviroment.Teste && batalha == null && !iSpawn)
            if (batalha == null && !iSpawn)
            {

                bool have = false;
                if (Health > 0 && Health < MaxHealth)
                {
                    Health += MaxHealth / 100;
                    have = true;
                }
                if (Health >= MaxHealth)
                    Health = MaxHealth;

                if (Health > 0 && VP < MaxVP)
                {
                    VP += MaxVP / 100;
                    have = true;
                }
                if (VP >= MaxVP)
                    VP = MaxVP;

                if (Health > 0 && EV < MaxEV)
                {
                    EV += MaxEV / 100;
                    have = true;
                }
                if (EV >= MaxEV)
                    EV = MaxEV;

                if (have)
                {

                    Tamer.Client.Connection.Send(new Network.Packets.PACKET_DIGIMON_ATT(this));
                    SaveFloppy();
                }
                //Console.WriteLine("Recuperou");
                //Comandos.Send(Tamer.Client, "HP: " + Health + "/" + MaxHealth);
            }
        }

        // Usado em batalha
        public TimerPlus aTimer;
        public Stopwatch lastAct;
        public double TimeLeft = 0;
        public bool TP = false; // Barra amarela (TP) cheia?
        private bool Paused = false;
        public Batalha batalha = null; // Batalha onde o Digimon está envolvido
        public long BattleId { get; set; } // ID em Batalha
        public string BattleSufix { get; set; } // Sufixo do ID em batalha
        public Digimon alvo = null; // Alvo travado em batalha
        public byte nextAction = 0x0A; // Próxima ação a ser executada
        public byte atacando = 0;
        public byte atacado = 0;
        public bool execute = false;
        public bool restart = true;
        public int SpawnTP = 0; // Barra de TP de Spawn
        public int SpawnTPGain = 0; // Valor que a barra de TP do Spawn aumenta por segundo
        public bool iSpawn = false; // Este digimon é selvagem?
        public bool Participou = false;
        // Valores adicionais em atributos
        public int ExtraBLevel = 0;
        public int ExtraAttack = 0;
        public int ExtraDefense = 0;
        public int ExtraDamage = 0; // %
        public int HPAlvoLimite = 0;
        public bool drill = false;
        public bool onehitko = false;
        public bool whitewing = false;
        public bool cardrk = false;
        // Cards usados por este Digimon (Card Slash)
        // Isso será enviado no PACKET_BATTLE_EXECUTE_ACTION. Nele não é enviado o ID do item no banco,
        // mas o Item ID do codex, e a quantidade do item.
        public Item card1 = null;
        public Item card2 = null;
        public Item card3 = null;

        // Usado em batalha:
        // Startando temporizador, que vai processar um comando após conclusão do tempo
        // Temporizador de Digimon de Tamer
        public void startTp()
        {
            Participou = true;
            TP = false;
            if (aTimer != null) aTimer.Close();
            // Create a timer with a two second interval.
            aTimer = new TimerPlus(GetTp() + 250); // 1000 = 1 segundo
            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += TPBar;
            aTimer.AutoReset = false;
            aTimer.Enabled = true;
        }
        public void startTp(int value)
        {
            Participou = true;
            TP = false;
            if (aTimer != null) aTimer.Close();
            // Create a timer with a two second interval.
            aTimer = new TimerPlus(value); // 1000 = 1 segundo
            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += TPBar;
            aTimer.AutoReset = false;
            aTimer.Enabled = true;
        }
        // Função que aciona o temporizador do lastAction
        public void Act()
        {
            if (lastAct == null)
            {
                lastAct = new Stopwatch();
                lastAct.Start();
            }
            else
                lastAct.Restart();
        }
        public bool CheckAct()
        {
            return lastAct == null || lastAct.ElapsedMilliseconds >= GetTp() + 3000;
        }
        // Função para pausar o temporizador
        public void Pause()
        {
            if (!Paused)
            {
                TimeLeft = aTimer.Interval - aTimer.TimeLeft;
                if (TimeLeft <= 0) TimeLeft = 100;
                aTimer.Pause();
                aTimer.Close();
                Paused = true;
            }
        }
        // Função para resumir o temporizador
        public void Resume()
        {
            if (Paused)
            {
                if (TimeLeft > 0)
                    startTp((int)TimeLeft);
                aTimer.Resume();

                TimeLeft = 0;
                Paused = false;
            }

            // Se o EVP acabou, devo voltar a digievolução
            if (!iSpawn && EV <= 0 && estage > 2)
                BackDigivolve(false, false);
        }

        public void StopBattle()
        {
            if (aTimer != null)
                aTimer.Close();
            TP = false;
            batalha = null;
            alvo = null; // Alvo travado em batalha
            nextAction = 0x0A; // Próxima ação a ser executada
            atacando = 0;
            atacado = 0;
            execute = false;
            restart = true;
        }

        // Função que executa ação após barra de TP encher
        private void TPBar(Object source, ElapsedEventArgs e)
        {
            ExecuteTPBar();
        }
        private void ExecuteTPBar()
        {
            if (batalha != null && !batalha.inAction)
            {
                ExecuteExp();
                TP = true;
                if (execute)
                {
                    execute = false;
                    Executar();
                }
            }
            else
                startTp();
        }
        // Função que aumenta a barra de TP de Digimon Selvagem, e executa a ação após completa
        private void TPBarSpawn(Object source, ElapsedEventArgs e)
        {
            if (batalha != null)
            {
                if (SpawnTP >= 500)
                {
                    SpawnTP = 0;
                    Random r = new Random();
                    // Alvo aleatório
                    Digimon d = null;
                    int index = r.Next(batalha.EquipeA.Length);
                    for (int i = 0; i < 5; i++)
                    {
                        if (batalha.EquipeA[index] != null && d == null)
                            d = batalha.EquipeA[index];
                        else
                        {
                            index++;
                            if (index > 4) index = 0;
                        }
                    }
                    // Skill aleatória
                    nextAction = 10;
                    if (Level >= 6)
                        if (r.Next(100) < 40)
                            nextAction = 11;
                    if (Level >= 16)
                        if (r.Next(100) < 30)
                            nextAction = 12;

                    if (d != null)
                    {
                        alvo = d;
                        batalha.ListAction.Enqueue(this);
                        batalha.ExecuteinAction();
                    }
                }
                else if (!batalha.inAction)
                {
                    SpawnTP += SpawnTPGain;
                    if (!batalha.inAction)
                    {
                        foreach (Client c in batalha.Clients)
                        {
                            if (c != null)
                                c.Connection.Send(new Network.Packets.PACKET_BATTLE_TP(BattleId, BattleSufix, SpawnTP));
                        }
                    }
                }
            }
        }

        // Função que executa uma ação na batalha
        public void Executar()
        {
            // A execução só ocorre se a Barra de TP estiver cheia
            if (TP && batalha != null)
            {
                if (!batalha.ListAction.Contains(this)) // Se já estou na fila, não devo entrar novamente
                    batalha.ListAction.Enqueue(this);

                batalha.ExecuteinAction();
            }
        }

        // Função para retornar este Digimon para sua forma Rookie
        public void BackDigivolve(bool checkAura, bool heal)
        {
            // Primeiro, verificamos se o Tamer tem aura, e se a aura tem efeito para manter a digievolução
            if (Health > 0 && checkAura && Tamer != null)
            {
                // Pet
                if (Tamer.Pet == 5 && Tamer.PetHP > 0)
                {
                    if (heal)
                    {
                        AddHP(MaxHealth);
                        AddVP(MaxVP);
                        //Tamer.Client.Connection.Send(new Network.Packets.PACKET_DIGIMON_ATT(this));
                    }
                    return;
                }

                Item aura = Tamer.Items[(int)Enums.EquipSlots.aura - 1];
                if (aura != null && (aura.CheckEffect(61) || aura.CheckEffect(60)))
                {
                    if (heal)
                    {
                        AddHP(MaxHealth);
                        AddVP(MaxVP);
                        //Tamer.Client.Connection.Send(new Network.Packets.PACKET_DIGIMON_ATT(this));
                    }
                    return;
                }
            }

            if (!iSpawn && estage > 2)
            {
                Digivolver(0);
                Tamer.Client.Connection.Send(new Network.Packets.PACKET_BATTLE_DIGIEVOLUTION(this));
            }
        }

        // Função para digivolver este Digimon
        public void Digivolver(int fase)
        {
            if (Level >= 11)
            {
                int Model = RModel;
                double STR = RStrength;
                double DEX = RDexterity;
                double CON = RConstitution;
                double INT = RIntelligence;
                int type = Rtype;
                int MaxHealth = R_MaxHealth;
                int MaxVP = R_MaxVP;
                int Attack = RAttack;
                int Defence = RDefence;
                int BattleLevel = RBattleLevel;
                int skill1lvl = Rskill1lvl;
                int skill2lvl = Rskill2lvl;
                Skill skill1 = Rskill1;
                Skill skill2 = Rskill2;
                estage = 2;

                switch (fase)
                {
                    case 0x14:
                    case 3:
                        Model = CModel;
                        STR = CStrength;
                        DEX = CDexterity;
                        CON = CConstitution;
                        INT = CIntelligence;
                        type = Ctype;
                        MaxHealth = C_MaxHealth;
                        MaxVP = C_MaxVP;
                        Attack = CAttack;
                        Defence = CDefence;
                        BattleLevel = CBattleLevel;
                        skill1lvl = Cskill1lvl;
                        skill2lvl = Cskill2lvl;
                        skill1 = Cskill1;
                        skill2 = Cskill2;
                        estage = 3;
                        break;
                    case 0x15:
                    case 4:
                        Model = UModel;
                        STR = UStrength;
                        DEX = UDexterity;
                        CON = UConstitution;
                        INT = UIntelligence;
                        type = Utype;
                        MaxHealth = U_MaxHealth;
                        MaxVP = U_MaxVP;
                        Attack = UAttack;
                        Defence = UDefence;
                        BattleLevel = UBattleLevel;
                        skill1lvl = Uskill1lvl;
                        skill2lvl = Uskill2lvl;
                        skill1 = Uskill1;
                        skill2 = Uskill2;
                        estage = 4;
                        break;
                    case 0x16:
                    case 5:
                        Model = MModel;
                        STR = MStrength;
                        DEX = MDexterity;
                        CON = MConstitution;
                        INT = MIntelligence;
                        type = Mtype;
                        MaxHealth = M_MaxHealth;
                        MaxVP = M_MaxVP;
                        Attack = MAttack;
                        Defence = MDefence;
                        BattleLevel = MBattleLevel;
                        skill1lvl = Mskill1lvl;
                        skill2lvl = Mskill2lvl;
                        skill1 = Mskill1;
                        skill2 = Mskill2;
                        estage = 5;
                        break;
                }
                this.Model = (ushort)Model;
                Strength = STR;
                Dexterity = DEX;
                Constitution = CON;
                Intelligence = INT;
                this.type = type;
                this.MaxHealth = MaxHealth;
                if (estage > 2 && Level > 11 && fase != 0)
                    Health = MaxHealth;
                if (Health > MaxHealth) Health = MaxHealth;
                this.MaxVP = MaxVP;
                if (estage > 2 && Level > 11 && fase != 0)
                    VP = MaxVP;
                if (VP > MaxVP) VP = MaxVP;
                this.Attack = Attack;
                this.Defence = Defence;
                this.BattleLevel = BattleLevel;
                this.skill1lvl = skill1lvl;
                this.skill2lvl = skill2lvl;
                this.skill1 = skill1;
                this.skill2 = skill2;
            }

        }
        // Função para obter o tempo do preenchimento da barra amarela (TP)
        private int GetTp()
        {
            if (Level == 2) return 1700;
            if (Level == 3) return 1400;
            if (Level == 4) return 1300;
            if (Level >= 5 && Level <= 8) return 1200;
            if (Level >= 9 && Level <= 12) return 1100;
            if (Level >= 13 && Level <= 20) return 1000;
            if (Level >= 21 && Level <= 30) return 900;
            if (Level >= 31 && Level <= 50) return 800;
            if (Level >= 51 && Level <= 69) return 700;
            if (Level >= 70 && Level <= 99) return 600;
            if (Level >= 100) return 500;

            return 2500; // Padrão, Level 1
        }
        // Função para limpar variáveis de batalha
        public void Limpar()
        {
            atacando = 0;
            atacado = 0;
            ExtraBLevel = 0;
            ExtraAttack = 0;
            ExtraDefense = 0;
            ExtraDamage = 0;
            HPAlvoLimite = 0;
            card1 = null;
            card2 = null;
            card3 = null;
        }

        // Função para carregar informações básicas deste Digimon no banco
        public void Carregar(int d_id)
        {
            DigimonBaseResult r = Emulator.Enviroment.Database.Select<DigimonBaseResult>(
                "dg.skill1, dg.skill2, dg.str, dg.dex, dg.con, dg.inte, dg.estage, dg.tipo, dg.model"
                + ", dg.evolution_line, dg.nome AS OriName"
                , "digimon AS dg"
                , "WHERE dg.id=@id"
                , new Database.QueryParameters() { { "id", d_id } });

            switch (r.estage)
            {
                case 1:
                    skill1 = r.skill1;
                    skill2 = r.skill2;
                    statsLevel = r.statsLevel;
                    type = r.type;

                    DigimonELResult eLResult = Emulator.Enviroment.Database.Select<DigimonELResult>(
                        "i, r, c, u, m", "evolution_line", "WHERE id=@id", new Database.QueryParameters() {
                            { "id", r.EL} });

                    if (eLResult.Valid)
                    {
                        if (RookieForm == 0)
                            RookieForm = eLResult.fases[1];
                        if (ChampForm == 0)
                            ChampForm = eLResult.fases[2];
                        if (UltimForm == 0)
                            UltimForm = eLResult.fases[3];
                        if (MegaForm == 0)
                            MegaForm = eLResult.fases[4];
                        OriName = r.OriName;
                        if (!iSpawn)
                            Carregar(RookieForm);
                    }

                    break;
                case 2:
                    Rskill1 = r.skill1;
                    Rskill2 = r.skill2;
                    statsRLevel = r.statsLevel;
                    Rtype = r.type;
                    RModel = r.model;

                    /*
                    DigimonELResult eL = Emulator.Enviroment.Database.Select<DigimonELResult>(
                        "i, r, c, u, m", "evolution_line", "WHERE id=@id", new Database.QueryParameters() {
                            { "id", r.EL} });

                    if (eL.Valid)
                    {
                        if (RookieForm == 0)
                            RookieForm = eL.fases[1];
                        if (ChampForm == 0)
                            ChampForm = eL.fases[2];
                        if (UltimForm == 0)
                            UltimForm = eL.fases[3];
                        if (MegaForm == 0)
                            MegaForm = eL.fases[4];
                    }
                    */
                    break;
                case 3:
                    Cskill1 = r.skill1;
                    Cskill2 = r.skill2;
                    statsCLevel = r.statsLevel;
                    Ctype = r.type;
                    CModel = r.model;
                    COriName = r.OriName;
                    break;
                case 4:
                    Uskill1 = r.skill1;
                    Uskill2 = r.skill2;
                    statsULevel = r.statsLevel;
                    Utype = r.type;
                    UModel = r.model;
                    UOriName = r.OriName;
                    break;
                case 5:
                    Mskill1 = r.skill1;
                    Mskill2 = r.skill2;
                    statsMLevel = r.statsLevel;
                    Mtype = r.type;
                    MModel = r.model;
                    MOriName = r.OriName;
                    break;
            }
            if (iSpawn)
            {
                statsCLevel = r.statsLevel;
                statsULevel = r.statsLevel;
                statsMLevel = r.statsLevel;
                skill1 = r.skill1;
                skill2 = r.skill2;
                statsLevel = r.statsLevel;
                type = r.type;
                estage = r.estage;
                Calculate();
                Health = MaxHealth;
            }
        }

        // Function to load the basic information of the evolutionary line of this Digimon
        public void CarregarEvolutions()
        {
            Carregar(DigimonId);
            if (RookieForm != 0)
                Carregar(RookieForm);
            if (ChampForm != 0)
                Carregar(ChampForm);
            if (UltimForm != 0)
                Carregar(UltimForm);
            if (MegaForm != 0)
                Carregar(MegaForm);

            Calculate();
        }

        // Liberando recursos
        public void Close()
        {
            if (recTimer != null) recTimer.Close();
            Tamer = null;
            Dispose();
        }

        // Destrutor
        ~Digimon()
        {
            Debug.Print("Digimon {0} Destruido", Name);
        }
    }
}
