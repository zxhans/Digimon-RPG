using Digimon_Project.Game;
using System;
using System.Diagnostics;

namespace Digimon_Project.Utils
{
    public static class XP
    {
        public static long MaxForTamerLevel(ushort level)
        {
            Config c = Emulator.Enviroment.Config;
            return c.TamerMaxXPSoma + (c.TamerMaxXPMult * (level - c.TamerMaxXPSub));
        }

        public static long MaxForDigimonLevel(ushort level)
        {
            Config c = Emulator.Enviroment.Config;
            ushort lv = level;
            if (level >= 81 && level <= 100)
            {
                return (long)2000000000;
            }
            else if (level >= 101 && level <= 105)
            {
                //return (long) 3000000000; //3 trilhoes
                return (long)2147483640;
            }
            else if (level >= 106)
            {
                //return (long) 4000000000;   //4 trilhoes
                return (long)2147483640;
            }
            else
            {
                return (long)(c.DigimonMaxXPConst1 * Math.Pow(c.DigimonMaxXPConst2, lv - 1) * lv);
            }
        }

        public static long GainForDigimonLevel(ushort level, byte rank)
        {
            long result = 0;

            Config c = Emulator.Enviroment.Config;
            Debug.Print("ExpFator: {0}", c.ExpFator);
            result = (long)(c.DigimonXPGainConst1 * Math.Pow(c.DigimonXPGainConst2, level) * c.ExpFator);
            switch (rank)
            {
                case 1:
                    result = (long)(result * c.DigimonXPRank1);
                    break;
                case 2:
                    result = (long)(result * c.DigimonXPRank2);
                    break;
                case 3:
                    result = (long)(result * c.DigimonXPRank3);
                    break;
                case 4:
                    result = (long)(result * c.DigimonXPRank4);
                    break;
                case 5:
                    result = (long)(result * c.DigimonXPRank5);
                    break;
                case 6:
                    result = (long)(result * c.DigimonXPRank6);
                    break;
                case 7:
                    result = (long)(result * c.DigimonXPRank7);
                    break;
            }

            return result;
        }

        public static double BitForDigimonLevel(ushort level)
        {
            Config c = Emulator.Enviroment.Config;
            return level * c.DigimonBit;
        }

        public static int MaxDigimonLevel()
        {
            Config c = Emulator.Enviroment.Config;
            return c.MaxDigimonLevel;
        }

        public static int MaxTamerLevel()
        {
            Config c = Emulator.Enviroment.Config;
            return c.MaxTamerLevel;
        }
    }
}
