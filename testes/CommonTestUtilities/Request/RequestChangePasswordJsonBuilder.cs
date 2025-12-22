using Bogus;
using CashFlow.Communication.Requests;
using CashFlow.Domain.Entities;

namespace CommonTestUtilities.Request
{
    public class RequestChangePasswordJsonBuilder 
    {

        public static RequestChangePasswordUserJson Build()
        {
            return new Faker<RequestChangePasswordUserJson>()
                .RuleFor(user => user.Password , faker => faker.Internet.Password())
                .RuleFor(user => user.NewPassword , faker => faker.Internet.Password(prefix:"!Aa1"));
        }
    }
}
