using Nest;

namespace NestExtensions.Tests.Models
{
    public class Location
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public int Area { get; set; }

        public GeoLocation Coordinate { get; set; }
    }
}
