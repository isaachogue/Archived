using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Configuration;
using System.Net;

namespace AviSpl.Vnoc.Symphony.Services.Api
{
    public class SymphonyRepository : ISymphonyRepository
    {

        string _clientKey;
        string _frameworkUrl;
        string _name;

        public SymphonyRepository()
        {
            this._clientKey = ConfigurationManager.AppSettings[""];
        }

        public SpaceSyncPoint FindRoomSyncPoint(string ThirdPartyId, string enterpriseSystemName)
        {
            return null;
        }

        public SpaceSyncPoint FindRoomSyncPoint(Guid SpaceId)
        {
            return null;
        }

        public ConferenceSyncPoint FindConferenceSyncPoint(string ThirdPartyId, string enterpriseSystemName)
        {
            return null;
        }

        public object ContactRestfulUri(string Uri)
        {
            object result = null;
            using (HttpClient httpClient = new HttpClient())
            {
                HttpResponseMessage response = new HttpResponseMessage();
                try
                {
                    // in order to avoid the / to be seen as a delimiter, we need to replace it here
                    httpClient.DefaultRequestHeaders.Authorization =
                      new AuthenticationHeaderValue(
                        "Basic",
                        Convert.ToBase64String(
                            System.Text.ASCIIEncoding.ASCII.GetBytes(
                                string.Format("{0}:{1}", "ClientKey", this._clientKey))));

                    httpClient.GetAsync(_frameworkUrl + "/" + this._name + "/" + Uri);

                    result = response.Content.ReadAsStreamAsync(); ;
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    response.Dispose();
                }
            }
            return result;
        }
    }
}
