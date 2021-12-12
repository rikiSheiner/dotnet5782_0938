using System;
using System.Collections.Generic;
using BL.BlApi;
using BL.BO;

namespace ConsoleUI_BL
{
    /*name: Rivka Sheiner
     * ID: 324060938
     * exercise number: 2
     */
    class Program
    {
        public static void Adding(BL.IBL.BL datasource)
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
                        int stationForCharging;

                        Console.WriteLine("enter id, model, weight category, station id for charging");

                        droneID = int.Parse(Console.ReadLine());
                        droneName = Console.ReadLine();
                        w = int.Parse(Console.ReadLine());
                        stationForCharging = int.Parse(Console.ReadLine());


                        datasource.AddDrone(droneID, droneName, w,stationForCharging );
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

                        Console.WriteLine("enter sender id, target id, weight, priority. ");

                        sid = int.Parse(Console.ReadLine());
                        tid = int.Parse(Console.ReadLine());
                        parcelWeight = int.Parse(Console.ReadLine());
                        priority = int.Parse(Console.ReadLine());
                        
                        datasource.AddParcel(sid, tid, parcelWeight, priority);
                        break;
                }
            }
            catch (AddingProblemException addingError )
            {
                Console.WriteLine(addingError.Message);
            }
            catch(DAL.DO.ExistIdException existID)
            {
                Console.WriteLine(existID .Message );
            }
            catch (Exception)
            {
                Console.WriteLine("ERROR");
            }

        }
        public static void Updating(BL.IBL.BL datasource)
        {
            int secondChoose;
            
            Console.WriteLine("Enter 1 for updating of drone details.\n" + "Enter 2 for updating of station details.\n"+
              "Enter 3 for updating of customer details.\n" + "Enter 4 for drone charging.\n" +
              "Enter 5 for end of drone charging.\n" + "Enter 6 for assining of parcel to drone .\n" +
              "Enter 7 for parcel collecting by drone.\n" + "Enter 8 for delivery of parcel by drone.\n" );
            secondChoose = int.Parse(Console.ReadLine());

            try
            {
                switch (secondChoose)
                {
                    
                    case 1:
                        Console.WriteLine("Enter id and model of drone: ");
                        int idDrone = int.Parse(Console.ReadLine());
                        string model = Console.ReadLine();
                        datasource.UpdateDrone(idDrone, model);
                        break;
                    case 2:
                        Console.WriteLine("Enter id, name and number of charge slots in station: ");
                        int idStation = int.Parse(Console.ReadLine());
                        int newNameStation = int.Parse (Console .ReadLine ());
                        int newChargeSlots = int.Parse(Console.ReadLine());
                        datasource.UpdateStation(idStation, newNameStation , newChargeSlots);
                        break;
                    case 3:
                        Console.WriteLine("Enter id, name and phone number of customer: ");
                        int idCustomer = int.Parse(Console.ReadLine());
                        string newNameCustomer = Console.ReadLine();
                        string newPhoneCustoemr = Console.ReadLine();
                        datasource.UpdateCustomer(idCustomer, newNameCustomer, newPhoneCustoemr);
                        break; 
                    case 4:
                        Console.WriteLine("Enter id number of the drone for charging: ");
                        int droneChargeID = int.Parse(Console.ReadLine());
                        datasource.CreateDroneCharge(droneChargeID);
                        break;
                    case 5:
                        Console.WriteLine("Enter id number of the drone for end of charging and time of charging(in hours): ");
                        int droneEndChargeID = int.Parse(Console.ReadLine());
                        int timeOfCharging = int.Parse(Console.ReadLine());
                        datasource.EndDroneCharge(droneEndChargeID, timeOfCharging);
                        break;
                    case 6:
                        Console.WriteLine("Enter id of drone for assining the parcel to the drone: ");
                        int parcelToDroneID = int.Parse(Console.ReadLine());
                        datasource.AssignParcelToDrone(parcelToDroneID);
                        break;
                    case 7:
                        Console.WriteLine("Enter id of drone for collecting of parcel by the drone: ");
                        int droneCollectParcelID = int.Parse(Console.ReadLine());
                        datasource.CollectParcel(droneCollectParcelID);
                        break;
                    case 8:
                        Console.WriteLine("Enter id of drone for delivery of parcel: ");
                        int droneInDeliveryID = int.Parse(Console.ReadLine());
                        datasource.ParcelDelivery(droneInDeliveryID);
                        break;
                    default:
                        break;
                }
            }
            catch (UpdateProblemException  updatingError)
            {
                Console.WriteLine(updatingError.Message);
            }
            catch(DAL.DO.ObjectNotFoundException onf)
            {
                Console.WriteLine(onf.Message );
            }
            catch (Exception)
            {
                Console.WriteLine("ERROR");
            }
        }
        public static void ItemPresent(BL.IBL.BL datasource)
        {
            int secondChoose, id;
            Console.WriteLine("Enter 1 for presentation of basis station.\nEnter 2 for presentation of drone." +
                        "\nEnter 3 for presentation of customer.\nEnter 4 for presentation of parcel.");
            secondChoose = int.Parse(Console.ReadLine());
            Console.Write("enter ID number: ");
            id = int.Parse(Console.ReadLine());

            try
            {
                switch (secondChoose)
                {
                    case 1:
                        Console.WriteLine(datasource.GetStation(id).ToString() );
                        break;
                    case 2:
                        Console.WriteLine(datasource.GetDrone(id).ToString());
                        break;
                    case 3:
                        Console.WriteLine(datasource.GetCustomer(id).ToString());
                        break;
                    case 4:
                        Console.WriteLine(datasource.GetParcel(id).ToString());
                        break;
                }
            }
            catch (GetDetailsProblemException  presentError)
            {
                Console.WriteLine(presentError.Message);
            }
            catch (DAL.DO.ObjectNotFoundException onf)
            {
                Console.WriteLine(onf.Message);
            }
            catch (Exception)
            {
                Console.WriteLine("ERROR");
            }

        }                       
        public static void ListsPresent(BL.IBL.BL datasource)
        {
            int secondChoose;

            Console.WriteLine("Enter 1 for list of basis stations.\nEnter 2 for list of drones." +
                        "\nEnter 3 for list of customers.\nEnter 4 for list of parcels\n" +
                        "Enter 5 for list of parcels without drone.\nEnter 6 for available basis stations.");

            secondChoose = int.Parse(Console.ReadLine());

            switch (secondChoose)
            {
                case 1:
                    foreach (StationToList stationToPrint in datasource.GetListStations())
                    {
                        Console.WriteLine(stationToPrint);
                    }
                    break;
                case 2:
                    foreach (DroneToList droneToPrint in datasource.GetListDrones())
                    {
                        Console.WriteLine(droneToPrint);
                    }
                    break;
                case 3:
                    foreach (CustomerToList customerToPrint in datasource.GetListCustomers())
                    {
                        Console.WriteLine(customerToPrint);
                    }
                    break;
                case 4:
                    foreach (ParcelToList parcelToPrint in datasource.GetListParcels())
                    {
                        Console.WriteLine(parcelToPrint);
                    }
                    break;
                case 5:
                    bool noDrone(ParcelToList p) { return p.droneSender.ID == 0; }
                    foreach (ParcelToList parcelToPrint in datasource.GetListParcelsWithCondition(noDrone))
                    {
                        Console.WriteLine(parcelToPrint);
                    }
                    
                    break;
                case 6:
                    bool stationNotFull(StationToList s) { return s.availableChargeSlots>0; }
                    foreach (StationToList stationToPrint in datasource.GetListStationsWithCondition(stationNotFull))
                    {
                        Console.WriteLine(stationToPrint);
                    }
                    break;
            }

        }
        public static void Menu()
        {
            BL.BL blObject = new();

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
                        Adding(blObject);
                        break;

                    case 2://updating
                        Updating(blObject);
                        break;

                    case 3: //item's presentation
                        ItemPresent(blObject);
                        break;

                    case 4: //presentation of lists
                        ListsPresent(blObject);
                        break;

                    default:
                        break;
                }
                Console.Write("Enter your choose: ");
                mainChoose = int.Parse(Console.ReadLine());
            }
        }

        

        static void Main(string[] args)
        {

            Menu();
        }
    }
}
