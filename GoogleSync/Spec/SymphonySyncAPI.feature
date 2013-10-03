Feature: Create a library that will provide sync services when integrating with the Symphony API
	As an external enterprise system
	I need a library-based API for interact with Symphony's web services
	So that I can sync conference and space data

Scenario Outline: Get a list space sync point types from the Symphony Repository service
	Given I have a set of <Room Count> rooms that are synchronized
	And I am a sync agent for <External System> 
	When I request the sync point from the Symphony Repository Service with <Id>
	Then the response should provide the third party sync point for the Id
	Examples: 
	| External System | Room Count | Id |
	| SabreVM         | 4          | 1  |
	| MSExchange      | 3          | 3  |
	| Google          | 2          | 2  |