using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DalApi.DO
{
    /// <summary>
    /// struct for representing customer
    /// </summary>
    public struct Customer  
    {
        /// <summary>
        /// The identity number of the customer
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// The name of the customer
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// The phone number of the customer
        /// </summary>
        public string phone { get; set; }
        /// <summary>
        /// The longitude of the location of the customer
        /// </summary>
        public double longitude { get; set; }
        /// <summary>
        /// The latitude of the location of the customer
        /// </summary>
        public double latitude { get; set; }

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
