using DAL.IDAL.DO;
using System;

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
            // הנדרשים עבור ישויות הנתונים enums
            #region 
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
            //מערכים של ישויות הנתונים
            internal static Drone[] drones = new Drone[10];
            internal static Station[] basisStations = new Station[5];
            internal static Customer[] customers = new Customer[100];
            internal static Parcel[] parcels = new Parcel[1000];
            internal static DroneCharge[] dronesInCharge = new DroneCharge[100];

            public class Config
            {
                //אינדקסים של המערכים של ישויות הנתונים
                internal static int indexDrones = 0;
                internal static int indexBasisStations = 0;
                internal static int indexCustomers = 0;
                internal static int indexParcels = 0;
                internal static int indexDroneInCharge = 0;
                internal static int idNumberParcels=1;//מספר מזהה רץ עבור חבילות

                

                //מתודות המחזירות העתקים של המאגרים של ישויות הנתונים
                public static Drone[] GetDrones()
                {
                    Drone[] d = new Drone[indexDrones];
                    for (int i = 0; i < indexDrones; i++)
                    {
                        d[i] = drones[i];
                    }
                    return d;
                }
                public static Station[] GetBasisStations()
                {
                    Station[] s = new Station[indexBasisStations];
                    for (int i = 0; i < indexBasisStations; i++)
                    {
                        s[i] = basisStations[i];
                    }
                    return s;
                }
                public static Customer[] GetCustomers()
                {
                    Customer[] c = new Customer[indexCustomers];
                    for (int i = 0; i < indexCustomers; i++)
                    {
                        c[i] = customers[i];
                    }
                    return c;
                }
                public  static Parcel[] GetParcels()
                {
                    Parcel[] p = new Parcel[indexParcels];
                    for (int i = 0; i < indexParcels; i++)
                    {
                        p[i] = parcels[i];
                    }
                    return p;
                }

                //מתודה המאתחלת עם נתונים את המאגרים של ישויות הנתונים
                public static void Initialize()
                {
                    Random r = new Random();
                    DateTime d = new DateTime(2020, r.Next(1, 12), r.Next(1, 30));
                    int id=111111111;

                    //הוספת 5 רחפנים למאגר
                    for (int i = 0; i < 5; i++)
                    {
                        drones[indexDrones++] = new Drone(i, "m" + i, (WeightCategories)(r.Next(0, 3)), 0, r.Next(1, 15) + 0.5);
                    }

                    //הוספת 2 תחנות בסיס
                    basisStations[indexBasisStations++] = new Station(1, 111, r.Next (1,360), r.Next (1,360), 3);
                    basisStations[indexBasisStations++] = new Station(2, 222, r.Next(1, 360), r.Next(1, 360), 3);
                    //הוספת 10 לקוחות
                    for (int i = 0; i < 10; i++)
                    {
                        customers[indexCustomers++] = new Customer(id, ((NamesOfPeople)(i)).ToString () , (r.Next(0520000000, 0589999999)).ToString(), r.Next(1, 360), r.Next(1, 360));
                        id++;
                    }

                    //הוספת 10 חבילות
                    for (int i = 0; i < 10; i++)
                    {
                        d.AddDays(i);
                        parcels[indexParcels++] = new Parcel(idNumberParcels , r.Next(1, 5), r.Next(6, 10), (WeightCategories)(i % 3),
                            (Priorities)((i + 1) % 3), d, i, d.AddHours(i), d.AddHours(i % 3), d.AddDays(i * 2));
                        d.AddDays(-i);
                        idNumberParcels++;
                    }

                    
                }

                //מתודות להוספת איבר למערכי ישויות הנתונים
                
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

                    drones[indexDrones++] = new Drone(id, n, (WeightCategories)w, (DroneStatuses)ds, b);
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

                    customers[indexCustomers++] = new Customer(id, n, p, lo, la);
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

                    basisStations[indexBasisStations++] = new Station(id, n, lo, la, cs);
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
                    parcels[indexParcels++] = new Parcel(id,sid,tid,(WeightCategories)w,(Priorities)p,r,did,s,pi,d);
                }

                //מתודות לחיפוש ישות מיקום ישות נותנים במערך לפי מספר זהות
                public static int SearchDroneByID(int id)
                {
                    for (int i = 0; i < indexDrones; i++)
                    {
                        if (drones[i].ID == id)
                            return i;
                    }

                    return -1;
                }
                public static int SearchCustomerByID(int id)
                {
                    for (int i = 0; i < indexCustomers; i++)
                    {
                        if (customers [i].ID == id)
                            return i;
                    }

                    return -1;
                }
                public static int SearchStationByID(int id)
                {
                    for (int i = 0; i < indexBasisStations ; i++)
                    {
                        if (basisStations [i].stationID == id)
                            return i;
                    }

                    return -1;
                }
                public static int SearchParcelByID(int id)
                {
                    for (int i = 0; i < indexParcels; i++)
                    {
                        if (parcels[i].ID == id)
                            return i;
                    }

                    return -1;
                }
                public static int SearchDroneChargeByID(int id)
                {
                    for (int i = 0; i < indexDroneInCharge; i++)
                    {
                        if (dronesInCharge[i].droneID  == id)
                            return i;
                    }

                    return -1;
                }
                
                //מתודות להצגת ישויות הנתונים
                public static void ShowDrone(int id)
                {
                    Console.WriteLine(drones[SearchDroneByID (id)]);
                }
                public static void ShowCustomer(int id)
                {
                    Console.WriteLine(customers [SearchCustomerByID (id)]);
                }
                public static void ShowStation(int id)
                {
                    Console.WriteLine(basisStations [SearchStationByID (id)]);
                }
                public static void ShowParcel(int id)
                {
                    Console.WriteLine(parcels[id-1]);
                }
                public static void ShowDroneInCharge(int id)
                {
                    Console.WriteLine(dronesInCharge [SearchDroneChargeByID (id)]);
                }

                //מתודות להצגת רשימות של נתונים
                public static void ShowListParcels()
                {
                    Parcel[] p = GetParcels();
                    for (int i = 0; i < p.Length /*indexParcels*/; i++)
                    {
                        Console.WriteLine(p[i]);
                    }
                    
                }
                public static void ShowListStations()
                {
                    Station[] s = GetBasisStations();
                    for (int i = 0; i < s.Length /*indexBasisStations*/; i++)
                    {
                        Console.WriteLine(s[i]);
                    }
                }
                public static void ShowListDrones()
                {
                    Drone[] d = GetDrones();
                    for (int i = 0; i < d.Length /*indexDrones*/; i++)
                    {
                        Console.WriteLine(d[i]);
                    }

                }
                public static void ShowListCustomers()
                {
                    Customer[] c = GetCustomers();
                    for (int i = 0; i < c.Length /*indexCustomers*/; i++)
                    {
                        Console.WriteLine(c[i]);
                    }

                }
                public static void ShowParcelsNoDrone()
                {
                    for (int i = 0; i < indexParcels; i++)
                    {
                        if (parcels[i].droneID < 0)
                            Console.WriteLine(parcels[i] );
                    }

                }
                public static void ShowAvailableStations()
                {
                    for (int i = 0; i < indexBasisStations; i++)
                    {
                        if (basisStations[i].chargeSlots > 0)
                            Console.WriteLine(basisStations[i]);
                    }

                }

                //מתודות לעדכון מאגרי הנתונים
                
                //מתודה לשיוך חבילה לרחפן
                public static void ParcelToDrone()
                {
                    Console.Write("enter ID of parcel: ");
                    int parcelID = int.Parse(Console.ReadLine());
                    Console.Write("enter ID of the matching drone: ");
                    int droneId = int.Parse(Console.ReadLine());
                    parcels[parcelID-1].droneID = droneId;

                }

                //איסוף חבילה ע"י רחפן
                public static void ParcelCollection()
                {
                    Console.Write("enter ID of parcel for collecting: ");
                    int parcelId = int.Parse(Console.ReadLine());
                    Console.Write("enter ID of the drone: ");
                    int collectorId = int.Parse(Console.ReadLine());
                    parcels[parcelId-1].droneID = collectorId;
                }

                //אספקת חבילה ללקוח
                public static void DeliveryParcel()
                {
                    Console.Write("enter ID of parcel for delivery: ");
                    int parcelID = int.Parse(Console.ReadLine());
                    Console.Write("enter ID of customer: ");
                    int customerId = int.Parse(Console.ReadLine());
                    parcels[parcelID-1].targetID = customerId ;
                }

                //שליחת רחפן לטעינה
                public static void DroneCharge(int stationId)
                {
                    Console.Write("enter ID of drone for charging: ");
                    int droneId = int.Parse(Console.ReadLine());
                    drones[SearchDroneByID (droneId )].status = (DroneStatuses)1; // שינוי מצב הרחפן למצב טעינה
                    dronesInCharge[indexDroneInCharge++] = new DroneCharge(droneId, stationId,true); //הוספת ישות טעינת רחפן
                    basisStations[SearchStationByID (stationId)].chargeSlots --; 
                }

                //שחרור רחפן מטעינה בתחנת בסיס
                public static void EndDroneCharge()
                {
                    Console.Write("enter drone ID for ending of charging: ");
                    int dID = int.Parse(Console.ReadLine()); //קבלת מספר הרחפן לשחרור
                    int indexD = SearchDroneChargeByID(dID);
                    drones[indexD].status = 0;//שינוי מצב הרחפן לזמין
                    int stationNum = dronesInCharge[indexD].stationID;//מספר התחנה שהתפנתה
                    basisStations[SearchStationByID (stationNum)].chargeSlots++;//עדכון מספר חריצי הטעינה 
                    dronesInCharge[SearchDroneChargeByID(dID)].activeCharge = false;
                }


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
