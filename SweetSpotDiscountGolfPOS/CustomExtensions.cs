using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SweetSpotDiscountGolfPOS.ClassLibrary
{
    public static class CustomExtensions
    {
        public static string ToNullSafeString(this object obj)
        {
            return (obj ?? string.Empty).ToString();
        }
        public static bool IsNumeric(this string input)
        {
            int number;
            return int.TryParse(input, out number);
        }

    }
}