using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EnglishForCoachesWeb.Helpers
{
    public static class RandomHelper
    {
        private static Random rand = new Random();
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rand.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }


        public static int Random(int max)
        {
            return rand.Next(max);
        }



        public static char GetRandomLetter()
        {
            // This method returns a random lowercase letter.
            // ... Between 'a' and 'z' inclusize.
            int num = rand.Next(0, 26); // Zero to 25
            char let = (char)('a' + num);
            return let;
        }
    }
}