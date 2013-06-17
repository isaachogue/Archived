using System;
using TechTalk.SpecFlow;

namespace ServiceSampleSpecs.StepDefinitions
{
    [Binding]
    public class ServicePollingLogicFeatureSteps
    {
        [Given(@"I have a known IMAP email account")]
        public void GivenIHaveAKnownIMAPEmailAccount()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Given(@"the Sabre Excel Importer has meetings that were created")]
        public void GivenTheSabreExcelImporterHasMeetingsThatWereCreated()
        {
            ScenarioContext.Current.Pending();
        }
        
        [When(@"I connect to the account using known credentials")]
        public void WhenIConnectToTheAccountUsingKnownCredentials()
        {
            ScenarioContext.Current.Pending();
        }
        
        [When(@"a new message is available in the inbox")]
        public void WhenANewMessageIsAvailableInTheInbox()
        {
            ScenarioContext.Current.Pending();
        }
        
        [When(@"the message has an xls attachment")]
        public void WhenTheMessageHasAnXlsAttachment()
        {
            ScenarioContext.Current.Pending();
        }
        
        [When(@"the message is from Sabre Virtual Meetings")]
        public void WhenTheMessageIsFromSabreVirtualMeetings()
        {
            ScenarioContext.Current.Pending();
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
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"the count should be greater than (.*)")]
        public void ThenTheCountShouldBeGreaterThan(int p0)
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"the attachment should be loaded into the Sabre Excel Importer")]
        public void ThenTheAttachmentShouldBeLoadedIntoTheSabreExcelImporter()
        {
            ScenarioContext.Current.Pending();
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
