using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.BO
{
    public class User
    {
        /// <summary>
        /// The name of the user that he enters to the system to sign in
        /// </summary>
        public string UserName { set; get; }
        /// <summary>
        /// The password of the user that he enters to the system to sign in
        /// </summary>
        public string UserPassword { set; get; }
        /// <summary>
        ///  If the user has a possibility to access to the managment of the system or not
        /// </summary>
        public bool UserAccessManagement { set; get; }
        public override string ToString()
        {
            return "user name= " + UserName + " password= " + UserPassword;
        }
    }

    public class UserToLIst
    {
        /// <summary>
        /// The name of the user that he enters to the system to sign in
        /// </summary>
        public string UserName { set; get; }
        /// <summary>
        /// The password of the user that he enters to the system to sign in
        /// </summary>
        public string UserPassword { set; get; }
        /// <summary>
        ///  If the user has a possibility to access to the managment of the system or not
        /// </summary>
        public bool UserAccessManagement { set; get; }
        /// <summary>
        /// returns string that represents the user
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "user name= " + UserName + " password= " + UserPassword;
        }
    }

}
