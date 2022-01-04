using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.DalApi;

namespace DAL.DalApi.DO
{
    /// <summary>
    /// struct for representing a parcel
    /// </summary>
    public struct Parcel 
    {
        /// <summary>
        /// The identity number of the parcel to delivery
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// The identity number of the sender of the parcel
        /// </summary>
        public int senderID { get; set; }
        /// <summary>
        /// The identity number of the accepter of the parcel
        /// </summary>
        public int targetID { get; set; }
        /// <summary>
        /// The weight category of the parcel: if it is light, intermediate or heavy
        /// </summary>
        public Enums.WeightCategories weight { get; set; }
        /// <summary>
        /// The priority of the delivery of the parcel: if it is normal, quick or emergency case
        /// </summary>
        public Enums.Priorities priority { get; set; }
        /// <summary>
        /// The time of creation of a parcel for delivery
        /// </summary>
        public DateTime? requested { get; set; }
        /// <summary>
        /// The identity number of the drone that picked up the parcel
        /// </summary>
        public int droneID { get; set; }
        /// <summary>
        /// The time of assignment the package to the drone
        /// </summary>
        public DateTime? scheduled { get; set; }
        /// <summary>
        /// The package collection time from the sender
        /// </summary>
        public DateTime? pickedUp { get; set; }
        /// <summary>
        /// The time of arrival of the package to the recipient
        /// </summary>
        public DateTime? delivered { get; set; }
        /// <summary>
        /// true if the customer approves that the parcel has been sent , false else
        /// </summary>
        public bool confirmedSending { get; set; }
        /// <summary>
        /// true if the customer approves that the parcel has been recieved , false else
        /// </summary>
        public bool confirmRecieving { get; set; }

        //parameters constructor of parcel
        public Parcel(int id, int sid, int tid, Enums.WeightCategories w, Enums.Priorities p) 
        {
            ID = id;
            senderID = sid;
            targetID = tid;
            weight = w;
            priority = p;
            requested = DateTime.Now ;  //זמן יצירת החבילה 
            droneID = -1;
            scheduled = null;
            pickedUp = null;
            delivered = null;
            confirmedSending = false;
            confirmRecieving = false;
        }

        //printing details of parcel
        public override string ToString()
        {
            DateTime one = new DateTime(1, 1, 1);
            string str = "Parcel ID: " + ID + "\nsender ID: " + senderID + "\ntarget ID: " + targetID
                + "\nweight: " + weight + "\npriority: " + priority + "\nrequested: " + requested;
            if (scheduled!= null && scheduled != one)
            {
                str += "\nscheduled: " + scheduled;
                /*if(pickedUp != null && pickedUp !=one)
                {
                    str += "\npicked up:" + pickedUp;
                    if(delivered != null && delivered !=one)
                        str+= "\ndelivered: " + delivered + '\n';
                } */   
            }
            if (droneID >-1)
                str += "drone ID: " + droneID;
            return str;
        }
    }

}
