using Digimon_Project.Game.Data;
using MySql.Data.MySqlClient;

namespace Digimon_Project.Database.Results
{
    public class ConfigResult : ISelectResult
    {
        public bool IsValid { get; private set; }
        public int TamerMaxXPSoma = 10, TamerMaxXPMult = 5, TamerMaxXPSub = 1;
        public int TamerXPGainMin = 26, TamerXPGainMax = 31;
        public int DigimonMaxXPConst1 = 24, DigimonXPGainConst1 = 11, DigimonBit = 20;
        public double DigimonMaxXPConst2 = 1.2, ExpFator = 1, DigimonXPGainConst2 = 1.1;
        public double DigimonXPRank1 = 1, DigimonXPRank2 = 1, DigimonXPRank3 = 1, DigimonXPRank4 = 1
            , DigimonXPRank5 = 1, DigimonXPRank6 = 1, DigimonXPRank7 = 1;
        public double GainXPBreak2Digimon = .25, GainXPBreak3Digimon = .33, GainXPBreak4Digimon = .37
            , GainXPBreak5Digimon = .4;
        public double DigimonXPRed4Level = .5, DigimonXPRed7Level = .2, DigimonXPRed9Level = 0;
        public double DigimonBitRed4Level = .5, DigimonBitRed7Level = .2, DigimonBitRed9Level = 0
            , EVMaxMult = 2;
        public int MaxDigimonLevel = 250, MaxTamerLevel = 500, EVMaxBase = 0;
        public int ITAttackBase = 130, ITHPBase = 75, ITDEFBase = 67, ITBLBase = 195, ITVPBase = 80
            , RokieHPBase = 530, RokieAttackBase = 345, RokieDEFBase = 160, RokieBLBase = 410, RokieVPBase = 150
            , ChampAttackBase = 20, ChampHPBase = 20, ChampDEFBase = 20, ChampBLBase = 20, ChampVPBase = 20
            , UltimAttackBase = 30, UltimHPBase = 30, UltimDEFBase = 30, UltimBLBase = 30, UltimVPBase = 30
            , MegaAttackBase = 0, MegaHPBase = 0, MegaDEFBase = 0, MegaBLBase = 0, MegaVPBase = 0;
        public double HPMaxConTx = 3, HPMaxStrTx = 1, AttackStrTx = 1, AttackDexTx = .5, DEFConTx = 1.5
            , DEFIntTx = .5, BLDexTx = 1.5, BLIntTx = 5, VPIntTx = 2, VPConTx = .5;
        public int SpawnHPBaseRank1 = 116, SpawnHPLvSubRank1 = 0
            , SpawnHPBaseRank2 = 116, SpawnHPLvSubRank2 = 0
            , SpawnHPBaseRank3 = 823, SpawnHPLvSubRank3 = 10
            , SpawnHPBaseRank4 = 823, SpawnHPLvSubRank4 = 10
            , SpawnHPBaseRank5 = 823, SpawnHPLvSubRank5 = 10
            , SpawnHPBaseRank6 = 823, SpawnHPLvSubRank6 = 10
            , SpawnHPBaseRank7 = 823, SpawnHPLvSubRank7 = 10
            , SpawnAttackBase = 100, SpawnDEFBase = 100, SpawnBLBase = 100;
        public double SpawnHPTxRank1 = 6.5
            , SpawnHPTxRank2 = 6.5
            , SpawnHPTxRank3 = 18
            , SpawnHPTxRank4 = 18
            , SpawnHPTxRank5 = 18
            , SpawnHPTxRank6 = 18
            , SpawnHPTxRank7 = 18
            , SpawnAttackTx = 1, SpawnDEFTx = 1, SpawnBLTx = 1
            , EVPGastoTx = 5, F2Tx = .08, F3Tx = .1
            , DanoTx = 1, DefTx = .8, DamageTx = 1
            , TypeDecrDamage = .9, TypeIncrDamage = 1.1
            , DanoArea2 = .5, DanoArea3 = .5, DanoArea4 = .5, DanoArea5 = .5;
        public bool Manutencao = false;

