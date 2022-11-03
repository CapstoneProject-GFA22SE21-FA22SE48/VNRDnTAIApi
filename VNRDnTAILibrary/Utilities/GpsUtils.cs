using System;

namespace VNRDnTAILibrary.Utilities
{
    public static class GpsUtils
    {
        private static double EarthRadiusInKilometers = 6371.0;
        private static double EarthRadiusInMeters = 6371000.0;

        public static double ToRadian(this double d)
        {
            return d * (Math.PI / 180);
        }

        public static double DiffRadian(this double val1, double val2)
        {
            return ToRadian(val2) - ToRadian(val1);
        }
        public static double GetDistance(double originLatitude, double originLongitude, double destinationLatitude, double destinationLongitude, string distanceUnit)
        {
            int decimalPlaces = 1;
            double radius = GetRadius(distanceUnit);
            return Math.Round(
                    radius * 2 *
                    Math.Asin(Math.Min(1,
                                       Math.Sqrt(
                                           (Math.Pow(Math.Sin(DiffRadian(originLatitude, destinationLatitude) / 2.0), 2.0) +
                                            Math.Cos(ToRadian(originLatitude)) * Math.Cos(ToRadian(destinationLatitude)) *
                                            Math.Pow(Math.Sin((DiffRadian(originLongitude, destinationLongitude)) / 2.0),
                                                     2.0))))), decimalPlaces);
        }

        private static double GetRadius(string distanceUnit)
        {
            switch (distanceUnit.ToUpper())
            {
                case "KM":
                    return EarthRadiusInKilometers;
                case "M":
                    return EarthRadiusInMeters;
                default:
                    return EarthRadiusInKilometers;
            }
        }
    }
}
