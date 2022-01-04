using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Collections.Generic;
using DalApi;
using DalApi.DO;

namespace DalXml
{
    public class DalXml :IDal
    {
        #region singelton
        static readonly DalXml instance = new DalXml();
        static DalXml() 
        { 
          
        }
        DalXml() { }
        public static DalXml Instance => instance;
        #endregion

        #region paths of lists
        string customersPath = @"CustomersXml.xml";
        string dronesInChargePath = @"DronesInChargeXml.xml";
        string dronesPath = @"DronesXml.xml";
        string parcelsPath = @"ParcelssXml.xml";
        string stationsPath = @"StationsXml.xml"; // XElement
        string usersPath = @"UsersXml.xml";
        string configPath = @"ConfigXml.xml";
        #endregion

        #region get lists
        public override IEnumerable<Drone> GetDrones()
        {
            return XMLtools.LoadListFromXMLSerializer<Drone>(dronesPath);
        }
        public override IEnumerable<Station> GetBasisStations()
        {
            XElement stationsRoot = XMLtools.LoadListFromXMLElement(stationsPath);
            var allStations = from station in stationsRoot.Elements()
                              select new Station
                              {
                                  stationID = int.Parse(station.Element("stationID").Value),
                                  name = int.Parse(station.Element("name").Value),
                                  longitude = double.Parse(station.Element("longitude").Value),
                                  latitude = double.Parse(station.Element("latitude").Value),
                                  chargeSlots = int.Parse(station.Element("chargeSlots").Value)
                              };
            return allStations;
        }
        public override IEnumerable<Customer> GetCustomers()
        {
            return XMLtools.LoadListFromXMLSerializer<Customer>(customersPath);
        }
        public override IEnumerable<Parcel> GetParcels()
        {
            return XMLtools.LoadListFromXMLSerializer<Parcel>(parcelsPath);
        }
        public override IEnumerable<DroneCharge> GetDronesInCharge()
        {
            return XMLtools.LoadListFromXMLSerializer<DroneCharge>(dronesInChargePath);
        }
        public override IEnumerable<User> GetUsers()
        {
            return XMLtools.LoadListFromXMLSerializer<User>(usersPath);
        }
        #endregion

        #region add item
        public override void AddDrone(int id, string n, int w)
        {
            var listDrones = XMLtools.LoadListFromXMLSerializer<Drone>(dronesPath);
            if (listDrones.FindAll(drone => drone.ID == id).Count () > 0 )
                throw new ExistIdException("The drone already exist in the system");
            listDrones.Add(new Drone(id, n, (Enums.WeightCategories )w));
            XMLtools.SaveListToXMLSerializer<Drone>(listDrones, dronesPath); 
        }
        public override void AddCustomer(int id, string n, string p, double lo, double la)
        {
            var listCustomers = XMLtools.LoadListFromXMLSerializer<Customer>(customersPath);
            if (listCustomers.FindAll(customer => customer.ID == id).Count() > 0)
                throw new ExistIdException("The customer already exist in the system");
            listCustomers.Add(new Customer(id,n,p,lo,la));
            XMLtools.SaveListToXMLSerializer<Customer>(listCustomers, customersPath);
        }
        public override void AddStation(int id, int n, double lo, double la, int cs)
        {
            XElement stationsRoot = XMLtools.LoadListFromXMLElement(stationsPath);
            var stationElement = (from station in stationsRoot.Elements()
                                  where (station.Element("stationID").Value == id.ToString ())
                                  select station).FirstOrDefault();
            if (stationElement != null)
                throw new ExistIdException("The station already exist in the system");
            XElement newStationElement = new XElement("station",
                new XElement("stationID", id.ToString()),
                new XElement("name", n.ToString()),
                new XElement("longitude", lo.ToString()),
                new XElement("latitude", la.ToString()),
                new XElement("chargeSlots", cs.ToString()));
                
            stationsRoot.Add(stationElement);
            XMLtools.SaveListToXMLElement(stationsRoot, stationsPath);
        }
        public override void AddParcel(int sid, int tid, int w, int p)
        {
            XElement configRoot = XMLtools.LoadListFromXMLElement(configPath);
            int parcelID = int.Parse(configRoot.Element("config").Element("idNumberParcels").Value);
            configRoot.Element("config").Element("idNumberParcels").Value = (parcelID + 1).ToString();
            XMLtools.SaveListToXMLElement(configRoot, configPath);

            var listParcels = XMLtools.LoadListFromXMLSerializer<Parcel>(parcelsPath);
            listParcels.Add(new Parcel(parcelID, sid, tid, (Enums.WeightCategories)w, (Enums.Priorities)p));
            XMLtools.SaveListToXMLSerializer<Parcel>(listParcels, parcelsPath);

        }
        public override void AddDroneCharge(int dID, int sID, bool active, DateTime s)
        {
            var listDronesCharge = XMLtools.LoadListFromXMLSerializer<DroneCharge>(dronesInChargePath);
            listDronesCharge.Add(new DroneCharge(dID, sID, active, s ));
            XMLtools.SaveListToXMLSerializer<DroneCharge>(listDronesCharge , dronesInChargePath);
        }
        public override void AddUser(string name, string password, bool access)
        {
            var listUsers = XMLtools.LoadListFromXMLSerializer<User>(usersPath);
            if (listUsers.FindAll(user => user.UserName == name).Count() > 0)
                throw new ExistIdException("The user already exist in the system");
            listUsers.Add(new User(name, password , access ));
            XMLtools.SaveListToXMLSerializer<User >(listUsers, usersPath);
        }
        #endregion

