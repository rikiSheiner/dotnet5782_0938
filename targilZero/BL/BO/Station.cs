using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.BO
{
   
    public class Station 
    {
        /// <summary>
        /// The identity number of the station
        /// </summary>
        public int stationID {get; set;}
        /// <summary>
        /// The name of the station
        /// </summary>
        public int name {get; set; }
        /// <summary>
        /// The location of the basis station
        /// </summary>
        public LogicalEntities .Location location { get; set; }
        /// <summary>
        /// The number of available chrage slots of drones in the station
        /// </summary>
        public int chargeSlots {get; set;}
        /// <summary>
        /// List of drones in charge in this station
        /// </summary>
        public List<DroneInCharge> dronesInCharge { get; set; }

        /// <summary>
        /// returns the details of the basis station
        /// </summary>
        /// string
        public override string ToString()
        {
            return "station ID: " + stationID + "\nname: " + name + "\n"+location.ToString ()+ "\ncharge slots: " + chargeSlots + '\n';
        }
    }

    public class StationToList
    {
        public int ID { get; set; }
        public string name { get; set; }
        public int fullChargeSlots { get; set; }
        public int availableChargeSlots { get; set; }
        public List</*DAL.DalApi.DO.Drone*/DroneToList > dronesInCharge { get; set; } 
        public override string ToString()
        {
            string str = "Station: ID=" + ID + ", name=" + name + ", full charge slots=" + fullChargeSlots +
               ", available charge slots=" + availableChargeSlots;
            if(dronesInCharge .Count ()>0)
            {
                str += "\ndrones in charge:";
                foreach (/*DAL.DalApi.DO.Drone*/DroneToList  drone in dronesInCharge )
                {
                    str += " ID="+drone.ID+" ";
                }
                
            }
            str += '\n';
            return str;

        }
    }
}
