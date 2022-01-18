using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.BO;

namespace BL.BlApi
{
    public abstract class IBL
    {
        #region Adding new entity
        /// <summary>
        /// the method adds new station to the storage
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="longitude"></param>
        /// <param name="latitude"></param>
        /// <param name="cs"></param>
        public abstract void AddStation(int id, int name, double longitude, double latitude,int cs);
        /// <summary>
        /// the method adds new drone to the storage
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <param name="maxWeight"></param>
        /// <param name="stationNum"></param>
        public abstract void AddDrone(int id, string model, int maxWeight, int stationNum);
        /// <summary>
        /// the method adds new customer to the storage
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="longitude"></param>
        /// <param name="latitude"></param>
        public abstract void AddCustomer(int id, string name, string phoneNumber, double longitude, double latitude);
        /// <summary>
        /// the method adds new parcel to the storage
        /// </summary>
        /// <param name="senderID"></param>
        /// <param name="targetID"></param>
        /// <param name="weight"></param>
        /// <param name="priority"></param>
        public abstract void AddParcel(int senderID, int targetID, int weight, int priority);
        /// <summary>
        /// the method adds new user to the storage
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <param name="access"></param>
        public abstract void AddUser(string name, string password,bool access);
        #endregion

        #region Removing of item from list
        /// <summary>
        /// the method removes specfic drone from the storage
        /// </summary>
        /// <param name="id"></param>
        abstract public void DeleteDrone(int id);
        /// <summary>
        ///  the method removes specfic customer from the storage
        /// </summary>
        /// <param name="id"></param>
        abstract public void DeleteCustomer(int id);
        /// <summary>
        ///  the method removes specfic station from the storage
        /// </summary>
        /// <param name="id"></param>
        abstract public void DeleteStation(int id);
        /// <summary>
        ///  the method removes specfic parcel from the storage
        /// </summary>
        /// <param name="id"></param>
        abstract public void DeleteParcel(int id);
        /// <summary>
        ///  the method removes specfic drone in charge from the storage
        /// </summary>
        /// <param name="id"></param>
        abstract public void DeleteDroneInCharge(int id);
        /// <summary>
        ///  the method removes specfic user from the storage
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        abstract public void DeleteUser(string name, string password);

        #endregion

        #region Updating
        
        public abstract void UpdateSendingOfParcel(int parcelID);
        public abstract void UpdateRecievingOfParcel(int parcelID);
        /// <summary>
        /// the method updates the model of specific drone in the storage
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        public abstract void UpdateDrone(int id, string model);
        /// <summary>
        /// the method updates the name amd num of charge slots of specific station in the storage
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="chargeSlots"></param>
        public abstract void UpdateStation(int id, int name, int chargeSlots);
        /// <summary>
        /// the method updates the name and phone num of specific customer in the storage
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="phoneNum"></param>
        public abstract void UpdateCustomer(int id, string name = "", string phoneNum = "");
        /// <summary>
        /// the method updates the password of specific user in the storage
        /// </summary>
        /// <param name="uName"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        public abstract void UpdateUser(string uName, string oldPassword, string newPassword);
        /// <summary>
        /// the method creates charging of drone in the availabe soon station
        /// </summary>
        /// <param name="droneId"></param>
        public abstract void CreateDroneCharge(int droneID);
        /// <summary>
        /// the method ends the charging of the drone in station
        /// </summary>
        /// <param name="droneID"></param>
        /// <param name="timeOfCharging"></param>
        public abstract void EndDroneCharge(int droneID, int timeOfCharging);
        /// <summary>
        /// the method assigns parcel to drone
        /// </summary>
        /// <param name="droneID"></param>
        public abstract void AssignParcelToDrone(int droneID);
        /// <summary>
        /// method for collecting of the parcel that assigned to the drone by drone
        /// </summary>
        /// <param name="parcelId"></param>
        /// <param name="collectorId"></param>
        public abstract void CollectParcel(int droneID);
        /// <summary>
        /// method for delivery of parcel that has been collected by the drone to the destination
        /// </summary>
        /// <param name="parcelID"></param>
        /// <param name="customerId"></param>
        public abstract void ParcelDelivery(int droneID); 
        #endregion

