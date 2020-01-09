namespace ProductionCode.Orders.Models
{
    using System;

    /// <summary>
    ///     Credit/Debit order adjustment
    /// </summary>
    public class Adjustment
    {
        public int AdjustmentId { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedDate { get; set; }
        public long? OrderId { get; set; }
    }
}
