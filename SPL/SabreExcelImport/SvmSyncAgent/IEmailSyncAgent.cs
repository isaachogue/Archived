using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AviSpl.Vnoc.Symphony.Services.Sync
{
    public interface IEmailSyncAgent : ISyncAgent
    {
        string EmailDomain { get; set; }
    }
}
