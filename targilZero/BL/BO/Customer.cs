using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace BL.BO
{
    public class Customer
    {
        /// <summary>
        /// The identity number of the customer
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// The name of the customer
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// The phone number of the customer
        /// </summary>
        public string phone { get; set; }
        /// <summary>
        /// The current location of the customer
        /// </summary>
        public LogicalEntities.Location location { get; set; }
        /// <summary>
        /// List of parcels in delivery from the customer
        /// </summary>
        public List <ParcelToList > parcelsFromCustomer { get; set; }
        /// <summary>
        /// List of parcels in delivery to the customer
        /// </summary>
        public List<ParcelToList > parcelsToCustomer { get; set; }

        /// <summary>
        /// returns the details of the customer
        /// </summary>
        /// string
        public override string ToString()
        {
            return "customer ID: " + ID + "\nname: " + name + "\nphone: " + phone +
                "\n"+location .ToString ()+"\n";
        }
    }

    public class CustomerToList : INotifyPropertyChanged  //לקוח לרשימה
    {
        private int _id;
        public int ID //מספר מזהה ייחודי
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
        public string name  //שם הלקוח
        {
            get { return _name; }
            set
            {
                _name = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("name"));
            }
        }
        private string _phoneNumber;
        public string phoneNumber //מספר טלפון
        {
            get { return _phoneNumber; }
            set
            {
                _phoneNumber = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("phoneNumber"));
            }
        }
        private int _numParcelsSentAndDelivered;
        public int numParcelsSentAndDelivered  //מספר חבילות ששלח וסופקו
        {
            get { return _numParcelsSentAndDelivered; }
            set
            {
                _numParcelsSentAndDelivered = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("numParcelsSentAndDelivered"));
            }
        }
        private int _numParcelsSentNotDelivered;
        public int numParcelsSentNotDelivered  //מספר חבילות ששלח אך עוד לא סופקו
        {
            get { return _numParcelsSentNotDelivered; }
            set
            {
                _numParcelsSentNotDelivered = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("numParcelsSentNotDelivered"));
            }
        }
        private int _numParcelsRecieved;
        public int numParcelsRecieved  //מספר חבילות שקיבל
        {
            get { return _numParcelsRecieved; }
            set
            {
                _numParcelsRecieved = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("numParcelsRecieved"));
            }
        }
        private int _numParcelsInDelivery;
        public int numParcelsInDelivery  //מספר חבילות שבדרך אל הלקוח
        {
            get { return _numParcelsInDelivery; }
            set
            {
                _numParcelsInDelivery = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("numParcelsInDelivery"));
            }
        }
        public override string ToString()
        {
            return "Customer: name="+name+", phone= "+phoneNumber +", number of parcels delivered="+numParcelsSentAndDelivered 
                +"\nnumber of parcels not delivered="+numParcelsSentNotDelivered +", number of parcels accepted="+numParcelsRecieved 
                +", number of parcels in delivery="+numParcelsInDelivery +"\n";
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }


}
