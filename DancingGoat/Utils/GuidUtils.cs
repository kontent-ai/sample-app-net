using System;

namespace DancingGoat.Utils
{
    public class GuidUtils
    {
        public static Guid FromShortString(string str)
        {
            return Guid.ParseExact(str, "N");
        }
    }
}