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

        public static Guid ToGuid(this object obj)
        {
            return Guid.Parse(obj.ToString());
        }

        public static object GetValueOrDbNull(this object obj)
        {
            if (obj == null)
                return DBNull.Value;

            if (obj.ToString().IsNullOrWhiteSpace())
                return DBNull.Value;

            return obj;
        }
    }
}
