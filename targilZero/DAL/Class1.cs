using DAL.IDAL.DO;
using System;
using System.Collections.Generic;

/*name: Rivka Sheiner
 * id: 324060938
 * course: .net
 * exercise numbe: 1
 */

namespace DAL
{
    namespace IDAL
    {
        namespace DO
        {

            #region הנדרשים עבור ישויות הנתונים enums
            public enum WeightCategories { light, intermediate, heavy}
            public enum DroneStatuses { available, maintenance, delivery }
            public enum Priorities { normal, quick, emergency }
            public enum NamesOfPeople { Chana, Ester, Chaya, Dvora, Shalom, Josef, Dan, Shira, Talya, Michal}
            #endregion 

        }

    }

    namespace DalObject
    {
        public class DataSource
        {
            #region רשימות של ישויות הנתונים         
            internal static List<Drone> drones=new List<Drone> () ;
            internal static List<Station> basisStations=new List<Station> ();
            internal static List<Customer> customers=new List<Customer> ();
            internal static List<Parcel> parcels=new List<Parcel> ();
            internal static List<DroneCharge> dronesInCharge=new List<DroneCharge> ();
            #endregion 

            public class Config
            {
                internal static int idNumberParcels=1;//מספר מזהה רץ עבור חבילות
                internal static int countActive = 0;


                #region מתודות המחזירות העתקים של המאגרים של ישויות הנתונים
                public static List<Drone> GetDrones()
                {
                    List<Drone> d=new List<Drone> ();
                    foreach (Drone it in drones)
                        d.Add(it);
                    return d;
                }
                public static List<Station> GetBasisStations()
                {
                    List<Station> s = new List<Station>();
                    foreach (Station it in basisStations )
                        s.Add(it);
                    return s;
                }
                public static List<Customer> GetCustomers()
                {
                    List<Customer> c = new List<Customer>();
                    foreach (Customer it in customers )
                        c.Add(it);
                    return c;
                }
                public static List<Parcel> GetParcels()
                {
                    List<Parcel> p = new List<Parcel>();
                    foreach (Parcel it in parcels)
                        p.Add(it);
                    return p;
                }
                #endregion

                #region מתודה המאתחלת עם נתונים את המאגרים של ישויות הנתונים
                public static void Initialize()
                {
                    Random r = new Random();
                    DateTime d = new DateTime(2020, r.Next(1, 12), r.Next(1, 30));
                    int id=111111111;

                    //הוספת 5 רחפנים למאגר
                    for (int i = 0; i < 5; i++)
                    {
                        drones.Add(new Drone(i, "m" + i, (WeightCategories)(r.Next(0, 3)), 0, r.Next(1, 15) + 0.5));
                    }

                    //הוספת 2 תחנות בסיס
                    basisStations.Add (new Station(1, 111, r.Next (1,360), r.Next (1,360), 3));
                    basisStations.Add ( new Station(2, 222, r.Next(1, 360), r.Next(1, 360), 3));
                    //הוספת 10 לקוחות
                    for (int i = 0; i < 10; i++)
                    {
                        customers.Add (new Customer(id, ((NamesOfPeople)(i)).ToString () , (r.Next(0520000000, 0589999999)).ToString(), r.Next(1, 360), r.Next(1, 360)));
                        id++;
                    }

                    //הוספת 10 חבילות
                    for (int i = 0; i < 10; i++)
                    {
                        d.AddDays(i);
                        parcels.Add (new Parcel(idNumberParcels , r.Next(1, 5), r.Next(6, 10), (WeightCategories)(i % 3),
                            (Priorities)((i + 1) % 3), d, i, d.AddHours(i), d.AddHours(i % 3), d.AddDays(i * 2)));
                        d.AddDays(-i);
                        idNumberParcels++;
                    }

                    
                }
                #endregion 

                #region מתודות להוספת איבר למערכי ישויות הנתונים
                public static void AddDrone()
                {
                    int id,w,ds;
                    string n;
                    double b;

                    Console.WriteLine("enter id, name, weight category, status and battery of drone.");

                    id = int.Parse(Console.ReadLine());
                    n = Console.ReadLine();
                    w = int.Parse(Console.ReadLine());
                    ds = int.Parse(Console.ReadLine());
                    b = double.Parse(Console.ReadLine());

                    drones.Add (new Drone(id, n, (WeightCategories)w, (DroneStatuses)ds, b));
                }
                
