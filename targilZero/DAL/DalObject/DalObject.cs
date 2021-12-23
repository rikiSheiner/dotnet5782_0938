using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.DalApi;
using DAL.DalApi.DO;

namespace DAL.DalObject
{
    public sealed class DalObject : IDal
    {
        private static readonly Lazy<DalObject> lazy = new Lazy<DalObject>(() => new DalObject());

        internal static DalObject Instance { get { return lazy.Value; } }

        #region constructor
        /// <summary>
        /// constructor of DalObject
        /// </summary>
        internal DalObject()
        {
            DataSource.Initialize();
        }
        #endregion 

        #region get lists of items
        public override IEnumerable<Drone> GetDrones()
        {
            List<Drone> d = new List<Drone>();
            foreach (Drone it in DataSource.drones)
                d.Add(it);
            return d;
        }
        public override IEnumerable<Station> GetBasisStations()
        {
            List<Station> s = new List<Station>();
            foreach (Station it in DataSource.basisStations)
                s.Add(it);
            return s;
        }
        public override IEnumerable<Customer> GetCustomers()
        {
            List<Customer> c = new List<Customer>();
            foreach (Customer it in DataSource.customers)
                c.Add(it);
            return c;
        }
        public override IEnumerable<Parcel> GetParcels()
        {
            List<Parcel> p = new List<Parcel>();
            foreach (Parcel it in DataSource.parcels)
                p.Add(it);
            return p;
        }
        public override IEnumerable<DroneCharge> GetDronesInCharge()
        {
            List<DroneCharge> d = new List<DroneCharge>();
            foreach (DroneCharge it in DataSource.dronesInCharge)
                d.Add(it);
            return d;
        }
        public override IEnumerable<User> GetUsers()
        {
            List<User> u = new List<User>();
            foreach (User it in DataSource.users)
                u.Add(it);
            return u;
        }
        #endregion

        #region add item to list
        public override void AddDrone(int id, string n, int w)
        {
            //if (FindDrone(id) >= 0)
            //    throw new ExistIdException("this drone  id is already in the sortage");
            DataSource.drones.Add(new Drone(id, n, (Enums.WeightCategories)w));
        }
        public override void AddCustomer(int id, string n, string p, double lo, double la)
        {
            if (FindCustomer(id) >= 0)
                throw new ExistIdException("this customer id is already in the sortage");
            DataSource.customers.Add(new Customer(id, n, p, lo, la));
        }
        public override void AddStation(int id, int n, double lo, double la, int cs)
        {
            if (FindStation(id) >= 0)
                throw new ExistIdException("this station id is already in the sortage");
            DataSource.basisStations.Add(new Station(id, n, lo, la, cs));
        }
        public override void AddParcel(int sid, int tid, int w, int p/*, int did*/)
        {
            int id = DataSource.Config.idNumberParcels;
            DataSource.Config.idNumberParcels++;
            DataSource.Config.CountParcelsPriority[p]++;
            DataSource.parcels.Add(new Parcel(id, sid, tid, (Enums.WeightCategories)w, (Enums.Priorities)p/*, did*/));
        }
        public override void AddDroneCharge(int dID, int sID, bool active, DateTime s)
        {
            DroneCharge dc = new DroneCharge(dID, sID, active, s);
            DataSource.dronesInCharge.Add(dc);
        }
        public override void AddUser(string name, string password,bool access)
        {
            if (FindUser(name,password ) >= 0)
                throw new ExistIdException("Exist user name");
            DataSource.users.Add(new User(name, password,access));
        }
        #endregion

        #region remove item from list
        public override void DeleteDrone(int id)
        {
            int indexDrone = FindDrone(id);
            if (indexDrone >= 0)
                DataSource.drones.Remove(DataSource.drones[indexDrone]);
            else
                throw new ObjectNotFoundException("Drone not found\n");
        }
        public override void DeleteCustomer(int id)
        {
            int indexCustomer = FindCustomer(id);
            if (indexCustomer >= 0)
                DataSource.customers.Remove(DataSource.customers[indexCustomer]);
            else
                throw new ObjectNotFoundException("Customer not found\n");
        }
        public override void DeleteStation(int id)
        {
            int indexStation = FindStation(id);
            if (indexStation >= 0)
                DataSource.basisStations.Remove(DataSource.basisStations[indexStation]);
            else
                throw new ObjectNotFoundException("Station not found\n");
        }
        public override void DeleteParcel(int id)
        {
            int indexParcel = FindParcel(id);
            if (indexParcel >= 0)
            {
                DataSource.parcels.Remove(DataSource.parcels[indexParcel]);
                DataSource.Config.CountParcelsPriority[(int)DataSource.parcels[indexParcel].priority]--;
            }
            else
                throw new ObjectNotFoundException("Parcel not found\n");
        }
        public override void DeleteDroneInCharge(int id)
        {
            int indexDroneInCharge = FindDroneInCharge(id);
            if (indexDroneInCharge >= 0)
                DataSource.dronesInCharge.Remove(DataSource.dronesInCharge[indexDroneInCharge]);
            else
                throw new ObjectNotFoundException("Drone in charge not found\n");
        }

