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
        public WeightCategories weight
        {
            get;
            set;
        }
        public Priorities priority
        {
            get;
            set;
        }
        public DateTime requested
        {
            get;
            set;
        }
        public int droneID
        {
            get;
            set;
        }

        public DateTime scheduled
        {
            get;
            set;
        }
        public DateTime pickedUp
        {
            get;
            set;
        }
        public DateTime delivered
        {
            get;
            set;
        }

        //parameters constructor of parcel
        public Parcel(int id, int sid, int tid, WeightCategories w, Priorities p, DateTime r,
            int did, DateTime s, DateTime pi, DateTime d)
        {
            this.ID = id;
            this.senderID = sid;
            this.targetID = tid;
            this.weight = w;
            this.priority = p;
            this.requested = r;
            this.droneID = did;
            this.scheduled = s;
            this.pickedUp = pi;
            this.delivered = d;
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
