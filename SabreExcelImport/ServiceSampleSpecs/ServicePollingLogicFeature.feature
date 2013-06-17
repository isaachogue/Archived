Feature: ServicePollingLogicFeature
	As the SVM Service Polling Agent
	I want to check a known email account for new email
	so that I can open new svm meeting report attachments

Scenario: Service authenticates against a known IMAP email account and checks for new messages
	Given I have a known IMAP email account
	When I connect to the account using known credentials
	Then I should be able to view the new message count

Scenario: Service finds a new message after authetnicating a known IMAP email account
	Given I have a known IMAP email account
	When I connect to the account using known credentials
	And a new message is available in the inbox
	Then I should be able to view the new message count
	And the count should be greater than 0

Scenario: Service finds a new message from SVM with an attachment after authetnicating a known IMAP email account
	Given I have a known IMAP email account
	When I connect to the account using known credentials
	And a new message is available in the inbox
	And the message has an xls attachment 
	And the message is from Sabre Virtual Meetings
	Then I should be able to view the new message count
	And the count should be greater than 0
	And the attachment should be loaded into the Sabre Excel Importer

Scenario: Service loads an excel attachment into the Sabre Excel Importer and sends created meetings to Symphony
	Given the Sabre Excel Importer has meetings that were created
	When the SVM Service Polling Agent Locates these
	And the Agent sends each one to the Symphony Api
	Then the Api should return a confirmation number for each created meeting

Scenario: Service loads an excel attachment into the Sabre Excel Importer and failes to sends the created meetings to Symphony
	Given the Sabre Excel Importer has meetings that were created
	When the SVM Service Polling Agent Locates these
	And the Agent sends each one to the Symphony Api
	And the Symphony Api finds conflicts with some of the new meetings
	Then the Agent should add these meetings into an issues dictionary

Scenario: Service loads an excel attachment into the Sabre Excel Importer and failes process the file
	Given the Sabre Excel Importer has meetings that were created
	When the SVM Service Polling Agent Locates these
	And the Agent failes to load the file into the Sabre Excel Importer
	Then the Agent should add the file into an issues dictionary



