using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.IDAL.DO;

namespace DAL
{
    public struct Station //מבנה לייצוג תחנת הטענה
    {
        public int stationID
        {
            get; /*{ return stationID; }*/
            set;/*{ stationID = value; }*/
        }
        public int name
        {
            get; //{ return name; }
            set; //{ name = value; }
        }
        public double longitude
        {
            get;// { return longitude; }
            set;// { longitude = value; }
        }
        public double latitude
        {
            get;// { return latitude; }
            set;// { latitude = value; }
        }
        public int chargeSlots
        {
            get;// { return chargeSlots; }
            set;// { chargeSlots = value; }
        }

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
