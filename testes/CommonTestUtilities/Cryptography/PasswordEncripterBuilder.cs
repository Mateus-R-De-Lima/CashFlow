using CashFlow.Domain.Security.Cryotography;
using Moq;

namespace CommonTestUtilities.Cryptography
{
    public class PasswordEncripterBuilder
    {
        private readonly Mock<IPasswordEncripter> _mock;

        public PasswordEncripterBuilder()
        {
            _mock = new Mock<IPasswordEncripter>();

            _mock.Setup(config => config.Encrypt(It.IsAny<string>())).Returns("sadsaeqweqwesa!@!3");

        }
        public  IPasswordEncripter Build() => _mock.Object;

        public PasswordEncripterBuilder Verify(string? password)
        {
            if(!string.IsNullOrWhiteSpace(password))
                  _mock.Setup(passwordEncrypter => passwordEncrypter.Verify(password, It.IsAny<string>())).Returns(true);

            return this;
        }
    }
}