        #region delete item
        public override void DeleteDrone(int id)
        {
            List<Drone> listOfAllDrones = XMLtools.LoadListFromXMLSerializer<Drone>(dronesPath);
            var drones = listOfAllDrones.FindAll(x => x.ID == id);
            if (drones.Count() == 0)
                throw new ObjectNotFoundException("The drone doesn't exist in the system");
            listOfAllDrones.Remove(drones.FirstOrDefault ());
            XMLtools.SaveListToXMLSerializer<Drone>(listOfAllDrones, dronesPath);
        }
        public override void DeleteCustomer(int id)
        {
            List<Customer> listOfAllCustomers = XMLtools.LoadListFromXMLSerializer<Customer>(customersPath);
            var customers = listOfAllCustomers.FindAll(x => x.ID == id);
            if (customers.Count() == 0)
                throw new ObjectNotFoundException("The customer doesn't exist in the system");
            listOfAllCustomers.Remove(customers.FirstOrDefault());
            XMLtools.SaveListToXMLSerializer<Customer>(listOfAllCustomers, customersPath );
        }
        public override void DeleteStation(int id)
        {
            XElement stationsRoot = XMLtools.LoadListFromXMLElement(stationsPath);
            var stationElement = (from station in stationsRoot.Elements()
                                 where (station.Element("stationID").Value == id.ToString())
                                 select station).FirstOrDefault();
            if (stationElement == null)
                throw new ObjectNotFoundException("The station does not exist in the system");

            stationElement.Remove();
            XMLtools.SaveListToXMLElement(stationsRoot, stationsPath);
        }
        public override void DeleteParcel(int id)
        {
            List<Parcel> listOfAllParcels = XMLtools.LoadListFromXMLSerializer<Parcel>(parcelsPath);
            var parcels = listOfAllParcels.FindAll(x => x.ID == id);
            if (parcels.Count() == 0)
                throw new ObjectNotFoundException("The parcel doesn't exist in the system");
            listOfAllParcels.Remove(parcels.FirstOrDefault());
            XMLtools.SaveListToXMLSerializer<Parcel>(listOfAllParcels, parcelsPath);
        }
        public override void DeleteDroneInCharge(int id)
        {
            List<DroneCharge> listOfAllDronesInCharge = XMLtools.LoadListFromXMLSerializer<DroneCharge>(dronesInChargePath);
            var dronesInCharge = listOfAllDronesInCharge.FindAll(x => x.droneID == id);
            if (dronesInCharge.Count() == 0)
                throw new ObjectNotFoundException("The drone in charge doesn't exist in the system");
            listOfAllDronesInCharge.Remove(dronesInCharge.FirstOrDefault());
            XMLtools.SaveListToXMLSerializer<DroneCharge>(listOfAllDronesInCharge, dronesInChargePath);
        }
        public override void DeleteUser(string name, string password)
        {
            List<User> listOfAllUsers = XMLtools.LoadListFromXMLSerializer<User>(usersPath);
            User user = listOfAllUsers.Find(x => x.UserName == name);
            if (user == null)
                throw new ObjectNotFoundException("The user doesn't exist in system");
            listOfAllUsers.Remove(user);
            XMLtools.SaveListToXMLSerializer<User>(listOfAllUsers, usersPath);
        }
        #endregion

