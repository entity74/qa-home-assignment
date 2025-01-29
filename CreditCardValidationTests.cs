using System;
using Xunit;
using CardValidation.Core.Services;
using CardValidation.Core.Enums;

namespace CreditCardValidation.Tests
{
    public class CreditCardValidationTests
    {
        private readonly CardValidationService _service;

        public CreditCardValidationTests()
        {
            _service = new CardValidationService();
        }

        [Fact]
        public void Should_ReturnFalse_When_CardOwnerIsInvalid()
        {
            string invalidOwner = "John123"; // Contains numbers, which is not allowed.
            var result = _service.ValidateOwner(invalidOwner);
            Assert.False(result);
        }

        [Fact]
        public void Should_ReturnTrue_When_CardOwnerIsValid()
        {
            string validOwner = "John Doe";
            var result = _service.ValidateOwner(validOwner);
            Assert.True(result);
        }

        [Fact]
        public void Should_ReturnFalse_When_IssueDateIsInvalid()
        {
            string invalidDate = "13/25"; // Invalid month (13)
            var result = _service.ValidateIssueDate(invalidDate);
            Assert.False(result);
        }

        [Fact]
        public void Should_ReturnFalse_When_CardIsExpired()
        {
            string expiredDate = "01/22"; // Past year
            var result = _service.ValidateIssueDate(expiredDate);
            Assert.False(result);
        }

        [Fact]
        public void Should_ReturnTrue_When_CardIsValidAndNotExpired()
        {
            string validDate = "12/30"; // Future date
            var result = _service.ValidateIssueDate(validDate);
            Assert.True(result);
        }

        [Fact]
        public void Should_ReturnFalse_When_CvcIsInvalid()
        {
            string invalidCvc = "12"; // Less than 3 digits
            var result = _service.ValidateCvc(invalidCvc);
            Assert.False(result);
        }

        [Fact]
        public void Should_ReturnTrue_When_CvcIsValid()
        {
            string validCvc = "123";
            var result = _service.ValidateCvc(validCvc);
            Assert.True(result);
        }

        [Fact]
        public void Should_ReturnFalse_When_CardNumberIsInvalid()
        {
            string invalidCardNumber = "1234567890123456"; // Doesn't match Visa, MasterCard, or Amex format
            var result = _service.ValidateNumber(invalidCardNumber);
            Assert.False(result);
        }

        [Fact]
        public void Should_ReturnTrue_When_ValidVisaCardNumberProvided()
        {
            string visaCard = "4111111111111111"; // Visa test card
            var result = _service.ValidateNumber(visaCard);
            Assert.True(result);
        }

        [Fact]
        public void Should_ReturnMasterCard_When_ValidMasterCardNumberProvided()
        {
            string masterCard = "5555555555554444"; // MasterCard test card
            var result = _service.GetPaymentSystemType(masterCard);
            Assert.Equal(PaymentSystemType.MasterCard, result);
        }

        [Fact]
        public void Should_ReturnVisa_When_ValidVisaCardNumberProvided()
        {
            string visaCard = "4111111111111111"; // Visa test card
            var result = _service.GetPaymentSystemType(visaCard);
            Assert.Equal(PaymentSystemType.Visa, result);
        }

        [Fact]
        public void Should_ReturnAmericanExpress_When_ValidAmexCardNumberProvided()
        {
            string amexCard = "371449635398431"; // Amex test card
            var result = _service.GetPaymentSystemType(amexCard);
            Assert.Equal(PaymentSystemType.AmericanExpress, result);
        }

        [Fact]
        public void Should_ThrowException_When_CardNumberIsUnknown()
        {
            string unknownCard = "9999999999999999"; // Not Visa, MasterCard, or Amex
            // Act & Assert
            Assert.Throws<NotImplementedException>(() => _service.GetPaymentSystemType(unknownCard));
        }
    }
}
