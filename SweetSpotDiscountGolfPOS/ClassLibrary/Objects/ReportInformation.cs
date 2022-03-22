using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SweetSpotDiscountGolfPOS.OB
{
    public class ReportInformation
    {
        public DateTime dtmStartDate { get; set; }
        public DateTime dtmEndDate { get; set; }
        public int intGroupTimeFrame { get; set; }
        public int intLocationID { get; set; }
        public string varLocationName { get; set; }
        public ReportInformation() { }
        public ReportInformation(DateTime startDate, DateTime endDate, int groupTimeFrame, int locationID, string locationName)
        {
            dtmStartDate = startDate;
            dtmEndDate = endDate;
            intGroupTimeFrame = groupTimeFrame;
            intLocationID = locationID;
            varLocationName = locationName;
        }
    }
}