using System.Net.Sockets;

namespace Delivery.API.Domain.Entities;

public class Point
{
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        
        public Point (double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;

        }
        
        
}
        