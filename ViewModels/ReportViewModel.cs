using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HyosungManagement.ViewModels
{
    public class ReportViewModel
    {
        public int ReportTypeIdentifier { get; }
        public string ReportNameKey { get; }

        public DateTime LogDate { get; set; }
        public Guid ReportID { get; set; }

        protected ReportViewModel(int typeID, string nameKey)
        {
            ReportTypeIdentifier = typeID;
            ReportNameKey = nameKey;
        }
    }
}
