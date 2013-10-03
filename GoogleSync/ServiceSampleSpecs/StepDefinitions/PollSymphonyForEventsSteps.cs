using System;
using AviSpl.Vnoc.Symphony.Services.Api;
using Spec.Mocks;
using TechTalk.SpecFlow;

namespace ServiceSampleSpecs.StepDefinitions
{
    [Binding]
    public class PollSymphonyForEventsSteps
    {
        SymphonySyncApi _api;
        MockSymphonyApiRepository _repository;
        SpaceSyncPoint _roomSync;

        [Given(@"I have a Symphony API Control")]
        public void GivenIHaveASymphonyAPIControl()
        {
            _repository = new MockSymphonyApiRepository();
            _api = new SymphonySyncApi(_repository);
        }
        
        [Given(@"I have a valid token")]
        public void GivenIHaveAValidToken()
        {
            ScenarioContext.Current.Pending();
        }
        
        [When(@"I request new events")]
        public void WhenIRequestNewEvents()
        {
            ScenarioContext.Current.Pending();
        }
        
        [When(@"there are none")]
        public void WhenThereAreNone()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"there should be no events returned")]
        public void ThenThereShouldBeNoEventsReturned()
        {
            ScenarioContext.Current.Pending();
        }
    }
}