        #region find item
        public override int FindDrone(int id)
        {
            var listOfDrones = XMLtools.LoadListFromXMLSerializer<Drone>(dronesPath);
            var indexDrone = listOfDrones.FindIndex(x => x.ID == id);
            return indexDrone;
        }
        public override int FindCustomer(int id)
        {
            var listOfCustomers = XMLtools.LoadListFromXMLSerializer<Customer>(customersPath);
            var indexCustomer = listOfCustomers.FindIndex(x => x.ID == id);
            return indexCustomer;
        }
        public override int FindStation(int id)
        {
            XElement stationsRoot = XMLtools.LoadListFromXMLElement(stationsPath);
            int index = 0;
            foreach (XElement stationElement in stationsRoot .Elements ())
            {
                if (stationElement.Element("stationID").Value == id.ToString())
                    return index;
                index++;
            }
            return -1;
        }
        public override int FindParcel(int id)
        {
            var listOfParcels = XMLtools.LoadListFromXMLSerializer<Parcel>(parcelsPath);
            var indexParcel = listOfParcels.FindIndex(x => x.ID == id);
            return indexParcel;
        }
        public override int FindDroneInCharge(int id)
        {
            var listOfDronesInCharge = XMLtools.LoadListFromXMLSerializer<DroneCharge>(dronesInChargePath);
            var indexDroneCharge = listOfDronesInCharge.FindIndex(x => x.droneID == id);
            return indexDroneCharge;
        }
        public override int FindUser(string name, string password)
        {
            var listOfUsers = XMLtools.LoadListFromXMLSerializer<User>(usersPath);
            var indexUser = listOfUsers.FindIndex(x => x.UserName == name && x.UserPassword == password);
            return indexUser;
        }
        #endregion

