using System;
using GoogleSyncAgent;
using TechTalk.SpecFlow;

namespace Specs.StepDefinition
{
    [Binding]
    public class SyncMeetingsFromGoogle_DiscoverGoogleCalendarEventsSteps
    {
        [Given(@"a google sync agent exists")]
        public void GivenAGoogleSyncAgentExists()
        {
            IGoogleSyncAgent sut;
        }
        
        [Given(@"it has the ability to sync one or more of symphony resource schedules between symphony and a google calendar")]
        public void GivenItHasTheAbilityToSyncOneOrMoreOfSymphonyResourceSchedulesBetweenSymphonyAndAGoogleCalendar()
        {

            ScenarioContext.Current.Pending();
        }
    }
}
