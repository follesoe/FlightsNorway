using System;

namespace FlightsNorway.Phone.Model
{
    public class Location
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public Location()
        {
            
        }

        public Location(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        // Implementation of the Haversine Forumla (http://en.wikipedia.org/wiki/Haversine_formula)
        public double DistanceTo(Location to)
        {
            double R = 6378.1; // radius of earth.

            double dLat = ToRadian(to.Latitude - Latitude);
            double dLon = ToRadian(to.Longitude - Longitude);

            double a =
                Math.Sin(dLat/2.0)*Math.Sin(dLat/2.0) +
                Math.Cos(ToRadian(Latitude))*Math.Cos(ToRadian(to.Latitude))*
                Math.Sin(dLon/2.0)*Math.Sin(dLon/2.0);

            double c = 2.0*Math.Asin(Math.Min(1, Math.Sqrt(a)));
            double d = R*c;
            return d;
        }

        private static double ToRadian(double value)
        {
            return (Math.PI/180.0)*value;
        }
    }
}
