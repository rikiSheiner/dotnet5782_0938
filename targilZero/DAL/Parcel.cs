using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.IDAL.DO;

namespace DAL
{
    public struct Parcel //מבנה לייצוג חבילה
    {
        public int ID
        {
            get; set;
            //get { return ID; }
            //set { ID = value; }
        }
        public int senderID 
        {
            get; set;
            //get { return senderID; }
            //set { senderID = value; }
        }
        public int targetID
        {
            get; set;
            //get { return targetID; }
            //set { targetID = value; }
        }
        public WeightCategories weight
        {
            get; set;
            //get { return weight; }
            //set { weight = value; }
        }
        public Priorities priority
        {
            get; set;
            //get { return priority; }
            //set { priority = value; }
        }
        public DateTime requested
        {
            get; set;
            //get { return requested; }
            //set { requested = value; }
        }
        public int droneID
        {
            get; set;
            //get { return droneID; }
            //set { droneID = value; }
        }

        public DateTime scheduled
        {
            get; set;
            //get { return scheduled; }
            //set { scheduled = value; }
        }
        public DateTime pickedUp
        {
            get; set;
            //get { return pickedUp; }
            //set { pickedUp = value; }
        }
        public DateTime delivered
        {
            get; set;
            //get { return delivered; }
            //set { delivered = value; }
        }

        //parameters constructor of parcel
        public Parcel(int id, int sid, int tid, WeightCategories w, Priorities p, DateTime r,
            int did, DateTime s, DateTime pi, DateTime d) 
        {
            ID = id;
            senderID = sid;
            targetID = tid;
            weight = w;
            priority = p;
            requested = r;
            droneID = did;
            scheduled = s;
            pickedUp = pi;
            delivered = d;
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
