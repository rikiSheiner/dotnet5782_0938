using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    namespace IDAL
    {
        abstract public class IDal
        {
            abstract public IEnumerable<Drone> GetDrones();
            abstract public IEnumerable<Station> GetBasisStations();
            abstract public IEnumerable<Customer> GetCustomers();
            abstract public IEnumerable<Parcel> GetParcels();
            abstract public IEnumerable<DroneCharge> GetDronesInCharge();

            abstract public void Initialize();

            abstract public void AddDrone(int id, string n, int w);
            abstract public void AddCustomer(int id, string n, string p, double lo, double la);
            abstract public void AddStation(int id, int n, double lo, double la, int cs);
            abstract public void AddParcel(int sid, int tid, int w, int p, int did);
            abstract public void AddDroneCharge(int dID, int sID, bool active,DateTime s);

            abstract public void DeleteDrone(int id);
            abstract public void DeleteCustomer(int id);
            abstract public void DeleteStation(int id);
            abstract public void DeleteParcel(int id);
            abstract public void DeleteDroneInCharge(int id);

            abstract public int FindDrone(int id);
            abstract public int FindCustomer(int id);
            abstract public int FindStation(int id);
            abstract public int FindParcel(int id);
            abstract public int FindDroneInCharge(int id);

            abstract public Drone FindAndGetDrone(int id);
            abstract public Customer FindAndGetCustomer(int id);
            abstract public Station FindAndGetStation(int id);
            abstract public Parcel FindAndGetParcel(int id);
            abstract public DroneCharge FindAndGetDroneInCharge(int id);

            abstract public void ParcelToDrone(int parcelID, int droneId);
            abstract public void ParcelCollection(int parcelId, int collectorId);
            abstract public void DeliveryParcel(int parcelID, int customerId);
            abstract public void CreateDroneCharge(int stationId, int droneId);
            abstract public void EndDroneCharge(int dID, int hoursOfCharging);

            abstract public double[] GetPowerConsumption(); //בקשת צריכת חשמל ע"י רחפן
            abstract public int[] GetParcelsPriority();
            abstract public int GetDroneLoadingRate();
        }
    }

}
