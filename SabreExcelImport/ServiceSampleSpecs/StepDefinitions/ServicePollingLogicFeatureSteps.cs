using System;
using System.Collections.Generic;
using System.Data;
using AutoMoq;
using AviSpl.Vnoc.Symphony.Services.Api;
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
        IMailRepository _sut;
        
        [Given(@"the Sabre Excel Importer has meetings that were created")]
        public void GivenTheSabreExcelImporterHasMeetingsThatWereCreated()
        {
            ScenarioContext.Current.Pending();
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
            List<Attachment> attachments = new List<Attachment>();
            attachments.Add(new Attachment(@"Z:\Downloads\SPL\SabreExcelImport\ServiceSample\Samples\out.xls"));
            _message = new MailMessage(_message.From, _message.Body, attachments);
        }
        
        [When(@"the message is from Sabre Virtual Meetings")]
        public void WhenTheMessageIsFromSabreVirtualMeetings()
        {
            _agent = new SvmServiceAgent();
            _agent.EmailDomain = "sabrevm.com";
            _message = new MailMessage("somewhere@"+_agent.EmailDomain, _message.Body, _message.Attachments);
        }
        
        [When(@"the SVM Service Polling Agent Locates these")]
        public void WhenTheSVMServicePollingAgentLocatesThese()
        {
            ScenarioContext.Current.Pending();
        }
        
        [When(@"the Agent sends each one to the Symphony Api")]
        public void WhenTheAgentSendsEachOneToTheSymphonyApi()
        {
            ScenarioContext.Current.Pending();
        }
        
        [When(@"the Symphony Api finds conflicts with some of the new meetings")]
        public void WhenTheSymphonyApiFindsConflictsWithSomeOfTheNewMeetings()
        {
            ScenarioContext.Current.Pending();
        }
        
        [When(@"the Agent failes to load the file into the Sabre Excel Importer")]
        public void WhenTheAgentFailesToLoadTheFileIntoTheSabreExcelImporter()
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
            ea.Load("");
            DataSet _dsReport = ea.DataSource;

            _report = new MeetingReport(_dsReport, new SymphonySyncApi(null));
        }
        
        [Then(@"the Api should return a confirmation number for each created meeting")]
        public void ThenTheApiShouldReturnAConfirmationNumberForEachCreatedMeeting()
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
