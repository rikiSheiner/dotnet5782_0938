using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.DalApi;
using DAL.DalApi.DO;

namespace DAL
{
    namespace DalApi
    {
        abstract public class IDal
        {
            #region get lists
            abstract public IEnumerable<Drone> GetDrones();
            abstract public IEnumerable<Station> GetBasisStations();
            abstract public IEnumerable<Customer> GetCustomers();
            abstract public IEnumerable<Parcel> GetParcels();
            abstract public IEnumerable<DroneCharge> GetDronesInCharge();
            abstract public IEnumerable<User> GetUsers();
            #endregion

            #region add item
            abstract public void AddDrone(int id, string n, int w);
            abstract public void AddCustomer(int id, string n, string p, double lo, double la);
            abstract public void AddStation(int id, int n, double lo, double la, int cs);
            abstract public void AddParcel(int sid, int tid, int w, int p);
            abstract public void AddDroneCharge(int dID, int sID, bool active,DateTime s);
            abstract public void AddUser(string name, string password, bool access);
            #endregion

            #region delete item
            abstract public void DeleteDrone(int id);
            abstract public void DeleteCustomer(int id);
            abstract public void DeleteStation(int id);
            abstract public void DeleteParcel(int id);
            abstract public void DeleteDroneInCharge(int id);
            abstract public void DeleteUser(string name,string password);
            #endregion 

            #region find item
            abstract public int FindDrone(int id);
            abstract public int FindCustomer(int id);
            abstract public int FindStation(int id);
            abstract public int FindParcel(int id);
            abstract public int FindDroneInCharge(int id);
            abstract public int FindUser(string name, string password);
            #endregion

            #region find and get item
            abstract public Drone FindAndGetDrone(int id);
            abstract public Customer FindAndGetCustomer(int id);
            abstract public Station FindAndGetStation(int id);
            abstract public Parcel FindAndGetParcel(int id);
            abstract public DroneCharge FindAndGetDroneInCharge(int id);
            abstract public User FindAndGetUser(string name, string password);
            #endregion

            #region update 
            abstract public void ParcelToDrone(int parcelID, int droneId);
            abstract public void ParcelCollection(int parcelId, int collectorId);
            abstract public void DeliveryParcel(int parcelID, int customerId);
            abstract public void CreateDroneCharge(int stationId, int droneId);
            abstract public void EndDroneCharge(int dID, int hoursOfCharging);
            public abstract void UpdateSendingOfParcel(int parcelID);
            public abstract void UpdateRecievingOfParcel(int parcelID);
            #endregion

            #region get fields of class config
            abstract public double[] GetPowerConsumption(); //בקשת צריכת חשמל ע"י רחפן
            abstract public int[] GetParcelsPriority();
            abstract public int GetDroneLoadingRate();
            #endregion 
        }
    }

}
