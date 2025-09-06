using Bogus;
using CashFlow.Communication.Enums;
using CashFlow.Communication.Requests;

namespace CommonTestUtilities.Request
{
    public class RequestRegisterExpenseJsonBuilder
    {
        
        public RequestRegisterExpenseJson Build()
        {

            return new Faker<RequestRegisterExpenseJson>()
                 .RuleFor(r => r.Title, faker => faker.Commerce.ProductName())
                 .RuleFor(r => r.Description, faker => faker.Commerce.ProductDescription())
                 .RuleFor(r => r.Amount, faker => faker.Random.Decimal(min: 1, max: 1000))
                 .RuleFor(r => r.PaymentType, faker => faker.PickRandom<PaymentTypes>())
                 .RuleFor(r => r.Date, faker => faker.Date.Past());               


        }

    }
}
