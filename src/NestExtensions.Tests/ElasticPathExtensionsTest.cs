using NestExtensions.Tests.Models;
using System;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace NestExtensions.Tests.Extensions
{
    public class ElasticPathExtensionsTest
    {
        [Fact]
        public void ToPathString_Should_Return_Correct_Path()
        {
            Expression<Func<Person, object>> expression = p => p.BestFriend.Name;
            var path = expression.ToPathString();

            Assert.Equal("bestFriend.name", path);
        }

        [Fact]
        public void ToPathString_Should_Return_Correct_Path_When_Have_IEnumerable_Property()
        {
            Expression<Func<Person, object>> expression = p => p.OtherFriends.First().Name;
            var path = expression.ToPathString();

            Assert.Equal("otherFriends.name", path);
        }

        [Fact]
        public void ToPathString_Should_Throw_ArguementNullException_When_Expression_IsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                Expression<Func<Person, object>> expression = null;
                var path = expression.ToPathString();
            });
        }
    }
}
