using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using DAL.DalApi;
using DAL.DalApi.DO;

namespace DAL.DalObject
{
    public sealed class DalObject : IDal
    {
        #region singleton
        private static readonly Lazy<DalObject> lazy = new Lazy<DalObject>(() => new DalObject());
        public static DalObject Instance { get { return lazy.Value; } }
        private DalObject()
        {
            DataSource.Initialize();
        }
        #endregion 

        #region get lists of items
        /// <summary>
        /// the method returns the list of the drones
        /// </summary>
        /// <returns>list of drones</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override IEnumerable<Drone> GetDrones()
        {
            List<Drone> d = new List<Drone>();
            foreach (Drone it in DataSource.drones)
                d.Add(it);
            return d;
        }
        /// <summary>
        /// the method returns the list of the stations
        /// </summary>
        /// <returns>list of stations</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override IEnumerable<Station> GetBasisStations()
        {
            List<Station> s = new List<Station>();
            foreach (Station it in DataSource.basisStations)
                s.Add(it);
            return s;
        }
        /// <summary>
        /// the method returns the list of the customers
        /// </summary>
        /// <returns>list of customers</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override IEnumerable<Customer> GetCustomers()
        {
            List<Customer> c = new List<Customer>();
            foreach (Customer it in DataSource.customers)
                c.Add(it);
            return c;
        }
        /// <summary>
        /// the method returns the list of the parcels
        /// </summary>
        /// <returns>list of parcels</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override IEnumerable<Parcel> GetParcels()
        {
            List<Parcel> p = new List<Parcel>();
            foreach (Parcel it in DataSource.parcels)
                p.Add(it);
            return p;
        }
        /// <summary>
        /// the method returns the list of the drones in charge
        /// </summary>
        /// <returns>list of drones in charge</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override IEnumerable<DroneCharge> GetDronesInCharge()
        {
            List<DroneCharge> d = new List<DroneCharge>();
            foreach (DroneCharge it in DataSource.dronesInCharge)
                d.Add(it);
            return d;
        }
        /// <summary>
        /// the method returns the list of the users
        /// </summary>
        /// <returns>list of users</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override IEnumerable<User> GetUsers()
        {
            List<User> u = new List<User>();
            foreach (User it in DataSource.users)
                u.Add(it);
            return u;
        }
        #endregion

        #region add item to list
        /// <summary>
        /// the method adds new drone to the storage
        /// </summary>
        /// <param name="id"></param>
        /// <param name="n"></param>
        /// <param name="w"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override void AddDrone(int id, string n, int w)
        {
            if (FindDrone(id) >= 0)
                throw new ExistIdException("this drone  id is already in the sortage");
            DataSource.drones.Add(new Drone(id, n, (Enums.WeightCategories)w));
        }
        /// <summary>
        /// the method adds new customer to the storage
        /// </summary>
        /// <param name="id"></param>
        /// <param name="n"></param>
        /// <param name="p"></param>
        /// <param name="lo"></param>
        /// <param name="la"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override void AddCustomer(int id, string n, string p, double lo, double la)
        {
            if (FindCustomer(id) >= 0)
                throw new ExistIdException("this customer id is already in the sortage");
            DataSource.customers.Add(new Customer(id, n, p, lo, la));
        }
        /// <summary>
        /// the method adds new station to the storage
        /// </summary>
        /// <param name="id"></param>
        /// <param name="n"></param>
        /// <param name="lo"></param>
        /// <param name="la"></param>
        /// <param name="cs"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override void AddStation(int id, int n, double lo, double la, int cs)
        {
            if (FindStation(id) >= 0)
                throw new ExistIdException("this station id is already in the sortage");
            DataSource.basisStations.Add(new Station(id, n, lo, la, cs));
        }
        /// <summary>
        /// the method adds new parcel to the storage
        /// </summary>
        /// <param name="sid"></param>
        /// <param name="tid"></param>
        /// <param name="w"></param>
        /// <param name="p"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override void AddParcel(int sid, int tid, int w, int p)
        {
            int id = DataSource.Config.idNumberParcels;
            DataSource.Config.idNumberParcels++;
            DataSource.Config.CountParcelsPriority[p]++;
            DataSource.parcels.Add(new Parcel(id, sid, tid, (Enums.WeightCategories)w, (Enums.Priorities)p/*, did*/));
        }
        /// <summary>
        /// the method adds new drone in charge to the storage
        /// </summary>
        /// <param name="dID"></param>
        /// <param name="sID"></param>
        /// <param name="active"></param>
        /// <param name="s"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override void AddDroneCharge(int dID, int sID, bool active, DateTime s)
        {
            DroneCharge dc = new DroneCharge(dID, sID, active, s);
            DataSource.dronesInCharge.Add(dc);
        }
        /// <summary>
        /// the method adds new user to the storage
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <param name="access"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override void AddUser(string name, string password,bool access)
        {
            if (FindUser(name,password ) >= 0)
                throw new ExistIdException("Exist user name");
            DataSource.users.Add(new User(name, password,access));
        }
        #endregion

