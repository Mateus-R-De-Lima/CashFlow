using CashFlow.Application.Services.LoggerUser;
using CashFlow.Domain.Entities;
using Moq;

namespace CommonTestUtilities.LoggedUser
{
    public class LoggedUserBuilder
    {
        public static ILoggerUser Build(User user)
        {
            var mock = new Mock<ILoggerUser>();

            mock.Setup(loggedUser => loggedUser.Get()).ReturnsAsync(user);

            return mock.Object;
        }
    }
}
