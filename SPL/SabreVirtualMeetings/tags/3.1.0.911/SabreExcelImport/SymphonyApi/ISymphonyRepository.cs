using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Iformata.Vnoc.Symphony.Enterprise.Data.InformationModel;
using Iformata.Vnoc.Symphony.Enterprise.Data.InformationModel.ServiceTypes;

namespace AviSpl.Vnoc.Symphony.Services.Api
{
    public interface ISymphonyRepository
    {
        Session Login(string username, string password, string domain);
        
        SpaceSyncPoint FindRoomSyncPoint(string ThirdPartyId, string enterpriseSystemName);
        SpaceSyncPoint FindRoomSyncPoint(Guid SpaceId);
        ConferenceSyncPoint FindConferenceSyncPoint(string ThirdPartyId, string enterpriseSystemName);

        SchedulingResponse SaveConference(Conference c);
        bool SetConferenceStatus(long confirmationNumber, ScheduleStatus status);
    }
}
