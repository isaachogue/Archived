using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Iformata.Vnoc.Symphony.Enterprise.Data.InformationModel;
using Iformata.Vnoc.Symphony.Enterprise.Data.InformationModel.ServiceTypes;

namespace AviSpl.Vnoc.Symphony.Services.Api
{
    public interface ISymphonyApi
    {
        Session Authentication { get; }
        Session Authenticate(string username, string password, string domain);
        bool LogOut();

        SchedulingResponse ProcessMeeting(Conference conference);
    }
}
