using System;
using System.Collections.Generic;
using System.Data;
using AutoMoq;
using AviSpl.Vnoc.Symphony.Services.Api;
using AviSpl.Vnoc.Symphony.Services.Sync;
using Iformata.Vnoc.Symphony.Enterprise.Data.InformationModel.ServiceTypes;
using MailRepository;
using Moq;
using NUnit.Framework;
using Office.Framework.Excel;
using SabreExcelImport;
using ServiceSample;
using TechTalk.SpecFlow;

namespace ServiceSampleSpecs.StepDefinitions
{
    [Binding]
    public class ServicePollingLogicFeatureSteps
    {
        private AutoMoqer mocker = new AutoMoqer();

        SvmServiceAgent _agent;
        MailMessage _message;
        IMailRepository _mailRepository;
        ISymphonySyncApi _api;
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

        [When(@"the Symphony platform has conflicts with some of the new meetings")]
        public void WhenTheSymphonyPlatformHasConflictsWithSomeOfTheNewMeetings()
        {
            mocker.GetMock<ISyncAgent>()
                .Setup(agent => agent.Results)
                .Returns(new SchedulingResponse("Conflict", Iformata.Vnoc.Symphony.Enterprise.Data.InformationModel.SymphonyErrorType.ScheduleConflict));
            _api = mocker.Resolve<ISymphonySyncApi>();
        }

        [When(@"the Agent processes these")]
        public void WhenTheAgentProcessesThese()
        {
            _agentProcessingResponse = _agent.ProcessMeetingsByStatus(_meetings);
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

        [Then(@"the Agent should have a confirmation number for each created meeting")]
        public void ThenTheAgentShouldHaveAConfirmationNumberForEachCreatedMeeting()
        {
            foreach (Conference meeting in _meetings)
            {
                if (_agentProcessingResponse.ContainsKey(meeting.ThirdPartyConferenceId))
                {
                    Assert.IsTrue(_agentProcessingResponse[meeting.ThirdPartyConferenceId].ConfirmationNumber > 0);
                }
                else
                {
                    Assert.Fail("Third Party Id not located for meeting: " + meeting.ThirdPartyConferenceId + " " + meeting.Title);
                }
            }
        }
    }
}