                public static void AddCustomer()
                {
                    int id;
                    string n,p;
                    double lo, la;

                    Console.WriteLine("enter id, name, phone number, longitude and latitude of customer.");

                    id = int.Parse(Console.ReadLine());
                    n = Console.ReadLine();
                    p = Console.ReadLine();
                    lo = double.Parse(Console.ReadLine());
                    la = double.Parse(Console.ReadLine());

                    customers.Add (new Customer(id, n, p, lo, la));
                }

                public static void AddStation()
                {
                    int id,n,cs;
                    double lo, la;

                    Console.WriteLine("enter id, name, longitude, latitude and number of charge slots of station.");

                    id = int.Parse(Console.ReadLine());
                    n = int.Parse(Console.ReadLine());
                    lo = double.Parse(Console.ReadLine());
                    la = double.Parse(Console.ReadLine());
                    cs = int.Parse(Console.ReadLine());

                    basisStations.Add (new Station(id, n, lo, la, cs));
                }

                public static void AddParcel()
                {
                    int id, sid, tid, did, w, p;
                    DateTime r, s, pi, d;

                    Console.WriteLine("enter sender id, target id, weight, priority, requested time," +
                        " drone id, scheduled time, pickedup time and delivered time. ");

                    id = idNumberParcels;
                    idNumberParcels++;
                    sid = int.Parse(Console.ReadLine());
                    tid = int.Parse(Console.ReadLine());
                    w = int.Parse(Console.ReadLine());
                    p = int.Parse(Console.ReadLine());
                    r = DateTime.Parse(Console.ReadLine());
                    did = int.Parse(Console.ReadLine());
                    s = DateTime.Parse(Console.ReadLine());
                    pi = DateTime.Parse(Console.ReadLine());
                    d = DateTime.Parse(Console.ReadLine());
                    parcels.Add (new Parcel(id,sid,tid,(WeightCategories)w,(Priorities)p,r,did,s,pi,d));
                }
                #endregion 

                #region  מתודות להצגת ישויות הנתונים
                public static void ShowDrone(int id)
                {
                    foreach(Drone drone in drones)
                    {
                        if(drone.ID == id)
                        {
                            Console.WriteLine(drone);
                            break;
                        }
                    }
                }
                public static void ShowCustomer(int id)
                {
                    foreach (Customer customer in customers )
                    {
                        if (customer.ID  == id)
                        {
                            Console.WriteLine(customer);
                            break;
                        }
                    }
                }
                public static void ShowStation(int id)
                {
                    foreach (Station station in basisStations )
                    {
                        if (station.stationID == id)
                        {
                            Console.WriteLine(station );
                            break;
                        }
                    }
                }
                public static void ShowParcel(int id)
                {
                    foreach (Parcel parcel in parcels)
                    {
                        if (parcel .ID == id)
                        {
                            Console.WriteLine(parcel);
                            break;
                        }
                    }
                }
                public static void ShowDroneInCharge(int id)
                {
                    foreach (DroneCharge  droneCharge in dronesInCharge)
                    {
                        if (droneCharge.droneID == id)
                        {
                            Console.WriteLine(droneCharge );
                            break;
                        }
                    }
                }
                #endregion 

                #region מתודות להצגת רשימות של נתונים
                public static void ShowListParcels()
                {
                    List<Parcel> p = GetParcels();
                    foreach (Parcel parcelToPrint in p)
                    {
                        Console.WriteLine(parcelToPrint );
                    }
                }
                public static void ShowListStations()
                {
                    List<Station> s = GetBasisStations ();
                    foreach (Station stationToPrint in s)
                    {
                        Console.WriteLine(stationToPrint);
                    }
                }
                public static void ShowListDrones()
                {
                    List<Drone> d = GetDrones();
                    foreach (Drone droneToPrint in d)
                    {
                        Console.WriteLine(droneToPrint);
                    }
                }
                public static void ShowListCustomers()
                {
                    List<Customer> c = GetCustomers ();
                    foreach (Customer customerToPrint in c)
                    {
                        Console.WriteLine(customerToPrint);
                    }
                }
                public static void ShowParcelsNoDrone()
                {
                    List<Parcel> p = GetParcels();
                    foreach (Parcel parcelToPrint in p)
                    {
                        if(parcelToPrint .droneID<=0) //בהנחה שמספר זהות תקין גדול מ-0
                            Console.WriteLine(parcelToPrint);
                    }

                }
                public static void ShowAvailableStations()
                {
                    List<Station> s = GetBasisStations();
                    foreach (Station stationToPrint in s)
                    {
                        if (stationToPrint.chargeSlots > 0)
                            Console.WriteLine(stationToPrint);
                    }
                }
                #endregion

