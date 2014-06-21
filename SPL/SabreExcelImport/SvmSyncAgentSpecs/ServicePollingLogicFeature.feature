Feature: ServicePollingLogicFeature
	As the SVM Service Polling Agent
	I want to check a known email account for new email
	so that I can open new svm meeting report attachments

Scenario: Service authenticates against a known IMAP email account and checks for new messages
	Given I have a mail repository with a new message in the inbox
	Then I should be able to view the new message count

Scenario: Service finds a new message after authetnicating a known IMAP email account
	Given I have a mail repository with a new message in the inbox
	Then I should be able to view the new message count
	And the count should be greater than 0

Scenario: Service finds a new message from SVM with an attachment after authetnicating a known IMAP email account
	Given I have a mail repository with a new message in the inbox
	When the message has an xls attachment 
	And the attachment is loaded into the Sabre Excel Importer
	Then I should be able to view the new message count
	And the count should be greater than 0

Scenario: Service loads an excel attachment into the Sabre Excel Importer and sends created meetings to Symphony
	Given I have a mail repository with a new message in the inbox
	When the message has an xls attachment 
	And the attachment is loaded into the Sabre Excel Importer
	And the Symphony platform has no conflicts for the new meetings
	And the Agent processes these
	Then the Agent should have a confirmation number for each meeting without a conflict

Scenario: Service loads an excel attachment into the Sabre Excel Importer and failes process the file
	Given I have a mail repository with a new message in the inbox
	When the message has an xls attachment 
	And the attachment fails to load into the Sabre Excel Importer
	Then the Sabre Excel Importer should throw an exception



