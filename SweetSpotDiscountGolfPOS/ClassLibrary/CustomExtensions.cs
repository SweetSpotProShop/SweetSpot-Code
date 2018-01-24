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

        public static bool isNumber(this string val)
        {
            Double result;
            return double.TryParse(val, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.CurrentCulture, out result);
        }

        
        

    }
}