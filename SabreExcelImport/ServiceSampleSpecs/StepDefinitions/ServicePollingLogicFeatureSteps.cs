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
        MeetingReport _importer;
                
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
            
            mocker.GetMock<ISymphonySyncApi>()
                .Setup(mock => mock.GetConferenceSyncPoint(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new ConferenceSyncPoint() { ConfirmationNumber = 1 });
            mocker.GetMock<ISymphonySyncApi>()
                .Setup(mock => mock.GetRoomSyncPoint(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new SpaceSyncPoint());
            _importer = new MeetingReport(_dsReport, mocker.Resolve<ISymphonySyncApi>());
        }

        [When(@"the Symphony platform has no conflicts for the new meetings")]
        public void WhenTheSymphonyPlatformHasNoConflictsForTheNewMeetings()
        {
            mocker.GetMock<ISymphonyRepository>()
                .Setup(mock => mock.SaveConference(It.IsAny<Conference>()))
                .Returns(new SchedulingResponse() {ConfirmationNumber = 1});
            mocker.GetMock<ISymphonyRepository>()
                .Setup(mock => mock.SetConferenceStatus(It.IsAny<long>(), It.IsAny<ScheduleStatus>()))
                .Returns(true);
            
            _agent = new SvmAgent(mocker.Resolve<ISymphonyRepository>());
        }

        [When(@"the Agent processes these")]
        public void WhenTheAgentProcessesThese()
        {
            _agentProcessingResponse = _agent.ProcessMeetings(string.Empty, _importer.Meetings);
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
            foreach (Conference meeting in _importer.Meetings)
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
