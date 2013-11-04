using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetOpenAuth.OAuth2;
using Google.Apis.Authentication.OAuth2;
using Google.Apis.Authentication.OAuth2.DotNetOpenAuth;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;

namespace GoogleSynch
{
    public class GoogleAgent 
    {

        public void Authenticate(string username, string password, string apiKey)
        {
            // Register the authenticator. The Client ID and secret have to be copied from the API Access
            // tab on the Google APIs Console.

            var provider = new NativeApplicationClient(GoogleAuthenticationServer.Description);
            provider.ClientIdentifier = "Google_API_Client_ID";
            provider.ClientSecret = "Sa1SX05WXwmtd1hMAzFcJFw2";

            var auth = new OAuth2Authenticator<NativeApplicationClient>(provider, GetAuthentication);

            // what is the best way to interact with this for testability and clean code
            // should a mock be used?

            CalendarService service = new CalendarService(new BaseClientService.Initializer()
            {
                Authenticator = auth,
                GZipEnabled = false
            });
        }

        private static IAuthorizationState GetAuthentication(NativeApplicationClient arg)
        {
            // Retrieve the access token by using the authorization code:
            return arg.ProcessUserAuthorization("AUTH_CODE");
        }
    }
}
