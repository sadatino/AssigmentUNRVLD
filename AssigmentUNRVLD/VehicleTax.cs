using System;
using System.Globalization;
using System.Linq;

namespace AssigmentUNRVLD
{
    public class VehicleTax
    {
        readonly string[] ListOfVehicles = new string[3] { "car", "van", "motorbike" };

        readonly DateTime[] ListOfTimes = new DateTime[]
        {
            DateTime.ParseExact("07:00", "HH:mm", CultureInfo.InvariantCulture),
            DateTime.ParseExact("12:00", "HH:mm", CultureInfo.InvariantCulture),
            DateTime.ParseExact("19:00", "HH:mm", CultureInfo.InvariantCulture)
        };

        public Vehicle StartCalculatingVehiclesTotalPrice(string input)
        {
            Vehicle vehicle = CreateNewVehicle(input);
            
            var loopingTime = vehicle.TimeSpent.Start; // var for looping through time until we reach the end
            var leavingTime = vehicle.TimeSpent.End;

            int arrIndx = FindIndexOfTimeThatsClosestToStartTime(loopingTime); // index of ListOfTimes array, finds which time is needed
             
            var firstTaxChangeTime = ListOfTimes[GetCircularArrayIndex(arrIndx)];
            firstTaxChangeTime = AddDateWithHours(vehicle.TimeSpent.Start, firstTaxChangeTime);

            var dtOfTaxChange = firstTaxChangeTime;

            CalculateTotalPrice(leavingTime, loopingTime, dtOfTaxChange, vehicle, arrIndx);

            PrintOutReceipt(vehicle);

            return vehicle;
            
        }
        public Vehicle CreateNewVehicle(string input)
        {
            string vehicleType = GetVehicleTypeFromInput(input);
            var dates = SplitInputToStartAndEndDates(input);
            Time visitTime = new Time(dates[0], dates[1]);
            
            switch (vehicleType)
            {
                case "motorbike":
                    return new Vehicle(vehicleType, visitTime, new double[] { 0d, 1.00d, 1.00d }, new Price(0,0));
                default:
                    return new Vehicle(vehicleType, visitTime, new double[] { 0d, 2.00d, 2.50d }, new Price(0,0));
            }
        }

        public string GetVehicleTypeFromInput(string input)
        {
            string vehicleType = input.Split(':')[0].ToLower();

            foreach (var lv in ListOfVehicles)
            {
                if (vehicleType.Equals(lv))
                {
                    return vehicleType;
                }
            }
            return "";
        }

        public string[] SplitInputToStartAndEndDates(string input)
        {
            var inputInformation = input.Split(' ');
            string dateStart = inputInformation[1];
            string hoursStart = inputInformation[2];

            string dateEnd = inputInformation[4];
            string hoursEnd = inputInformation[5];

            string fullStartingDate = string.Format("{0} {1}", dateStart.Trim(), hoursStart.Trim());
            string fullEndingDate = string.Format("{0} {1}", dateEnd.Trim(), hoursEnd.Trim());

            string[] dates = new string[2];
            dates[0] = fullStartingDate;
            dates[1] = fullEndingDate;

            return dates;
        }

        public int FindIndexOfTimeThatsClosestToStartTime(DateTime loopingTime)
        {
            double[] closestTimeToStart = CalculateDifferenceBetweenTimes(loopingTime);
            int smallestDiffIndex = ClosestTimeIndex(loopingTime, closestTimeToStart);
            
            return smallestDiffIndex;
        }

        private int ClosestTimeIndex(DateTime loopingTime, double[] closestTimeToStart)
        {
            int i = 0;
            double smallestDiff = Int32.MaxValue;
            int smallestDiffIndex = 0;
            if (loopingTime > ListOfTimes[2])
            {
                foreach (var ctts in closestTimeToStart)
                {
                    //Console.WriteLine(ctts);
                    if (ctts < smallestDiff)
                    {
                        smallestDiff = ctts;
                        smallestDiffIndex = i;
                    }
                    i++;
                }
            }
            else
            {
                foreach (var ctts in closestTimeToStart)
                {
                    //Console.WriteLine(ctts);
                    if (ctts < smallestDiff && ctts >= 0)
                    {
                        smallestDiff = ctts;
                        smallestDiffIndex = i;
                    }
                    i++;
                }
            }

            return smallestDiffIndex;
        }

        private double[] CalculateDifferenceBetweenTimes(DateTime loopingTime)
        {
            double[] closestTimeToStart = new double[ListOfTimes.Length];
            int indexOfClosestTime = 0;

            foreach(var dt in ListOfTimes)
            {
                var td = dt - GetHHmm(loopingTime);
                closestTimeToStart[indexOfClosestTime] = td.TotalMinutes;
                //Console.WriteLine(td);
                indexOfClosestTime++;
            }

            return closestTimeToStart;
        }

