using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.IBL.BO
{
    public struct Drone //מבנה לייצוג רחפן
    {
        /// <summary>
        /// The identity number of the drone
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// The name of the drone's model
        /// </summary>
        public string model { get; set; }
        /// <summary>
        /// The weight category of the drone: if it is light, intermediate or heavy
        /// </summary>
        public Enums.WeightCategories maxWeight { get; set; }
        /// <summary>
        /// The current status of the drone- if it is available, in maintenance or in delievry
        /// </summary>
        public Enums.DroneStatuses droneStatus { get; set; }
        /// <summary>
        /// The battery of the drone in percents (0%-100%)
        /// </summary>
        public int battery { get; set; }
        /// <summary>
        /// The identity number of the station where we want to charge the drone
        /// </summary>
        public int stationID { get; set; }
        /// <summary>
        /// The location of the drone
        /// </summary>
        public LogicalEntities .Location location { get; set; }

        //printing details of drone
        public override string ToString()
        {
            return "Drone ID: " + ID + "\nmodel: " + model + "\nmax weight: " + maxWeight + "\n"
                +"drone status: "+droneStatus +'\n';
        }

    }

    public class DroneToList
    {
        /// <summary>
        /// The identity number of the drone
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// The name of the drone's model
        /// </summary>
        public string model { get; set; }
        /// <summary>
        /// The weight category of the drone: if it is light, intermediate or heavy
        /// </summary>
        public Enums.WeightCategories maxWeight { get; set; }
        /// <summary>
        /// The current status of the drone- if it is available, in maintenance or in delievry
        /// </summary>
        public Enums.DroneStatuses droneStatus { get; set; }
        /// <summary>
        /// The battery of the drone in percents (0%-100%)
        /// </summary>
        public int battery { get; set; }
        /// <summary>
        /// The location of the drone
        /// </summary>
        public LogicalEntities.Location location { get; set; }
        /// <summary>
        /// The identity number of the parcel in delivery in this drone 
        /// </summary>
        public int parcelInDroneID { get; set; }

        public override string ToString()
        {
            string str = "Drone: ID=" + ID + ", model=" + model + ", max weight=" + maxWeight + ", status=" + droneStatus
                + ", battery=" + battery + ", location=" + location ;
            //print number of parcel only if there is parcel in delivery in this drone
            if (parcelInDroneID > -1)
                str += (" id of parcel in drone=" + parcelInDroneID);
            str += '\n';
            return  str;
        }
    }

    public class DroneInParcel
    {
        public int ID { get; set; }
        public int battery { get; set; }
        public LogicalEntities.Location loaction { get; set; }
    }

    public class DroneInCharge
    {
        public int ID { get; set; }
        public int battery { get; set; }
    }
}
