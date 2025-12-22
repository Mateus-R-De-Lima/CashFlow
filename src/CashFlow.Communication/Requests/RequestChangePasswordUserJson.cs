namespace CashFlow.Communication.Requests
{
    public class RequestChangePasswordUserJson
    {
        public string Password { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}
