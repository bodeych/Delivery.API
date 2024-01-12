using System.Net.Sockets;

namespace Delivery.API.Domain.Entities;

public class Point
{
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        
        public static Point AddCoordinate(double latitude, double longitude)
        {
                var coordinate = new Point
                {
                    Latitude = latitude,
                    Longitude = longitude
                };
                return coordinate;
        }
}
        