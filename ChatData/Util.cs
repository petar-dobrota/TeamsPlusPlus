using System;
using System.Collections.Generic;
using System.Text;

namespace ChatData
{
    public static class Util
    {
        public static long Hash(string toHash)
        {
            long h = 1125899906842597L;
            int len = toHash.Length;

            for (int i = 0; i < len; i++)
            {
                h = 31 * h + toHash[i];
            }
            return h;
        }
    }
}
