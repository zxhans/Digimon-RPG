using System;
using System.Collections.Generic;
using System.Globalization;

namespace Digimon_Project.Utils
{
    public static class Randomizer
    {
        public static void Randomize<T>(T[] items)
        {
            Random rand = new Random();

            // For each spot in the array, pick
            // a random item to swap into that spot.
            for (int i = 0; i < items.Length - 1; i++)
            {
                int j = rand.Next(i, items.Length);
                T temp = items[i];
                items[i] = items[j];
                items[j] = temp;
            }
        }
    }
}
