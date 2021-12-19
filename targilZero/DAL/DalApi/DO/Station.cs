using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.DalApi;

namespace DAL.DalApi.DO
{
    /// <summary>
    /// struct for representing a station for charging drones
    /// </summary>
    public struct Station 
    {
        /// <summary>
        /// the identity number of station
        /// </summary>
        public int stationID { get; set; }
        /// <summary>
        /// the name of the station
        /// </summary>
        public int name { get; set; }
        /// <summary>
        /// the longitude of thelocation of the station
        /// </summary>
        public double longitude { get; set; }
        /// <summary>
        ///  the latitude of thelocation of the station
        /// </summary>
        public double latitude { get; set; }
        /// <summary>
        /// the number of carge slots in the station
        /// </summary>
        public int chargeSlots { get; set; }

        //parameters constructor of station
        public Station(int id, int n, double lo, double la, int cS)
        {
            stationID = id;
            name = n;
            longitude = lo;
            latitude = la;
            chargeSlots = cS;
        }
        //printing of station's details
        public override string ToString()
        {
            return "station ID: " + stationID + "\nname: " + name + "\nlongitude: " + longitude
                + "\nlatitude: " + latitude + "\ncharge slots: " + chargeSlots + '\n';
        }
    }

}
