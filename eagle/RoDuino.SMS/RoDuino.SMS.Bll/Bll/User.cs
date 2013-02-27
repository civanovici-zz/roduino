using System;
using System.Linq;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using RoDuino.SMS.Bll.Bll.Base;
using RoDuino.SMS.Bll.Util;

namespace RoDuino.SMS.Bll.Bll
{
    [ActiveRecord("Users")]
    public class User:BaseItem<User>
    {
        [Property,ValidateIsUnique("Username should be unique"),ValidateNonEmpty("Username required")]
        public string Username { get; set; }

        [Property,ValidateNonEmpty("Password required")]
        public string Password { get; set; }

        public static Guid Login(string username, string password)
        {
            Guid newTicket = Guid.Empty;

            IQueryable<User> results = (from u in User.Queryable where (u.Username == username && u.Password==password) select u);
//                ActiveRecordMediator<User>.ExecuteQuery2(
//                    new VRQuery<User>("select u from User u where u.Username = ? and u.Password = ?", username,
//                                          password));
            if (results.Count() > 0)
            {
                newTicket = Guid.NewGuid();
                User user = results.First();
                RoSession.Instance[newTicket] = user;
                Ticket = newTicket;
            }


            return newTicket;
        }
    }
}
