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
            get;/* { return ID; }*/
            set;/*{ ID = value; }*/
        }
        public string model
        {
            get;/*{ return model; }*/
            set;/*{ model = value; }*/
        }
        public WeightCategories maxWeight
        {
            get; /*{ return maxWeight; }*/
            set;/*{ maxWeight = value; }*/
        }
        public DroneStatuses status
        {
            get; /*{ return status; }*/
            set;/*{ status = value; }*/
        }
        public double battery
        {
            get; /*{ return battery; }*/
            set;/*{ battery = value; }*/
        }

        //parameters costructor of drone
        public Drone(int id, string m, WeightCategories mw, DroneStatuses s, double b)
        {
            ID = id;
            model = m;
            maxWeight = mw;
            status = s;
            battery = b;
        }

        //printing details of drone
        public override string ToString()
        {
            return "Drone ID: " + ID + "\nmodel: " + model + "\nmax weight: " + maxWeight
                + "\nstatus: " + status + "\nbattery: " + battery + "\n";
        }

    }
}
