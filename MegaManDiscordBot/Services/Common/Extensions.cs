using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegaManDiscordBot.Services.Common
{
    public static class Extensions
    {
        static Random rnd = new Random();

        public static T RandomItem<T>(this List<T> list)
        {
            return list[rnd.Next(list.Count)];
        }

    }
}
