using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
