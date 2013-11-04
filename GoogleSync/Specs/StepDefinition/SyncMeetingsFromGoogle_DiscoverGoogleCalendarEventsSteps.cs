using System;
using GoogleSynch;
using TechTalk.SpecFlow;

namespace Specs.StepDefinition
{
    [Binding]
    public class SyncMeetingsFromGoogle_DiscoverGoogleCalendarEventsSteps
    {
        [Given(@"Symphony has a valid Google API key")]
        public void GivenSymphonyHasAValidGoogleAPIKey()
        {
            GoogleAgent _agent = new GoogleAgent();
            _agent.Authenticate("username", "password", "api_key");
        }
        
        [Given(@"a managed space has been selected for google sync")]
        public void GivenAManagedSpaceHasBeenSelectedForGoogleSync()
        {
            ScenarioContext.Current.Pending();
        }
        
        [When(@"Symphony makes a watch request for the space")]
        public void WhenSymphonyMakesAWatchRequestForTheSpace()
        {
            ScenarioContext.Current.Pending();
        }
        
        [When(@"the watch request includes an id property")]
        public void WhenTheWatchRequestIncludesAnIdProperty()
        {
            ScenarioContext.Current.Pending();
        }
        
        [When(@"the watch request includes a type property")]
        public void WhenTheWatchRequestIncludesATypeProperty()
        {
            ScenarioContext.Current.Pending();
        }
        
        [When(@"the watch request includes a Google API registered URI where requests should be provided")]
        public void WhenTheWatchRequestIncludesAGoogleAPIRegisteredURIWhereRequestsShouldBeProvided()
        {
            ScenarioContext.Current.Pending();
        }
        
        [When(@"the watch request includes a token property with the space GUID")]
        public void WhenTheWatchRequestIncludesATokenPropertyWithTheSpaceGUID()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"the result should be an HTTP (.*) OK status code")]
        public void ThenTheResultShouldBeAnHTTPOKStatusCode(int p0)
        {
            ScenarioContext.Current.Pending();
        }
    }
}
