using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using RoDuino.SMS.Bll;
using RoDuino.SMS.Bll.Bll;
using RoDuino.SMS.Tests.Base;

namespace RoDuino.SMS.Tests.Unit
{
    [TestFixture]
    public class UserTests:BaseFactoryTest
    {

        [Test]
        public void CRUDUserTest()
        {
            var users = from us in User.Queryable select us;
            Assert.AreEqual(0,users.Count());

            User u = CreateUser("admin");

            users = from us in User.Queryable select us;
            Assert.AreEqual(1, users.Count());
            Assert.AreEqual("admin",users.First().Username);
            Assert.AreEqual("admin",users.First().Password);
            


        }
    }
}