        #region find and get item
        public override Drone FindAndGetDrone(int id)
        {
            var listOfDrones = XMLtools.LoadListFromXMLSerializer<Drone>(dronesPath);
            var drone = listOfDrones.FindAll(x => x.ID == id);
            if (drone.Count () > 0)
                return drone.FirstOrDefault ();
            throw new ObjectNotFoundException("The drone doesn't exist in the system");
        }
        public override Customer FindAndGetCustomer(int id)
        {
            var listOfCustomers = XMLtools.LoadListFromXMLSerializer<Customer>(customersPath);
            var customer = listOfCustomers.FindAll(x => x.ID == id);
            if (customer.Count() > 0)
                return customer.FirstOrDefault();
            throw new ObjectNotFoundException("The customer doesn't exist in the system");
        }
        public override Station FindAndGetStation(int id)
        {
            XElement stationsRoot = XMLtools.LoadListFromXMLElement(stationsPath);
            var station = (from stationElement in stationsRoot.Elements()
                               where (stationElement.Element("stationID").Value == id.ToString ())
                               select stationElement).FirstOrDefault();
            if (station == null)
                throw new ObjectNotFoundException("The station doesn't exist in the system");
            return new Station
            {
                stationID = int.Parse (station .Element ("stationID").Value ),
                name = int.Parse (station.Element ("name").Value),
                longitude = double .Parse (station.Element ("longitude").Value ),
                latitude = double.Parse(station.Element("latitude").Value),
                chargeSlots = int.Parse (station.Element("chargeSlots").Value)
            };
        }
        public override Parcel FindAndGetParcel(int id)
        {
            var listOfParcels = XMLtools.LoadListFromXMLSerializer<Parcel>(parcelsPath);
            var parcel = listOfParcels.FindAll(x => x.ID == id);
            if (parcel.Count() > 0)
                return parcel.FirstOrDefault();
            throw new ObjectNotFoundException("The parcel doesn't exist in the system");
        }
        public override DroneCharge FindAndGetDroneInCharge(int id)
        {
            var listOfDronesCharge = XMLtools.LoadListFromXMLSerializer<DroneCharge>(dronesInChargePath);
            var droneCharge = listOfDronesCharge.FindAll(x => x.droneID == id);
            if (droneCharge.Count() > 0)
                return droneCharge.FirstOrDefault();
            throw new ObjectNotFoundException("The drone in charge doesn't exist in the system");
        }
        public override User FindAndGetUser(string name, string password)
        {
            var listOfUsers = XMLtools.LoadListFromXMLSerializer<User>(usersPath);
            var user = listOfUsers.FindAll(x => x.UserName == name && x.UserPassword== password);
            if (user.Count() > 0)
                return user.FirstOrDefault();
            throw new ObjectNotFoundException("The user doesn't exist in the system");
        }
        #endregion