                #region  מתודות לעדכון מאגרי הנתונים
                //מתודה לשיוך חבילה לרחפן
                public static void ParcelToDrone()
                {
                    Console.Write("enter ID of parcel: ");
                    int parcelID = int.Parse(Console.ReadLine());

                    Console.Write("enter ID of the matching drone: ");
                    int droneId = int.Parse(Console.ReadLine());

                    Parcel p = parcels[parcelID - 1];
                    p.droneID = droneId;
                    parcels[parcelID - 1] = p;
                }

                
               
                //איסוף חבילה ע"י רחפן
                public static void ParcelCollection()
                {
                    Console.Write("enter ID of parcel for collecting: ");
                    int parcelId = int.Parse(Console.ReadLine());

                    Console.Write("enter ID of the drone: ");
                    int collectorId = int.Parse(Console.ReadLine());

                    //לבדוק אם זה עובד!!
                    Parcel temp = parcels[parcelId - 1];
                    temp.droneID = collectorId ;
                    parcels[parcelId - 1] = temp;
                }

                //אספקת חבילה ללקוח
                public static void DeliveryParcel()
                {
                    Console.Write("enter ID of parcel for delivery: ");
                    int parcelID = int.Parse(Console.ReadLine());

                    Console.Write("enter ID of customer: ");
                    int customerId = int.Parse(Console.ReadLine());

                    Parcel temp = parcels[parcelID - 1];
                    temp.targetID   = customerId ;
                    parcels[parcelID - 1] = temp;
                }

                //שליחת רחפן לטעינה
                public static void DroneCharge(int stationId)
                {
                    Console.Write("enter ID of drone for charging: ");
                    int droneId = int.Parse(Console.ReadLine());
                    
                    int indexD = 0,indexS=0;
                    for (indexD = 0; indexD < drones.Count; indexD++)
                    {
                        if (drones[indexD].ID == droneId)
                            break;
                    }
                    for (indexS = 0; indexS < basisStations .Count; indexS++)
                    {
                        if (basisStations [indexS].stationID  == stationId )
                            break;
                    }
                    Drone temp1 = drones[indexD];
                    temp1 .status = (DroneStatuses)1;// שינוי מצב הרחפן למצב טעינה
                    drones[indexD] = temp1;

                    Station temp2 = basisStations[indexS];
                    temp2.chargeSlots--; //עדכון מספר חריצי הטעינה בתחנה
                    basisStations[indexS] = temp2;

                    dronesInCharge.Add (new DroneCharge(droneId, stationId,true)); //הוספת ישות טעינת רחפן
                    countActive++;
                }

                //שחרור רחפן מטעינה בתחנת בסיס
                public static void EndDroneCharge()
                {
                    Console.Write("enter drone ID for ending of charging: ");
                    int dID = int.Parse(Console.ReadLine()); //קבלת מספר הרחפן לשחרור

                    int indexD = 0, indexS = 0, indexDcharge=0;
                    for (indexD = 0; indexD < drones.Count; indexD++)
                    {
                        if (drones[indexD].ID == dID)
                            break;
                    }

                    Drone temp1 = drones[indexD];
                    temp1.status = (DroneStatuses)0;// שינוי מצב הרחפן לזמין
                    drones[indexD] = temp1;
                    
                    int stationNum = dronesInCharge[indexD].stationID;//מספר התחנה שהתפנתה
                    
                    for (indexS = 0; indexS < basisStations.Count; indexS++)
                    {
                        if (basisStations[indexS].stationID == stationNum)
                            break;
                    }

                    Station tempS = basisStations[indexS];
                    tempS.chargeSlots++;
                    basisStations[indexS] = tempS;

                    for (indexDcharge = 0; indexDcharge < dronesInCharge.Count; indexDcharge++)
                    {
                        if (dronesInCharge[indexDcharge].droneID == dID)
                            break;
                    }

                    DroneCharge tempDC = dronesInCharge[indexDcharge];
                    tempDC.activeCharge = false;
                    dronesInCharge[indexDcharge] = tempDC;
                    
                    countActive--;
                }
                #endregion 


            }




        }
        
        public class DalObject
        { 
            public DalObject() 
            {
                DataSource.Config.Initialize();
            }

        }

    }



}
