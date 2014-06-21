using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SabreExcelImport
{
    public class SvmMeeting
    {
        public string Title { get; set; }
        public string Id { get; set; }
        public string Locator { get; set; }
        public string Status { get; set; }

        public DateTime StartTimeInGMT { get; set; }
        public DateTime EndTimeInGMT { get; set; }

        public string CompanyId { get; set; }
        public string HostEmail { get; set; }
        public string CreatorUserId { get; set; }

        public string RoomId { get; set; }
        public string RoomName { get; set; }
    }
}
