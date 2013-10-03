Feature: Poll Symphony For Events
	As a sync agent for the Symphony Platform
	I want to poll the Symphony for events that have occurred since my last poll
	So that I can identify events that need to be synchronized

@mytag
Scenario: Poll For new events using valid token and locate none
	Given I have a Symphony API Control
	And I have a valid token
	When I request new events
	And there are none
	Then there should be no events returned

