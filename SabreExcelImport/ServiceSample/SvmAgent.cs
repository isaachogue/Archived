using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AviSpl.Vnoc.Symphony.Services.Api;
using Iformata.Vnoc.Symphony.Enterprise.Data.InformationModel;
using Iformata.Vnoc.Symphony.Enterprise.Data.InformationModel.ServiceTypes;
using SabreExcelImport;

namespace AviSpl.Vnoc.Symphony.Services.Sync
{
    public class SvmAgent : ISyncAgent
    {
        private ISymphonySyncApi _api;
        private Dictionary<string, SchedulingResponse> _processingResults;

        public string EmailDomain { get; set; }
        public Dictionary<string, SchedulingResponse> Results
        {
            get
            {
                if (this._processingResults == null)
                {
                    this._processingResults = new Dictionary<string, SchedulingResponse>();
                }
                return this._processingResults;
            }
        }

        public SvmAgent(ISymphonyRepository repository)
        {
            this._api = new SymphonySyncApi(repository);
        }

        public Dictionary<string, SchedulingResponse> ProcessMeetings(string from, List<Conference> meetings)
        {
            TryVerifyingEmailDomain(from);
            ProcessMeetingsIntoResults(meetings);
            return _processingResults;
        }

        private void TryVerifyingEmailDomain(string from)
        {
            if (EmailDomain != string.Empty)
            {
                if (from.Contains(EmailDomain) != true)
                {
                    throw new Exception("Invalid Sender: " + from);
                }
            }
        }

        private void ProcessMeetingsIntoResults(List<Conference> meetings)
        {
            foreach (Conference meeting in meetings)
            {
                SchedulingResponse response = this._api.ProcessMeeting(meeting);
                AddResponseToResults(meeting.ThirdPartyConferenceId, response);
            }
        }

        private void AddResponseToResults(string thirdPartyId, SchedulingResponse response)
        {
            if (Results.ContainsKey(thirdPartyId) == false)
            {
                Results.Add(thirdPartyId, response);
            }
        }
    }
}
