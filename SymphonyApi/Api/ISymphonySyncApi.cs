using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AviSpl.Vnoc.Symphony.Services.Api
{
    public interface ISymphonySyncApi : ISymphonyApi
    {
        SpaceSyncPoint GetRoomSyncPoint(string ThirdPartyId, string EnterpriseSystemName);
        SpaceSyncPoint GetRoomSyncPoint(Guid SpaceId);
        ConferenceSyncPoint GetConferenceSyncPoint(string ThirdPartyId, string EnterpriseSystemName);
    }
}
