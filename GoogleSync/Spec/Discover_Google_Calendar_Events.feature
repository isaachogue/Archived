Feature: Sync Meetings From Google - Discover Google Calendar Events
	As the VNNOC Symphony Cloud Services
	I want to be notified when a managed space is scheduled for a meeting through google calendar
	So that I can maintain an accurate understanding of the the space's scheduled meetings

Scenario: Inform google calendar to notifiy Symphony of events for a managed space
	Given Symphony has a valid Google API key
	And a managed space has been selected for google sync
	When Symphony makes a watch request for the space
	And the watch request includes an id property
	And the watch request includes a type property
	And the watch request includes a URI where requests should be provided
	And the watch request includes a token property with the space GUID
	Then the result should be an HTTP 200 OK status code
