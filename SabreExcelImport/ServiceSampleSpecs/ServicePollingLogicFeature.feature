﻿Feature: ServicePollingLogicFeature
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
	And the message is from Sabre Virtual Meetings
	Then I should be able to view the new message count
	And the count should be greater than 0
	And the attachment should be loaded into the Sabre Excel Importer

Scenario: Service loads an excel attachment into the Sabre Excel Importer and sends created meetings to Symphony
	Given the Sabre Excel Importer has meetings that were created
	When the Agent processes these
	Then the Agent should have a confirmation number for each created meeting

Scenario: Service loads an excel attachment into the Sabre Excel Importer and failes to sends the created meetings to Symphony
	Given the Sabre Excel Importer has meetings that were created
	When the Symphony platform has conflicts with some of the new meetings
	And the Agent processes these
	Then the Agent should add these meetings into an issues dictionary

Scenario: Service loads an excel attachment into the Sabre Excel Importer and failes process the file
	Given the Sabre Excel Importer has meetings that were created
	When the SVM Service Polling Agent Locates these
	And the Agent failes to load the file into the Sabre Excel Importer
	Then the Agent should add the file into an issues dictionary



