using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Google.Apis.Calendar;
using Google.Apis.Calendar.v3;
using Google.Apis.Authentication;
using Google.Apis.Authentication.OAuth2;
using Google.Apis.Authentication.OAuth2.DotNetOpenAuth;
using DotNetOpenAuth.OAuth2;
using System.Diagnostics;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;

namespace consoleGoogleResearch
{
    class Program
    {
        public static void Main(string[] args)
        {
            // Register the authenticator. The Client ID and secret have to be copied from the API Access
            // tab on the Google APIs Console.
            var provider = new NativeApplicationClient(GoogleAuthenticationServer.Description);
            provider.ClientIdentifier = "84383943259-98qi37g6fubpvovgk51ckoab5op4g8t4.apps.googleusercontent.com";
            provider.ClientSecret = "Sa1SX05WXwmtd1hMAzFcJFw2";

            var auth = new OAuth2Authenticator<NativeApplicationClient>(provider, GetAuthentication);

            CalendarService service = new CalendarService(new BaseClientService.Initializer()
            {
                Authenticator = auth,
                GZipEnabled = false
            }); 
            
            Google.Apis.Calendar.v3.CalendarListResource.ListRequest clrq = service.CalendarList.List();
            var result = clrq.Execute();

            Console.WriteLine("Calendars: ");
            foreach (CalendarListEntry calendar in result.Items)
            {
                Console.WriteLine("{0}", calendar.Id);
                Console.WriteLine("\tAppointments:");
                Google.Apis.Calendar.v3.EventsResource.ListRequest elr = service.Events.List(calendar.Id);
                var events = elr.Execute();
                foreach (Event e in events.Items)
                {
                    Console.WriteLine("\t From: {0} To: {1} Description: {2}, Location: {3}, Id: {4}", e.Start.DateTime, e.End.DateTime, e.Summary, e.Location, e.Id);
                }
            }
            Console.ReadKey();
        }

        private static void WatchCalendar(CalendarService service, string id)
        {
            Channel c = new Channel();
            c.Address = "www.vnocsymphony.com/google/notifications";
            c.Id = Guid.NewGuid().ToString();
            c.Type = "web_hook";
            c.Token = Guid.NewGuid().ToString(); // space Id could go here
            c.ResourceUri = @"https://www.googleapis.com/calendar/v3/calendars/" + id + "/events";
            service.Events.Watch(c, id);
        }

        private static Events LoadCalendarEvent(CalendarService service, string calendarId, DateTime timeStamp )
        {
            service.Events.List(calendarId).UpdatedMin = timeStamp.ToShortTimeString();
            Google.Apis.Calendar.v3.EventsResource.ListRequest request = service.Events.List(calendarId);
            return request.Execute();
        }

        private static void ReceiveNotification()
        {
            //POST https://mydomain.com/notifications // Your receiving URL.
            //Content-Type: application/json; utf-8
            //Content-Length: 0
            //X-Goog-Channel-ID: 4ba78bf0-6a47-11e2-bcfd-0800200c9a66
            //X-Goog-Channel-Token: 398348u3tu83ut8uu38
            //X-Goog-Channel-Expiration: 1367869013915
            //X-Goog-Resource-ID:  ret08u3rv24htgh289g
            //X-Goog-Resource-URI: https://www.googleapis.com/calendar/v3/calendars/my_calendar@gmail.com/events
            //X-Goog-Resource-State:  exists
            //X-Goog-Message-Number: 10

            // Resolve the token to a spaceId
            // Invoke LoadCalendarEvent(service, my_calendar@gmail.com, timestamp)
        }

        private static IAuthorizationState GetAuthentication(NativeApplicationClient arg)
        {
            // Get the auth URL:
            //IAuthorizationState state = new AuthorizationState(new[] { CalendarService.Scopes.Calendar.ToString() });
            IAuthorizationState state = new AuthorizationState(new[] { "https://www.google.com/calendar/feeds" });
            state.Callback = new Uri(NativeApplicationClient.OutOfBandCallbackUrl);
            Uri authUri = arg.RequestUserAuthorization(state);

            // Request authorization from the user (by opening a browser window):
            Process.Start(authUri.ToString());
            Console.Write("  Authorization Code: ");
            string authCode = Console.ReadLine();

            // Retrieve the access token by using the authorization code:
            return arg.ProcessUserAuthorization(authCode, state);
        }
    }
}