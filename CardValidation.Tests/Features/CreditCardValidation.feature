Feature: Credit Card Validation
  As a user
  I want to validate my credit card details
  So that I can confirm my payment method is valid

  Scenario: Validate a valid Visa card
    Given I have a credit card with owner "John Doe"
    And card number "4111111111111111"
    And issue date "01/29"
    And CVV "444"
    When I validate the card
    Then the response should contain "Visa"

  Scenario: Validate a valid MasterCard card
    Given I have a credit card with owner "Melisa Lokki"
    And card number "5105105105105100"
    And issue date "11/31"
    And CVV "555"
    When I validate the card
    Then the response should contain "MasterCard"

Scenario: Validate a valid American Express card
    Given I have a credit card with owner "Jane White"
    And card number "378282246310005"
    And issue date "06/30"
    And CVV "333"
    When I validate the card
    Then the response should contain "AmericanExpress"

Scenario: Validate an expired credit card
    Given I have a credit card with owner "Bob Domar"
    And card number "4111111111111111"
    And issue date "01/20"
    And CVV "444"
    When I validate the card
    Then the response should contain "Wrong date"

Scenario: Validate a Visa card with missing CVV
    Given I have a credit card with owner "John Doe"
    And card number "4111111111111111"
    And issue date "12/30"
    When I validate the card
    Then the response should contain "Cvv is required"

Scenario: Validate a MasterCard with CVC instead of CVV
    Given I have a credit card with owner "Melisa Lokki"
    And card number "5105105105105100"
    And issue date "11/31"
    And CVC "555"
    When I validate the card
    Then the response should contain "Cvv is required"

Scenario: Validate an invalid card number
    Given I have a credit card with owner "Jack Black"
    And card number "1234567890123456"
    And issue date "11/31"
    And CVV "999"
    When I validate the card
    Then the response should contain "Wrong number"

Scenario: Validate a card number without ownername
    Given I have a credit card with owner ""
    And card number "4111111111111111"
    And issue date "12/30"
    And CVV "444"
    When I validate the card
    Then the response should contain "Owner is required"