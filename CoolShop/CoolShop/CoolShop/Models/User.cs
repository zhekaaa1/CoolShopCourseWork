using System;
using System.Collections.Generic;
using System.Text;

namespace CoolShop.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FName { get; set; }
        public string IName { get; set; }
        public string OName { get; set; }
        public string Mail { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }

        public override bool Equals(object obj)
        {
            User user = obj as User;
            return Id == user.Id;
        }
    }
    public static class LoggedUser
    {
        public static User User { get; set; }
    }
}
