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
            get;
            set;
        }
        public int name
        {
            get;
            set; 
        }
        public double longitude
        {
            get;
            set;
        }
        public double latitude
        {
            get;
            set;
        }
        public int chargeSlots
        {
            get;
            set;
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
