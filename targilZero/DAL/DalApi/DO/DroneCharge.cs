using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DalApi.DO
{
    /// <summary>
    /// struct for representing drone in charge
    /// </summary>
    public struct DroneCharge 
    {
        /// <summary>
        /// The identity number of the drone in charge
        /// </summary>
        public int droneID { get; set; }
        /// <summary>
        /// The identity number of the station where the drone is being charged
        /// </summary>
        public int stationID { get; set; }
        /// <summary>
        /// The status of the charging - is the charging of the drone is active or not
        /// </summary>
        public bool activeCharge { get; set; }
        /// <summary>
        /// The start time of the charging
        /// </summary>
        public DateTime start { get; set; }
        /// <summary>
        /// The end time of the charging
        /// </summary>
        public DateTime? end { get; set; } 

        //parameters costructor of drone in charge
        public DroneCharge(int did, int sid, bool active,DateTime s)
        {
            droneID = did;
            stationID = sid;
            activeCharge = active;
            start = s;
            end = null;
        }

        //printing details of drone in charge
        public override string ToString()
        {
            return "drone ID: " + droneID + "\nstation ID: " + stationID + '\n';
        }
    }
}
