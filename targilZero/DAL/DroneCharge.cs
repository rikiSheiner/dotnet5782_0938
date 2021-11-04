using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.IDAL.DO;

namespace DAL
{
    public struct DroneCharge //מבנה לייצוג הטענת רחפן
    {
        public int droneID
        {
            get;// { return droneID; }
            set;// { droneID = value; }
        }
        public int stationID
        {
            get;// { return stationID; }
            set;// { stationID = value; }
        }
        public bool activeCharge //מייצג האם הישות בטעינה או לא
        {
            get;// { return activeCharge; }
            set;// { activeCharge = value; }
        }

        //parameters costructor of drone in charge
        public DroneCharge(int did, int sid, bool active)
        {
            droneID = did;
            stationID = sid;
            activeCharge = active;
        }

        //printing details of drone in charge
        public override string ToString()
        {
            return "drone ID: " + droneID + "\nstation ID: " + stationID + '\n';
        }
    }
}
