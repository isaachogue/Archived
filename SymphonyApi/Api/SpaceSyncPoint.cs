using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AviSpl.Vnoc.Symphony.Services.Api
{
    public class SpaceSyncPoint
    {
        public string Watermark { get; set; }
        public string ThirdPartyId { get; set; }
        public Guid SpaceId { get; set; }
        public string Name { get; set; }
        public string SubscriptionId { get; set; }
    }
}
