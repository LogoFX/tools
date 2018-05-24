Feature: Login
	In order to use the system
	As an authenticated user
	I want to be able log in to the system

Scenario: Login screen is displayed first		
	When I open the application	
	Then the login screen is displayed

Scenario: Navigate to the main screen when the login is successful
	Given I am able to log in successfully with username 'Admin' and password 'pass'
	When I open the application
	And I set the username to 'Admin'
	And I set the password to 'pass'
	And I log in to the system	
	Then Application navigates to the main screen

Scenario: Remain at the login screen when the login fails (wrong password)
	Given I am able to log in successfully with username 'Admin' and password 'pass'
	When I open the application
	And I set the username to 'Admin'
	And I set the password to 'wrong password'
	And I log in to the system	
	Then Login error message is displayed with the following text 'Failed to log in: Unable to login.'

Scenario: Remain at the login screen when the login fails (wrong username)
	Given I am able to log in successfully with username 'Admin' and password 'pass'
	When I open the application
	And I set the username to 'Wrong Username'
	And I set the password to 'pass'
	And I log in to the system	
	Then Login error message is displayed with the following text 'Failed to log in: Login not found.'
