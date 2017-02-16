using Nest;
using System;

namespace NestExtensions.Sorts
{
    public class GeoDistanceSort<TDocument> : Sort<TDocument> where TDocument : class
    {
        public GeoDistanceType DistanceType { get; set; } = GeoDistanceType.SloppyArc;

        public DistanceUnit Unit { get; set; } = DistanceUnit.Kilometers;

        public GeoLocation FromLocation { get; set; }

        public override SortDescriptor<TDocument> DescribeTo(SortDescriptor<TDocument> sortDescriptor)
        {
            if (sortDescriptor == null)
            {
                throw new ArgumentNullException(nameof(sortDescriptor));
            }

            return sortDescriptor.GeoDistance(sortGeoDescriptor => sortGeoDescriptor
                .Field(Path)
                .DistanceType(DistanceType)
                .Unit(Unit)
                .PinTo(FromLocation)
                .Order(SortOrder));
        }
    }
}
