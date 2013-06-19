using System;
using System.Collections.Generic;
using System.Data;
using AutoMoq;
using AviSpl.Vnoc.Symphony.Services.Api;
using AviSpl.Vnoc.Symphony.Services.Sync;
using Iformata.Vnoc.Symphony.Enterprise.Data.InformationModel;
using Iformata.Vnoc.Symphony.Enterprise.Data.InformationModel.ServiceTypes;
using MailRepository;
using Moq;
using NUnit.Framework;
using Office.Framework.Excel;
using SabreExcelImport;
using TechTalk.SpecFlow;

namespace ServiceSampleSpecs.StepDefinitions
{
    [Binding]
    public class ServicePollingLogicFeatureSteps
    {
        private AutoMoqer mocker = new AutoMoqer();

        ISyncAgent _agent;
        MailMessage _message;
        IMailRepository _mailRepository;
        Dictionary<string, SchedulingResponse> _agentProcessingResponse;
        List<Conference> _meetings;
                
        [Given(@"I have a mail repository with a new message in the inbox")]
        public void WhenANewMessageIsAvailableInTheInbox()
        {
            MailMessage mailMessage = new MailMessage(string.Empty, string.Empty, null);

            mocker.GetMock<IMailRepository>()
                .Setup(messages => messages.GetUnreadMails("INBOX"))
                .Returns(new List<MailMessage> {mailMessage});

            _mailRepository = mocker.Resolve<IMailRepository>();
            var newMessages = _mailRepository.GetUnreadMails("INBOX");
            _message = newMessages[0];
        }
        
        [When(@"the message has an xls attachment")]
        public void WhenTheMessageHasAnXlsAttachment()
        {
            List<MailRepository.Attachment> attachments = new List<MailRepository.Attachment>();
            attachments.Add(new MailRepository.Attachment(@"Z:\Downloads\SPL\SabreExcelImport\ServiceSample\Samples\out.xls"));
            _message = new MailMessage(_message.From, _message.Body, attachments);
        }

        [When(@"the attachment is loaded into the Sabre Excel Importer")]
        public void WhenTheAttachmentIsLoadedIntoTheSabreExcelImporter()
        {
            ExcelAdapter ea = new ExcelAdapter();
            ea.Load(_message.Attachments[0].FolderLocation);
            DataSet _dsReport = ea.DataSource;
            MeetingReport report = new MeetingReport(_dsReport, new SymphonySyncApi(new SymphonyRepository()));
            _meetings = report.Meetings;
            report.Dispose();
            _agent = new SvmAgent(new SymphonyRepository());
        }

        [When(@"the message is from Sabre Virtual Meetings")]
        public void WhenTheMessageIsFromSabreVirtualMeetings()
        {
            _agent.EmailDomain = "sabrevm.com";
            _message = new MailMessage("somewhere@"+_agent.EmailDomain, _message.Body, _message.Attachments);
        }

        [When(@"the Symphony platform has (.*) with some of the new meetings")]
        public void WhenTheSymphonyPlatformHasWithSomeOfTheNewMeetings(int conflicts)
        {
            Dictionary<string, SchedulingResponse> results = GenerateResults(conflicts);
            string from = _agent.EmailDomain;

            mocker.GetMock<ISyncAgent>()
                .Setup(agent => agent.ProcessMeetings(_agent.EmailDomain, _meetings))
                .Returns(results);
            _agent = mocker.Resolve<ISyncAgent>();
            _agent.EmailDomain = from;
        }

        private Dictionary<string, SchedulingResponse> GenerateResults(int conflicts)
        {
            Dictionary<string, SchedulingResponse> results = new Dictionary<string, SchedulingResponse>();

            int meetingConflicts = (conflicts > _meetings.Count) ? _meetings.Count : conflicts;
            for (int i = 0; i < meetingConflicts; ++i)
            {
                SchedulingResponse response = CreateSchedulingResponse(true);
                results.Add(_meetings[i].ThirdPartyConferenceId, response);
            }

            for (int i = _meetings.Count - 1; i >= meetingConflicts; i--)
            {
                SchedulingResponse response = CreateSchedulingResponse(false);
                results.Add(_meetings[i].ThirdPartyConferenceId, response);
            }
            return results;
        }

        private SchedulingResponse CreateSchedulingResponse(bool isConflict)
        {
            SchedulingResponse response = new SchedulingResponse();
            response.IsError = !isConflict;
            response.ErrorType = (isConflict) ? SymphonyErrorType.ScheduleConflict : SymphonyErrorType.None;
            response.Error = (isConflict) ? "Schedule Conflict" : string.Empty;
            response.ConfirmationNumber = (isConflict) ? 0 : 1;
            return response;
        }

        [When(@"the Agent processes these")]
        public void WhenTheAgentProcessesThese()
        {
            _agentProcessingResponse = _agent.ProcessMeetings(_agent.EmailDomain, _meetings);
        }

        [Then(@"I should be able to view the new message count")]
        public void ThenIShouldBeAbleToViewTheNewMessageCount()
        {
            Assert.IsInstanceOf<int>(_mailRepository.GetUnreadMails("INBOX").Count);
        }
        
        [Then(@"the count should be greater than (.*)")]
        public void ThenTheCountShouldBeGreaterThan(int p0)
        {
            Assert.IsTrue(_mailRepository.GetUnreadMails("INBOX").Count > 0);
        }

        [Then(@"the Agent should have a confirmation number for each meeting without a conflict")]
        public void ThenTheAgentShouldHaveAConfirmationNumberForEachMeetingWithoutAConflict()
        {
            foreach (Conference meeting in _meetings)
            {
                if (_agentProcessingResponse.ContainsKey(meeting.ThirdPartyConferenceId))
                {
                    SchedulingResponse response = _agentProcessingResponse[meeting.ThirdPartyConferenceId];
                    bool shouldBeConflict = (response.ConfirmationNumber == 0);
                    Assert.AreEqual(response.IsError, shouldBeConflict);
                }
            }
        }
    }
}