        public int GetCircularArrayIndex(int n)
        {
            return n % 3;
        }
        public DateTime GetHHmm(DateTime dt)
        {
            return DateTime.ParseExact(dt.ToString("HH:mm"), "HH:mm", CultureInfo.InvariantCulture);

        }

        public void CalculateTotalPrice(DateTime leavingTime, DateTime loopingTime, 
            DateTime dtOfTaxChange, Vehicle vehicle, int arrIndx)
        {
            double taxPrice;
            while (leavingTime > loopingTime)
            {
                taxPrice = CheckWeekDayAndGetTax(vehicle, loopingTime, arrIndx);
                 
                if (dtOfTaxChange > vehicle.TimeSpent.End)
                {
                    dtOfTaxChange = vehicle.TimeSpent.End;
                }

                TimeSpan timeUntilTaxChange = dtOfTaxChange - loopingTime;
                
                var priceWithThisTax = RoundToNearestTenth(timeUntilTaxChange.TotalHours * taxPrice);

                AddPriceAndHoursToTotal(loopingTime, vehicle, priceWithThisTax, timeUntilTaxChange);
                
                loopingTime = loopingTime.Add(timeUntilTaxChange);
                
                arrIndx++;

                dtOfTaxChange = ChangeDateTimeOfTaxHours(dtOfTaxChange, loopingTime, arrIndx);

            }
        }

        public DateTime AddDateWithHours(DateTime dmy, DateTime hm)
        {
            string startingDate = dmy.ToString("dd/MM/yyyy");
            string taxChangeHours = hm.ToString("HH:mm");

            string fullDate = string.Format("{0} {1}", startingDate.Trim(), taxChangeHours.Trim());
            fullDate = fullDate.Replace('-', '/');
            
            DateTime correctDt = DateTime.ParseExact(fullDate, "dd/MM/yyyy HH:mm",
                CultureInfo.InvariantCulture);
            
            return correctDt;
        }
        
        private double CheckWeekDayAndGetTax(Vehicle vehicle,DateTime loopingTime, int arrIndx)
        {
            double taxPrice;
            if (loopingTime.ToString("ddd", new CultureInfo("en-EN")).StartsWith('S'))
            {
                taxPrice = 0;
                return taxPrice;
            }
            taxPrice = vehicle.Tax[GetCircularArrayIndex(arrIndx)];
            return taxPrice;

        }

        private DateTime ChangeDateTimeOfTaxHours(DateTime dtOfTaxChange, DateTime loopingTime, int arrIndx)
        {
            if ((int)dtOfTaxChange.Hour >= 19)
            {
                return AddDateWithHours(loopingTime,ListOfTimes[GetCircularArrayIndex(arrIndx)]).AddDays(1);
                     
            }
            return AddDateWithHours(loopingTime,ListOfTimes[GetCircularArrayIndex(arrIndx)]);
        }

        private double RoundToNearestTenth(double price)
        {
            if (price == 0)
                return 0;
            double pounds = Math.Floor(price);
            double pennies = (price - pounds) * 100;

            if (pennies % 10 > 0 && pennies % 10 < 6)
            {
                pennies = (pennies - (pennies % 10)) / 100;
            }
            else
            {
                pennies = Math.Round(pennies / 100, 1);
            }

            pounds = pounds + pennies;
            return pounds;
        }

        public void AddPriceAndHoursToTotal(DateTime loopingTime, Vehicle vehicle, double priceWithThisTax,
            TimeSpan timeUntilTaxChange)
        {
            
            if (loopingTime.Hour < 12 && priceWithThisTax > 0)
            {
                vehicle.TimeSpent.TotalTimeAm += timeUntilTaxChange;
                vehicle.PriceIs.PriceAm += priceWithThisTax;
            }
            else if(loopingTime.Hour >= 12 && priceWithThisTax > 0)
            {
                vehicle.TimeSpent.TotalTimePm += timeUntilTaxChange;
                vehicle.PriceIs.PricePm += priceWithThisTax;
            }
            
        }

        public void PrintOutReceipt(Vehicle vehicle)
        {
            Console.WriteLine("Charge for " + vehicle.TimeSpent.TotalTimeAm.Hours + "h " + vehicle.TimeSpent.TotalTimeAm.Minutes +"m (AM Rate): £" +
                              String.Format("{0:0.00}", vehicle.PriceIs.PriceAm));
            Console.WriteLine("Charge for " + vehicle.TimeSpent.TotalTimePm.Hours + "h " + vehicle.TimeSpent.TotalTimePm.Minutes + "m (PM Rate): £" +
                              String.Format("{0:0.00}", vehicle.PriceIs.PricePm));

            double totalSum = Math.Round(vehicle.PriceIs.PriceAm + vehicle.PriceIs.PricePm, 1);
            Console.WriteLine("Total Charge: £" + String.Format("{0:0.00}", totalSum));
        }
    }
}