        #region Check if item exist
        /// <summary>
        /// the method checks and returns if user exist in the system
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public abstract bool IsUserExist(string name, string password);
        #endregion

        #region Find item's index in list
        /// <summary>
        /// the method finds and returns the index in the drone's array of specific drone in the storage
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        abstract public int FindDrone(int id);
        /// <summary>
        /// the method finds and returns the index in the customer's array of specific customer in the storage
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        abstract public int FindCustomer(int id);
        /// <summary>
        /// the method finds and returns the index in the station's array of specific station in the storage
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        abstract public int FindStation(int id);
        /// <summary>
        /// the method finds and returns the index in the parcel's array of specific parcel in the storage
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        abstract public int FindParcel(int id);
        /// <summary>
        /// the method finds and returns the index in the array of drones in charge of specific drone in the storage
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        abstract public int FindDroneInCharge(int id);
        /// <summary>
        /// the method finds and returns the index in the user's array of specific user in the storage
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        abstract public int FindUser(string name, string password);
        #endregion

        #region Find and get item
        /// <summary>
        /// the method finds and returns specific drone in the storage
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        abstract public DAL.DalApi.DO.Drone FindAndGetDrone(int id);
        /// <summary>
        /// the method finds and returns specific customer in the storage
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        abstract public DAL.DalApi.DO.Customer FindAndGetCustomer(int id);
        /// <summary>
        /// the method finds and returns specific station in the storage
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        abstract public DAL.DalApi.DO.Station FindAndGetStation(int id);
        /// <summary>
        /// the method finds and returns specific parcel in the storage
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        abstract public DAL.DalApi.DO.Parcel FindAndGetParcel(int id);
        /// <summary>
        /// the method finds and returns specific drone charge in the storage
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        abstract public DAL.DalApi.DO.DroneCharge FindAndGetDroneInCharge(int id);
        /// <summary>
        /// the method finds and returns specific user in the storage
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        abstract public DAL.DalApi.DO.User FindAndGetUser(string name, string password);
        #endregion

        #region Getting item for presenting
        /// <summary>
        /// the method finds and returns specific station to list in the storage 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public abstract StationToList GetStation(int id);
        /// <summary>
        /// the method finds and returns specific drone to list in the storage 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public abstract DroneToList GetDrone(int id);
        /// <summary>
        /// the method finds and returns specific customer to list in the storage 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public abstract CustomerToList GetCustomer(int id);
        /// <summary>
        /// /// <summary>
        /// the method finds and returns specific parcel to list in the storage 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public abstract ParcelToList GetParcel(int id);
        #endregion

        #region Presenting of lists
        /// <summary>
        /// the method returns the list of the stations
        /// </summary>
        /// <returns>list of stations</returns>
        public abstract IEnumerable<StationToList> GetListStations();
        /// <summary>
        /// the method returns the list of the drones
        /// </summary>
        /// <returns>list of drones</returns>
        public abstract IEnumerable<DroneToList> GetListDrones();
        /// <summary>
        /// the method returns the list of the customers
        /// </summary>
        /// <returns>list of customers</returns>
        public abstract IEnumerable<CustomerToList> GetListCustomers();
        /// <summary>
        /// the method returns the list of the parcels
        /// </summary>
        /// <returns>list of parcels</returns>
        public abstract IEnumerable<ParcelToList> GetListParcels();
        /// <summary>
        /// the method returns the list of the users
        /// </summary>
        /// <returns>list of users</returns>
        public abstract IEnumerable<User> GetListUsers();
        /// <summary>
        /// the method returns partial list of the parcels that meet certain condition
        /// </summary>
        /// <param name="parcelCondition"></param>
        /// <returns></returns>
        public abstract IEnumerable<ParcelToList> GetListParcelsWithCondition(Predicate <ParcelToList> parcelCondition);
        /// <summary>
        ///  the method returns partial list of the stations that meet certain condition
        /// </summary>
        /// <param name="stationCondition"></param>
        /// <returns></returns>
        public abstract IEnumerable<StationToList> GetListStationsWithCondition(Predicate<StationToList> stationCondition);
        /// <summary>
        ///  the method returns partial list of the drones that meet certain condition
        /// </summary>
        /// <param name="droneCondition"></param>
        /// <returns></returns>
        public abstract IEnumerable<DroneToList> GetListDronesWithCondition(Predicate<DroneToList> droneCondition);
        /// <summary>
        ///  the method returns partial list of the customers that meet certain condition
        /// </summary>
        /// <param name="customerCondition"></param>
        /// <returns></returns>
        public abstract IEnumerable<CustomerToList> GetListCustomersWithCondition(Predicate<CustomerToList> customerCondition);
        /// <summary>
        ///  the method returns partial list of the users that meet certain condition
        /// </summary>
        /// <param name="userCondition"></param>
        /// <returns></returns>
        public abstract IEnumerable<User> GetListUsersWithCondition(Predicate<User> userCondition);
        #endregion

