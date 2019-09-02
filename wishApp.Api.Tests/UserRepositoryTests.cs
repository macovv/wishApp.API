using System;
using Moq;
using wishApp.Api.Data;
using wishApp.Api.Models;
using Xunit;

namespace wishApp.Api.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var user = new User();
            var repo = new Mock<IUserRepository>();
            Console.WriteLine("chuj");
        }
    }
}
