﻿using System.Collections.Generic;
using System.Linq;

namespace Germadent.Common.Extensions
{
    public static class StringExtensions
    {
        public static bool IsNullOrWhiteSpace(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        public static bool ContainsIgnoreCase(this IEnumerable<string> strings, string item)
        {
            if (item.IsNullOrWhiteSpace())
                return false;

            return strings.Select(x => x.ToLower()).Contains(item.ToLower());
        }
    }
}
