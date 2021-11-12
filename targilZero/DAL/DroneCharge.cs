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
            get;
            set;
        }
        public int stationID
        {
            get;
            set;
        }
        public bool activeCharge //מייצג האם הישות בטעינה או לא
        {
            get;
            set;
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
