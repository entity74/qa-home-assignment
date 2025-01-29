using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;
using Reqnroll;
using Microsoft.AspNetCore.Mvc.Testing;
using CardValidation.Core.Enums;
using System.Collections.Generic;

namespace CardValidation.Tests.StepDefinitions
{
    [Binding]
    public class CreditCardValidationSteps
    {
        private readonly HttpClient _client;
        private readonly ScenarioContext _scenarioContext;
        private HttpResponseMessage _response = new HttpResponseMessage();

        public CreditCardValidationSteps(ScenarioContext scenarioContext)
        {
            var application = new WebApplicationFactory<Program>();
            _client = application.CreateClient();
            _scenarioContext = scenarioContext;
        }

        [Given(@"I have a credit card with owner ""(.*)""")]
        public void GivenIHaveACreditCardWithOwner(string owner)
        {
            _scenarioContext["Owner"] = owner;
        }

        [Given(@"card number ""(.*)""")]
        public void GivenCardNumber(string cardNumber)
        {
            _scenarioContext["Number"] = cardNumber;
        }

        [Given(@"issue date ""(.*)""")]
        public void GivenIssueDate(string issueDate)
        {
            _scenarioContext["Date"] = issueDate;
        }

        [Given(@"CVV ""(.*)""")]
        public void GivenCVV(string cvv)
        {
            _scenarioContext["CVV"] = cvv;
        }

        [Given(@"CVC ""(.*)""")]
        public void GivenCVC(string cvc)
        {
            _scenarioContext["CVC"] = cvc;
        }

        [When(@"I validate the card")]
        public async Task WhenIValidateTheCard()
        {
            // Dynamically construct request object based on available parameters
            var cardDetails = new Dictionary<string, object>
            {
                { "owner", _scenarioContext["Owner"] },
                { "number", _scenarioContext["Number"] },
                { "date", _scenarioContext["Date"] }
            };

            // Include either CVV or CVC depending on what is present
            if (_scenarioContext.ContainsKey("CVV"))
            {
                cardDetails.Add("cvv", _scenarioContext["CVV"]);
            }
            else if (_scenarioContext.ContainsKey("CVC"))
            {
                cardDetails.Add("cvc", _scenarioContext["CVC"]);
            }

            var content = new StringContent(JsonConvert.SerializeObject(cardDetails), Encoding.UTF8, "application/json");
            _response = await _client.PostAsync("https://localhost:7135/CardValidation/card/credit/validate", content);
        }

        [Then(@"the response should contain ""(.*)""")]
        public async Task ThenTheResponseShouldContain(string expectedResponse)
        {
            var responseBody = await _response.Content.ReadAsStringAsync();

            if (string.IsNullOrEmpty(responseBody))
            {
                throw new NullReferenceException("API response body is empty.");
            }

           // Console.WriteLine($"Received API Response: {responseBody}");

            // Convert response number to enum name if applicable
            if (int.TryParse(responseBody.Trim(), out int responseNumber))
            {
                var cardTypeName = Enum.GetName(typeof(PaymentSystemType), responseNumber);
                Assert.Equal(expectedResponse, cardTypeName);
            }
            else
            {
                Assert.Contains(expectedResponse, responseBody);
            }
        }
    }
}
