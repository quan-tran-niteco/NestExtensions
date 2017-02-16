using Nest;
using NestExtensions.Sorts;
using NestExtensions.Tests.Models;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace NestExtensions.Tests.Extensions
{
    public class ElasticSearchExtensionsTest
    {
        [Fact]
        public void ToSearchRequest_Page_Should_Be_Correct()
        {
            var parameters = new SearchParameters<object>()
            {
                Start = 5,
                Take = 10
            };

            var searchRequest = parameters.ToSearchRequest();

            Assert.Equal(5, searchRequest.From.Value);
            Assert.Equal(10, searchRequest.Size.Value);
        }

        [Fact]
        public void ToSearchRequest_Sort_Should_Be_Correct_Describe()
        {
            var parameters = new SearchParameters<Location>()
            {
                Sorts = new List<Sort<Location>>()
                {
                    new Sort<Location>
                    {
                        Path = l => l.Name,
                        SortOrder = SortOrder.Ascending
                    }
                }
            };

            var searchRequest = parameters.ToSearchRequest();
            var sort = searchRequest.Sort.First();

            Assert.Equal(parameters.Sorts.First().Path, sort.SortKey.Expression);
            Assert.Equal(SortOrder.Ascending, sort.Order.Value);
        }

        [Fact]
        public void ToSearchRequest_GeoDistanceSort_Should_Be_Correct_Describe()
        {
            var parameters = new SearchParameters<Location>()
            {
                Sorts = new List<Sort<Location>>()
                {
                    new GeoDistanceSort<Location>
                    {
                        Path = l => l.Coordinate,
                        DistanceType = GeoDistanceType.Plane,
                        FromLocation = new GeoLocation(22.2, 33.3),
                        SortOrder = SortOrder.Ascending,
                        Unit = DistanceUnit.Kilometers
                    }
                }
            };

            var searchRequest = parameters.ToSearchRequest();
            var sort = searchRequest.Sort.First();

            //不同于普通的Sort
            Assert.Equal("_geo_distance", sort.SortKey.Name);

            var geoDistanceSort = (IGeoDistanceSort)sort;
            Assert.Equal(GeoDistanceType.Plane, geoDistanceSort.DistanceType.Value);
            Assert.Equal(SortOrder.Ascending, geoDistanceSort.Order.Value);
            Assert.Equal(DistanceUnit.Kilometers, geoDistanceSort.GeoUnit.Value);
            Assert.Equal(new GeoLocation(22.2, 33.3), geoDistanceSort.Points.First());
        }

        [Fact]
        public void ToSearchRequest_Sorts_Should_Be_Null_When_InputSorts_IsNull()
        {
            var parameters = new SearchParameters<object>();
            var searchRequest = parameters.ToSearchRequest();

            Assert.Null(searchRequest.Sort);
        }
    }
}
