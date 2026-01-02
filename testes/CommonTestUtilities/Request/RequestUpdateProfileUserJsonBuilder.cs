using Bogus;
using CashFlow.Communication.Requests;

namespace CommonTestUtilities.Request
{
    public class RequestUpdateProfileUserJsonBuilder
    {

        public static RequestUpdateProfileUserJson Build()
        {
            return new Faker<RequestUpdateProfileUserJson>()
                .RuleFor(user => user.Name, faker => faker.Person.FirstName)
                .RuleFor(user => user.Email, (faker, user) => faker.Internet.Email(user.Name));               


        }
    }
}
