using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AviSpl.Vnoc.Symphony.Services.Api
{
    public class ConferenceSyncPoint
    {
        public long ConfirmationNumber { get; set; }
        public string MD5 { get; set; }
        public string Origin { get; set; }
        public string ThirdPartyId { get; set; }
        public string ThirdPartyName { get; set; }
        public long UtcTimestamp { get; set; }
    }

}