        #region updating 
        public override void UpdateDrone(int id, string model)
        {
            List<Drone> listOfAllDrones = XMLtools.LoadListFromXMLSerializer<Drone>(dronesPath);
            int indexDrone = listOfAllDrones.FindIndex(x => x.ID == id);
            Drone d = listOfAllDrones [indexDrone];
            d.model = model;
            listOfAllDrones [indexDrone ] = d;
            XMLtools.SaveListToXMLSerializer<Drone>(listOfAllDrones , dronesPath );
        }
        public override void UpdateStation(int id, int name, int chargeSlots)
        {
            List<Station > listOfAllStations = XMLtools.LoadListFromXMLSerializer<Station>(stationsPath);
            int indexStation = listOfAllStations .FindIndex(x => x.stationID == id);
            Station s = listOfAllStations [indexStation];
            s.name = name;
            s.chargeSlots = chargeSlots;
            listOfAllStations [indexStation ] = s;
            XMLtools.SaveListToXMLSerializer<Station >(listOfAllStations , stationsPath );
        }
        public override void UpdateCustomer(int id, string name = "", string phoneNum = "")
        {
            List<Customer> listOfAllCustomers = XMLtools.LoadListFromXMLSerializer<Customer>(customersPath);
            int indexCustomer = listOfAllCustomers.FindIndex(x => x.ID == id);
            Customer c = listOfAllCustomers[indexCustomer];
            c.name = name;
            c.phone = phoneNum;
            listOfAllCustomers[indexCustomer] = c;
            XMLtools.SaveListToXMLSerializer<Customer>(listOfAllCustomers, customersPath);

        }
        public override void UpdateUser(string uName, string oldPassword, string newPassword)
        {
            List<User> listOfAllUsers = XMLtools.LoadListFromXMLSerializer<User>(usersPath);
            int indexUser = listOfAllUsers.FindIndex(x => x.UserName == uName && x. UserPassword == oldPassword );
            User u = listOfAllUsers[indexUser];
            u.UserPassword = newPassword;
            listOfAllUsers [indexUser] = u;
            XMLtools.SaveListToXMLSerializer<User>(listOfAllUsers , usersPath);
        }
        public override void ParcelToDrone(int parcelID, int droneId)
        {
            List<Parcel> listOfAllParcels = XMLtools.LoadListFromXMLSerializer<Parcel>(parcelsPath);
            int indexParcel = listOfAllParcels.FindIndex(x => x.ID == parcelID);
            Parcel p = listOfAllParcels[indexParcel];
            p.droneID = droneId;
            p.scheduled = DateTime.Now;
            listOfAllParcels[indexParcel] = p;
            XMLtools.SaveListToXMLSerializer<Parcel>(listOfAllParcels, parcelsPath);
        }
        public override void ParcelCollection(int parcelId, int collectorId)
        {
            List<Parcel> listOfAllParcels = XMLtools.LoadListFromXMLSerializer<Parcel>(parcelsPath);
            int indexParcel = listOfAllParcels.FindIndex(x => x.ID == parcelId);
            Parcel p = listOfAllParcels[indexParcel];
            p.droneID = collectorId;
            p.pickedUp = DateTime.Now;
            listOfAllParcels[indexParcel] = p;
            XMLtools.SaveListToXMLSerializer<Parcel>(listOfAllParcels, parcelsPath);
        }
        public override void DeliveryParcel(int parcelID, int customerId)
        {
            List<Parcel> listOfAllParcels = XMLtools.LoadListFromXMLSerializer<Parcel>(parcelsPath);
            int indexParcel = listOfAllParcels.FindIndex(x => x.ID == parcelID);
            Parcel p = listOfAllParcels[indexParcel];
            p.targetID = customerId;
            p.delivered = DateTime.Now;
            p.droneID = -1;
            listOfAllParcels[indexParcel] = p;
            XMLtools.SaveListToXMLSerializer<Parcel>(listOfAllParcels, parcelsPath);
        }
        public override void CreateDroneCharge(int stationId, int droneId)
        {
            if (FindDrone(droneId) < 0)
                throw new ObjectNotFoundException("The drone to charge does not exist in the system");

            int indexDroneInCharge = FindDroneInCharge(droneId);
            if (indexDroneInCharge < 0)
                AddDroneCharge(droneId, stationId, true, DateTime.Now);
            else
            {
                List<DroneCharge> listOfAllDronesCharge = XMLtools.LoadListFromXMLSerializer<DroneCharge>(dronesInChargePath);
                int indexDroneCharge = listOfAllDronesCharge.FindIndex(x => x.droneID == droneId);
                DroneCharge dc = listOfAllDronesCharge[indexDroneCharge];
                if (dc.stationID != stationId )
                    AddDroneCharge(droneId, stationId, true, DateTime.Now);
                else
                {
                    dc.activeCharge = true;
                    dc.start = DateTime.Now;
                    listOfAllDronesCharge[indexDroneCharge] = dc;
                    XMLtools.SaveListToXMLSerializer<DroneCharge>(listOfAllDronesCharge, dronesInChargePath);
                }
            }

            List<Station> listOfAllStations = XMLtools.LoadListFromXMLSerializer<Station>(stationsPath);
            int indexStation = listOfAllStations.FindIndex(x => x.stationID == stationId);
            if (indexStation < 0)
                throw new ObjectNotFoundException("The station of charging does not exist in the system");
            Station s = listOfAllStations[indexStation ];
            s.chargeSlots--;
            listOfAllStations[indexStation] = s;
            XMLtools.SaveListToXMLSerializer<Station>(listOfAllStations, stationsPath);

            XElement configRoot = XMLtools.LoadListFromXMLElement(configPath);
            int countActive = int.Parse (configRoot.Element("config").Element("countActive").Value)+1;
            configRoot.Element("config").Element("countActive").Value = countActive.ToString ();
            XMLtools.SaveListToXMLElement(configRoot, configPath);

        }  
        public override void EndDroneCharge(int dID, int hoursOfCharging)
        {
            int indexDcharge = FindDroneInCharge(dID);
            if (indexDcharge < 0)
                throw new ObjectNotFoundException("The drone is not in charge\n");

            List<DroneCharge> listOfAllDronesCharge = XMLtools.LoadListFromXMLSerializer<DroneCharge>(dronesInChargePath);
            int indexDroneCharge = listOfAllDronesCharge.FindIndex(x => x.droneID == dID);
            DroneCharge dc = listOfAllDronesCharge[indexDroneCharge];
            dc.activeCharge = false;
            dc.end = dc.start.AddHours(hoursOfCharging);
            listOfAllDronesCharge[indexDroneCharge] = dc;
            XMLtools.SaveListToXMLSerializer<DroneCharge>(listOfAllDronesCharge, dronesInChargePath);

            int stationID = dc.stationID;
            List<Station> listOfAllStations = XMLtools.LoadListFromXMLSerializer<Station>(stationsPath);
            int indexStation = listOfAllStations.FindIndex(x => x.stationID == stationID);
            if (indexStation < 0)
                throw new ObjectNotFoundException("The station where the drone has been charged does not exist in the system");
            Station s = listOfAllStations[indexStation];
            s.chargeSlots++;
            listOfAllStations[indexStation] = s;
            XMLtools.SaveListToXMLSerializer(listOfAllStations, stationsPath);

            XElement configRoot = XMLtools.LoadListFromXMLElement(configPath);
            int countActive = int.Parse(configRoot.Element("config").Element("countActive").Value) - 1;
            configRoot.Element("config").Element("countActive").Value = countActive.ToString();
            XMLtools.SaveListToXMLElement(configRoot, configPath);
        }
        public override void UpdateSendingOfParcel(int parcelID)
        {
            List<Parcel> listOfAllParcels = XMLtools.LoadListFromXMLSerializer<Parcel>(parcelsPath);
            int indexParcel = listOfAllParcels.FindIndex(x => x.ID == parcelID);
            Parcel p = listOfAllParcels[indexParcel];
            p.confirmedSending = true;
            listOfAllParcels[indexParcel] = p;
            XMLtools.SaveListToXMLSerializer<Parcel>(listOfAllParcels, parcelsPath);
        }
        public override void UpdateRecievingOfParcel(int parcelID)
        {
            List<Parcel> listOfAllParcels = XMLtools.LoadListFromXMLSerializer<Parcel>(parcelsPath);
            int indexParcel = listOfAllParcels.FindIndex(x => x.ID == parcelID);
            Parcel p = listOfAllParcels[indexParcel];
            p.confirmRecieving = true;
            listOfAllParcels[indexParcel] = p;
            XMLtools.SaveListToXMLSerializer<Parcel>(listOfAllParcels, parcelsPath);
        }
        #endregion

