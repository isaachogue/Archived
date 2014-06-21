Feature: SabreExcelImport
	As a user of the VNOC Symphony system 
	I want to transform the Sabre VM out file into Symphony Conferences
	So that I can import Sabre VM conferences into Symphony

 Scenario: Parse SVM Meeting Report into an a set of Symphony Conferences
	Given I have a SVM meeting report
	When the file is loaded into the Svm Meeting Importer
	Then the meetings should be accessible from the system 
	And the meetings should be Symphony Conferences

 Scenario Outline: Transform the SVM Meeting Schedule into a Symphony Conference Schedule
	Given I have a SVM meeting
	When the SVM meeting start time is <SVM_START_TIME>
	And the SVM meeting end time is <SVM_END_TIME>
	And the SVM meeting includes a SVM room with Id XYZ_Sabre_room
	And the file is loaded into the Svm Meeting Importer
	And I have a way to sync the meeting to Symphony
	Then the meetings should be accessible from the system 
	And the Symphony conference should have a setup time of <SETUP_TIME>
	And the Symphony conference should have a start time of <START_TIME> 
	And the Symphony conference should have a end time of <END_TIME>
	And the Symphony conference should be represented as a UTC date
	And the Symphony conference should use Europe/London as the timezone
	And the Symphony conference should include a space with third party id XYZ_Sabre_room

	Examples: 
	| SVM_START_TIME                | SVM_END_TIME                  | SETUP_TIME                | START_TIME                | END_TIME                  |
	| '06/20/2013 04:30:00.000 UTC' | '06/20/2013 05:30:00.000 UTC' | '06/20/2013 04:30:00.000' | '06/20/2013 04:30:00.000' | '06/20/2013 05:30:00.000' |

 Scenario Outline: Transform the SVM Meeting Title into a Symphony Conference Title
	Given I have a SVM meeting
	When the SVM meeting title is <SVM_TITLE>
	And the file is loaded into the Svm Meeting Importer
	And I have a way to sync the meeting to Symphony
	Then the meetings should be accessible from the system 
	And the Symphony conference should have a title of <TITLE>
	Examples: 
	| SVM_TITLE             | TITLE                 |
	| 'QTP_MYMEETINGS_AUTO' | 'QTP_MYMEETINGS_AUTO' |

 Scenario Outline: Transform the SVM Meeting Status into a Symphony Conference Status
	Given I have a SVM meeting
	When the SVM meeting status is <SVM_STATUS>
	And the SVM meeting id <EXISTS> within Symphony
	And the file is loaded into the Svm Meeting Importer
	And I have a way to sync the meeting to Symphony
	Then the meetings should be accessible from the system 
	And the Symphony conference should have a status of <STATUS>
	Examples: 
	| SVM_STATUS  | EXISTS  | STATUS      |
	| 'Active'    | 'false' | 'Scheduled' |
	| 'Active'    | 'true'  | 'Modified'  |
	| 'Cancelled' | 'true'  | 'Cancelled' |
	| 'Cancelled' | 'false' | 'Cancelled' |
	| 'Requested' | 'true'  | 'Modified'  |
	| 'Requested' | 'false' | 'Scheduled' |

Scenario Outline: Transform the SVM meeting into a Symphony Conference while maintaining existing confirmation number
	Given I have a SVM meeting
	When the SVM meeting id is XYZLocator
	And the svm conference has a sync point with a confimation number of <CONFIRMAION NUMBER>
	And the file is loaded into the Svm Meeting Importer
	And I have a way to sync the meeting to Symphony
	Then the meetings should be accessible from the system 
	And the Symphony conference confirmation number should remain unchanged
	Examples: 
	| CONFIRMATION NUMBER |
	| 0                   |
	| 12345               |

Scenario: Load a list of SVM Meeting Ids from the SVM Meeting Report
	Given I have a SVM meeting report
	When the file is loaded into the Svm Meeting Importer
	Then the cancelled meetings should be accessible from the system
	And the meetings should be Symphony Conferences
