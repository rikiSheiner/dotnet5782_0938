using System;
using System.IO;
using DAL.DalObject;
using DAL.DalApi;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml.Serialization;



/*name: Rivka Sheiner
 * id: 324060938
 * course: .net
 * exercise number: 2
 */

namespace ConsoleUI
{
    class Program
    {
       /* public static void Adding(DalObject datasource)
        {
            int secondChoose;

            Console.WriteLine("Enter 1 for new basis station.\nEnter 2 for new drone." +
                        "\nEnter 3 for new customer.\nEnter 4 for new parcel.");
            secondChoose = int.Parse(Console.ReadLine());
            try
            {
                switch (secondChoose)
                {
                    case 1:
                        int id, n, cs;
                        double lo, la;

                        Console.WriteLine("enter id, name, longitude, latitude and number of charge slots of station.");

                        id = int.Parse(Console.ReadLine());
                        n = int.Parse(Console.ReadLine());
                        lo = double.Parse(Console.ReadLine());
                        la = double.Parse(Console.ReadLine());
                        cs = int.Parse(Console.ReadLine());

                        datasource.AddStation(id, n, lo, la, cs);
                        break;

                    case 2:
                        int droneID, w;
                        string droneName;

                        Console.WriteLine("enter id, name, weight category");

                        droneID = int.Parse(Console.ReadLine());
                        droneName = Console.ReadLine();
                        w = int.Parse(Console.ReadLine());

                        datasource.AddDrone(droneID, droneName, w);
                        break;

                    case 3:
                        int customerID;
                        string customerName, p;
                        double loCustomer, laCustomer;

                        Console.WriteLine("enter id, name, phone number, longitude and latitude of customer.");

                        customerID = int.Parse(Console.ReadLine());
                        customerName = Console.ReadLine();
                        p = Console.ReadLine();
                        loCustomer = double.Parse(Console.ReadLine());
                        laCustomer = double.Parse(Console.ReadLine());

                        datasource.AddCustomer(customerID, customerName, p, loCustomer, laCustomer);
                        break;

                    case 4:
                        int sid, tid, did, parcelWeight, priority;

                        Console.WriteLine("enter sender id, target id, weight, priority, drone id. ");

                        sid = int.Parse(Console.ReadLine());
                        tid = int.Parse(Console.ReadLine());
                        parcelWeight = int.Parse(Console.ReadLine());
                        priority = int.Parse(Console.ReadLine());
                        did = int.Parse(Console.ReadLine());
                        
                        datasource.AddParcel(sid, tid, parcelWeight, priority ,did);
                        break;
                }
            }
            catch(ExistIdException eID)
            {
                Console.WriteLine(eID.Message );
            }
            catch(Exception)
            {
                Console.WriteLine("ERROR");
            }

        }
        public static void Updating(DalObject datasource)
        {
            int secondChoose;
            Console.WriteLine("Enter 1 for assigning of parcel.\nEnter 2 for collecting of parcel.\n" +
                        "Enter 3 for parcel delivery.\nEnter 4 for drone charging.\n" +
                        "Enter 5 for release from charging.\n");
            secondChoose = int.Parse(Console.ReadLine());

            try
            {
                switch (secondChoose)
                {
                    case 1:

                        Console.Write("enter ID of parcel: ");
                        int parcelID = int.Parse(Console.ReadLine());
                        Console.Write("enter ID of the matching drone: ");
                        int droneId = int.Parse(Console.ReadLine());

                        datasource.ParcelToDrone(parcelID ,droneId );
                        break;
                    case 2:

                        Console.Write("enter ID of parcel for collecting: ");
                        int parcelId = int.Parse(Console.ReadLine());

                        Console.Write("enter ID of the drone: ");
                        int collectorId = int.Parse(Console.ReadLine());

                        datasource.ParcelCollection(parcelId , collectorId );
                        break;
                    case 3:

                        Console.Write("enter ID of parcel for delivery: ");
                        int deliveredID = int.Parse(Console.ReadLine());

                        Console.Write("enter ID of customer: ");
                        int customerId = int.Parse(Console.ReadLine());

                        datasource.DeliveryParcel(deliveredID ,customerId );
                        break;
                    case 4:
                        Console.Write("enter station ID: ");
                        int sID = int.Parse(Console.ReadLine());

                        Console.Write("enter ID of drone for charging: ");
                        int chargedId = int.Parse(Console.ReadLine());

                        datasource.CreateDroneCharge(sID,chargedId );
                        break;
                    case 5:
                        Console.Write("enter drone ID for ending of charging: ");
                        int dID = int.Parse(Console.ReadLine()); //קבלת מספר הרחפן לשחרור
                        datasource.EndDroneCharge(dID,1);
                        break;
                }
            }
            catch(ObjectNotFoundException onf)
            {
                Console.WriteLine(onf.Message );
            }
            catch(ExistIdException eID)
            {
                Console.WriteLine(eID .Message );
            }
            catch(Exception)
            {
                Console.WriteLine("ERROR");
            }
        }
        public static void ItemPresent(DalObject datasource)
        {
            int secondChoose, id;
            Console.WriteLine("Enter 1 for presentation of basis station.\nEnter 2 for presentation of drone." +
                        "\nEnter 3 for presentation of customer.\nEnter 4 for presentation of parcel.");
            secondChoose = int.Parse(Console.ReadLine());
            Console.Write("enter ID number: ");
            id=int.Parse (Console .ReadLine());

            try
            {
                switch (secondChoose)
                {
                    case 1:
                        Console.WriteLine(((List<Station>)datasource.GetBasisStations())[datasource.FindStation(id)]);
                        break;
                    case 2:
                        Console.WriteLine(((List<Drone>)datasource.GetDrones())[datasource.FindDrone(id)]);
                        break;
                    case 3:
                        Console.WriteLine(((List<Customer>)datasource.GetCustomers())[datasource.FindCustomer(id)]);
                        break;
                    case 4:
                        Console.WriteLine(((List<Parcel>)datasource.GetParcels())[datasource.FindParcel(id)]);
                        break;
                }
            }
            catch (ObjectNotFoundException onf)
            {
                Console.WriteLine(onf.Message);
            }
            catch (ExistIdException eID)
            {
                Console.WriteLine(eID.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR");
            }

        }
        public static void ListsPresent(DalObject  datasource)
        {
            int secondChoose;
            
            Console.WriteLine("Enter 1 for list of basis stations.\nEnter 2 for list of drones." +
                        "\nEnter 3 for list of customers.\nEnter 4 for list of parcels\n" +
                        "Enter 5 for list of parcels without drone.\nEnter 6 for available basis stations.");
            
            secondChoose = int.Parse(Console.ReadLine());
            
            switch (secondChoose)
            {
                case 1:
                    foreach (Station stationToPrint in datasource .GetBasisStations())
                    {
                        Console.WriteLine(stationToPrint);
                    }
                    break;
                case 2:
                    foreach (Drone droneToPrint in datasource .GetDrones ())
                    {
                        Console.WriteLine(droneToPrint);
                    }
                    break;
                case 3:
                    foreach (Customer customerToPrint in datasource .GetCustomers ())
                    {
                        Console.WriteLine(customerToPrint);
                    }
                    break;
                case 4:
                    foreach (Parcel parcelToPrint in datasource .GetParcels())
                    {
                        Console.WriteLine(parcelToPrint);
                    }
                    break;
                case 5:
                    foreach (Parcel parcelToPrint in datasource .GetParcels ())
                    {
                        if (parcelToPrint.droneID <= 0) //בהנחה שמספר זהות תקין גדול מ-0
                            Console.WriteLine(parcelToPrint);
                    }
                    break;
                case 6:
                    foreach (Station stationToPrint in datasource .GetBasisStations ())
                    {
                        if (stationToPrint.chargeSlots > 0)
                            Console.WriteLine(stationToPrint);
                    }
                    break;
            }

        }
        public static void Menu()
        {
            DalObject dalobject = new();

            int mainChoose;
            Console.WriteLine("menu:\nEnter 1 for adding.\nEnter 2 for updating.\n" +
                "Enter 3 for item's presentation.\nEnter 4 for presentation of lists.\n" +
                "Enter 5 for exit.\n");

            mainChoose = int.Parse(Console.ReadLine());
            while (mainChoose != 5)
            {
                switch (mainChoose)
                {
                    case 1: //adding 
                        Adding(dalobject);
                        break;

                    case 2://updating
                        Updating(dalobject);
                        break;

                    case 3: //item's presentation
                        ItemPresent(dalobject);
                        break;

                    case 4: //presentation of lists
                        ListsPresent(dalobject);
                        break;

                    default:
                        break;
                }
                Console.Write("Enter your choose: ");
                mainChoose = int.Parse(Console.ReadLine());
            }
        }*/
        
