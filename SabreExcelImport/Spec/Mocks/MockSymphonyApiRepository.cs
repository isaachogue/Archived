using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AviSpl.Vnoc.Symphony.Services.Api;
using Iformata.Vnoc.Symphony.Enterprise.Data.InformationModel.ServiceTypes;

namespace Spec.Mocks
{
    public class MockSymphonyApiRepository : ISymphonyRepository
    {
        private List<SpaceSyncPoint> _spaceSyncPoints;
        private List<ConferenceSyncPoint> _conferenceSyncPoints;

        public MockSymphonyApiRepository()
        {
            this._spaceSyncPoints = new List<SpaceSyncPoint>();
            this._conferenceSyncPoints = new List<ConferenceSyncPoint>();
        }

        public SpaceSyncPoint FindRoomSyncPoint(string ThirdPartyId, string enterpriseSystemName)
        {
            SpaceSyncPoint result = _spaceSyncPoints.Find(ssp => ssp.ThirdPartyId == ThirdPartyId);
            if (result == null)
            {
                result = new SpaceSyncPoint();
            }
            return result;
        }

        public SpaceSyncPoint FindRoomSyncPoint(Guid SpaceId)
        {
            return _spaceSyncPoints.Find(ssp => ssp.SpaceId == SpaceId);
        }


        public ConferenceSyncPoint FindConferenceSyncPoint(string ThirdPartyId, string enterpriseSystemName)
        {
            return _conferenceSyncPoints.Find(csp => csp.ThirdPartyId == ThirdPartyId && csp.ThirdPartyName == enterpriseSystemName);
        }

        public Guid Authenticate(string username, string password, string domain)
        {
            return Guid.NewGuid();
        }
        public void InitializeRooms(int SyncedRooms)
        {
            for (int i = 0; i < SyncedRooms; ++i)
            {
                SpaceSyncPoint room = CreateSyncPointFromInt(i);
                this._spaceSyncPoints.Add(room);
            }
        }

        private SpaceSyncPoint CreateSyncPointFromInt(int i)
        {
            SpaceSyncPoint ssp = new SpaceSyncPoint();
            ssp.ThirdPartyId = (i+1).ToString();
            ssp.SpaceId = Guid.NewGuid();
            ssp.SubscriptionId = string.Empty;
            ssp.Watermark = string.Empty;
            ssp.Name = "Test Room " + i + " for Sync Point";
            return ssp;
        }

        internal void AddSpaceSyncPoint(SpaceSyncPoint ssp)
        {
            SpaceSyncPoint existingSpace = FindRoomSyncPoint(ssp.ThirdPartyId, string.Empty);
            if (existingSpace.ThirdPartyId == null)
            {
                this._spaceSyncPoints.Add(ssp);
            }
        }

        internal void AddConferenceSyncPoint(ConferenceSyncPoint csp)
        {
            ConferenceSyncPoint existingConfSyncPoint = FindConferenceSyncPoint(csp.ThirdPartyId, csp.ThirdPartyName);
            if (existingConfSyncPoint == null)
            {
                this._conferenceSyncPoints.Add(csp);
            }
        }

        public SchedulingResponse SaveMeeting(Conference conference)
        {
            SchedulingResponse response = new SchedulingResponse();
            response.ConfirmationNumber = conference.ConfirmationNumber;
            if (response.ConfirmationNumber == 0)
            {
                response.ConfirmationNumber = 100;
            }
            response.Error = string.Empty;

            return response;
        }
    }
}
