using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.IBL.BO
{
    public struct DroneCharge //מחלקה לייצוג הטענת רחפן
    {
        /// <summary>
        /// The identity number of the drone in chraging 
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
        public DateTime end { get; set; } 

        /// <summary>
        /// returns the details of the drone in charge
        /// </summary>
        /// string
        public override string ToString()
        {
            return "drone ID: " + droneID + "\nstation ID: " + stationID + '\n';
        }
    }
}
