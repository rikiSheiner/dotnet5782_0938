using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.BO
{
    public class LogicalEntities :INotifyPropertyChanged 
    {
        public class Location :INotifyPropertyChanged //מיקום
        {
            private double _longitude;
            public double longitude
            {
                get { return _longitude; }
                set
                {
                    _longitude = value;
                    if (PropertyChanged != null)
                        PropertyChanged(this, new PropertyChangedEventArgs("longitude"));
                }
            }
            private double _latitude;
            public double latitude
            {
                get { return _latitude; }
                set
                {
                    _latitude = value;
                    if (PropertyChanged != null)
                        PropertyChanged(this, new PropertyChangedEventArgs("latitude"));
                }
            }
            public Location (double lo, double la)
            { 
                _longitude = lo; 
                _latitude = la;
            }
            public override string ToString()
            {
                return '<'+longitude.ToString () +','+latitude.ToString () +'>' ;
            }

            public event PropertyChangedEventHandler PropertyChanged;
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
