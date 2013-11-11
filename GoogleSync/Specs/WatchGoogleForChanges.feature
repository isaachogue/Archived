Feature: Sync Meetings From Google - Discover Google Calendar Events
	As the VNNOC Symphony Cloud Services
	I want to be notified when a managed space is scheduled for a meeting through google calendar
	So that I can maintain an accurate understanding of the the space's scheduled meetings

Scenario: Inform google calendar to notifiy Symphony of events for a managed space
	Given a google sync agent exists
	And it has the ability to sync one or more of symphony resource schedules between symphony and a google calendar
	And a managed space has been selected for synchronization
	When the user makes a request to synchronize their resource
	Then Symphony should notify the google calendar of the request
	And Symphony should recieve a notification from Google with an event state of 'sync' for the room

 