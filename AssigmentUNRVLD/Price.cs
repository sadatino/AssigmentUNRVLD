namespace AssigmentUNRVLD
{
    public class Price
    {
        public Price()
        {
            
        }

        public Price(double priceAm, double pricePm)
        {
            this.PriceAm = priceAm;
            this.PricePm = pricePm;
            PriceAm = priceAm;
            PricePm = pricePm;
        }

        public double PriceAm { get; set; }

        public double PricePm { get; set; }
    }
}