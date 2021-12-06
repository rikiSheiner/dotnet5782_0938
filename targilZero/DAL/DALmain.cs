using DAL.IDAL.DO;
using DAL.IDAL;
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
            public enum Priorities { normal, quick, emergency }
            public enum NamesOfPeople { Chana, Ester, Chaya, Dvora, Shalom, Josef, Dan, Shira, Talya, Michal}
            #endregion

            #region מחלקות ליצוג חריגות
            public class ExistIdException : Exception //מחלקה ליצוג חריגה של מספר זהות קיים במערכת
            {
                public ExistIdException() : base() { }
                public ExistIdException(string message) : base(message) { }
                public ExistIdException(string message, Exception inner) : base(message, inner) { }

            }
            public class ObjectNotFoundException : Exception //מחלקה ליצוג חריגה של אובייקט שלא קיים במאגר
            {
                public ObjectNotFoundException() : base() { }
                public ObjectNotFoundException(string message) : base(message) { }
                public ObjectNotFoundException(string message, Exception inner) : base(message, inner) { }

            }
            #endregion 
        }

    }

    namespace DalObject
    {
        public class DataSource : IDal
        {
            #region רשימות של ישויות הנתונים         
            internal static List<Drone> drones=new List<Drone> () ;
            internal static List<Station> basisStations=new List<Station> ();
            internal static List<Customer> customers=new List<Customer> ();
            internal static List<Parcel> parcels=new List<Parcel> ();
            internal static List<DroneCharge> dronesInCharge=new List<DroneCharge> ();
            #endregion 

            internal class Config
            {
                internal static int idNumberParcels=1;//מספר מזהה רץ עבור חבילות
                internal static int countActive = 0;
                internal static double idlePowerConsumption = 0.01; //צריכת חשמל במצב פנוי
                internal static double lightPowerConsumption = 0.02; //צריכת חשמל במצב נושא משקל קל
                internal static double mediumPowerConsumption = 0.03; //צריכת חשמל במצב נושא משקל בינוני
                internal static double heavyPowerConsumption = 0.04; //צריכת חשמל במצב נושא משקל כבד
                internal static int droneLoadingRate = 40; //קצב טעינת רחפן בשעה באחוזים
                internal static int[] CountParcelsPriority = new int[3]; //מערך עבור מניית מספר החבילות שהן בעדיפות מסוימת
                                                                         //מקום 0=normal, מקום 1=quick,מקום 2 =emergency
            }

            #region Constructor
            public DataSource()
            {
                Initialize();
            }
            #endregion

            #region מתודות המחזירות העתקים של המאגרים של ישויות הנתונים
            public override IEnumerable<Drone> GetDrones()
            {
                List<Drone> d = new List<Drone>();
                foreach (Drone it in drones)
                    d.Add(it);
                return d;
            }
            public override IEnumerable<Station> GetBasisStations()
            {
                List<Station> s = new List<Station>();
                foreach (Station it in basisStations)
                    s.Add(it);
                return s;
            }
            public override IEnumerable<Customer> GetCustomers()
            {
                List<Customer> c = new List<Customer>();
                foreach (Customer it in customers)
                    c.Add(it);
                return c;
            }
            public override IEnumerable<Parcel> GetParcels()
            {
                List<Parcel> p = new List<Parcel>();
                foreach (Parcel it in parcels)
                    p.Add(it);
                return p;
            }
            public override IEnumerable<DroneCharge> GetDronesInCharge()
            {
                List<DroneCharge> d = new List<DroneCharge>();
                foreach (DroneCharge it in dronesInCharge)
                    d.Add(it);
                return d;
            }
            #endregion

            #region מתודה המאתחלת עם נתונים את המאגרים של ישויות הנתונים
            public override void Initialize()
            {
                Random r = new Random();
                
                int id = 111111111;
                for (int i = 0; i < 3; i++)
                {
                    Config.CountParcelsPriority[i] = 0;
                }

                //הוספת 2 תחנות בסיס
                basisStations.Add(new Station(1, 111, r.Next(1, 360), r.Next(1, 360), 10));
                basisStations.Add(new Station(2, 222, r.Next(1, 360), r.Next(1, 360), 13));

                //הוספת 5 רחפנים למאגר
                for (int i = 0; i < 5; i++)
                {
                    drones.Add(new Drone(i+1, "m" +( i+1), (WeightCategories)(r.Next(0, 3))));
                }

                //הוספת 10 לקוחות
                for (int i = 0; i < 10; i++)
                {
                    customers.Add(new Customer(id, ((NamesOfPeople)(i)).ToString(), (r.Next(0520000000, 0589999999)).ToString(), r.Next(1, 360), r.Next(1, 360)));
                    id++;
                }

                //הוספת 10 חבילות
                for (int i = 0; i < 10; i++)
                {
                    parcels.Add(new Parcel(Config.idNumberParcels , r.Next(1, 5), r.Next(6, 10), (WeightCategories)(i % 3),
                        (Priorities)((i + 1) % 3), i));
                    Config.CountParcelsPriority[(i + 1) % 3]++;
                    Config.idNumberParcels++;
                }


            }
            #endregion

            #region מתודות להוספת איבר למערכי ישויות הנתונים
            public override void AddDrone(int id,string n, int w)
            {
                //if (FindDrone(id) >= 0)
                //    throw new ExistIdException("this drone  id is already in the sortage");
                drones.Add(new Drone(id, n, (WeightCategories)w));
            }

            public override void AddCustomer(int id, string n, string p, double lo,double la)
            {
                if (FindCustomer(id) >= 0)
                    throw new ExistIdException("this customer id is already in the sortage");
                customers.Add(new Customer(id, n, p, lo, la));
            }

            public override void AddStation(int id,int n, double lo,double la, int cs )
            {
                if (FindStation(id) >= 0)
                    throw new ExistIdException("this station id is already in the sortage");
                basisStations.Add(new Station(id, n, lo, la, cs));
            }

            public override void AddParcel(int sid,int tid, int w, int p, int did)
            {
                int id = Config.idNumberParcels;
                Config.idNumberParcels++;
                Config.CountParcelsPriority[p]++;
                parcels.Add(new Parcel(id, sid, tid, (WeightCategories)w, (Priorities)p, did));
            }

            public override void AddDroneCharge(int dID, int sID, bool active,DateTime s )
            {
                DroneCharge dc = new DroneCharge(dID, sID, active, s);
                dronesInCharge.Add(dc);
            }
            #endregion

            #region מתודות למחיקת איבר ממערכי ישויות הנתונים
            public override void DeleteDrone(int id)
            {
                int indexDrone = FindDrone(id);
                if (indexDrone >= 0)
                    drones.Remove(drones[indexDrone]);
                else
                    throw new ObjectNotFoundException("Drone not found\n");
            }
            public override void DeleteCustomer(int id)
            {
                int indexCustomer = FindCustomer(id);
                if (indexCustomer >= 0)
                    customers.Remove(customers[indexCustomer]);
                else
                    throw new ObjectNotFoundException("Customer not found\n");
            }
            public override void DeleteStation(int id)
            {
                int indexStation = FindStation(id);
                if (indexStation >= 0)
                    basisStations.Remove(basisStations[indexStation]);
                else
                    throw new ObjectNotFoundException("Station not found\n");
            }
            public override void DeleteParcel(int id)
            {
                int indexParcel = FindParcel(id);
                if (indexParcel >= 0)
                { 
                    parcels.Remove(parcels[indexParcel]);
                    Config.CountParcelsPriority[(int)parcels[indexParcel ].priority]--;
                }
                else
                    throw new ObjectNotFoundException("Parcel not found\n");
            }
            public override void DeleteDroneInCharge(int id)
            {
                int indexDroneInCharge = FindDroneInCharge(id);
                if (indexDroneInCharge >= 0)
                    dronesInCharge.Remove(dronesInCharge[indexDroneInCharge]);
                else
                    throw new ObjectNotFoundException("Drone in charge not found\n");
            }
            #endregion

            #region  מתודות למציאת מיקום ישות נתונים לפי מספר זהות
            public override int FindDrone(int id)
            {
                for (int i = 0; i < drones .Count ; i++)
                {
                    if (drones[i].ID == id)
                        return i;
                }
                return -1;
            }
            public override int FindCustomer(int id)
            {
                for (int i = 0; i < customers .Count; i++)
                {
                    if (customers [i].ID == id)
                        return i;
                }
                return -1;
            }
            public override int FindStation(int id)
            {
                for (int i = 0; i < basisStations.Count; i++)
                {
                    if (basisStations [i].stationID  == id)
                        return i;
                }
                return -1;
            }
            public override int FindParcel(int id)
            {
                for (int i = 0; i < parcels.Count; i++)
                {
                    if (parcels[i].ID == id)
                        return i;
                }
                return -1;
            }
            public override int FindDroneInCharge(int id)
            {
                for (int i = 0; i < dronesInCharge.Count; i++)
                {
                    if (dronesInCharge[i].droneID  == id)
                        return i;
                }
                return -1;
            }
            #endregion

            #region מתודות המקבלות מספר זהות ומחזירות את ישות הנתונים המתאימה
            public override Drone FindAndGetDrone(int id)
            {
                for (int i = 0; i < drones.Count; i++)
                {
                    if (drones[i].ID == id)
                        return drones[i];
                }
                return new Drone();
            }
            public override Customer FindAndGetCustomer(int id)
            {
                for (int i = 0; i < customers.Count; i++)
                {
                    if (customers[i].ID == id)
                        return customers[i];
                }
                return new Customer();
            }
            public override Station FindAndGetStation(int id)
            {
                for (int i = 0; i < basisStations.Count; i++)
                {
                    if (basisStations[i].stationID == id)
                        return basisStations[i];
                }
                return new Station();
            }
            public override Parcel FindAndGetParcel(int id)
            {
                for (int i = 0; i < parcels.Count; i++)
                {
                    if (parcels[i].ID == id)
                        return parcels [i];
                }
                return new Parcel();
            }

            public override DroneCharge FindAndGetDroneInCharge(int id)
            {
                for (int i = 0; i < dronesInCharge.Count; i++)
                {
                    if (dronesInCharge[i].droneID == id)
                        return dronesInCharge[i];
                }
                return new DroneCharge();
            }
            #endregion

            #region  מתודות לעדכון מאגרי הנתונים
            //מתודה לשיוך חבילה לרחפן
            public override void ParcelToDrone(int parcelID, int droneId)
            {
                Parcel p = parcels[parcelID - 1];
                p.droneID = droneId;
                p.scheduled = DateTime.Now;
                parcels[parcelID - 1] = p;
            }

            //איסוף חבילה ע"י רחפן
            public override void ParcelCollection(int parcelId, int collectorId)
            {
                Parcel temp = parcels[parcelId - 1];
                temp.droneID = collectorId;
                temp.pickedUp = DateTime.Now;
                parcels[parcelId - 1] = temp;
            }

            //אספקת חבילה ללקוח
            public override void DeliveryParcel(int parcelID, int customerId)
            {
                Parcel temp = parcels[parcelID - 1];
                temp.targetID = customerId;
                temp.delivered = DateTime.Now;
                parcels[parcelID - 1] = temp;
            }

            //שליחת רחפן לטעינה
            public override void CreateDroneCharge(int stationId,int droneId)
            {
                int indexD = FindDrone (droneId ), indexS = FindStation (stationId );
                if (indexD < 0)
                    throw new ObjectNotFoundException("drone not found\n");
                if (indexS < 0)
                    throw new ObjectNotFoundException("station not found\n");

                //עדכון נתוני התחנה שבה מטעינים את הרחפן
                Station tempStation = basisStations[indexS];
                tempStation.chargeSlots--; //עדכון מספר חריצי הטעינה בתחנה
                basisStations[indexS] = tempStation;
                
                //הוספת רחפן לרשימת הרחפנים בטעינה
                int indexDroneInCharge = FindDroneInCharge(droneId);
                bool check = indexDroneInCharge < 0;
                if (check) //אם הרחפן לא קיים ברשימת הרחפנים בטעינה
                    dronesInCharge.Add(new DroneCharge(droneId, stationId, true, DateTime.Now)); //הוספת ישות טעינת רחפן 
                else
                {
                    DroneCharge dc = dronesInCharge[indexDroneInCharge];
                    if (dc.stationID != tempStation.stationID) //אם הרחפן קיים ברשימה אך זו טעינה בתחנה אחרת
                        dronesInCharge.Add(new DroneCharge(droneId, stationId, true, DateTime.Now)); //הוספת ישות טעינת רחפן
                    else
                    {
                        dc.activeCharge = true;
                        dc.start = DateTime.Now;
                        dronesInCharge[indexDroneInCharge] = dc;
                    }
                }
                
                Config.countActive++;
            }

            //שחרור רחפן מטעינה בתחנת בסיס
            public override void EndDroneCharge(int dID, int hoursOfCharging)
            {
                int indexDcharge = FindDroneInCharge(dID);
                if (indexDcharge < 0)
                    throw new ObjectNotFoundException("The drone is not in charge\n");
                int stationNum = dronesInCharge[indexDcharge].stationID;//מספר התחנה שהתפנתה
                int indexS = FindStation(stationNum);
                if (indexS < 0)
                    throw new ObjectNotFoundException("The station where the drone was charged does not exist.\n");
                //עדכון נתוני התחנה שממנה שחררנו את הרחפן
                Station tempS = basisStations[indexS];
                tempS.chargeSlots++;
                basisStations[indexS] = tempS;

                //עדכון נתוני הרחפן ששחררנו מטעינה ברשימת רחפנים בטעינה
                DroneCharge tempDC = dronesInCharge[indexDcharge];
                tempDC.activeCharge = false;
                tempDC.end = tempDC.start.AddHours(hoursOfCharging);
                dronesInCharge[indexDcharge] = tempDC;

                Config.countActive--;
            }
            #endregion

            public override double[] GetPowerConsumption() //בקשת צריכת חשמל ע"י רחפן
            {
                double[] power = new double[4];
                power[0] = Config.idlePowerConsumption;
                power[1] = Config.lightPowerConsumption;
                power[2] = Config.mediumPowerConsumption;
                power[3] = Config.heavyPowerConsumption;

                return power;
            }

            public override int[] GetParcelsPriority() //מערך מונים של מספר החבילות בכל עדיפות
            {
                return Config.CountParcelsPriority;
            }
            public override int GetDroneLoadingRate() { return Config.droneLoadingRate; }
        }

        public class DalObject 
        {

            public static DataSource ds;
            public DalObject() 
            {
                ds = new();
                ds.Initialize();
            }
            public DataSource GetDS() { return ds; }



        }

    }



}
