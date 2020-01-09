namespace OpenClosedShoppingCartAfter
{
    using OpenClosedShoppingCartAfter.Contracts;

    public class OrderItem : ITotalizer
    {
        public string Sku { get; set; }

        public int Quantity { get; set; }

        public decimal GetTotal()
        {
            decimal total = 0m;
            if (Sku.StartsWith("EACH"))
            {
                total += Quantity * 5m;
            }
            else if (Sku.StartsWith("WEIGHT"))
            {
                // quantity is in grams, price is per kg
                total += Quantity * 4m / 1000;
            }
            else if (Sku.StartsWith("SPECIAL"))
            {
                // $0.40 each; 3 for a $1.00
                total += Quantity * .4m;
                int setsOfThree =Quantity / 3;
                total -= setsOfThree * .2m;
            }
            return total;
        }
    }
}