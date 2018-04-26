Feature: EditWarehouseItem
	In order to reflect the current warehouse items' state
	As a warehouse manager
	I want to be able to update warehouse items' properties

Scenario: Edit item price
	Given warehouse contains the following items:
	| Kind | Price  | Quantity |
	| Oven | 34.95  | 20       |
	| TV   | 346.95 | 50       |
	And I am able to log in successfully with username 'Admin' and password 'pass'
	When I open the application
	And I set the username to "Admin"
	And I set the password to "pass"
	And I log in to the system	
	And I set the Price for "TV" item to 350
	Then Total cost of "TV" item is 17500

Scenario: Display error for incorrect Price value
	Given warehouse contains the following items:
	| Kind | Price  | Quantity |
	| Oven | 34.95  | 20       |
	| TV   | 346.95 | 50       |
	| PC   | 423.95 | 70       |	
	And I am able to log in successfully with username 'Admin' and password 'pass'
	When I open the application
	And I set the username to "Admin"
	And I set the password to "pass"
	And I log in to the system	
	And I set the Price for "TV" item to -10
	Then Error message is displayed with the following text "Price must be positive."

Scenario: Display error for incorrect Quantity value
	Given warehouse contains the following items:
	| Kind | Price  | Quantity |
	| Oven | 34.95  | 20       |
	| TV   | 346.95 | 50       |
	| PC   | 423.95 | 70       |	
	And I am able to log in successfully with username 'Admin' and password 'pass'
	When I open the application
	And I set the username to "Admin"
	And I set the password to "pass"
	And I log in to the system	
	And I set the Quantity for "TV" item to -10
	Then Error message is displayed with the following text "Quantity must be positive."

Scenario: Display error for incorrect Kind value
	Given warehouse contains the following items:
	| Kind | Price  | Quantity |
	| Oven | 34.95  | 20       |
	| TV   | 346.95 | 50       |
	| PC   | 423.95 | 70       |	
	And I am able to log in successfully with username 'Admin' and password 'pass'
	When I open the application
	And I set the username to "Admin"
	And I set the password to "pass"
	And I log in to the system	
	And I set the Kind for "TV" item to ""
	Then Error message is displayed with the following text "Kind should not be empty."