namespace CashFlow.Domain.Security.Cryotography
{
    public interface IPasswordEncripter
    {
        string Encrypt(string password);
    }
}
