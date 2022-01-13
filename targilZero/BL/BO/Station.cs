using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.BO
{
   
    public class Station : INotifyPropertyChanged 
    {
        /// <summary>
        /// The identity number of the station
        /// </summary>
        private int _stationID;
        public int stationID
        {
            get { return _stationID; }
            set
            {
                _stationID = value;
                if(PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("stationID"));
            }
        }
        /// <summary>
        /// The name of the station
        /// </summary>
        private int _name;
        public int name
        {
            get { return _name; }
            set
            {
                _name = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("name"));
            }
        }
        /// <summary>
        /// The location of the basis station
        /// </summary>
        private LogicalEntities.Location _location;
        public LogicalEntities .Location location
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
        /// The number of available chrage slots of drones in the station
        /// </summary>
        private int _chargeSlots;
        public int chargeSlots
        {
            get { return _chargeSlots; }
            set
            {
                _chargeSlots = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("chargeSlots"));
            }
        }
        /// <summary>
        /// List of drones in charge in this station
        /// </summary>
        private List<DroneInCharge> _dronesInCharge;
        public List<DroneInCharge> dronesInCharge
        {
            get { return _dronesInCharge; }
            set
            {
                _dronesInCharge = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("dronesInCharge"));
            }
        }

        /// <summary>
        /// returns the details of the basis station
        /// </summary>
        /// string
        public override string ToString()
        {
            return "station ID: " + stationID + "\nname: " + name + "\n"+location.ToString ()+ "\ncharge slots: " + chargeSlots + '\n';
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class StationToList : INotifyPropertyChanged
    {
        private int _id;
        public int ID 
        {
            get { return _id; }
            set
            {
                _id = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ID"));
            }
        }
        private string _name;
        public string name
        {
            get { return _name; }
            set
            {
                _name = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("name"));
            }
        }
        private int _fullChargeSlots;
        public int fullChargeSlots
        {
            get { return _fullChargeSlots; }
            set
            {
                _fullChargeSlots = value;
                if(PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("fullChargeSlots"));
            }
        }
        private int _availableChargeSlots;
        public int availableChargeSlots
        {
            get { return _availableChargeSlots; }
            set
            {
                _availableChargeSlots = value;
                if(PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("availableChargeSlots"));
            }
        }
        private List<DroneToList> _dronesInCharge;
        public List<DroneToList > dronesInCharge
        {
            get { return _dronesInCharge; }
            set
            {
                _dronesInCharge = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("dronesInCharge"));
            }
        }
        public override string ToString()
        {
            string str = "Station: ID=" + ID + ", name=" + name + ", full charge slots=" + fullChargeSlots +
               ", available charge slots=" + availableChargeSlots;
            if(dronesInCharge .Count ()>0)
            {
                str += "\ndrones in charge:";
                foreach (DroneToList  drone in dronesInCharge )
                {
                    str += " ID="+drone.ID+" ";
                }
                
            }
            str += '\n';
            return str;

        }
        public event PropertyChangedEventHandler PropertyChanged;
    }

}
