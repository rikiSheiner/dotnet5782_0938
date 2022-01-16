using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace BL.BO
{
    public struct DroneCharge : INotifyPropertyChanged
    {
        /// <summary>
        /// The identity number of the drone in chraging 
        /// </summary>
        private int _droneID;
        public int droneID
        {
            get { return _droneID ; }
            set
            {
                _droneID = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("droneID"));
            }
        }
        /// <summary>
        /// The identity number of the station where the drone is being charged
        /// </summary>
        private int _stationID;
        public int stationID
        {
            get { return _stationID; }
            set
            {
                _stationID = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("stationID"));
            }
        }
        /// <summary>
        /// The status of the charging - is the charging of the drone is active or not
        /// </summary>
        private bool _activeCharge;
        public bool activeCharge
        {
            get { return _activeCharge; }
            set
            {
                _activeCharge = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("activeCharge"));
            }
        }
        /// <summary>
        /// The start time of the charging
        /// </summary>
        private DateTime _start;
        public DateTime start
        {
            get { return _start; }
            set
            {
                _start = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("start"));
            }
        }
        /// <summary>
        /// The end time of the charging
        /// </summary>
        public DateTime? _end;
        public DateTime? end
        {
            get { return _end; }
            set
            {
                _end = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("end"));
            }
        }

        /// <summary>
        /// returns the details of the drone in charge
        /// </summary>
        /// string
        public override string ToString()
        {
            return "drone ID: " + droneID + "\nstation ID: " + stationID + '\n';
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
