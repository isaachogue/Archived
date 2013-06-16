using System;
using System.Collections.Generic;
using System.Data;
using AviSpl.Vnoc.Symphony.Services.Api;
using Iformata.Vnoc.Symphony.Enterprise.Data.InformationModel.ServiceTypes;
using NUnit.Framework;
using Office.Framework.Excel;
using SabreExcelImport;
using Spec.Mocks;
using TechTalk.SpecFlow;

namespace Spec.StepDefinitions
{
    [Binding]
    public class SabreExcelImportStepDefinition : IDisposable
    {
        string _sample;

        MeetingReport _report;
        DataSet _dsReport;
        MockSymphonyApiRepository _repository = new MockSymphonyApiRepository();
        SymphonySyncApi _symphonyApi;
        ExcelAdapter _ea;
        List<Conference> _meetings;

        long _confirmationNumber;

        [Given(@"I have a SVM meeting")]
        public void GivenIHaveASVMMeeting()
        {
            _dsReport = new DataSet();
            DataTable _dtReport = new DataTable();
            _dtReport.Columns.Add("meetingTitle");
            _dtReport.Columns.Add("meetingId");
            _dtReport.Columns.Add("meetingLocator");
            _dtReport.Columns.Add("meetingStatus");
            _dtReport.Columns.Add("meetingStartTimeInGMT");
            _dtReport.Columns.Add("meetingEndTimeInGMT");
            _dtReport.Columns.Add("meetingCreatorCompanyId");
            _dtReport.Columns.Add("meetingHostName");
            _dtReport.Columns.Add("meetingHostEmail");
            _dtReport.Columns.Add("meetingHostPhoneNo");
            _dtReport.Columns.Add("meetingCreatorUserId");
            _dtReport.Columns.Add("roomId");
            _dtReport.Columns.Add("roomName");
            _dtReport.Columns.Add("roomIsActive");
            _dtReport.Columns.Add("roomLocationCity");
            _dtReport.Columns.Add("roomLocationStateCode");
            _dtReport.Columns.Add("roomLocationCountryCode");
            _dtReport.Columns.Add("roomParticipantsOnCamera");
            _dtReport.Columns.Add("roomIsPublic");
            _dtReport.Columns.Add("roomType");
            _dtReport.Columns.Add("roomPropertyName");
            _dtReport.Columns.Add("roomSupplierName");
            _dtReport.Columns.Add("roomPropertyMeetingContactName");
            _dtReport.Columns.Add("roomPropertyMeetingContactPhone");
            _dtReport.Columns.Add("roomPropertyMeetingContactEmail");
            _dtReport.Columns.Add("roomMeetingCost");
            _dtReport.Columns.Add("roomCurrencyCode");
            _dtReport.Columns.Add("bridgeNumber");
            _dtReport.Columns.Add("bridgeNoOfPorts");
            _dtReport.Columns.Add("bridgeIsActive");
            _dtReport.Columns.Add("bridgeIsPublic");
            _dtReport.Columns.Add("bridgeCompanyName");
            _dtReport.Columns.Add("bridgeDialInInstruction1");
            _dtReport.Columns.Add("bridgeNumberToDial1");
            _dtReport.Rows.Add();
            _dtReport.TableName = "Meeting Report";
            _dsReport.Tables.Add(_dtReport);
        }

        [When(@"I have a way to sync the meeting to Symphony")]
        public void WhenIHaveAWayToSyncTheMeetingToSymphony()
        {
            _symphonyApi = new SymphonySyncApi(_repository);
        }

        #region Scenario 1
        [Given(@"I have a SVM meeting report")]
        public void GivenIHaveASVMMeetingReport()
        {
            _sample = @"Z:\Downloads\SPL\SabreExcelImport\Spec\Samples\out.xls";
            _ea = new ExcelAdapter();
            _ea.Load(_sample);
            _dsReport = _ea.DataSource;
        }


        [When(@"the file is loaded into the Svm Meeting Importer")]
        public void WhenTheFileIsLoadedIntoTheSvmMeetingImporter()
        {
            _report = new MeetingReport(_dsReport, new SymphonySyncApi(_repository));
        }

        [Then(@"the meetings should be accessible from the system")]
        public void ThenTheMeetingsShouldBeAccessibleFromTheSystem()
        {
            _meetings = _report.Meetings;
            Assert.IsNotNull(_meetings);
        }

        [Then(@"the meetings should be Symphony Conferences")]
        public void ThenTheMeetingsShoulBeSymphonyConferences()
        {
            Assert.IsInstanceOf<List<Conference>>(_report.Meetings);
        }
        #endregion

        [When(@"the SVM meeting start time is '(.*)'")]
        public void WhenTheSVMMeetingStartTimeIs(string startTime)
        {
            _dsReport.Tables["Meeting Report"].Rows[0]["meetingStartTimeInGMT"] = startTime;
        }

        [When(@"the SVM meeting end time is '(.*)'")]
        public void WhenTheSVMMeetingEndTimeIs(string endTime)
        {
            _dsReport.Tables["Meeting Report"].Rows[0]["meetingEndTimeInGMT"] = endTime;
        }

        [When(@"the SVM meeting includes a SVM room with Id XYZ_Sabre_room")]
        public void WhenTheSVMMeetingIncludesASVMRoomWithIdXYZ_Sabre_Room()
        {
            SpaceSyncPoint ssp = new SpaceSyncPoint();
            ssp.ThirdPartyId = "XYZ_Sabre_room";
            ssp.SpaceId = Guid.NewGuid();

            _repository.AddSpaceSyncPoint(ssp);
            _dsReport.Tables["Meeting Report"].Rows[0]["roomId"] = ssp.ThirdPartyId;
        }

