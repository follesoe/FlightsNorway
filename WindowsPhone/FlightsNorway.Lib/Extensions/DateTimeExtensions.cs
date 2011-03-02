using System;

namespace FlightsNorway.Lib.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime InTimeZone(this DateTime time, int timeZone)
        {
            return time.ToUniversalTime().AddHours(timeZone);
        }
    }
}
