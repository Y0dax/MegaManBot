using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegaManDiscordBot.Services.Common
{
    public static class Extensions
    {
        public static T RandomItem<T>(this List<T> list)
        {
            return list[Globals.Random.Next(list.Count)];
        }

        public static T RandomItemLowerBias<T>(this List<T> list)
        {
            int result = (int)(Math.Floor(Math.Abs(Globals.Random.NextDouble() - Globals.Random.NextDouble()) * (1 + list.Count - 0) + 0));
            return list[result];
        }

        public static List<T> Shuffle<T>(this List<T> list)
        {
            return list.OrderBy(a => Guid.NewGuid()).ToList();
        }

    }
}
