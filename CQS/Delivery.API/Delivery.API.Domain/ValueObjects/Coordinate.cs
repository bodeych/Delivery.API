namespace Delivery.API.Domain.ValueObjects;

public sealed class Coordinate
{
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        
        private Coordinate (double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        private static bool IsValidLatitude(double latitude) => latitude >= -90 && latitude <= 90;
        private static bool IsValidLongitude(double longitude) => longitude >= -180 && longitude <= 180;

        public static Coordinate Create(double latitude, double longitude)
        {
            if (!IsValidLatitude(latitude))
            {
                throw new ArgumentException(nameof(latitude));
            }

            if (!IsValidLongitude(longitude))
            {
                throw new ArgumentException(nameof(longitude));
            }

            var coordinate = new Coordinate(latitude, longitude);

            return coordinate;
        }
}
        