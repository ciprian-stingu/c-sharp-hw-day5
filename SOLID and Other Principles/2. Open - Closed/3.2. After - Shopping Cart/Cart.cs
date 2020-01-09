namespace OpenClosedShoppingCartAfter
{
    using System.Collections.Generic;
    using OpenClosedShoppingCartAfter.Contracts;

    public class Cart
    {
        private readonly List<ITotalizer> items;

        public Cart()
        {
            this.items = new List<ITotalizer>();
        }

        public IEnumerable<ITotalizer> Items
        {
            get { return new List<ITotalizer>(this.items); }
        }

        public string CustomerEmail { get; set; }

        public void Add(ITotalizer orderItem)
        {
            this.items.Add(orderItem);
        }

        public decimal TotalAmount()
        {
            decimal total = 0m;
            foreach (ITotalizer orderItem in this.Items)
            {
                total += orderItem.GetTotal();

                // more rules are coming!
            }

            return total;
        }
    }
}