        public void OnExecuted(MySqlDataReader reader)
        {
            IsValid = reader.HasRows;
            if (IsValid)
            {
                while(reader.Read())
                    switch (reader.GetString("config"))
                    {
                        case "Manutencao":
                            int m = reader.GetInt32("valor");
                            if (m > 0)
                                Manutencao = true;
                            break;
                        case "DanoArea5":
                            DanoArea5 = reader.GetDouble("valor");
                            break;
                        case "DanoArea4":
                            DanoArea4 = reader.GetDouble("valor");
                            break;
                        case "DanoArea3":
                            DanoArea3 = reader.GetDouble("valor");
                            break;
                        case "DanoArea2":
                            DanoArea2 = reader.GetDouble("valor");
                            break;
                        case "TypeIncrDamage":
                            TypeIncrDamage = reader.GetDouble("valor");
                            break;
                        case "TypeDecrDamage":
                            TypeDecrDamage = reader.GetDouble("valor");
                            break;
                        case "DamageTx":
                            DamageTx = reader.GetDouble("valor");
                            break;
                        case "DefTx":
                            DefTx = reader.GetDouble("valor");
                            break;
                        case "DanoTx":
                            DanoTx = reader.GetDouble("valor");
                            break;
                        case "F3Tx":
                            F3Tx = reader.GetDouble("valor");
                            break;
                        case "F2Tx":
                            F2Tx = reader.GetDouble("valor");
                            break;
                        case "EVPGastoTx":
                            EVPGastoTx = reader.GetDouble("valor");
                            break;
                        case "SpawnBLTx":
                            SpawnBLTx = reader.GetDouble("valor");
                            break;
                        case "SpawnDEFTx":
                            SpawnDEFTx = reader.GetDouble("valor");
                            break;
                        case "SpawnAttackTx":
                            SpawnAttackTx = reader.GetDouble("valor");
                            break;
                        case "SpawnBLBase":
                            SpawnBLBase = reader.GetInt32("valor");
                            break;
                        case "SpawnDEFBase":
                            SpawnDEFBase = reader.GetInt32("valor");
                            break;
                        case "SpawnAttackBase":
                            SpawnAttackBase = reader.GetInt32("valor");
                            break;
                        case "SpawnHPBaseRank1":
                            SpawnHPBaseRank1 = reader.GetInt32("valor");
                            break;
                        case "SpawnHPLvSubRank1":
                            SpawnHPLvSubRank1 = reader.GetInt32("valor");
                            break;
                        case "SpawnHPTxRank1":
                            SpawnHPTxRank1 = reader.GetDouble("valor");
                            break;
                        case "SpawnHPBaseRank2":
                            SpawnHPBaseRank2 = reader.GetInt32("valor");
                            break;
                        case "SpawnHPLvSubRank2":
                            SpawnHPLvSubRank2 = reader.GetInt32("valor");
                            break;
                        case "SpawnHPTxRank2":
                            SpawnHPTxRank2 = reader.GetDouble("valor");
                            break;
                        case "SpawnHPBaseRank3":
                            SpawnHPBaseRank3 = reader.GetInt32("valor");
                            break;
                        case "SpawnHPLvSubRank3":
                            SpawnHPLvSubRank3 = reader.GetInt32("valor");
                            break;
                        case "SpawnHPTxRank3":
                            SpawnHPTxRank3 = reader.GetDouble("valor");
                            break;
                        case "SpawnHPBaseRank4":
                            SpawnHPBaseRank4 = reader.GetInt32("valor");
                            break;
                        case "SpawnHPLvSubRank4":
                            SpawnHPLvSubRank4 = reader.GetInt32("valor");
                            break;
                        case "SpawnHPTxRank4":
                            SpawnHPTxRank4 = reader.GetDouble("valor");
                            break;
                        case "SpawnHPBaseRank5":
                            SpawnHPBaseRank5 = reader.GetInt32("valor");
                            break;
                        case "SpawnHPLvSubRank5":
                            SpawnHPLvSubRank5 = reader.GetInt32("valor");
                            break;
                        case "SpawnHPTxRank5":
                            SpawnHPTxRank5 = reader.GetDouble("valor");
                            break;
                        case "SpawnHPBaseRank6":
                            SpawnHPBaseRank6 = reader.GetInt32("valor");
                            break;
                        case "SpawnHPLvSubRank6":
                            SpawnHPLvSubRank6 = reader.GetInt32("valor");
                            break;
                        case "SpawnHPTxRank6":
                            SpawnHPTxRank6 = reader.GetDouble("valor");
                            break;
                        case "SpawnHPBaseRank7":
                            SpawnHPBaseRank7 = reader.GetInt32("valor");
                            break;
                        case "SpawnHPLvSubRank7":
                            SpawnHPLvSubRank7 = reader.GetInt32("valor");
                            break;
                        case "SpawnHPTxRank7":
                            SpawnHPTxRank7 = reader.GetDouble("valor");
                            break;
                        case "VPConTx":
                            VPConTx = reader.GetDouble("valor");
                            break;
                        case "VPIntTx":
                            VPIntTx = reader.GetDouble("valor");
                            break;
                        case "BLIntTx":
                            BLIntTx = reader.GetDouble("valor");
                            break;
                        case "BLDexTx":
                            BLDexTx = reader.GetDouble("valor");
                            break;
                        case "DEFIntTx":
                            DEFIntTx = reader.GetDouble("valor");
                            break;
                        case "DEFConTx":
                            DEFConTx = reader.GetDouble("valor");
                            break;
                        case "AttackDexTx":
                            AttackDexTx = reader.GetDouble("valor");
                            break;
                        case "AttackStrTx":
                            AttackStrTx = reader.GetDouble("valor");
                            break;
                        case "HPMaxStrTx":
                            HPMaxStrTx = reader.GetDouble("valor");
                            break;
                        case "HPMaxConTx":
                            HPMaxConTx = reader.GetDouble("valor");
                            break;
                        case "MegaVPBase":
                            MegaVPBase = reader.GetInt32("valor");
                            break;
                        case "MegaBLBase":
                            MegaBLBase = reader.GetInt32("valor");
                            break;
                        case "MegaDEFBase":
                            MegaDEFBase = reader.GetInt32("valor");
                            break;
                        case "MegaHPBase":
                            MegaHPBase = reader.GetInt32("valor");
                            break;
                        case "MegaAttackBase":
                            MegaAttackBase = reader.GetInt32("valor");
                            break;
                        case "UltimVPBase":
                            UltimVPBase = reader.GetInt32("valor");
                            break;
                        case "UltimBLBase":
                            UltimBLBase = reader.GetInt32("valor");
                            break;
                        case "UltimDEFBase":
                            UltimDEFBase = reader.GetInt32("valor");
                            break;
                        case "UltimHPBase":
                            UltimHPBase = reader.GetInt32("valor");
                            break;
                        case "UltimAttackBase":
                            UltimAttackBase = reader.GetInt32("valor");
                            break;
                        case "ChampVPBase":
                            ChampVPBase = reader.GetInt32("valor");
                            break;
                        case "TamerMaxXPSoma":
                            TamerMaxXPSoma = reader.GetInt32("valor");
                            break;
                        case "TamerMaxXPMult":
                            TamerMaxXPMult = reader.GetInt32("valor");
                            break;
                        case "TamerMaxXPSub":
                            TamerMaxXPSub = reader.GetInt32("valor");
                            break;
                        case "TamerXPGainMin":
                            TamerXPGainMin = reader.GetInt32("valor");
                            break;
                        case "TamerXPGainMax":
                            TamerXPGainMax = reader.GetInt32("valor");
                            break;
                        case "DigimonMaxXPConst1":
                            DigimonMaxXPConst1 = reader.GetInt32("valor");
                            break;
                        case "DigimonMaxXPConst2":
                            DigimonMaxXPConst2 = reader.GetDouble("valor");
                            break;
                        case "ExpFator":
                            ExpFator = reader.GetDouble("valor");
                            break;
                        case "DigimonXPGainConst1":
                            DigimonXPGainConst1 = reader.GetInt32("valor");
                            break;
                        case "DigimonXPGainConst2":
                            DigimonXPGainConst2 = reader.GetDouble("valor");
                            break;
                        case "DigimonXPRank1":
                            DigimonXPRank1 = reader.GetDouble("valor");
                            break;
                        case "DigimonXPRank2":
                            DigimonXPRank2 = reader.GetDouble("valor");
                            break;
                        case "DigimonXPRank3":
                            DigimonXPRank3 = reader.GetDouble("valor");
                            break;
                        case "DigimonXPRank4":
                            DigimonXPRank4 = reader.GetDouble("valor");
                            break;
                        case "DigimonXPRank5":
                            DigimonXPRank5 = reader.GetDouble("valor");
                            break;
                        case "DigimonXPRank6":
                            DigimonXPRank6 = reader.GetDouble("valor");
                            break;
                        case "DigimonXPRank7":
                            DigimonXPRank7 = reader.GetDouble("valor");
                            break;
                        case "GainXPBreak2Digimon":
                            GainXPBreak2Digimon = reader.GetDouble("valor");
                            break;
                        case "GainXPBreak3Digimon":
                            GainXPBreak3Digimon = reader.GetDouble("valor");
                            break;
                        case "GainXPBreak4Digimon":
                            GainXPBreak4Digimon = reader.GetDouble("valor");
                            break;
                        case "GainXPBreak5Digimon":
                            GainXPBreak5Digimon = reader.GetDouble("valor");
                            break;
                        case "DigimonXPRed4Level":
                            DigimonXPRed4Level = reader.GetDouble("valor");
                            break;
                        case "DigimonXPRed7Level":
                            DigimonXPRed7Level = reader.GetDouble("valor");
                            break;
                        case "DigimonXPRed9Level":
                            DigimonXPRed9Level = reader.GetDouble("valor");
                            break;
                        case "DigimonBit":
                            DigimonBit = reader.GetInt32("valor");
                            break;
                        case "DigimonBitRed4Level":
                            DigimonBitRed4Level = reader.GetDouble("valor");
                            break;
                        case "DigimonBitRed7Level":
                            DigimonBitRed7Level = reader.GetDouble("valor");
                            break;
                        case "DigimonBitRed9Level":
                            DigimonBitRed9Level = reader.GetDouble("valor");
                            break;
                        case "MaxDigimonLevel":
                            MaxDigimonLevel = reader.GetInt32("valor");
                            break;
                        case "MaxTamerLevel":
                            MaxTamerLevel = reader.GetInt32("valor");
                            break;
                        case "EVMaxBase":
                            EVMaxBase = reader.GetInt32("valor");
                            break;
                        case "EVMaxMult":
                            EVMaxMult = reader.GetDouble("valor");
                            break;
                        case "ITAttackBase":
                            ITAttackBase = reader.GetInt32("valor");
                            break;
                        case "ITHPBase":
                            ITHPBase = reader.GetInt32("valor");
                            break;
                        case "ITDEFBase":
                            ITDEFBase = reader.GetInt32("valor");
                            break;
                        case "ITBLBase":
                            ITBLBase = reader.GetInt32("valor");
                            break;
                        case "ITVPBase":
                            ITVPBase = reader.GetInt32("valor");
                            break;
                        case "RokieHPBase":
                            RokieHPBase = reader.GetInt32("valor");
                            break;
                        case "RokieAttackBase":
                            RokieAttackBase = reader.GetInt32("valor");
                            break;
                        case "RokieDEFBase":
                            RokieDEFBase = reader.GetInt32("valor");
                            break;
                        case "RokieBLBase":
                            RokieBLBase = reader.GetInt32("valor");
                            break;
                        case "RokieVPBase":
                            RokieVPBase = reader.GetInt32("valor");
                            break;
                        case "ChampAttackBase":
                            ChampAttackBase = reader.GetInt32("valor");
                            break;
                        case "ChampHPBase":
                            ChampHPBase = reader.GetInt32("valor");
                            break;
                        case "ChampDEFBase":
                            ChampDEFBase = reader.GetInt32("valor");
                            break;
                        case "ChampBLBase":
                            ChampBLBase = reader.GetInt32("valor");
                            break;
                    }
            }
        }
    }
}
