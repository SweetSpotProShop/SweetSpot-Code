﻿using System;
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
            double result;
            return double.TryParse(val, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.CurrentCulture, out result);
        }
        //Start and end of week
        public static DateTime GetWeekStart(this DateTime date)
        {           
            return date.AddDays(-Convert.ToInt32(date.DayOfWeek));
        }
        public static DateTime GetWeekEnd(this DateTime date)
        {
            return date.AddDays((-Convert.ToInt32(date.DayOfWeek)) + 6);
        }
        //Start and end of month
        public static DateTime GetMonthStart(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }
        public static DateTime GetMonthEnd(this DateTime date)
        {
            DateTime monthStart = new DateTime(date.Year, date.Month, 1);
            return monthStart.AddMonths(1).AddDays(-1);
        }
    }
}