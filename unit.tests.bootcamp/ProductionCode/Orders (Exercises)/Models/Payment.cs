namespace ProductionCode.Orders.Models
{
    using System;

    public class Payment
    {
        public int PaymentId { get; set; }
        public int? InvoiceId { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? OrderId { get; set; }
    }
}
