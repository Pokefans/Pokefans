using System;
using System.Collections.Generic;
using System.Linq;
using Pokefans.Data.UserData;

namespace Pokefans.Areas.mitarbeit.Models.Dashboard
{
    public class PrivateMessageReportDashboardViewModel
    {
        public List<PrivateMessageReport> Reports { get; set; }
        public Dictionary<DateTime, int> ReportsPerDay { get; set; }
        public int Open { get; set; }
    }
}