        #region remove item from list
        /// <summary>
        /// the method removes specific drone from the storage
        /// </summary>
        /// <param name="id"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override void DeleteDrone(int id)
        {
            int indexDrone = FindDrone(id);
            if (indexDrone >= 0)
                DataSource.drones.Remove(DataSource.drones[indexDrone]);
            else
                throw new ObjectNotFoundException("Drone not found\n");
        }
        /// <summary>
        /// the method removes specific customer from the storage
        /// </summary>
        /// <param name="id"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override void DeleteCustomer(int id)
        {
            int indexCustomer = FindCustomer(id);
            if (indexCustomer >= 0)
                DataSource.customers.Remove(DataSource.customers[indexCustomer]);
            else
                throw new ObjectNotFoundException("Customer not found\n");
        }
        /// <summary>
        /// the method removes specific station from the storage
        /// </summary>
        /// <param name="id"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override void DeleteStation(int id)
        {
            int indexStation = FindStation(id);
            if (indexStation >= 0)
                DataSource.basisStations.Remove(DataSource.basisStations[indexStation]);
            else
                throw new ObjectNotFoundException("Station not found\n");
        }
        /// <summary>
        /// the method removes specific parcel from the storage
        /// </summary>
        /// <param name="id"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
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
        /// <summary>
        /// the method removes specific drone in charge from the storage
        /// </summary>
        /// <param name="id"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override void DeleteDroneInCharge(int id)
        {
            int indexDroneInCharge = FindDroneInCharge(id);
            if (indexDroneInCharge >= 0)
                DataSource.dronesInCharge.Remove(DataSource.dronesInCharge[indexDroneInCharge]);
            else
                throw new ObjectNotFoundException("Drone in charge not found\n");
        }
        /// <summary>
        /// the method removes specific user from the storage
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
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
        /// <summary>
        /// the method finds and returns the index in the drone's array of specific drone in the storage
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override int FindDrone(int id)
        {
            for (int i = 0; i < DataSource.drones.Count; i++)
            {
                if (DataSource.drones[i].ID == id)
                    return i;
            }
            return -1;
        }
        /// <summary>
        /// the method finds and returns the index in the customer's array of specific customer in the storage
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override int FindCustomer(int id)
        {
            for (int i = 0; i < DataSource.customers.Count; i++)
            {
                if (DataSource.customers[i].ID == id)
                    return i;
            }
            return -1;
        }
        /// <summary>
        /// the method finds and returns the index in the station's array of specific station in the storage
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override int FindStation(int id)
        {
            for (int i = 0; i < DataSource.basisStations.Count; i++)
            {
                if (DataSource.basisStations[i].stationID == id)
                    return i;
            }
            return -1;
        }
        /// <summary>
        /// the method finds and returns the index in the parcel's array of specific parcel in the storage
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override int FindParcel(int id)
        {
            for (int i = 0; i < DataSource.parcels.Count; i++)
            {
                if (DataSource.parcels[i].ID == id)
                    return i;
            }
            return -1;
        }
        // <summary>
        /// the method finds and returns the index in the array of drones in charge of specific drone in the storage
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override int FindDroneInCharge(int id)
        {
            for (int i = 0; i < DataSource.dronesInCharge.Count; i++)
            {
                if (DataSource.dronesInCharge[i].droneID == id)
                    return i;
            }
            return -1;
        }
        /// <summary>
        /// the method finds and returns the index in the user's array of specific user in the storage
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
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
        /// <summary>
        /// the method finds and returns specific drone in the storage
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override Drone FindAndGetDrone(int id)
        {
            for (int i = 0; i < DataSource.drones.Count; i++)
            {
                if (DataSource.drones[i].ID == id)
                    return DataSource.drones[i];
            }
            return new Drone();
        }
        /// <summary>
        ///  the method finds and returns specific customer in the storage
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override Customer FindAndGetCustomer(int id)
        {
            for (int i = 0; i < DataSource.customers.Count; i++)
            {
                if (DataSource.customers[i].ID == id)
                    return DataSource.customers[i];
            }
            return new Customer();
        }
        /// <summary>
        ///  the method finds and returns specific station in the storage
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override Station FindAndGetStation(int id)
        {
            for (int i = 0; i < DataSource.basisStations.Count; i++)
            {
                if (DataSource.basisStations[i].stationID == id)
                    return DataSource.basisStations[i];
            }
            return new Station();
        }
        /// <summary>
        ///  the method finds and returns specific parcel in the storage
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override Parcel FindAndGetParcel(int id)
        {
            for (int i = 0; i < DataSource.parcels.Count; i++)
            {
                if (DataSource.parcels[i].ID == id)
                    return DataSource.parcels[i];
            }
            return new Parcel();
        }
        /// <summary>
        ///  the method finds and returns specific drone in charge in the storage
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override DroneCharge FindAndGetDroneInCharge(int id)
        {
            for (int i = 0; i < DataSource.dronesInCharge.Count; i++)
            {
                if (DataSource.dronesInCharge[i].droneID == id)
                    return DataSource.dronesInCharge[i];
            }
            return new DroneCharge();
        }
        /// <summary>
        ///  the method finds and returns specific user in the storage
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
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
        /// <summary>
        /// the method updates the model of specific drone in the storage
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override void UpdateDrone(int id, string model)
        {
            int indexDrone = FindDrone(id);
            if (indexDrone < 0)
                throw new ObjectNotFoundException("The drone does not exist in the system");
            Drone temp = DataSource.drones[indexDrone];
            temp.model = model;
            DataSource.drones[indexDrone] = temp;
        }
        /// <summary>
        /// the method updates the name amd num of charge slots of specific station in the storage
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="chargeSlots"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override void UpdateStation(int id, int name, int chargeSlots)
        {
            int indexStation = FindStation(id);
            if (indexStation < 0)
                throw new ObjectNotFoundException("The station does not exist in the system");

            Station temp =DataSource .basisStations [indexStation ];
            if (name > 0)
                temp.name = name;
            if (chargeSlots >= 0)
                temp.chargeSlots = chargeSlots;
            DataSource.basisStations[indexStation] = temp;
        }
        /// <summary>
        /// the method updates the name and phone num of specific customer in the storage
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="phoneNum"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override void UpdateCustomer(int id, string name = "", string phoneNum = "")
        {
            int indexCustomer = FindCustomer(id);
            if (indexCustomer < 0)
                throw new ObjectNotFoundException("The customer does not exist in the system");

            Customer temp = DataSource.customers[indexCustomer];
            if (name != "")
                temp.name = name;
            if (phoneNum != "")
                temp.phone = phoneNum;
            DataSource.customers[indexCustomer] = temp;

        }
        /// <summary>
        /// the method updates the password of specific user in the storage
        /// </summary>
        /// <param name="uName"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override void UpdateUser(string uName, string oldPassword, string newPassword)
        {
            int indexUser =FindUser(uName, oldPassword);
            if (indexUser < 0)
                throw new ObjectNotFoundException("The user does not exist in the system");

            User tempUser = DataSource.users[indexUser];
            tempUser.UserPassword = newPassword;
            DataSource.users[indexUser] = tempUser;
        }
        /// <summary>
        /// the method assigns parcel to drone
        /// </summary>
        /// <param name="parcelID"></param>
        /// <param name="droneId"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override void ParcelToDrone(int parcelID, int droneId)
        {
            Parcel p = DataSource.parcels[parcelID - 1];
            p.droneID = droneId;
            p.scheduled = DateTime.Now;
            DataSource.parcels[parcelID - 1] = p;
        }
        /// <summary>
        /// method for collecting of parcel by drone
        /// </summary>
        /// <param name="parcelId"></param>
        /// <param name="collectorId"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override void ParcelCollection(int parcelId, int collectorId)
        {
            Parcel temp = DataSource.parcels[parcelId - 1];
            temp.droneID = collectorId;
            temp.pickedUp = DateTime.Now;
            DataSource.parcels[parcelId - 1] = temp;
        }
        /// <summary>
        /// method for delivery of parcel by drone to the destination
        /// </summary>
        /// <param name="parcelID"></param>
        /// <param name="customerId"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override void DeliveryParcel(int parcelID, int customerId)
        {
            Parcel temp = DataSource.parcels[parcelID - 1];
            temp.targetID = customerId;
            temp.delivered = DateTime.Now;
            temp.droneID = -1;
            DataSource.parcels[parcelID - 1] = temp;
        }
        /// <summary>
        /// the method creates charging of drone in specific station
        /// </summary>
        /// <param name="stationId"></param>
        /// <param name="droneId"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
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
        /// <summary>
        /// the method ends the charging of the drone
        /// </summary>
        /// <param name="dID"></param>
        /// <param name="hoursOfCharging"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
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
        /// <summary>
        /// the method updates collecting of parcel confirmation
        /// </summary>
        /// <param name="parcelID"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override void UpdateSendingOfParcel(int parcelID)
        {
            int indexParcel = FindParcel(parcelID);
            if (indexParcel < 0)
                throw new ObjectNotFoundException("parcel not exist");
            Parcel parcelSent = DataSource.parcels[indexParcel];
            parcelSent.confirmedSending = true;
            DataSource.parcels[indexParcel] = parcelSent;
        }
        /// <summary>
        /// the method updates supplying of parcel confirmation
        /// </summary>
        /// <param name="parcelID"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
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
        /// <summary>
        /// the method returns the power consumption by drone for each weight
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override double[] GetPowerConsumption() 
        {
            double[] power = new double[4];
            power[0] = DataSource.Config.idlePowerConsumption;
            power[1] = DataSource.Config.lightPowerConsumption;
            power[2] = DataSource.Config.mediumPowerConsumption;
            power[3] = DataSource.Config.heavyPowerConsumption;

            return power;
        }
        /// <summary>
        /// the method returns an array of numbers of parcels in each priority
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override int[] GetParcelsPriority() 
        {
            return DataSource.Config.CountParcelsPriority;
        }
        /// <summary>
        /// the method returns the loading rate of drone
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override int GetDroneLoadingRate() { return DataSource.Config.droneLoadingRate; }
        #endregion
    }


}
