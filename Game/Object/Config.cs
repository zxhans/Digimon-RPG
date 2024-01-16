using Digimon_Project.Database.Results;
using Digimon_Project.Game.Data;
using Digimon_Project.Game.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Timers;

namespace Digimon_Project.Game
{
    // Classe que armazena configurações
    public class Config
    {
        public int TamerMaxXPSoma = 1, TamerMaxXPMult = 1, TamerMaxXPSub = 1;
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
        public int MaxDigimonLevel = 130, MaxTamerLevel = 9999, EVMaxBase = 0;
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

        public Config()
        {
            Atualizar();
        }

        public void Atualizar()
        {
            Console.WriteLine("Loading Settings...");

            ConfigResult result = Emulator.Enviroment.Database.Select<ConfigResult>("*", "config");

            if (result.IsValid)
            {
                ExpFator = result.ExpFator;
                Manutencao = result.Manutencao;
                DanoArea2 = result.DanoArea2;
                DanoArea3 = result.DanoArea3;
                DanoArea4 = result.DanoArea4;
                DanoArea5 = result.DanoArea5;
                TypeIncrDamage = result.TypeIncrDamage;
                TypeDecrDamage = result.TypeDecrDamage;
                DamageTx = result.DamageTx;
                DefTx = result.DefTx;
                DanoTx = result.DanoTx;
                F3Tx = result.F3Tx;
                F2Tx = result.F2Tx;
                EVPGastoTx = result.EVPGastoTx;
                SpawnBLTx = result.SpawnBLTx;
                SpawnDEFTx = result.SpawnDEFTx;
                SpawnAttackTx = result.SpawnAttackTx;
                SpawnBLBase = result.SpawnBLBase;
                SpawnDEFBase = result.SpawnDEFBase;
                SpawnAttackBase = result.SpawnAttackBase;
                SpawnHPBaseRank1 = result.SpawnHPBaseRank1;
                SpawnHPLvSubRank1 = result.SpawnHPLvSubRank1;
                SpawnHPTxRank1 = result.SpawnHPTxRank1;
                SpawnHPBaseRank2 = result.SpawnHPBaseRank2;
                SpawnHPLvSubRank2 = result.SpawnHPLvSubRank2;
                SpawnHPTxRank2 = result.SpawnHPTxRank2;
                SpawnHPBaseRank3 = result.SpawnHPBaseRank3;
                SpawnHPLvSubRank3 = result.SpawnHPLvSubRank3;
                SpawnHPTxRank3 = result.SpawnHPTxRank3;
                SpawnHPBaseRank4 = result.SpawnHPBaseRank4;
                SpawnHPLvSubRank4 = result.SpawnHPLvSubRank4;
                SpawnHPTxRank4 = result.SpawnHPTxRank4;
                SpawnHPBaseRank5 = result.SpawnHPBaseRank5;
                SpawnHPLvSubRank5 = result.SpawnHPLvSubRank5;
                SpawnHPTxRank5 = result.SpawnHPTxRank5;
                SpawnHPBaseRank6 = result.SpawnHPBaseRank6;
                SpawnHPLvSubRank6 = result.SpawnHPLvSubRank6;
                SpawnHPTxRank6 = result.SpawnHPTxRank6;
                SpawnHPBaseRank7 = result.SpawnHPBaseRank7;
                SpawnHPLvSubRank7 = result.SpawnHPLvSubRank7;
                SpawnHPTxRank7 = result.SpawnHPTxRank7;
                VPConTx = result.VPConTx;
                VPIntTx = result.VPIntTx;
                BLIntTx = result.BLIntTx;
                BLDexTx = result.BLDexTx;
                DEFIntTx = result.DEFIntTx;
                DEFConTx = result.DEFConTx;
                AttackDexTx = result.AttackDexTx;
                AttackStrTx = result.AttackStrTx;
                HPMaxStrTx = result.HPMaxStrTx;
                HPMaxConTx = result.HPMaxConTx;
                MegaVPBase = result.MegaVPBase;
                MegaBLBase = result.MegaBLBase;
                MegaDEFBase = result.MegaDEFBase;
                MegaHPBase = result.MegaHPBase;
                MegaAttackBase = result.MegaAttackBase;
                UltimVPBase = result.UltimVPBase;
                UltimBLBase = result.UltimBLBase;
                UltimDEFBase = result.UltimDEFBase;
                UltimHPBase = result.UltimHPBase;
                UltimAttackBase = result.UltimAttackBase;
                ChampVPBase = result.ChampVPBase;
                ChampBLBase = result.ChampBLBase;
                TamerMaxXPSoma = result.TamerMaxXPSoma;
                TamerMaxXPMult = result.TamerMaxXPMult;
                TamerMaxXPSub = result.TamerMaxXPSub;
                TamerXPGainMin = result.TamerXPGainMin;
                TamerXPGainMax = result.TamerXPGainMax;
                DigimonMaxXPConst1 = result.DigimonMaxXPConst1;
                DigimonMaxXPConst2 = result.DigimonMaxXPConst2;
                DigimonXPGainConst1 = result.DigimonXPGainConst1;
                DigimonXPGainConst2 = result.DigimonXPGainConst2;
                DigimonXPRank1 = result.DigimonXPRank1;
                DigimonXPRank2 = result.DigimonXPRank2;
                DigimonXPRank3 = result.DigimonXPRank3;
                DigimonXPRank4 = result.DigimonXPRank4;
                DigimonXPRank5 = result.DigimonXPRank5;
                DigimonXPRank6 = result.DigimonXPRank6;
                DigimonXPRank7 = result.DigimonXPRank7;
                GainXPBreak2Digimon = result.GainXPBreak2Digimon;
                GainXPBreak3Digimon = result.GainXPBreak3Digimon;
                GainXPBreak4Digimon = result.GainXPBreak4Digimon;
                GainXPBreak5Digimon = result.GainXPBreak5Digimon;
                DigimonXPRed4Level = result.DigimonXPRed4Level;
                DigimonXPRed7Level = result.DigimonXPRed7Level;
                DigimonXPRed9Level = result.DigimonXPRed9Level;
                DigimonBit = result.DigimonBit;
                DigimonBitRed4Level = result.DigimonBitRed4Level;
                DigimonBitRed7Level = result.DigimonBitRed7Level;
                DigimonBitRed9Level = result.DigimonBitRed9Level;
                MaxDigimonLevel = result.MaxDigimonLevel;
                MaxTamerLevel = result.MaxTamerLevel;
                EVMaxBase = result.EVMaxBase;
                EVMaxMult = result.EVMaxMult;
                ITAttackBase = result.ITAttackBase;
                ITHPBase = result.ITHPBase;
                ITDEFBase = result.ITDEFBase;
                ITBLBase = result.ITBLBase;
                ITVPBase = result.ITVPBase;
                RokieHPBase = result.RokieHPBase;
                RokieAttackBase = result.RokieAttackBase;
                RokieDEFBase = result.RokieDEFBase;
                RokieBLBase = result.RokieBLBase;
                RokieVPBase = result.RokieVPBase;
                ChampAttackBase = result.ChampAttackBase;
                ChampHPBase = result.ChampHPBase;
                ChampDEFBase = result.ChampDEFBase;
            }

            Console.WriteLine("Configurations Loaded!");
        }
    }
}
