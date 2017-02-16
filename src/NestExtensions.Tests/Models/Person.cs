using System.Collections.Generic;

namespace NestExtensions.Tests.Models
{
    public class Person
    {
        public Friend BestFriend { get; set; }

        public IEnumerable<Friend> OtherFriends { get; set; }
    }

    public class Friend
    {
        public string Name { get; set; }
    }
}
