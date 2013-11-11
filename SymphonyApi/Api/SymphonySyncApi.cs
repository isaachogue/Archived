using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Iformata.Vnoc.Symphony.Enterprise.Data.InformationModel;
using Iformata.Vnoc.Symphony.Enterprise.Data.InformationModel.ServiceTypes;

namespace AviSpl.Vnoc.Symphony.Services.Api
{
    public class SymphonySyncApi : ISymphonySyncApi
    {

        private ISymphonyRepository _repository;
        private Session _session;

        public SymphonySyncApi(ISymphonyRepository repository)
        {
            this._repository = repository;
        }

        public Session Authentication
        {
            get
            {
                if (_session == null)
                {
                    _session = new Session();
                }
                return _session;
            }
        }

        public SpaceSyncPoint GetRoomSyncPoint(string ThirdPartyId, string enterpriseSystemName)
        {
            return _repository.FindRoomSyncPoint(ThirdPartyId, enterpriseSystemName);;
        }

        public SpaceSyncPoint GetRoomSyncPoint(Guid SpaceId)
        {
            return _repository.FindRoomSyncPoint(SpaceId);
        }

        public ConferenceSyncPoint GetConferenceSyncPoint(string ThirdPartyId, string enterpriseSystemName)
        {
            return _repository.FindConferenceSyncPoint(ThirdPartyId, enterpriseSystemName);
        }

        public Session Authenticate(string username, string password, string domain)
        {
            this._session = _repository.Login(username, password, domain);
            return _session;
        }

        public SchedulingResponse ProcessMeeting(Conference conference)
        {
            SchedulingResponse response;

            if (conference.Status == ScheduleStatus.Cancelled || conference.Status == ScheduleStatus.Deleted)
            {
                response = new SchedulingResponse();
                response.IsError = _repository.SetConferenceStatus(conference.ConfirmationNumber, conference.Status);
                response.Error = (response.IsError) ? "Failed to modify the meeting status to " + conference.Status.ToString() : string.Empty;
            }
            else
            {
                response = _repository.SaveConference(conference);
            }
            return response;
        }

        public bool LogOut()
        {
            throw new NotImplementedException();
        }
    }
}
