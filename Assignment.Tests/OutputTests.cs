using NUnit.Framework;
using System;
using System.Globalization;
using AssigmentUNRVLD;

namespace Assignment.Tests
{
    public class OutputTests
    {

        private readonly VehicleTax _vehicleTax = new VehicleTax();

        [SetUp]
        public void Setup()
        {
        }

        [TestCase("Car: 24/04/2008 11:32 - 24/04/2008 14:42", 0, 28, 0.9, 2, 42, 6.7, 7.6)]
        [TestCase("Motorbike: 24/04/2008 17:00 - 24/04/2008 22:11", 0, 0, 0, 2, 0, 2, 2)]
        [TestCase("Van: 25/04/2008 10:23 - 28/04/2008 09:02", 3, 39, 7.3, 7, 0 , 17.5, 24.8)]
        public void CheckIfOutPutIsCorrect_IfGood_ReturnsTrue(string input, int hoursAm, int minutesAm, double chargeAm, 
            int hoursPm, int minutesPm, double chargePm, double actualChargeTotal)
        {
            bool allOutputIsGood = false;
            Vehicle vehicle = _vehicleTax.StartCalculatingVehiclesTotalPrice(input);
            

            double totalCharge = Math.Round(vehicle.PriceIs.PriceAm + vehicle.PriceIs.PricePm, 1);
            allOutputIsGood = hoursAm == vehicle.TimeSpent.TotalTimeAm.Hours && minutesAm == vehicle.TimeSpent.TotalTimeAm.Minutes
                && chargeAm == vehicle.PriceIs.PriceAm && hoursPm == vehicle.TimeSpent.TotalTimePm.Hours
                && minutesPm == vehicle.TimeSpent.TotalTimePm.Minutes
                && chargePm == vehicle.PriceIs.PricePm && actualChargeTotal == totalCharge;

            
            Assert.IsTrue(allOutputIsGood);

        }


    }
}