        public override void DeleteUser(string name,string password)
        {
            int indexUser = FindUser(name, password);
            if (indexUser >= 0)
                DataSource.users.Remove(DataSource.users[indexUser]);
            else
                throw new ObjectNotFoundException("User not found\n");
        }
        #endregion

        #region  find specific item in list
        public override int FindDrone(int id)
        {
            for (int i = 0; i < DataSource.drones.Count; i++)
            {
                if (DataSource.drones[i].ID == id)
                    return i;
            }
            return -1;
        }
        public override int FindCustomer(int id)
        {
            for (int i = 0; i < DataSource.customers.Count; i++)
            {
                if (DataSource.customers[i].ID == id)
                    return i;
            }
            return -1;
        }
        public override int FindStation(int id)
        {
            for (int i = 0; i < DataSource.basisStations.Count; i++)
            {
                if (DataSource.basisStations[i].stationID == id)
                    return i;
            }
            return -1;
        }
        public override int FindParcel(int id)
        {
            for (int i = 0; i < DataSource.parcels.Count; i++)
            {
                if (DataSource.parcels[i].ID == id)
                    return i;
            }
            return -1;
        }
        public override int FindDroneInCharge(int id)
        {
            for (int i = 0; i < DataSource.dronesInCharge.Count; i++)
            {
                if (DataSource.dronesInCharge[i].droneID == id)
                    return i;
            }
            return -1;
        }
        public override int FindUser(string name, string password)
        {
            for (int i = 0; i < DataSource.users.Count; i++)
            {
                if (DataSource.users[i].UserName == name && DataSource.users[i].UserPassword == password )
                    return i;
            }
            return -1;
        }
        #endregion

        #region find and return item in list
        public override Drone FindAndGetDrone(int id)
        {
            for (int i = 0; i < DataSource.drones.Count; i++)
            {
                if (DataSource.drones[i].ID == id)
                    return DataSource.drones[i];
            }
            return new Drone();
        }
        public override Customer FindAndGetCustomer(int id)
        {
            for (int i = 0; i < DataSource.customers.Count; i++)
            {
                if (DataSource.customers[i].ID == id)
                    return DataSource.customers[i];
            }
            return new Customer();
        }
        public override Station FindAndGetStation(int id)
        {
            for (int i = 0; i < DataSource.basisStations.Count; i++)
            {
                if (DataSource.basisStations[i].stationID == id)
                    return DataSource.basisStations[i];
            }
            return new Station();
        }
        public override Parcel FindAndGetParcel(int id)
        {
            for (int i = 0; i < DataSource.parcels.Count; i++)
            {
                if (DataSource.parcels[i].ID == id)
                    return DataSource.parcels[i];
            }
            return new Parcel();
        }

        public override DroneCharge FindAndGetDroneInCharge(int id)
        {
            for (int i = 0; i < DataSource.dronesInCharge.Count; i++)
            {
                if (DataSource.dronesInCharge[i].droneID == id)
                    return DataSource.dronesInCharge[i];
            }
            return new DroneCharge();
        }
        public override User FindAndGetUser(string name, string password)
        {
            for (int i = 0; i < DataSource.users.Count; i++)
            {
                if (DataSource.users[i].UserName == name && DataSource .users [i].UserPassword == password )
                    return DataSource.users[i];
            }
            return new User();
        }
        #endregion

        #region  updating
        //מתודה לשיוך חבילה לרחפן
        public override void ParcelToDrone(int parcelID, int droneId)
        {
            Parcel p = DataSource.parcels[parcelID - 1];
            p.droneID = droneId;
            p.scheduled = DateTime.Now;
            DataSource.parcels[parcelID - 1] = p;
        }

        //איסוף חבילה ע"י רחפן
        public override void ParcelCollection(int parcelId, int collectorId)
        {
            Parcel temp = DataSource.parcels[parcelId - 1];
            temp.droneID = collectorId;
            temp.pickedUp = DateTime.Now;
            DataSource.parcels[parcelId - 1] = temp;
        }

