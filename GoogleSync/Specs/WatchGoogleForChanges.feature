Feature: Sync Meetings From Google - Discover Google Calendar Events
	As the VNNOC Symphony Cloud Services
	I want to be notified when a managed space is scheduled for a meeting through google calendar
	So that I can maintain an accurate understanding of the the space's scheduled meetings

Scenario: Configure a managed space so that when meetings are scheduled in Google's calendar it will notify Symphony
	Given a google sync agent exists
	And it has the ability to configure a managed space's google calendar for sync options
	And the google sync agent is configured with a URI location that google should use when notifying Symphony of new events for the managed room 
	When the google sync agent receives a request to configure a managed space's google calendar sync options
	And the google sync agent request includes a unique space identifier
	Then the google sync agent should create a google watch request
	And the google watch request should include an id property
	And the google watch request should include a type property with a value of web_hook
	And the google watch request should include a token property and the value should be the symphony space id

Scenario: Inform google calendar to notifiy Symphony of events for a managed space
	Given a google sync agent exists
	And it has the ability to sync one or more of symphony resource schedules between symphony and a google calendar
	And a managed space has been configured for synchronization
	When the sync agent receives a request to synchronize the managed space
	Then Symphony should notify the google calendar of the request
	And Symphony should recieve a notification from Google with an event state of 'sync' for the managed space

 Scenario: Google calendar informs Symphony of new watch request for a space
    Given a google calendar has successfully registered a watch request for a Symphony Space
    And a valid Symphony URI was provided as part of the watch requests
    Then the Symphony URI should recieve a google sync message
    And the message should have a google channel id value
    And the message should have a resource id value
    And the message should have a token property with the space GUID
    And the message should have a resource state of 'sync'
        
Scenario: Google calendar informs Symphony of new event for a space
    Given a google calendar has successfully registered a watch request for a Symphony Space
    And a valid Symphony URI was provided as part of the watch requests
    Then the Symphony URI should recieve a google sync message
    And the message should have a google channel id value
    And the message should have a resource id value
    And the message should have a token property with the space GUID
    And the message should have a resource state of 'exists'
    And the message should have a resource URI where events are located
    And the message should have a message number greater than 1
    And the result should be an HTTP 200 OK status code
