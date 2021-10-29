using System;
using DAL.DalObject;


/*name: Rivka Sheiner
 * id: 324060938
 * course: .net
 * exercise numbe: 1
 */

namespace ConsoleUI
{
    class Program
    {
        public static void Adding()
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
                        DataSource.Config.AddStation();
                        break;
                    case 2:
                        DataSource.Config.AddDrone();
                        break;
                    case 3:
                        DataSource.Config.AddCustomer();
                        break;
                    case 4:
                        DataSource.Config.AddParcel();
                        break;
                }
            }
            catch(IndexOutOfRangeException )
            {
                Console.WriteLine("ERROR. The storage is already full");
            }

        }

        public static void Updating()
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
                        DataSource.Config.ParcelToDrone();
                        break;
                    case 2:
                        DataSource.Config.ParcelCollection();
                        break;
                    case 3:
                        DataSource.Config.DeliveryParcel();
                        break;
                    case 4:
                        Console.Write("enter station ID: ");
                        int sID = int.Parse(Console.ReadLine());
                        DataSource.Config.DroneCharge(sID);
                        break;
                    case 5:
                        DataSource.Config.EndDroneCharge();
                        break;
                }
            }
            catch(IndexOutOfRangeException )
            {
                Console.WriteLine("ERROR. Wrong ID");
            }
            catch(OverflowException )
            {
                Console.WriteLine("ERROR. There are too many drones to charge");
            }
        }

        public static void ItemPresent()
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
                        DataSource.Config.ShowStation(id);
                        break;
                    case 2:
                        DataSource.Config.ShowDrone(id);
                        break;
                    case 3:
                        DataSource.Config.ShowCustomer(id);
                        break;
                    case 4:
                        DataSource.Config.ShowParcel(id);
                        break;
                }
            }
            catch(IndexOutOfRangeException )
            {
                Console.WriteLine("ERROR. Wrong item's id");
            }
            
        }
        public static void ListsPresent()
        {
            int secondChoose;
            
            Console.WriteLine("Enter 1 for list of basis stations.\nEnter 2 for list of drones." +
                        "\nEnter 3 for list of customers.\nEnter 4 for list of parcels\n" +
                        "Enter 5 for list of parcels without drone.\nEnter 6 for available basis stations.");
            
            secondChoose = int.Parse(Console.ReadLine());
            
            switch (secondChoose)
            {
                case 1:
                    DataSource.Config.ShowListStations();
                    break;
                case 2:
                    DataSource.Config.ShowListDrones();
                    break;
                case 3:
                    DataSource.Config.ShowListCustomers();
                    break;
                case 4:
                    DataSource.Config.ShowListParcels();
                    break;
                case 5:
                    DataSource.Config.ShowParcelsNoDrone();
                    break;
                case 6:
                    DataSource.Config.ShowAvailableStations();
                    break;
            }

        }

        public static void Menu()
        {
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
                        Adding();
                        break;

                    case 2://updating
                        Updating();
                        break;

                    case 3: //item's presentation
                        ItemPresent();
                        break;

                    case 4: //presentation of lists
                        ListsPresent();
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
            DalObject d = new DalObject();
            Menu();
            
        }

       
    }
}
