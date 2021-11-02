using System;
using System.Collections.Generic;
using System.Text;

namespace AssigmentUNRVLD
{
    public class Vehicle
    {
        private string type;
        private Time timeSpent;
        private double[] tax;
        private Price priceIs;

        public Vehicle()
        {
        }

        public Vehicle(string type, Time timeSpent, double[] tax, Price priceIs)
        {
            this.type = type;
            Type = this.type;
            this.timeSpent = timeSpent;
            TimeSpent = this.timeSpent;
            this.tax = tax;
            Tax = this.tax;
            this.priceIs = priceIs;
            PriceIs = this.priceIs;
        }

        public string Type { get; set; }
        public Time TimeSpent { get; set; }
        public double[] Tax { get; set; }
        public Price PriceIs { get; set; }
    }

}
