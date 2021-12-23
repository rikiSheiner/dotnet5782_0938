using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.BO
{
    public class Parcel
    {
        /// <summary>
        /// The identity number of the parcel to delivery
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// The identity number of the sender of the parcel
        /// </summary>
        public int senderID { get; set;}
        /// <summary>
        /// The identity number of the accepter of the parcel
        /// </summary>
        public int targetID {get; set; }
        /// <summary>
        /// The weight category of the parcel: if it is light, intermediate or heavy
        /// </summary>
        public Enums.WeightCategories  weight { get; set; }
        /// <summary>
        /// The priority of the delivery of the parcel: if it is normal, quick or emergency case
        /// </summary>
        public Enums.Priorities  priority {get; set;}
        /// <summary>
        /// The time of creation of a parcel for delivery
        /// </summary>
        public DateTime? requested {get; set; }
        /// <summary>
        /// The identity number of the drone that picked up the parcel
        /// </summary>
        public int droneID { get; set;}
        /// <summary>
        /// The time of assignment the package to the drone
        /// </summary>
        public DateTime? scheduled {get; set; }
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

        /// <summary>
        /// returns the details of the parcel
        /// </summary>
        /// string
        public override string ToString()
        {
            string str= "Parcel ID: " + ID + "\nsender ID: " + senderID + "\ntarget ID: " + targetID
                + "\nweight: " + weight + "\npriority: " + priority + "\nrequested: " + requested
                + "\nscheduled: " + scheduled + "\npicked up:" +
                pickedUp + "\ndelivered: " + delivered + '\n';
            if (droneID >-1)
                str += "\ndrone ID: " + droneID;
            return str;
        }
    }

    public class ParcelToList
    {
        public int ID { get; set; }
        public string nameOfSender { get; set; }
        public string nameOfTarget { get; set; }
        public Enums.WeightCategories weight { get; set; }
        public Enums.Priorities priority { get; set; }
        public Enums.ParcelStatuses parcelStatus { get; set; }
        /// <summary>
        /// true if the customer approves that the parcel has been sent , false else
        /// </summary>
        public bool confirmedSending { get; set; }
        /// <summary>
        /// true if the customer approves that the parcel has been recieved , false else
        /// </summary>
        public bool confirmRecieving { get; set; }
        public DAL.DalApi.DO.Drone droneSender { get; set; }
        public override string ToString()
        {
            string str = "Parcel: ID=" + ID + " name of sender=" + nameOfSender;
            if (parcelStatus != Enums.ParcelStatuses.supplied)
                str += " ,name of target=" + nameOfTarget;
            str+= "\nweight=" + weight + " priority=" + priority + " parcel status=" + parcelStatus;
            if (parcelStatus == Enums.ParcelStatuses.collected)
                    str += ", ID of drone sender=" + droneSender.ID;

            str += "\n";
            return str;
        }
    }

    public class ParcelDeliveryOfCustomer
    {
        public int ID { get; set; }
        public Enums.WeightCategories weight { get; set; }
        public Enums.Priorities priority { get; set; }
        public Enums.ParcelStatuses parcelStatus { get; set; }
        public CustomerInParcelDelivery inParcelDelivery { get; set; } //הצד השני של משלוח חבילה

    }

    public class ParcelInDelivery
    {
        public int ID { get; set; }
        public bool isParcelInDelivery { get; set; } //(true)מצב משלוח חבילה: ממתין לאיסוף \ בדרך ליעד
        public Enums.Priorities priority { get; set; }
        public Enums.WeightCategories weight { get; set; }
        public Customer sender { get; set; }
        public Customer target { get; set; }
        public LogicalEntities.Location senderLocation{ get; set; }
        public LogicalEntities.Location targetLocation { get; set; }
        public double distanceTransport { get; set; }
    }
}
