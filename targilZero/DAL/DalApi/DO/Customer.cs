using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DalApi.DO
{
    public struct Customer  //מבנה לייצוג לקוח
    {
        public int ID 
        {
            get; 
            set;
        }
        public string name 
        {
            get;
            set;
        }
        public string phone 
        {
            get;
            set;
        }
        public double longitude 
        {
            get;
            set;
        }
        public double latitude 
        {
            get; 
            set;
        }

        //parameters constructor of customer
        public Customer(int id, string n, string p, double lo, double la)
        {
            ID = id;
            name = n;
            phone = p;
            longitude = lo;
            latitude = la;
        }
        //printing details of customer
        public override string ToString()
        {
            return "customer ID: " + ID + "\nname: " + name + "\nphone: " + phone +
                "\nlongitude: " + longitude + "\nlatitude: " + latitude + '\n';
        }
    }
}
