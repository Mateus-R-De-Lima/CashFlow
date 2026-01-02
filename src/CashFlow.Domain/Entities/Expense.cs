using CashFlow.Domain.Enums;
using System.Net.Sockets;

namespace CashFlow.Domain.Entities
{
    public class Expense
    {

        public long Id { get; set; }
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public DateTime Date { get; set; }

        public PaymentTypes PaymentType { get; set; }

        public ICollection<Tag> Tags { get; set; } = [];

        public decimal Amount { get; set; }

        public long UserId { get; set; }

        public User User { get; set; } = default!;


    }
}
