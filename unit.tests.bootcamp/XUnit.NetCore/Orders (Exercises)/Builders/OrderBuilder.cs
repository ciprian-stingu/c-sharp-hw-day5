namespace xUnit.NetCore.Orders.Builders
{
    using System;
    using System.Collections.Generic;
    using ProductionCode.Orders.Models;

    internal class OrderBuilder
    {
        private readonly List<Adjustment> _adjustments = new List<Adjustment>();


        private decimal? _amount;
        private DateTime? _bookedDate = new DateTime(2019, 02, 13);
        private string _currencyCode;
        private bool _deleted;
        private int? _id;
        private readonly List<Refund> _refunds = new List<Refund>();

        public OrderBuilder WithBookedDate(DateTime? bookedDate)
        {
            this._bookedDate = bookedDate;
            return this;
        }

        public OrderBuilder WithOrderId(int id)
        {
            this._id = id;
            return this;
        }

        public OrderBuilder WithAmount(decimal amount)
        {
            this._amount = amount;
            return this;
        }

        public OrderBuilder WithCredit(decimal creditAmount)
        {
            this._adjustments.Add(new Adjustment {Amount = creditAmount * -1});
            return this;
        }

        public OrderBuilder WithDebit(decimal debitAmount)
        {
            this._adjustments.Add(new Adjustment {Amount = debitAmount});
            return this;
        }


        public OrderBuilder WithRefund(decimal refundAmount)
        {
            this._refunds.Add(new Refund {Amount = refundAmount});
            return this;
        }

        public OrderBuilder AsDeleted()
        {
            this._deleted = true;
            return this;
        }

        public OrderBuilder WithCurrency(string currencyCode)
        {
            this._currencyCode = currencyCode;
            return this;
        }

        public Order Build()
        {
            if (!this._amount.HasValue) this._amount = 1000;

            if (!this._id.HasValue) this._id = new Random(DateTime.UtcNow.Millisecond).Next();


            var order = new Order
            {
                OrderId = this._id.Value,
                Amount = this._amount.Value,
                Subtotal = this._amount.Value,
                BookedDate = this._bookedDate,

                IsDeleted = this._deleted,
                CurrencyIsoCode = this._currencyCode ?? "USD"
            };

            order.Adjustments.AddRange(this._adjustments);
            order.Refunds.AddRange(this._refunds);

            return order;
        }
    }
}
