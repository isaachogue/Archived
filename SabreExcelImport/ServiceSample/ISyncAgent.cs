using System;
using System.Collections.Generic;
using Iformata.Vnoc.Symphony.Enterprise.Data.InformationModel.ServiceTypes;

namespace AviSpl.Vnoc.Symphony.Services.Sync
{
    public interface ISyncAgent
    {
        public string EmailDomain { get; set; }
        public Dictionary<string, SchedulingResponse> Results { get; }
        Dictionary<string, SchedulingResponse> ProcessMeetings(string from, List<Conference> meetings);
    }
}
