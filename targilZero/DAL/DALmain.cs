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
                internal static double idlePowerConsumption; //צריכת חשמל במצב פנוי
                internal static double lightPowerConsumption; //צריכת חשמל במצב נושא משקל קל
                internal static double mediumPowerConsumption; //צריכת חשמל במצב נושא משקל בינוני
                internal static double heavyPowerConsumption; //צריכת חשמל במצב נושא משקל כבד
                internal static int droneLoadingRate; //קצב טעינת רחפן בשעה באחוזים

            }

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
            #endregion

            #region מתודה המאתחלת עם נתונים את המאגרים של ישויות הנתונים
            public override void Initialize()
            {
                Random r = new Random();
                DateTime d = new DateTime(2020, r.Next(1, 12), r.Next(1, 30));
                int id = 111111111;

                //הוספת 5 רחפנים למאגר
                for (int i = 0; i < 5; i++)
                {
                    drones.Add(new Drone(i, "m" + i, (WeightCategories)(r.Next(0, 3))));
                }

                //הוספת 2 תחנות בסיס
                basisStations.Add(new Station(1, 111, r.Next(1, 360), r.Next(1, 360), 3));
                basisStations.Add(new Station(2, 222, r.Next(1, 360), r.Next(1, 360), 3));
                //הוספת 10 לקוחות
                for (int i = 0; i < 10; i++)
                {
                    customers.Add(new Customer(id, ((NamesOfPeople)(i)).ToString(), (r.Next(0520000000, 0589999999)).ToString(), r.Next(1, 360), r.Next(1, 360)));
                    id++;
                }

                //הוספת 10 חבילות
                for (int i = 0; i < 10; i++)
                {
                    d.AddDays(i);
                    parcels.Add(new Parcel(Config.idNumberParcels , r.Next(1, 5), r.Next(6, 10), (WeightCategories)(i % 3),
                        (Priorities)((i + 1) % 3), d, i, d.AddHours(i), d.AddHours(i % 3), d.AddDays(i * 2)));
                    d.AddDays(-i);
                    Config.idNumberParcels++;
                }


            }
            #endregion

            #region מתודות להוספת איבר למערכי ישויות הנתונים
            public override void AddDrone(int id,string n, int w)
            {
                if (FindDrone(id) >= 0)
                    throw new ExistIdException("this drone  id is already in the sortage");
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

            public override void AddParcel(int sid,int tid, int w, int p, DateTime r, int did,
                DateTime s, DateTime pi,DateTime d)
            {
                int id = Config.idNumberParcels;
                Config.idNumberParcels++;
                parcels.Add(new Parcel(id, sid, tid, (WeightCategories)w, (Priorities)p, r, did, s, pi, d));
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

            #region  מתודות לעדכון מאגרי הנתונים
            //מתודה לשיוך חבילה לרחפן
            public override void ParcelToDrone(int parcelID, int droneId)
            {
                if (parcelID > parcels.Count )
                    throw new ObjectNotFoundException("this parcel is not existing in the storage");
                if (FindDrone(droneId) < 0)
                    throw new ObjectNotFoundException("this drone is not existing in the storage");
                    Parcel p = parcels[parcelID - 1];
                p.droneID = droneId;
                parcels[parcelID - 1] = p;
            }

            //איסוף חבילה ע"י רחפן
            public override void ParcelCollection(int parcelId, int collectorId)
            {
                if (parcelId > parcels.Count )
                    throw new ObjectNotFoundException("this parcel is not existing in the storage");
                if (FindDrone(collectorId) < 0)
                    throw new ObjectNotFoundException("this drone is not existing in the storage");
                Parcel temp = parcels[parcelId - 1];
                temp.droneID = collectorId;
                parcels[parcelId - 1] = temp;
            }

            //אספקת חבילה ללקוח
            public override void DeliveryParcel(int parcelID, int customerId)
            {
                if (parcelID > parcels.Count )
                    throw new ObjectNotFoundException("this parcel is not existing in the storage");
                if (FindCustomer(customerId) < 0)
                    throw new ObjectNotFoundException("this customer is not existing in the storage");
                
                Parcel temp = parcels[parcelID - 1];
                temp.targetID = customerId;
                parcels[parcelID - 1] = temp;
            }

            //שליחת רחפן לטעינה
            public override void DroneCharge(int stationId,int droneId)
            {
                if (FindStation(stationId) < 0)
                    throw new ObjectNotFoundException("this station is not existing in the storage");
                if (FindDrone(droneId) < 0)
                    throw new ObjectNotFoundException("this drone is not existing in the storage");

                int indexD = 0, indexS = 0;
                for (indexD = 0; indexD < drones.Count; indexD++)
                {
                    if (drones[indexD].ID == droneId)
                        break;
                }
                for (indexS = 0; indexS < basisStations.Count; indexS++)
                {
                    if (basisStations[indexS].stationID == stationId)
                        break;
                }

                Station temp2 = basisStations[indexS];
                temp2.chargeSlots--; //עדכון מספר חריצי הטעינה בתחנה
                basisStations[indexS] = temp2;

                dronesInCharge.Add(new DroneCharge(droneId, stationId, true)); //הוספת ישות טעינת רחפן

                Config.countActive++;
            }

            //שחרור רחפן מטעינה בתחנת בסיס
            public override void EndDroneCharge(int dID)
            {
                if (FindDrone(dID) < 0)
                    throw new ObjectNotFoundException("this drone is not existing in the storage");
                if (FindDroneInCharge(dID) < 0)
                    throw new ObjectNotFoundException("this drone is not in charging");

                int indexD = 0, indexS = 0, indexDcharge = 0;
                for (indexD = 0; indexD < drones.Count; indexD++)
                {
                    if (drones[indexD].ID == dID)
                        break;
                }

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

                Config.countActive--;
            }
            #endregion

            public override double[] GetPowerConsumption() //בקשת צריכת חשמל ע"י רחפן
            {
                double[] power = new double[5];
                power[0] = Config.idlePowerConsumption;
                power[1] = Config.lightPowerConsumption;
                power[2] = Config.mediumPowerConsumption;
                power[3] = Config.heavyPowerConsumption;
                power[4] = Config.droneLoadingRate;

                return power;
            }

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
