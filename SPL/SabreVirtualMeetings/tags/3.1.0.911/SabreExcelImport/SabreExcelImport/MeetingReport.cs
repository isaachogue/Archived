using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using AviSpl.Vnoc.Symphony.Services.Api;
using Iformata.Vnoc.Symphony.Enterprise.Data.InformationModel.ServiceTypes;

namespace SabreExcelImport
{
    public class MeetingReport : SabreExcelImport.IMeetingReport ,IDisposable
    {
        private System.Data.DataSet _report;
        private ISymphonySyncApi _syncApi;
        private List<SvmMeeting> _meetings;
        private bool _disposed;

        public string IanaTimezoneName { get; set; }
        public string EnterpriseSystemName { get; set; }

        public MeetingReport(System.Data.DataSet report, ISymphonySyncApi syncApi)
        {
            if (syncApi == null || report == null)
            {
                throw new NullReferenceException();
            }
            this._syncApi = syncApi;
            this._report = report;
            this._meetings = TryLoadMeetings();
            this.EnterpriseSystemName = "SabreVM";
            this.IanaTimezoneName = "Europe/London";
        }

        public List<Conference> Meetings {
            get
            {
                return ConvertToConferences(_meetings);
            }
        }

        private List<Conference> ConvertToConferences(List<SvmMeeting> meetings)
        {
            Dictionary<string, Conference> conferences = new Dictionary<string, Conference>();
            for (int i = 0; i < meetings.Count; ++i)
            {
                if (!conferences.ContainsKey(meetings[i].Id))
                {
                    Conference c = ConvertConference(meetings[i]);
                    conferences.Add(meetings[i].Id, c);
                }

                Participant p = ConvertParticipant(meetings[i]);

                conferences[meetings[i].Id].Participants.Add(p);
            }
            return conferences.Values.ToList();
        }

        private Participant ConvertParticipant(SvmMeeting meeting)
        {
            Participant p = new Participant();
            p.Schedule = new ConferenceSchedule();
            p.Schedule.UtcSetup = meeting.StartTimeInGMT;
            p.Schedule.UtcStart = meeting.StartTimeInGMT;
            p.Schedule.UtcEnd = meeting.EndTimeInGMT;
            SpaceSyncPoint sp = this._syncApi.GetRoomSyncPoint(meeting.RoomId, this.EnterpriseSystemName);
            p.SpaceId = sp.SpaceId;
            p.ParticipantType = (p.SpaceId == Guid.Empty) ? Iformata.Vnoc.Symphony.Enterprise.Data.InformationModel.ParticipantType.UnprofiledSpace : Iformata.Vnoc.Symphony.Enterprise.Data.InformationModel.ParticipantType.ProfiledSpace;
            return p;
        }

        private Conference ConvertConference(SvmMeeting meeting)
        {
            Conference c = new Conference();
            c.AccountId = Guid.Empty;
            
            ConferenceSyncPoint csp = this._syncApi.GetConferenceSyncPoint(meeting.Id, this.EnterpriseSystemName );
            c.ConfirmationNumber = (csp == null) ? 0 : csp.ConfirmationNumber;

            c.CustomFormResponses = new List<FormFieldResponse>();
            c.IsPrivate = false;
            c.Notes = meeting.Id;
            c.ThirdPartyConferenceId = meeting.Id;
            c.Owner = meeting.HostEmail;
            c.Participants = new List<Participant>();
            c.Requestor = meeting.CreatorUserId;
            c.Schedule = new ConferenceSchedule();
            c.Schedule.UtcSetup = meeting.StartTimeInGMT;
            c.Schedule.UtcStart = meeting.StartTimeInGMT;
            c.Schedule.UtcEnd = meeting.EndTimeInGMT;
            c.Status = ConverStatus(meeting.Status, csp);
            c.Timezone = this.IanaTimezoneName;
            c.Title = meeting.Title;
            c.Type = Iformata.Vnoc.Symphony.Enterprise.Data.InformationModel.ConnectionType.MultiPoint;
            return c;
        }

        private Iformata.Vnoc.Symphony.Enterprise.Data.InformationModel.ScheduleStatus ConverStatus(string status, ConferenceSyncPoint csp)
        {
            Iformata.Vnoc.Symphony.Enterprise.Data.InformationModel.ScheduleStatus result;
            if (status.ToLower().Equals("cancelled"))
            {
                result = Iformata.Vnoc.Symphony.Enterprise.Data.InformationModel.ScheduleStatus.Cancelled;
            }
            else if (csp == null)
            {
                result = Iformata.Vnoc.Symphony.Enterprise.Data.InformationModel.ScheduleStatus.Scheduled;
            }
            else
            {
                result = Iformata.Vnoc.Symphony.Enterprise.Data.InformationModel.ScheduleStatus.Modified;
            }
            return result;
        }

        private List<SvmMeeting> TryLoadMeetings()
        {
            List<SvmMeeting> meetings;
            if (_report.Tables.Contains("Meeting Report"))
            {
                meetings = LoadMeetings(_report.Tables["Meeting Report"]);
            }
            else
            {
                throw new Exception("Meeting Report table not present in data set");
            }
            return meetings;
        }

        private List<SvmMeeting> LoadMeetings(DataTable meetingReport)
        {
            List<SvmMeeting> meetings = new List<SvmMeeting>();
            foreach (DataRow row in meetingReport.Rows)
            {
                SvmMeeting meeting = ReadMeetingRow(row);
                meetings.Add(meeting);
            }
            return meetings;
        }

        private SvmMeeting ReadMeetingRow(DataRow row)
        {
            SvmMeeting m = new SvmMeeting();
            m.Title = row["meetingtitle"].ToString();
            m.Id = row["meetingId"].ToString();
            m.Locator = row["meetingLocator"].ToString();
            m.Status = row["meetingStatus"].ToString();

            //Sample data will fail if UTC is included: 06/20/2013 04:30:00.000 UTC
            string startString = RemoveUtcString(row["meetingStartTimeInGMT"].ToString());
            DateTime start = DateTime.MinValue;
            DateTime.TryParse(startString, out start);
            m.StartTimeInGMT = new DateTime(start.Ticks, DateTimeKind.Utc);

            //Sample data will fail if UTC is included: 06/20/2013 04:30:00.000 UTC
            string endString = RemoveUtcString(row["meetingEndTimeInGMT"].ToString());
            DateTime end = DateTime.MinValue;
            DateTime.TryParse(endString, out end);
            m.EndTimeInGMT = new DateTime(end.Ticks, DateTimeKind.Utc);

            m.CompanyId = row["meetingCreatorCompanyId"].ToString();
            m.HostEmail = row["meetingHostEmail"].ToString();
            m.CreatorUserId = row["meetingCreatorUserId"].ToString();

            m.RoomId = row["roomId"].ToString();
            m.RoomName = row["roomName"].ToString();
            return m;
        }

        private string RemoveUtcString(string svmDateTime)
        {
            return svmDateTime.TrimEnd(new char[] { 'U', 'T', 'C' });
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_report != null)
                        _report.Dispose();
                }

                // Indicate that the instance has been disposed.
                _report = null;
                _disposed = true;
            }
        }
    }
}
