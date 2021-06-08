using System;
using System.Collections.Generic;
using System.Text;

namespace QueryProcessor.Extensions
{
    public static class StringExtensions
    {
        public static bool IsEqualIgnoreCase(this string s1, string s2)
        {
            return s1.Equals(s2, StringComparison.OrdinalIgnoreCase);
        }
    }
}
