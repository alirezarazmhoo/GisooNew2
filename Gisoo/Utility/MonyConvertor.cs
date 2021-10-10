using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gisoo.Utility
{
    public static class MonyConvertor
    {
        public static string ToRial(this long mony)
        {
            return mony.ToString("#,00");
        }

        //public static string ToRial(this long? mony)
        //{
        //    string mony1 = mony.ToString();
        //    return mony1.ToString("#,00");
        //}
    }
}