        //אספקת חבילה ללקוח
        public override void DeliveryParcel(int parcelID, int customerId)
        {
            Parcel temp = DataSource.parcels[parcelID - 1];
            temp.targetID = customerId;
            temp.delivered = DateTime.Now;
            temp.droneID = -1;
            DataSource.parcels[parcelID - 1] = temp;
        }

        //שליחת רחפן לטעינה
        public override void CreateDroneCharge(int stationId, int droneId)
        {
            int indexD = FindDrone(droneId), indexS = FindStation(stationId);
            if (indexD < 0)
                throw new ObjectNotFoundException("drone not found\n");
            if (indexS < 0)
                throw new ObjectNotFoundException("station not found\n");

            //עדכון נתוני התחנה שבה מטעינים את הרחפן
            Station tempStation = DataSource.basisStations[indexS];
            tempStation.chargeSlots--; //עדכון מספר חריצי הטעינה בתחנה
            DataSource.basisStations[indexS] = tempStation;

            //הוספת רחפן לרשימת הרחפנים בטעינה
            int indexDroneInCharge = FindDroneInCharge(droneId);
            bool check = indexDroneInCharge < 0;
            if (check) //אם הרחפן לא קיים ברשימת הרחפנים בטעינה
                DataSource.dronesInCharge.Add(new DroneCharge(droneId, stationId, true, DateTime.Now)); //הוספת ישות טעינת רחפן 
            else
            {
                DroneCharge dc = DataSource.dronesInCharge[indexDroneInCharge];
                if (dc.stationID != tempStation.stationID) //אם הרחפן קיים ברשימה אך זו טעינה בתחנה אחרת
                    DataSource.dronesInCharge.Add(new DroneCharge(droneId, stationId, true, DateTime.Now)); //הוספת ישות טעינת רחפן
                else
                {
                    dc.activeCharge = true;
                    dc.start = DateTime.Now;
                    DataSource.dronesInCharge[indexDroneInCharge] = dc;
                }
            }

            DataSource.Config.countActive++;
        }

        //שחרור רחפן מטעינה בתחנת בסיס
        public override void EndDroneCharge(int dID, int hoursOfCharging)
        {
            int indexDcharge = FindDroneInCharge(dID);
            if (indexDcharge < 0)
                throw new ObjectNotFoundException("The drone is not in charge\n");
            int stationNum = DataSource.dronesInCharge[indexDcharge].stationID;//מספר התחנה שהתפנתה
            int indexS = FindStation(stationNum);
            if (indexS < 0)
                throw new ObjectNotFoundException("The station where the drone was charged does not exist.\n");
            //עדכון נתוני התחנה שממנה שחררנו את הרחפן
            Station tempS = DataSource.basisStations[indexS];
            tempS.chargeSlots++;
            DataSource.basisStations[indexS] = tempS;

            //עדכון נתוני הרחפן ששחררנו מטעינה ברשימת רחפנים בטעינה
            DroneCharge tempDC = DataSource.dronesInCharge[indexDcharge];
            tempDC.activeCharge = false;
            tempDC.end = tempDC.start.AddHours(hoursOfCharging);
            DataSource.dronesInCharge[indexDcharge] = tempDC;

            DataSource.Config.countActive--;
        }

        public override void UpdateSendingOfParcel(int parcelID)
        {
            int indexParcel = FindParcel(parcelID);
            if (indexParcel < 0)
                throw new ObjectNotFoundException("parcel not exist");
            Parcel parcelSent = DataSource.parcels[indexParcel];
            parcelSent.confirmedSending = true;
            DataSource.parcels[indexParcel] = parcelSent;
        }

        public override void UpdateRecievingOfParcel(int parcelID)
        {
            int indexParcel = FindParcel(parcelID);
            if (indexParcel < 0)
                throw new ObjectNotFoundException("parcel not exist");
            Parcel parcelSent = DataSource.parcels[indexParcel];
            parcelSent.confirmRecieving = true;
            DataSource.parcels[indexParcel] = parcelSent;
        }
        #endregion

        #region get fields of class config
        public override double[] GetPowerConsumption() //בקשת צריכת חשמל ע"י רחפן
        {
            double[] power = new double[4];
            power[0] = DataSource.Config.idlePowerConsumption;
            power[1] = DataSource.Config.lightPowerConsumption;
            power[2] = DataSource.Config.mediumPowerConsumption;
            power[3] = DataSource.Config.heavyPowerConsumption;

            return power;
        }

        public override int[] GetParcelsPriority() //מערך מונים של מספר החבילות בכל עדיפות
        {
            return DataSource.Config.CountParcelsPriority;
        }
        public override int GetDroneLoadingRate() { return DataSource.Config.droneLoadingRate; }
        #endregion
    }


}
