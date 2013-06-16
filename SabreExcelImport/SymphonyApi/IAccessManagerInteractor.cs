using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iformata.Vnoc.Symphony.Enterprise.Data.InformationModel.ServiceTypes;

namespace AviSpl.Vnoc.Symphony.Services.Api
{
    public interface IAccessManagerInteractor
    {
        SessionToken Login(Dictionary<string, string> credentials);
        SessionToken LoginWithCredentials(Credential credentials);
        bool Logout(string SessionId);
        Session Authenticate(string SessionId);
    }
}
