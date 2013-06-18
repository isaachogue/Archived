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
        ISymphonyRepository _repository;
        public SymphonySyncApi(ISymphonyRepository repository)
        {
            this._repository = repository;
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

        public Guid Authenticate(string username, string password, string domain)
        {
            throw new NotImplementedException();
        }

        public SchedulingResponse ProcessMeeting(Conference conference)
        {
            throw new NotImplementedException();
        }

        public bool ProcessMeetingStatusChange(long confirmationNumber, ScheduleStatus status)
        {
            throw new NotImplementedException();
        }

        public bool LogOut()
        {
            throw new NotImplementedException();
        }
    }
}
