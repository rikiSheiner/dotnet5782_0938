using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.IDAL.DO;

namespace DAL
{
    public struct Drone //מבנה לייצוג רחפן
    {
        public int ID
        {
            get;
            set;
        }
        public string model
        {
            get;
            set;
        }
        public WeightCategories maxWeight
        {
            get;
            set;
        }
        public DroneStatuses status
        {
            get;
            set;
        }
        public double battery
        {
            get;
            set;
        }

        //parameters costructor of drone
        public Drone(int id, string m, WeightCategories mw, DroneStatuses s, double b)
        {
            this.ID = id;
            this.model = m;
            this.maxWeight = mw;
            this.status = s;
            this.battery = b;
        }

        //printing details of drone
        public override string ToString()
        {
            return "Drone ID: " + ID + "\nmodel: " + model + "\nmax weight: " + maxWeight
                + "\nstatus: " + status + "\nbattery: " + battery + "\n";
        }

    }
}
