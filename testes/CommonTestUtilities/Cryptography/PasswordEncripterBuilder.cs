using CashFlow.Domain.Security.Cryotography;
using Moq;

namespace CommonTestUtilities.Cryptography
{
    public class PasswordEncripterBuilder
    {

        public static IPasswordEncripter Build()
        {
            var mock = new Mock<IPasswordEncripter>();

            mock.Setup(config => config.Encrypt(It.IsAny<string>())).Returns("sadsaeqweqwesa!@!3");

            return mock.Object;
        }
    }
}
