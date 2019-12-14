using System;

namespace Germadent.Common.Extensions
{
    public static class ObjectExtension
    {
        public static int ToInt(this object obj)
        {
            return int.Parse(obj.ToString());
        }

        public static DateTime ToDateTime(this object obj)
        {
            return DateTime.Parse(obj.ToString());
        }

        public static bool ToBool(this object obj)
        {
            return bool.Parse(obj.ToString());
        }
    }
}
