using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace AssigmentUNRVLD
{
    public class Time
    {
        private DateTime start;
        private DateTime end;
        private TimeSpan totalTimeAm = TimeSpan.Zero;
        private TimeSpan totalTimePm = TimeSpan.Zero;

        public Time()
        {
            
        }

        public Time(string start, string end)
        {
            this.start = DateTime.ParseExact(start, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
            this.end = DateTime.ParseExact(end, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

            Start = this.start;
            End = this.end;
            TotalTimeAm = totalTimeAm;
            TotalTimePm = totalTimePm;
        }

        public DateTime Start { get; }

        public DateTime End { get; }


        public TimeSpan GetTimeDifference()
        {
            return end - start;
        }

        public TimeSpan TotalTimeAm {get; set; }
        public TimeSpan TotalTimePm {get; set; }
    }
}
