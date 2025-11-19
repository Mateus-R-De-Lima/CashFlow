using CashFlow.Domain.Entities;
using CashFlow.Domain.Security.Token;
using Moq;


namespace CommonTestUtilities.Token
{
    public class JwtTokenGenaratorBuilder
    {
        public static IAccessTokenGenarator Buid()
        {
            var mock = new Mock<IAccessTokenGenarator>();
            mock.Setup(config => config.Generate(It.IsAny<User>())).Returns("Token");
            return mock.Object;
        }
    }
}
