using System;

namespace Germadent.Common.Extensions
{
    public static class ObjectExtension
    {
        public static int ToInt(this object obj)
        {
            return int.Parse(obj.ToString());
        }

        public static int? ToIntOrNull(this object obj)
        {
            if (obj == null || obj == DBNull.Value)
                return null;

            return obj.ToInt();
        }

        public static decimal ToDecimal(this object obj)
        {
            if (obj == null || obj == DBNull.Value)
                return 0;

            return decimal.Parse(obj.ToString());
        }

        public static DateTime ToDateTime(this object obj)
        {
            return DateTime.Parse(obj.ToString());
        }

        public static bool ToBool(this object obj)
        {
            if (bool.TryParse(obj.ToString(), out var boolValue))
            {
                return boolValue;
            }

            return false;
        }

        public static Guid ToGuid(this object obj)
        {
            return Guid.Parse(obj.ToString());
        }

        public static string ToYesNo(this bool boolValue)
        {
            if (boolValue)
                return "Да";

            return "Нет";
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
