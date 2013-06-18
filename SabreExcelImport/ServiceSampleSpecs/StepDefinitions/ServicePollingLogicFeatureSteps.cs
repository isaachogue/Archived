using System.Collections.Generic;
using System.Data;
using AutoMoq;
using AviSpl.Vnoc.Symphony.Services.Api;
using Iformata.Vnoc.Symphony.Enterprise.Data.InformationModel.ServiceTypes;
using MailRepository;
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
        IMailRepository _sut;
        SymphonyRepository _repository;
        MeetingReport _meetings;
        
        [Given(@"the Sabre Excel Importer has meetings that were created")]
        public void GivenTheSabreExcelImporterHasMeetingsThatWereCreated()
        {
            mocker.GetMock<MeetingReport>()
                .Setup(mr => mr.Meetings)
                .Returns(CreateMeetings());
            ScenarioContext.Current.Pending();
        }

        private List<Conference> CreateMeetings()
        {
            return new List<Conference>();
        }
        
        [Given(@"I have a mail repository with a new message in the inbox")]
        public void WhenANewMessageIsAvailableInTheInbox()
        {
            MailMessage mailMessage = new MailMessage(string.Empty, string.Empty, null);

            mocker.GetMock<IMailRepository>()
                .Setup(messages => messages.GetUnreadMails("INBOX"))
                .Returns(new List<MailMessage> {mailMessage});

            _sut = mocker.Resolve<IMailRepository>();
            var newMessages = _sut.GetUnreadMails("INBOX");
            _message = newMessages[0];

        }
        
        [When(@"the message has an xls attachment")]
        public void WhenTheMessageHasAnXlsAttachment()
        {
            List<MailRepository.Attachment> attachments = new List<MailRepository.Attachment>();
            attachments.Add(new MailRepository.Attachment(@"Z:\Downloads\SPL\SabreExcelImport\ServiceSample\Samples\out.xls"));
            _message = new MailMessage(_message.From, _message.Body, attachments);
        }
        
        [When(@"the message is from Sabre Virtual Meetings")]
        public void WhenTheMessageIsFromSabreVirtualMeetings()
        {
            _agent = new SvmServiceAgent(new List<Conference>());
            _agent.EmailDomain = "sabrevm.com";
            _message = new MailMessage("somewhere@"+_agent.EmailDomain, _message.Body, _message.Attachments);
        }
        
        [When(@"the SVM Service Polling Agent Locates these")]
        public void WhenTheSVMServicePollingAgentLocatesThese()
        {
            _agent = new SvmServiceAgent(_meetings.Meetings);
        }

        [When(@"the Symphony platform has conflicts with some of the new meetings")]
        public void WhenTheSymphonyPlatformHasConflictsWithSomeOfTheNewMeetings()
        {
            mocker.GetMock<ISymphonyApi>()
                .Setup(api => api.SaveMeeting(_meetings.Meetings[0]))
                .Returns(new SchedulingResponse("Conflict", Iformata.Vnoc.Symphony.Enterprise.Data.InformationModel.SymphonyErrorType.ScheduleConflict));
        }

        [When(@"the Agent failes to load the file into the Sabre Excel Importer")]
        public void WhenTheAgentFailesToLoadTheFileIntoTheSabreExcelImporter()
        {
            ScenarioContext.Current.Pending();
        }

        [When(@"the Agent processes these")]
        public void WhenTheAgentProcessesThese()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"I should be able to view the new message count")]
        public void ThenIShouldBeAbleToViewTheNewMessageCount()
        {
            Assert.IsInstanceOf<int>(_sut.GetUnreadMails("INBOX").Count);
        }
        
        [Then(@"the count should be greater than (.*)")]
        public void ThenTheCountShouldBeGreaterThan(int p0)
        {
            Assert.IsTrue(_sut.GetUnreadMails("INBOX").Count > 0);
        }
        
        [Then(@"the attachment should be loaded into the Sabre Excel Importer")]
        public void ThenTheAttachmentShouldBeLoadedIntoTheSabreExcelImporter()
        {
            MeetingReport _report;
            ExcelAdapter ea = new ExcelAdapter();
            ea.Load(_message.Attachments[0].FolderLocation);
            DataSet _dsReport = ea.DataSource;
            _report = new MeetingReport(_dsReport, new SymphonySyncApi(new SymphonyRepository()));
            Assert.IsNotNull(_report.Meetings);
        }

        [Then(@"the Agent should have a confirmation number for each created meeting")]
        public void ThenTheAgentShouldHaveAConfirmationNumberForEachCreatedMeeting()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"the Agent should add these meetings into an issues dictionary")]
        public void ThenTheAgentShouldAddTheseMeetingsIntoAnIssuesDictionary()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"the Agent should add the file into an issues dictionary")]
        public void ThenTheAgentShouldAddTheFileIntoAnIssuesDictionary()
        {
            ScenarioContext.Current.Pending();
        }
        
    }
}
