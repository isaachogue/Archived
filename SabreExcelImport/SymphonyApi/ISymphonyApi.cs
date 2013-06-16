using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Iformata.Vnoc.Symphony.Enterprise.Data.InformationModel.ServiceTypes;

namespace AviSpl.Vnoc.Symphony.Services.Api
{
    public interface ISymphonyApi
    {
        Guid Authenticate(string username, string password, string domain);
        SchedulingResponse SaveMeeting(Conference conference);
    }
}
