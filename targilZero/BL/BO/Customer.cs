using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    public class CustomerToList  //לקוח לרשימה
    {
        public int ID { get; set; }  //מספר מזהה ייחודי
        public string name { get; set; } //שם הלקוח
        public string phoneNumber { get; set; } //טלפון
        public int numParcelsSentAndDelivered { get; set; } //מספר חבילות ששלח וסופקו
        public int numParcelsSentNotDelivered { get; set; } //מספר חבילות ששלח אך עוד לא סופקו
        public int numParcelsRecieved { get; set; } //מספר חבילות שקיבל
        public int numParcelsInDelivery { get; set; } //מספר חבילות שבדרך אל הלקוח

        public override string ToString()
        {
            return "Customer: name="+name+" phone number= 0"+phoneNumber +" number of parcels delivered="+numParcelsSentAndDelivered 
                +"\nnumber of parcels not delivered="+numParcelsSentNotDelivered +" number of parcels accepted="+numParcelsRecieved 
                +" number of parcels in delivery="+numParcelsInDelivery +"\n";
        }
    }

    public class CustomerInParcelDelivery //לקוח במשלוח חבילה
    {
        public int ID { get; set; } //מספר מזהה ייחודי
        public string name { get; set; } //שם הלקוח
        public CustomerInParcelDelivery(int id, string n) { ID = id; name = n; }

    }

}
