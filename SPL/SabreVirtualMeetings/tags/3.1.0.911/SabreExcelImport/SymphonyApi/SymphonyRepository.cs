﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Configuration;
using System.Net;
using Iformata.Vnoc.Symphony.Enterprise.Data.InformationModel.ServiceTypes;
using Iformata.Vnoc.Symphony.Enterprise.Data.InformationModel;

namespace AviSpl.Vnoc.Symphony.Services.Api
{
    public class SymphonyRepository : ISymphonyRepository
    {

        string _clientKey;
        string _frameworkUrl;
        string _name;
        Session _session;

        public SymphonyRepository()
        {
            this._clientKey = ConfigurationManager.AppSettings[""];
            this._frameworkUrl = ConfigurationManager.AppSettings[""];
            this._name = ConfigurationManager.AppSettings[""];
        }

        public SpaceSyncPoint FindRoomSyncPoint(string ThirdPartyId, string enterpriseSystemName)
        {
            return new SpaceSyncPoint();
        }

        public SpaceSyncPoint FindRoomSyncPoint(Guid SpaceId)
        {
            return new SpaceSyncPoint();
        }

        public ConferenceSyncPoint FindConferenceSyncPoint(string ThirdPartyId, string enterpriseSystemName)
        {
            return new ConferenceSyncPoint();
        }

        public Session Login(string username, string password, string domain)
        {
            _session = new Session();
            return _session;
        }

        public SchedulingResponse SaveConference(Conference c)
        {
            return new SchedulingResponse();
        }

        public bool SetConferenceStatus(long confirmationNumber, ScheduleStatus status)
        {
            return false;
        }

        private object TryContactingRestfulUri(string Uri)
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
                    throw ex;
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
