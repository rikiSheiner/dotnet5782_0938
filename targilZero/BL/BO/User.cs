using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

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

    public class UserToLIst : INotifyPropertyChanged
    {
        /// <summary>
        /// The name of the user that he enters to the system to sign in
        /// </summary>
        private string _UserName;
        public string UserName
        {
            get { return _UserName; }
            set
            {
                _UserName = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("UserName"));
            }
        }
        /// <summary>
        /// The password of the user that he enters to the system to sign in
        /// </summary>
        private string _UserPassword;
        public string UserPassword
        {
            get { return _UserPassword; }
            set
            {
                _UserPassword = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("UserPassword"));
            }
        }
        /// <summary>
        ///  If the user has a possibility to access to the managment of the system or not
        /// </summary>
        private bool _UserAccessManagement;
        public bool UserAccessManagement
        {
            get { return _UserAccessManagement; }
            set
            {
                _UserAccessManagement = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("UserAccessManagement"));
            }
        }
        /// <summary>
        /// returns string that represents the user
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "user name= " + UserName + " password= " + UserPassword;
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }

}
