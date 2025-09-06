using CashFlow.Domain.Enums;

namespace CashFlow.Domain.Entities
{
    public class Expense
    {

        public long Id { get; set; }
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public DateTime Date { get; set; }

        public PaymentTypes PaymentType { get; set; }

        public decimal Amount { get; set; }

    }
}
