using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace BL.BO
{
    public struct Drone 
    {
        /// <summary>
        /// The identity number of the drone
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// The name of the drone's model
        /// </summary>
        public string model { get; set; }
        /// <summary>
        /// The weight category of the drone: if it is light, intermediate or heavy
        /// </summary>
        public Enums.WeightCategories maxWeight { get; set; }
        /// <summary>
        /// The current status of the drone- if it is available, in maintenance or in delievry
        /// </summary>
        public Enums.DroneStatuses droneStatus { get; set; }
        /// <summary>
        /// The battery of the drone in percents (0%-100%)
        /// </summary>
        public int battery { get; set; }
        /// <summary>
        /// The identity number of the station where we want to charge the drone
        /// </summary>
        public int stationID { get; set; }
        /// <summary>
        /// The location of the drone
        /// </summary>
        public LogicalEntities .Location location { get; set; }

        //printing details of drone
        public override string ToString()
        {
            return "Drone ID: " + ID + "\nmodel: " + model + "\nmax weight: " + maxWeight + "\n"
                +"drone status: "+droneStatus +'\n';
        }

    }
    public class DroneToList : INotifyPropertyChanged 
    {
        /// <summary>
        /// The identity number of the drone
        /// </summary>
        private int _ID;
        public int ID
        {
            get { return _ID; }
            set
            {
                _ID = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ID"));
            }
        }
        /// <summary>
        /// The name of the drone's model
        /// </summary>
        private string _model;
        public string model
        {
            get { return _model; }
            set
            {
                _model = value;
                if(PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("model"));
            }
        }
        /// <summary>
        /// The weight category of the drone: if it is light, intermediate or heavy
        /// </summary>
        private Enums.WeightCategories _maxWeight;
        public Enums.WeightCategories maxWeight
        {
            get { return _maxWeight; }
            set
            {
                _maxWeight = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("maxWeight"));
            }
        }
        /// <summary>
        /// The current status of the drone- if it is available, in maintenance or in delievry
        /// </summary>
        private Enums.DroneStatuses _droneStatus;
        public Enums.DroneStatuses droneStatus
        {
            get { return _droneStatus; }
            set
            {
                _droneStatus = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("droneStatus"));
            }
        }
        /// <summary>
        /// The battery of the drone in percents (0%-100%)
        /// </summary>
        private int _battery;
        public int battery
        {
            get { return _battery; }
            set
            {
                _battery = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("battery"));
            }
        }
        /// <summary>
        /// The location of the drone
        /// </summary>
        private LogicalEntities.Location _location;
        public LogicalEntities.Location location
        {
            get { return _location; }
            set
            {
                _location = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("location"));
            }
        }
        /// <summary>
        /// The identity number of the parcel in delivery in this drone 
        /// </summary>
        private int _parcelInDroneID;
        public int parcelInDroneID
        {

            get { return _parcelInDroneID; }
            set
            {
                _parcelInDroneID = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("parcelInDroneID"));
            }
        }

        public override string ToString()
        {
            string str = "Drone: ID=" + ID + ", model=" + model + ", max weight=" + maxWeight + ", status=" + droneStatus
                + ", battery=" + battery + "%, location=" + location ;
            //print number of parcel only if there is parcel in delivery in this drone
            if (parcelInDroneID > -1)
                str += ", id of parcel in drone=" + parcelInDroneID;
            str += '\n';
            return  str;
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class DroneInParcel
    {
        public int ID { get; set; }
        public int battery { get; set; }
        public LogicalEntities.Location loaction { get; set; }
    }

    public class DroneInCharge
    {
        public int ID { get; set; }
        public int battery { get; set; }
    }
}
