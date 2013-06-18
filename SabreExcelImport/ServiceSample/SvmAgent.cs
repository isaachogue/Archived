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
                SchedulingResponse response;
                if (meeting.Status == ScheduleStatus.Cancelled || meeting.Status == ScheduleStatus.Deleted)
                {
                    bool isSuccess = this._api.ProcessMeetingStatusChange(meeting.ConfirmationNumber, meeting.Status);
                    response = CreateResponse(isSuccess, meeting.ConfirmationNumber, meeting.Status);
                }
                else
                {
                    response = this._api.ProcessMeeting(meeting);
                }
                AddResponseToResults(meeting.ThirdPartyConferenceId, response);
            }
        }

        private SchedulingResponse CreateResponse(bool isSuccessful, long confirmationNumber, ScheduleStatus status)
        {
            SchedulingResponse response = new SchedulingResponse();
            if (isSuccessful)
            {
                response.Error = string.Empty;
                response.ErrorType = SymphonyErrorType.None;
            }
            else
            {
                response.ErrorType = SymphonyErrorType.IncompleteMeetingRequest;
                response.Error = "Failed to modify the meeting status to " + status.ToString();
            }
            return response;
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
