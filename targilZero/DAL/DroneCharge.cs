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
        public DroneCharge(int did, int sid)
        {
            this.droneID = did;
            this.stationID = sid;
        }
        public override string ToString()
        {
            return "drone ID: " + droneID + "\nstation ID: " + stationID + '\n';
        }
    }
}