        static void Main(string[] args)
        {
            IDal datafield = DAL.DalFactory.GetDal("DALXml");
            //Console.WriteLine(datafield .GetCustomers ());
            
            foreach (var item in datafield .GetCustomers ())
            {
                Console.WriteLine(item);
            }
            
            /*foreach (var item in datafield.GetParcels())
            {
                Console.WriteLine(item);
            }*/
            //try
            //{
            /*if(File .Exists(@"C:\Users\1\Source\Repos\sheiner32\dotnet5782_0938\targilZero\customersXML.xml") )
             {
                 var x = XElement.Load(@"C:\Users\1\Source\Repos\sheiner32\dotnet5782_0938\targilZero\customersXML.xml");
                 foreach(XElement o in x.Elements ())
                 {
                     Console.WriteLine(o);
                 }
             }
            else
                 Console.WriteLine("does not exist");
            */

            /*XmlRootAttribute xRoot = new XmlRootAttribute();
            xRoot.ElementName = "drones";
            // xRoot.Namespace = "http://www.cpandl.com";
            xRoot.IsNullable = true;

            XmlSerializer xmlser = new XmlSerializer(typeof(List<DAL.DalApi.DO.Drone>),xRoot);
            StreamReader srdr = new StreamReader(@"C:\Users\1\Source\Repos\sheiner32\dotnet5782_0938\targilZero\dronesXML.xml");
            List<DAL.DalApi.DO.Drone > p = (List<DAL.DalApi.DO.Drone >)xmlser.Deserialize(srdr);
            srdr.Close();
            Console.WriteLine(p.Count );
            foreach (var item in p)
            {
                Console.WriteLine(item);
            }

            IDal datafield = DAL.DalFactory.GetDal("DALXml");
                
                foreach (var item in datafield .GetUsers  ())
                {
                    Console.WriteLine(item);
                }
            //Console.WriteLine(Directory .GetCurrentDirectory ());

            Console.WriteLine(datafield .FindAndGetUser ("riki","3240"));
            
            */
            /*}
            catch(DAL.DalApi.LoadingException le)
            {
                Console.WriteLine(le.Message );
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message );
            }*/
        }

       
    }
}