        # region Converting an entity to entity in list
        /// <summary>
        /// the method converts station to station to list and returns it
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public abstract StationToList ConvertStationToStationInList(DAL.DalApi.DO.Station s);
        /// <summary>
        /// the method converts drone to drone to list and returns it
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public abstract DroneToList ConvertDroneToDroneInList(Drone d);
        /// <summary>
        /// the method converts parcel to parcel to list and returns it
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public abstract ParcelToList ConvertParcelToParcelInList(DAL.DalApi.DO.Parcel p);
        /// <summary>
        /// the method converts customer to customer to list and returns it
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public abstract CustomerToList ConvertCustomerToCustomerInList(DAL.DalApi.DO.Customer c);
        #endregion

        #region Converting of entity of DAL to enetity of BL
        /// <summary>
        /// the method converts customer of DAL to customer of BL and returns it
        /// </summary>
        /// <param name="customerDAL"></param>
        /// <returns></returns>
        public abstract Customer ConvertCustomerDalToCustomerBL(DAL.DalApi.DO.Customer customerDAL);
        /// <summary>
        /// the method converts drone of DAL to drone of BL and returns it
        /// </summary>
        /// <param name="droneDAL"></param>
        /// <returns></returns>
        public abstract Drone ConvertDroneDalToDroneBL(DAL.DalApi.DO.Drone droneDAL);
        #endregion 

        #region Help methods
        public abstract int FindDroneBL(int id);
        /// <summary>
        /// The method gets station ID number and counts the number of drones in charge in this station
        /// </summary>
        /// <param name="stationID"></param>
        /// <returns>number of full charge slots</returns>
        internal abstract int CountFullChargeSlots(int stationID);
        /// <summary>
        /// The method finds the parcel in delivery in specific drone now
        /// </summary>
        /// <param name="droneID"></param>
        /// <returns>id number of parcel in drone</returns>
        internal abstract int FindParcelInDrone(int droneID);
        /// <summary>
        /// The method counts the number of parcels in each state of specific customer
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns>array of numbers of parcels in each status</returns>
        internal abstract int[] CountParcelsInEachStatus(int customerID);
        /// <summary>
        ///The method gets drone id number and returns list of customers got parcels from this drone
        /// </summary>
        /// <param name="droneID"></param>
        /// <returns>returns list of customers got parcels from specific drone</returns>
        internal abstract List<DAL.DalApi.DO.Customer> GetListCustomersGotParcelsFromDrone(int droneID);
        /// <summary>
        /// The method gets the parcel for sending and the drone and returns 
        /// whether the drone have enough battery for the delivery of this parcel
        /// </summary>
        /// <param name="parcelForDelivery"></param>
        /// <param name="battery"></param>
        /// <returns></returns>
        internal abstract bool DroneHaveEnoughBattery(DAL.DalApi.DO.Parcel parcelForDelivery, Drone d);
        
        #endregion

        #region function for acting of the drones' simulator
        /// <summary>
        /// the method acts the simulator of the drones
        /// </summary>
        /// <param name="droneID"></param>
        /// <param name="updateDisplay"></param>
        /// <param name="checkStop"></param>
        public abstract void AutomaticDroneAct(int droneID, Action updateDisplay, Func<bool> checkStop);
        #endregion
    }
}
