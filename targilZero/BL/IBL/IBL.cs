using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.IBL.BO;

namespace BL.IBL
{
    public abstract class IBL
    {
        #region Adding new entity
        public abstract void AddStation(int id, int name, double longitude, double latitude,int cs);
        public abstract void AddDrone(int id, string model, int maxWeight, int stationNum/*, LogicalEntities.Location location*/);
        public abstract void AddCustomer(int id, string name, string phoneNumber, double longitude, double latitude);
        public abstract void AddParcel(int senderID, int targetID, int weight, int priority);
        #endregion

        #region Updating
        public abstract void UpdateDrone(int id, string name);
        public abstract void UpdateStation(int id, int name, int chargeSlots);
        public abstract void UpdateCustomer(int id, string name = "", string phoneNum = "");
        public abstract void CreateDroneCharge(int droneID);
        public abstract void EndDroneCharge(int droneID, int timeOfCharging);
        public abstract void AssignParcelToDrone(int droneID);
        public abstract void CollectParcel(int droneID);     //איסוף חבילה ע"י רחפן
        public abstract void ParcelDelivery(int droneID); //אספקת חבילה ע"י רחפן
        #endregion

        #region Getting item for presenting
        public abstract StationToList GetStation(int id);
        public abstract DroneToList GetDrone(int id);
        public abstract CustomerToList GetCustomer(int id);
        public abstract ParcelToList GetParcel(int id);
        #endregion 

        #region Presenting of lists
        public abstract IEnumerable<StationToList> GetListStations();
        public abstract IEnumerable<DroneToList> GetListDrones();
        public abstract IEnumerable<CustomerToList> GetListCustomers();
        public abstract IEnumerable<ParcelToList> GetListParcels();
        public abstract IEnumerable<ParcelToList> GetListParcelsNoDrone();
        public abstract IEnumerable<StationToList> GetListStationsNotFull();
        #endregion

        # region Converting an entity to entity in list
        internal abstract StationToList ConvertStationToStationInList(DAL.Station s);
        internal abstract DroneToList ConvertDroneToDroneInList(Drone d);
        internal abstract ParcelToList ConvertParcelToParcelInList(DAL.Parcel p);
        internal abstract CustomerToList ConvertCustomerToCustomerInList(DAL.Customer c);
        #endregion

        #region Help methods
        public abstract int FindDrone(int id);
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
        internal abstract List<DAL.Customer> GetListCustomersGotParcelsFromDrone(int droneID);
        #endregion 


    }
}
