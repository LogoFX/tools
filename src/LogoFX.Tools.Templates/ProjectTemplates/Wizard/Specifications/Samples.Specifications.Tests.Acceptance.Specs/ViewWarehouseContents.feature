Feature: ViewWarehouseContents
	In order to view and manage warehouse contents
	As an entitled user
	I want to be able to edit the displayed items

Scenario: Display warehouse items
	Given warehouse contains the following items:
	| Kind | Price  | Quantity |
	| Oven | 34.95  | 20       |
	| TV   | 346.95 | 50       |
	| PC   | 423.95 | 70       |	
	And I am able to log in successfully with username 'Admin' and password '1234'
	When I open the application
	And I set the username to "Admin"
	And I set the password to "1234"
	And I log in to the system
	Then I expect to see the following data on the screen:
	| Kind | Price	| Quantity	| Total Cost	|
	| Oven | 34.95	| 20		| 699			|
	| TV   | 346.95	| 50		| 17347.5		|
	| PC   | 423.95	| 70		| 29676.5		|

Scenario: Edit item price
	Given warehouse contains the following items:
	| Kind | Price  | Quantity |
	| Oven | 34.95  | 20       |
	| TV   | 346.95 | 50       |
	And I am able to log in successfully with username 'Admin' and password '1234'
	When I open the application
	And I set the username to "Admin"
	And I set the password to "1234"
	And I log in to the system	
	And I set the Price for "TV" item to 350
	Then Total cost of "TV" item is 17500

Scenario: Delete warehouse item
	Given warehouse contains the following items:
	| Kind | Price  | Quantity |
	| Oven | 34.95  | 20       |
	| TV   | 346.95 | 50       |
	| PC   | 423.95 | 70       |	
	And I am able to log in successfully with username 'Admin' and password '1234'
	When I open the application
	And I set the username to "Admin"
	And I set the password to "1234"
	And I log in to the system	
	And I delete "TV" item
	Then I expect to see the following data on the screen:
	| Kind | Price	| Quantity	| Total Cost	|
	| Oven | 34.95	| 20		| 699			|
	| PC   | 423.95	| 70		| 29676.5		|

Scenario: Create new warehouse item
	Given warehouse contains the following items:
	| Kind | Price  | Quantity |
	| Oven | 34.95  | 20       |
	| TV   | 346.95 | 50       |
	And I am able to log in successfully with username 'Admin' and password '1234'
	When I open the application
	And I set the username to "Admin"
	And I set the password to "1234"
	And I log in to the system	
	And I create a new warehouse item with the following data:
	| Kind | Price  | Quantity |
	| PC   | 423.95 | 70       |	
	Then I expect to see the following data on the screen:
	| Kind | Price	| Quantity	| Total Cost	|
	| Oven | 34.95	| 20		| 699			|
	| TV   | 346.95	| 50		| 17347.5		|
	| PC   | 423.95	| 70		| 29676.5		|
