using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AviSpl.Vnoc.Symphony.Services.Api
{
    public interface ISymphonyRepository
    {
        SpaceSyncPoint FindRoomSyncPoint(string ThirdPartyId, string enterpriseSystemName);

        SpaceSyncPoint FindRoomSyncPoint(Guid SpaceId);

        ConferenceSyncPoint FindConferenceSyncPoint(string ThirdPartyId, string enterpriseSystemName);
    }
}
