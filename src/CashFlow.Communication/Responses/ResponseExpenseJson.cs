using CashFlow.Communication.Enums;

namespace CashFlow.Communication.Responses
{
    public class ResponseExpenseJson
    {
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public DateTime Date { get; set; }

        public PaymentTypes PaymentType { get; set; }

        public decimal Amount { get; set; }
    }
}