        #region get fields of class config
        public override double[] GetPowerConsumption()
        {
            XElement configRoot = XMLtools.LoadListFromXMLElement(configPath);
            double[] power = new double[4];
            power[0] =double.Parse(configRoot.Element("config").Element ("idlePowerConsumption").Value );
            power[1] = double.Parse(configRoot.Element("config").Element("lightPowerConsumption").Value);
            power[2] = double.Parse(configRoot.Element("config").Element("mediumPowerConsumption").Value);
            power[3] = double.Parse(configRoot.Element("config").Element("heavyPowerConsumption").Value);
            return power;
        }
        public override int[] GetParcelsPriority()
        {
            XElement configRoot = XMLtools.LoadListFromXMLElement(configPath);
            int[] parcelsPriority = new int[3];
            parcelsPriority[0] = int.Parse(configRoot.Element("config").Element("parcelsPriority").Element("normal").Value);
            parcelsPriority[1] = int.Parse(configRoot.Element("config").Element("parcelsPriority").Element("quick").Value);
            parcelsPriority[2] = int.Parse(configRoot.Element("config").Element("parcelsPriority").Element("emergency").Value);
            return parcelsPriority;
        }
        public override int GetDroneLoadingRate()
        {
            XElement configRoot = XMLtools.LoadListFromXMLElement(configPath);
            int loadingRate = int.Parse (configRoot.Element("config").Element("droneLoadingRate").Value);
            return loadingRate;
        }
        #endregion

    }
}
