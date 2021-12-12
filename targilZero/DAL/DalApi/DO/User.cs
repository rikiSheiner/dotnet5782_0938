using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DalApi.DO
{
    public class User
    {
        /// <summary>
        /// the user name
        /// </summary>
        public string UserName { set; get; }
        /// <summary>
        /// the password of the user
        /// </summary>
        public string UserPassword { set; get; }
        /// <summary>
        /// the access to management of the user
        /// </summary>
        public bool UserAccessManagement { set; get; }
        public User(string name, string password, bool access)
        {
            UserName = name;
            UserPassword = password;
            UserAccessManagement = access;
        }
        public User()
        {
        }
        public override string ToString()
        {
            return "user name= " + UserName + " password= " + UserPassword;
        }
    }
}
