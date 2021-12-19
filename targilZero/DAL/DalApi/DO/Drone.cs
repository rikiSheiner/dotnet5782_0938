using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.DalApi;


namespace DAL.DalApi.DO
{
    /// <summary>
    /// struct for representing a drone 
    /// </summary>
    public struct Drone 
    {
        /// <summary>
        /// the identity number of the drone
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// the name of the model of the drone
        /// </summary>
        public string model { get; set; }
        /// <summary>
        /// the maximum weight that the drone can carry
        /// </summary>
        public Enums.WeightCategories maxWeight { get; set; }
        //parameters costructor of drone
        public Drone(int id, string m, Enums.WeightCategories mw)
        {
            ID = id;
            model = m;
            maxWeight = mw;
        }

        //printing details of drone
        public override string ToString()
        {
            return "Drone ID: " + ID + "\nmodel: " + model + "\nmax weight: " + maxWeight+ "\n";
        }

    }
}
