Feature: Login
	In order to use the system
	As an authenticated user
	I want to be able log in to the system

Scenario: Login screen is displayed first		
	When I open the application	
	Then the login screen is displayed

Scenario: Navigate to the main screen when the login is successful
	Given I am able to log in successfully with username 'Admin' and password '1234'
	When I open the application
	And I set the username to "Admin"
	And I set the password to "1234"
	And I log in to the system
	Then Application navigates to the main screen
