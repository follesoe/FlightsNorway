using System;

namespace FlightsNorway.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime InTimeZone(this DateTime time, int timeZone)
        {
            return time.ToUniversalTime().AddHours(timeZone);
        }
    }
}
