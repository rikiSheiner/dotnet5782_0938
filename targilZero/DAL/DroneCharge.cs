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
        public int droneID { get; set; }
        public int stationID { get; set; }
        public bool activeCharge { get; set; }//מייצג האם הישות בטעינה או לא
        public DateTime start { get; set; } //זמן התחלת הטעינה
        public DateTime? end { get; set; } //זמן סיום הטעינה

        //parameters costructor of drone in charge
        public DroneCharge(int did, int sid, bool active,DateTime s)
        {
            droneID = did;
            stationID = sid;
            activeCharge = active;
            start = s;
            end = null;
        }

        //printing details of drone in charge
        public override string ToString()
        {
            return "drone ID: " + droneID + "\nstation ID: " + stationID + '\n';
        }
    }
}