        [Then(@"the Symphony conference should have a setup time of '(.*)'")]
        public void ThenTheSymphonyConferenceShouldHaveASetupTimeOf(DateTime setupTime)
        {
            Assert.IsTrue(_meetings[0].Schedule.UtcSetup.Ticks == setupTime.Ticks);
        }

        [Then(@"the Symphony conference should have a start time of '(.*)'")]
        public void ThenTheSymphonyConferenceShouldHaveAStartTimeOf(DateTime startTime)
        {
            Assert.IsTrue(_meetings[0].Schedule.UtcStart.Ticks == startTime.Ticks);
        }

        [Then(@"the Symphony conference should have a end time of '(.*)'")]
        public void ThenTheSymphonyConferenceShouldHaveAEndTimeOf(DateTime endTime)
        {
            Assert.IsTrue(_meetings[0].Schedule.UtcEnd.Ticks == endTime.Ticks);
        }

        [Then(@"the Symphony conference should be represented as a UTC date")]
        public void ThenTheSymphonyConferenceShouldBeRepresentedAsAUTCDate()
        {
            Assert.IsTrue(_meetings[0].Schedule.UtcSetup.Kind == DateTimeKind.Utc);
        }

        [Then(@"the Symphony conference should use Europe/London as the timezone")]
        public void ThenTheSymphonyConferenceShouldUseAmericasLondonAsTheTimezone()
        {
            Assert.IsTrue(_meetings[0].Timezone == "Europe/London");
        }

        [Then(@"the Symphony conference should include a space with third party id XYZ_Sabre_room")]
        public void ThenTheSymphonyConferenceShouldIncludeASpaceWithThirdPartyIdXYZ_Sabre_Room()
        {
            List<Participant> participants = _meetings[0].Participants;
            SpaceSyncPoint ssp = _symphonyApi.GetRoomSyncPoint(participants[0].SpaceId);
            Assert.AreEqual(ssp.ThirdPartyId, "XYZ_Sabre_room");
        }

        [When(@"the SVM meeting title is '(.*)'")]
        public void WhenTheSVMMeetingTitleIs(string title)
        {
            _dsReport.Tables["Meeting Report"].Rows[0]["meetingTitle"] = title;
        }

        [Then(@"the Symphony conference should have a title of '(.*)'")]
        public void ThenTheSymphonyConferenceShouldHaveATitleOf(string title)
        {
            Assert.IsTrue(_meetings[0].Title.Equals(title));
        }

        [When(@"the SVM meeting status is '(.*)'")]
        public void WhenTheSVMMeetingStatusIs(string status)
        {
            _dsReport.Tables["Meeting Report"].Rows[0]["meetingStatus"] = status;
        }

        [When(@"the SVM meeting id '(.*)' within Symphony")]
        public void WhenTheSVMMeetingIdWithinSymphony(bool IsPresent)
        {
            if (IsPresent)
            {
                InjectConferenceSyncPoint(100);
            }
        }

        [When(@"the SVM meeting id is XYZLocator")]
        public void WhenTheSVMMeetingIdIsXYZLocator()
        {
            _dsReport.Tables["Meeting Report"].Rows[0]["meetingId"] = "XyzLocator";
        }

        [When(@"the svm conference has a sync point with a confimation number of (.*)")]
        public void WhenTheSvmConferenceHasASyncPointWithAConfimationNumberOf(string confirmationNumber)
        {
            int confNum = 0;
            int.TryParse(confirmationNumber, out confNum);
            InjectConferenceSyncPoint(confNum);
        }

        private void InjectConferenceSyncPoint(int confirmationNumber)
        {
            _confirmationNumber = confirmationNumber;
            ConferenceSyncPoint csp = new ConferenceSyncPoint();
            csp.ThirdPartyId = _dsReport.Tables["Meeting Report"].Rows[0]["meetingId"].ToString();
            csp.ConfirmationNumber = confirmationNumber;
            csp.ThirdPartyName = "SabreVM";
            _repository.AddConferenceSyncPoint(csp);
        }

        [Then(@"the Symphony conference should have a status of '(.*)'")]
        public void ThenTheSymphonyConferenceShouldHaveAStatusOf(string status)
        {
            List<Conference> meetings = _meetings;
            Assert.IsTrue(meetings[0].Status.ToString() == status);
        }

        [Then(@"the Symphony conference confirmation number should remain unchanged")]
        public void ThenTheSymphonyConferenceConfirmationNumberShouldRemainUnchanged()
        {
            Assert.IsTrue(_meetings[0].ConfirmationNumber == _confirmationNumber);
        }

        [Then(@"the cancelled meetings should be accessible from the system")]
        public void ThenTheCancelledMeetingsShouldBeAccessibleFromTheSystem()
        {
            _meetings = _report.Meetings.FindAll(m => m.Status == Iformata.Vnoc.Symphony.Enterprise.Data.InformationModel.ScheduleStatus.Cancelled);
            Assert.IsNotNull(_meetings);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_dsReport != null)
                {
                    _dsReport.Dispose();
                    _report.Dispose();
                }
            }

            // Indicate that the instance has been disposed.
            _dsReport = null;
            _report = null;
        }
    }
}
