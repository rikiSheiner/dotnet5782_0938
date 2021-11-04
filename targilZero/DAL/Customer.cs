using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public struct Customer  //מבנה לייצוג לקוח
    {
        public int ID 
        {
            get; set;
            //get { return ID; }
            //set { ID = value; }
        }
        public string name 
        {
            get; set;
            //get { return name; }
            //set { name = value; }
        }
        public string phone 
        {
            get; set;
            //get { return phone; }
            //set { phone = value; }
        }
        public double longitude 
        {
            get; set;
            //get { return longitude; }
            //set { longitude = value; }
        }
        public double latitude 
        {
            get; set;
            //get { return latitude; }
            //set { latitude = value; }
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
