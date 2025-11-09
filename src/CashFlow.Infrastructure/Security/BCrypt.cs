using BC = BCrypt.Net.BCrypt;
using CashFlow.Domain.Security.Cryotography;

namespace CashFlow.Infrastructure.Security
{
    public class BCrypt : IPasswordEncripter
    {
        public string Encrypt(string password)
        {
            string passwordHash = BC.HashPassword(password);
            return passwordHash;
        }

        public bool Verify(string password,string passwordHash)
        {
            var result = BC.Verify(password, passwordHash);
         
            return result;
        }
    }
}
