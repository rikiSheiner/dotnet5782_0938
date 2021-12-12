using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.DalApi;

namespace DAL.DalApi.DO
{
    public struct Parcel //מבנה לייצוג חבילה
    {
        public int ID
        {
            get;
            set;
        }
        public int senderID 
        {
            get; 
            set;
        }
        public int targetID
        {
            get; 
            set;
        }
        public Enums.WeightCategories weight
        {
            get; 
            set;
        }
        public Enums.Priorities priority
        {
            get; 
            set;
        }
        public DateTime? requested
        {
            get;
            set;
        }
        public int droneID
        {
            get;
            set;
        }

        public DateTime? scheduled
        {
            get; 
            set;
        }
        public DateTime? pickedUp
        {
            get; 
            set;
        }
        public DateTime? delivered
        {
            get;
            set;
        }

        //parameters constructor of parcel
        public Parcel(int id, int sid, int tid, Enums.WeightCategories w, Enums.Priorities p, int did) 
        {
            ID = id;
            senderID = sid;
            targetID = tid;
            weight = w;
            priority = p;
            requested = DateTime.Now ;  //זמן יצירת החבילה 
            droneID = did;
            scheduled = null;
            pickedUp = null;
            delivered = null; 
        }

        //printing details of parcel
        public override string ToString()
        {
            return "Parcel ID: " + ID + "\nsender ID: " + senderID + "\ntarget ID: " + targetID
                + "\nweight: " + weight + "\npriority: " + priority + "\nrequested: " + requested
                + "\ndrone ID: " + droneID + "\nscheduled: " + scheduled + "\npicked up:" +
                pickedUp + "\ndelivered: " + delivered + '\n';
        }
    }

}
