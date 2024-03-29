﻿using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DAL.DalApi;
using DAL.DalApi.DO;

namespace DalXml
{
    public class DalXml :IDal
    {
        #region singelton
        private static readonly Lazy<DalXml> lazy = new Lazy<DalXml>(() => new DalXml());
        public static DalXml Instance { get { return lazy.Value; } }
        private DalXml(){}
        #endregion

        #region paths of lists
        string customersPath = @"C:\Users\1\Source\Repos\sheiner32\dotnet5782_0938\targilZero\customersXML.xml";
        string dronesInChargePath = @"C:\Users\1\Source\Repos\sheiner32\dotnet5782_0938\targilZero\dronesInChargeXML.xml";
        string dronesPath = @"C:\Users\1\Source\Repos\sheiner32\dotnet5782_0938\targilZero\dronesXML.xml";
        string parcelsPath = @"C:\Users\1\Source\Repos\sheiner32\dotnet5782_0938\targilZero\parcelsXML.xml";
        string stationsPath = @"C:\Users\1\Source\Repos\sheiner32\dotnet5782_0938\targilZero\stationsXML.xml"; // XElement
        string usersPath = @"C:\Users\1\Source\Repos\sheiner32\dotnet5782_0938\targilZero\usersXML.xml";
        string configPath = @"C:\Users\1\Source\Repos\sheiner32\dotnet5782_0938\targilZero\configXML.xml";

        #endregion

        #region static help functions for converting
        public static Enums.WeightCategories ConvertStringToWeight(string str)
        {
            if (str == "light")
                return (Enums.WeightCategories)0;
            else if (str == "intermediate")
                return (Enums.WeightCategories)1;
            return (Enums.WeightCategories)2;
        }
        public static Enums.Priorities ConvertStringToPriority(string str)
        {
            if (str == "normal")
                return (Enums.Priorities)0;
            else if (str == "quick")
                return (Enums.Priorities)1;
            return (Enums.Priorities)2;
        }
        public static bool ConvertStringToBool(string str)
        {
            if (str == "true" || str=="True")
                return true;
            return false;
        }
        #endregion

        #region get lists
        /// <summary>
        /// the method returns the list of the drones
        /// </summary>
        /// <returns>list of drones</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override IEnumerable<Drone> GetDrones()
        {
            XElement dronesRoot = XMLtools.LoadListFromXMLElement(dronesPath);
            var allDrones = from drone in dronesRoot.Elements()
                              select new Drone()
                              {
                                 ID= int.Parse (drone.Element ("ID").Value),
                                 model = drone .Element ("model").Value ,
                                 maxWeight = ConvertStringToWeight (drone.Element("maxWeight").Value) 
                              };
            return allDrones;
        }
        /// <summary>
        /// the method returns the list of the stations
        /// </summary>
        /// <returns>list of stations</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override IEnumerable<Station> GetBasisStations()
        {
            XElement stationsRoot = XMLtools.LoadListFromXMLElement(stationsPath);
            var allStations = from station in stationsRoot.Elements()
                              select new Station()
                              {
                                  stationID = int.Parse(station.Element("stationID").Value),
                                  name = int.Parse(station.Element("name").Value),
                                  longitude = double.Parse(station.Element("longitude").Value),
                                  latitude = double.Parse(station.Element("latitude").Value),
                                  chargeSlots = int.Parse(station.Element("chargeSlots").Value)
                              };
            return allStations;
        }
        /// <summary>
        /// the method returns the list of the customers
        /// </summary>
        /// <returns>list of customers</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override IEnumerable<Customer> GetCustomers()
        {
            XElement CustomersRoot = XMLtools.LoadListFromXMLElement(customersPath);
            var allCustomers = from customer in CustomersRoot.Elements()
                               select new Customer()
                               {
                                   ID = int.Parse(customer.Element("ID").Value),
                                   name = customer.Element("name").Value,
                                   phone = customer.Element("phone").Value,
                                   longitude = double.Parse (customer.Element("longitude").Value),
                                   latitude = double.Parse(customer.Element("latitude").Value)  
                              };
            return allCustomers;
        }
        /// <summary>
        /// the method returns the list of the parcels
        /// </summary>
        /// <returns>list of parcels</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override IEnumerable<Parcel> GetParcels()
        {
            XElement parcelsRoot = XMLtools.LoadListFromXMLElement(parcelsPath);
            List<Parcel> allParcels = new();
            Parcel help = new();
            foreach (var item in parcelsRoot .Elements ())
            {
                help.ID = int.Parse(item.Element("ID").Value);
                help.senderID = int.Parse(item.Element("senderID").Value);
                help.targetID = int.Parse(item.Element("targetID").Value);
                help.droneID = int.Parse(item.Element("droneID").Value);
                help.weight = ConvertStringToWeight(item.Element("weight").Value);
                help.priority = ConvertStringToPriority(item.Element("priority").Value);
                help.requested = DateTime.Parse(item.Element("requested").Value);
                help.scheduled = DateTime.Parse(item.Element("scheduled").Value);
                help.pickedUp = DateTime.Parse(item.Element("pickedUp").Value);
                help.delivered = DateTime.Parse(item.Element("delivered").Value);
                help.confirmedSending = ConvertStringToBool(item.Element("confirmedSending").Value);
                help.confirmRecieving = ConvertStringToBool(item.Element("confirmRecieving").Value);
                allParcels.Add(help);
            }
            return allParcels;
        }
        /// <summary>
        /// the method returns the list of the drones in charge
        /// </summary>
        /// <returns>list of drones in charge</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override IEnumerable<DroneCharge> GetDronesInCharge()
        {
            XElement DronesInChargeRoot = XMLtools.LoadListFromXMLElement(dronesInChargePath);
            var allDronesInCharge = from droneCharge in DronesInChargeRoot.Elements()
                             select new DroneCharge()
                             {
                                droneID = int.Parse (droneCharge .Element ("droneID").Value ),
                                stationID = int.Parse (droneCharge .Element ("stationID").Value ),
                                activeCharge = bool.Parse (droneCharge .Element ("activeCharge").Value ),
                                start = DateTime .Parse (droneCharge .Element ("start").Value )
                             };
            return allDronesInCharge;
        }
        /// <summary>
        /// the method returns the list of the users
        /// </summary>
        /// <returns>list of users</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override IEnumerable<User> GetUsers()
        {
            XElement usersRoot = XMLtools.LoadListFromXMLElement(usersPath);
            var allUsers = from user in usersRoot.Elements()
                           select new User()
                           {
                               UserName = user.Element("UserName").Value,
                               UserPassword = user.Element ("UserPassword").Value, 
                               UserAccessManagement = ConvertStringToBool (user.Element ("UserAccessManagement").Value )
                               
                           };

            return allUsers;
        }
        #endregion

        #region add item
        /// <summary>
        /// the method adds new drone to the storage
        /// </summary>
        /// <param name="id"></param>
        /// <param name="n"></param>
        /// <param name="w"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override void AddDrone(int id, string n, int w)
        {
            var listDrones = XMLtools.LoadListFromXMLSerializer<Drone>(dronesPath);
            if (listDrones.FindAll(drone => drone.ID == id).Count () > 0 )
                throw new ExistIdException("The drone already exist in the system");
            listDrones.Add(new Drone(id, n, (Enums.WeightCategories )w));
            XMLtools.SaveListToXMLSerializer<Drone>(listDrones, dronesPath); 
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
            var listCustomers = XMLtools.LoadListFromXMLSerializer<Customer>(customersPath);
            if (listCustomers.FindAll(customer => customer.ID == id).Count() > 0)
                throw new ExistIdException("The customer already exist in the system");
            listCustomers.Add(new Customer(id,n,p,lo,la));
            XMLtools.SaveListToXMLSerializer<Customer>(listCustomers, customersPath);
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
            var listStations = XMLtools.LoadListFromXMLSerializer<Station>(stationsPath);
            if (listStations.FindAll(station => station.stationID == id).Count() > 0)
                throw new ExistIdException("The station already exist in the system");
            listStations.Add(new Station(id, n, lo, la, cs));
            XMLtools.SaveListToXMLSerializer<Station>(listStations, stationsPath);
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
            XElement configRoot = XMLtools.LoadListFromXMLElement(configPath);
            int parcelID = int.Parse(configRoot.Element("idNumberParcels").Value);
            configRoot.Element("idNumberParcels").Value = (parcelID + 1).ToString();
            XMLtools.SaveListToXMLElement(configRoot, configPath);

            XElement parcelsRoot = XMLtools.LoadListFromXMLElement(parcelsPath);
            XElement newParcelElement = new XElement("Parcel",
               new XElement("ID", parcelID),
               new XElement("senderID", sid),
               new XElement("targetID", tid),
               new XElement("weight", (Enums.WeightCategories)w),
               new XElement("priority", (Enums.Priorities)p),
               new XElement("requested", DateTime.Now ),
               new XElement("droneID", -1),
               new XElement("scheduled", "0001-01-01"),
               new XElement("pickedUp", "0001-01-01"),
               new XElement("delivered", "0001-01-01"),
               new XElement("confirmedSending", "false"),
               new XElement("confirmRecieving", "false"));
            parcelsRoot.Add(newParcelElement);
            XMLtools.SaveListToXMLElement(parcelsRoot, parcelsPath);

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
            var listDronesCharge = XMLtools.LoadListFromXMLSerializer<DroneCharge>(dronesInChargePath);
            listDronesCharge.Add(new DroneCharge(dID, sID, active, s ));
            XMLtools.SaveListToXMLSerializer<DroneCharge>(listDronesCharge , dronesInChargePath);
        }
        /// <summary>
        /// the method adds new user to the storage
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <param name="access"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
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
        /// <summary>
        /// the method removes specific drone from the storage
        /// </summary>
        /// <param name="id"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override void DeleteDrone(int id)
        {
            List<Drone> listOfAllDrones = XMLtools.LoadListFromXMLSerializer<Drone>(dronesPath);
            var drones = listOfAllDrones.FindAll(x => x.ID == id);
            if (drones.Count() == 0)
                throw new ObjectNotFoundException("The drone doesn't exist in the system");
            listOfAllDrones.Remove(drones.FirstOrDefault ());
            XMLtools.SaveListToXMLSerializer<Drone>(listOfAllDrones, dronesPath);
        }
        /// <summary>
        /// the method removes specific customer from the storage
        /// </summary>
        /// <param name="id"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override void DeleteCustomer(int id)
        {
            List<Customer> listOfAllCustomers = XMLtools.LoadListFromXMLSerializer<Customer>(customersPath);
            var customers = listOfAllCustomers.FindAll(x => x.ID == id);
            if (customers.Count() == 0)
                throw new ObjectNotFoundException("The customer doesn't exist in the system");
            listOfAllCustomers.Remove(customers.FirstOrDefault());
            XMLtools.SaveListToXMLSerializer<Customer>(listOfAllCustomers, customersPath );
        }
        /// <summary>
        /// the method removes specific station from the storage
        /// </summary>
        /// <param name="id"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
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
        /// <summary>
        /// the method removes specific parcel from the storage
        /// </summary>
        /// <param name="id"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override void DeleteParcel(int id)
        {
            List<Parcel> listOfAllParcels = XMLtools.LoadListFromXMLSerializer<Parcel>(parcelsPath);
            var parcels = listOfAllParcels.FindAll(x => x.ID == id);
            if (parcels.Count() == 0)
                throw new ObjectNotFoundException("The parcel doesn't exist in the system");
            listOfAllParcels.Remove(parcels.FirstOrDefault());
            XMLtools.SaveListToXMLSerializer<Parcel>(listOfAllParcels, parcelsPath);
        }
        /// <summary>
        /// the method removes specific drone in charge from the storage
        /// </summary>
        /// <param name="id"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override void DeleteDroneInCharge(int id)
        {
            List<DroneCharge> listOfAllDronesInCharge = XMLtools.LoadListFromXMLSerializer<DroneCharge>(dronesInChargePath);
            var dronesInCharge = listOfAllDronesInCharge.FindAll(x => x.droneID == id);
            if (dronesInCharge.Count() == 0)
                throw new ObjectNotFoundException("The drone in charge doesn't exist in the system");
            listOfAllDronesInCharge.Remove(dronesInCharge.FirstOrDefault());
            XMLtools.SaveListToXMLSerializer<DroneCharge>(listOfAllDronesInCharge, dronesInChargePath);
        }
        /// <summary>
        /// the method removes specific user from the storage
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override void DeleteUser(string name, string password)
        {
            List<User> listOfAllUsers = XMLtools.LoadListFromXMLSerializer<User>(usersPath);
            var user = listOfAllUsers.FindAll(x => x.UserName == name);
            if (user.Count ()==0)
                throw new ObjectNotFoundException("The user doesn't exist in system");
            listOfAllUsers.Remove(user.FirstOrDefault ());
            XMLtools.SaveListToXMLSerializer<User>(listOfAllUsers, usersPath);
        }
        #endregion

        #region find item
        /// <summary>
        /// the method finds and returns the index in the drone's array of specific drone in the storage
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override int FindDrone(int id)
        {
            XElement dronesRoot = XMLtools.LoadListFromXMLElement(dronesPath);
            int index = 0;
            foreach (XElement droneElement in dronesRoot.Elements())
            {
                if (droneElement.Element("ID").Value == id.ToString())
                    return index;
                index++;
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
            XElement customersRoot = XMLtools.LoadListFromXMLElement(customersPath);
            int index = 0;
            foreach (XElement customerElement in customersRoot.Elements())
            {
                if (customerElement.Element("ID").Value == id.ToString())
                    return index;
                index++;
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
        /// <summary>
        /// the method finds and returns the index in the parcel's array of specific parcel in the storage
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override int FindParcel(int id)
        {
            XElement parcelsRoot = XMLtools.LoadListFromXMLElement(parcelsPath);
            int index = 0;
            foreach (XElement parcelElement in parcelsRoot.Elements())
            {
                if (parcelElement.Element("ID").Value == id.ToString())
                    return index;
                index++;
            }
            return -1;
        }
        /// <summary>
        /// the method finds and returns the index in the array of drones in charge of specific drone in the storage
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override int FindDroneInCharge(int id)
        {
            XElement dronesChargeRoot = XMLtools.LoadListFromXMLElement(dronesInChargePath);
            int index = 0;
            foreach (XElement droneChargeElement in dronesChargeRoot.Elements())
            {
                if (droneChargeElement .Element ("droneID").Value == id.ToString ())
                    return index;
                index++;
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
            XElement usersRoot = XMLtools.LoadListFromXMLElement(usersPath);
            int index = 0;
            foreach (XElement userElement in usersRoot.Elements())
            {
                if (userElement.Element("UserName").Value == name && userElement .Element ("UserPassword").Value== password )
                    return index;
                index++;
            }
            return -1;

        }
        #endregion

        #region find and get item
        /// <summary>
        /// the method finds and returns specific drone in the storage
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override Drone FindAndGetDrone(int id)
        {
            var listDrones = GetDrones();
            foreach (Drone current in listDrones)
            {
                if (current.ID == id)
                    return current;
            }
            throw new ObjectNotFoundException("The drone doesn't exist in the system");
        }
        /// <summary>
        ///  the method finds and returns specific customer in the storage
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override Customer FindAndGetCustomer(int id)
        {
            var listCustomers = GetCustomers();
            foreach (Customer current in listCustomers)
            {
                if (current.ID == id)
                    return current;
            }
            throw new ObjectNotFoundException("The customer doesn't exist in the system");
        }
        /// <summary>
        ///  the method finds and returns specific station in the storage
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
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
        /// <summary>
        ///  the method finds and returns specific parcel in the storage
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override Parcel FindAndGetParcel(int id)
        {
            var listParcels = GetParcels();
            foreach (Parcel current in listParcels)
            {
                if (current.ID == id)
                    return current;
            }
            throw new ObjectNotFoundException("The parcel doesn't exist in the system");
        }
        /// <summary>
        ///  the method finds and returns specific drone in charge in the storage
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override DroneCharge FindAndGetDroneInCharge(int id)
        {
            var listDronesCharge = GetDronesInCharge();
            foreach (DroneCharge current in listDronesCharge)
            {
                if (current.droneID == id )
                    return current;
            }
            throw new ObjectNotFoundException("The drone in charge doesn't exist in the system");
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
            var listUsers = GetUsers();
            foreach(User current in listUsers )
            {
                if (current.UserName == name && current.UserPassword == password)
                    return current;
            }
            throw new ObjectNotFoundException("The user doesn't exist in the system");
        }
        #endregion

        #region updating 
        /// <summary>
        /// the method updates the model of specific drone in the storage
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override void UpdateDrone(int id, string model)
        {
            List<Drone> listOfAllDrones = XMLtools.LoadListFromXMLSerializer<Drone>(dronesPath);
            int indexDrone = listOfAllDrones.FindIndex(x => x.ID == id);
            Drone d = listOfAllDrones [indexDrone];
            d.model = model;
            listOfAllDrones [indexDrone ] = d;
            XMLtools.SaveListToXMLSerializer<Drone>(listOfAllDrones , dronesPath );
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
            List<Station > listOfAllStations = XMLtools.LoadListFromXMLSerializer<Station>(stationsPath);
            int indexStation = listOfAllStations .FindIndex(x => x.stationID == id);
            Station s = listOfAllStations [indexStation];
            s.name = name;
            s.chargeSlots = chargeSlots;
            listOfAllStations [indexStation ] = s;
            XMLtools.SaveListToXMLSerializer<Station >(listOfAllStations , stationsPath );
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
            List<Customer> listOfAllCustomers = XMLtools.LoadListFromXMLSerializer<Customer>(customersPath);
            int indexCustomer = listOfAllCustomers.FindIndex(x => x.ID == id);
            Customer c = listOfAllCustomers[indexCustomer];
            c.name = name;
            c.phone = phoneNum;
            listOfAllCustomers[indexCustomer] = c;
            XMLtools.SaveListToXMLSerializer<Customer>(listOfAllCustomers, customersPath);

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
            List<User> listOfAllUsers = XMLtools.LoadListFromXMLSerializer<User>(usersPath);
            int indexUser = listOfAllUsers.FindIndex(x => x.UserName == uName && x. UserPassword == oldPassword );
            User u = listOfAllUsers[indexUser];
            u.UserPassword = newPassword;
            listOfAllUsers [indexUser] = u;
            XMLtools.SaveListToXMLSerializer<User>(listOfAllUsers , usersPath);
        }
        /// <summary>
        /// the method assigns parcel to drone
        /// </summary>
        /// <param name="parcelID"></param>
        /// <param name="droneId"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override void ParcelToDrone(int parcelID, int droneId)
        {
            XElement parcelsRoot = XMLtools.LoadListFromXMLElement(parcelsPath);
            XElement myParcel;
            foreach (var item in parcelsRoot.Elements())
            {
                if (int.Parse(item.Element("ID").Value) == parcelID)
                {
                    myParcel = item;
                    myParcel.Element("droneID").Value = droneId.ToString();
                    myParcel.Element("scheduled").Value = DateTime.Now.ToString();
                    break;
                }
            }
            XMLtools.SaveListToXMLElement(parcelsRoot, parcelsPath);
        }
        /// <summary>
        /// method for collecting of parcel by drone
        /// </summary>
        /// <param name="parcelId"></param>
        /// <param name="collectorId"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override void ParcelCollection(int parcelId, int collectorId)
        {
            XElement parcelsRoot = XMLtools.LoadListFromXMLElement(parcelsPath);
            XElement myParcel;
            foreach (var item in parcelsRoot.Elements())
            {

                if (int.Parse(item.Element("ID").Value) == parcelId)
                {
                    myParcel = item;
                    myParcel.Element("droneID").Value = collectorId.ToString();
                    myParcel.Element("pickedUp").Value = DateTime.Now.ToString();
                    break;
                }
            }
            XMLtools.SaveListToXMLElement(parcelsRoot, parcelsPath);
        }
        /// <summary>
        /// method for delivery of parcel by drone to the destination
        /// </summary>
        /// <param name="parcelID"></param>
        /// <param name="customerId"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override void DeliveryParcel(int parcelID, int customerId)
        {
            XElement parcelsRoot = XMLtools.LoadListFromXMLElement(parcelsPath);
            XElement myParcel;
            foreach (var item in parcelsRoot.Elements())
            {

                if (int.Parse(item.Element("ID").Value) == parcelID)
                {
                    myParcel = item;
                    myParcel.Element("targetID").Value = customerId .ToString ();
                    myParcel.Element("droneID").Value = "-1";
                    myParcel.Element("delivered").Value = DateTime.Now.ToString();
                    break;
                }
            }
            XMLtools.SaveListToXMLElement(parcelsRoot, parcelsPath);
        }
        /// <summary>
        /// the method creates charging of drone in specific station
        /// </summary>
        /// <param name="stationId"></param>
        /// <param name="droneId"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override void CreateDroneCharge(int stationId, int droneId)
        {
            if (FindDrone(droneId) < 0)
                throw new ObjectNotFoundException("The drone to charge does not exist in the system");
            
            int indexDroneInCharge = FindDroneInCharge(droneId);
            bool activeChargeIncreased = false , chargeSlotsDecreased = true, addNewCharge = false;
            Station stationToUpdate; //התחנה שבה יש לעדכן מספר חריצי טעינה
            DroneCharge dc;
            
            if (indexDroneInCharge < 0) //לא קיימת טעינת רחפן של רחפן זה
            {
                addNewCharge = true;
                activeChargeIncreased = true;
            }
            else //קיימת טעינה של רחפן זה
            {
                List<DroneCharge> listOfAllDronesCharge = XMLtools.LoadListFromXMLSerializer<DroneCharge>(dronesInChargePath);
                indexDroneInCharge = listOfAllDronesCharge.FindIndex(x =>x.droneID == droneId && x.stationID == stationId);
                if (indexDroneInCharge >= 0) //קיימת טעינה בתחנה זו
                {
                    dc = listOfAllDronesCharge[indexDroneInCharge];
                    if (!dc.activeCharge)//הטעינה בתחנה זו לא פעילה
                    {
                        dc.activeCharge = true;
                        dc.start = DateTime.Now;
                        activeChargeIncreased = true;
                    }
                    else//הטעינה בתחנה זו פעילה
                    {
                        chargeSlotsDecreased = false;
                    }
                    listOfAllDronesCharge[indexDroneInCharge] = dc;
                }
                else //לא קיימת טעינה בתחנה זו
                {
                    indexDroneInCharge = listOfAllDronesCharge.FindIndex(x => x.droneID == droneId && x.activeCharge );
                    addNewCharge = true;
                    if(indexDroneInCharge >= 0) //הטעינה בתחנה אחרת פעילה
                    {
                        dc = listOfAllDronesCharge[indexDroneInCharge];
                        dc.activeCharge = false;
                        dc.end = DateTime.Now;
                        stationToUpdate = FindAndGetStation(dc.stationID);
                        UpdateStation(dc.stationID, stationToUpdate.name, stationToUpdate.chargeSlots + 1);
                        listOfAllDronesCharge[indexDroneInCharge] = dc;
                    }
                    else //הטעינה בתחנה אחרת לא פעילה
                    {
                        activeChargeIncreased = true;
                    }
                }
                XMLtools.SaveListToXMLSerializer<DroneCharge>(listOfAllDronesCharge, dronesInChargePath);
            }
            if(addNewCharge)
                AddDroneCharge(droneId, stationId, true, DateTime.Now);
            if (chargeSlotsDecreased)
            {
                stationToUpdate = FindAndGetStation(stationId);
                UpdateStation(stationId, stationToUpdate .name , stationToUpdate .chargeSlots -1);
            }
            if(activeChargeIncreased)
            {
                XElement configRoot = XMLtools.LoadListFromXMLElement(configPath);
                int countActive = int.Parse(configRoot.Element("countActive").Value) + 1;
                configRoot.Element("countActive").Value = countActive.ToString();
                XMLtools.SaveListToXMLElement(configRoot, configPath);
            } 
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
                throw new ObjectNotFoundException("The drone does not in charge\n");

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
            int countActive = int.Parse(configRoot.Element("countActive").Value) - 1;
            configRoot.Element("countActive").Value = countActive.ToString();
            XMLtools.SaveListToXMLElement(configRoot, configPath);
        }
        /// <summary>
        /// the method updates collecting of parcel confirmation
        /// </summary>
        /// <param name="parcelID"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override void UpdateSendingOfParcel(int parcelID)
        {
            XElement parcelsRoot = XMLtools.LoadListFromXMLElement(parcelsPath);
            XElement myParcel;
            foreach (var item in parcelsRoot.Elements())
            {
                if (int.Parse(item.Element("ID").Value) == parcelID)
                {
                    myParcel = item;
                    myParcel.Element("confirmedSending").Value = true.ToString ();
                    break;
                }
            }
            XMLtools.SaveListToXMLElement(parcelsRoot, parcelsPath);
        }
        /// <summary>
        /// the method updates supplying of parcel confirmation
        /// </summary>
        /// <param name="parcelID"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override void UpdateRecievingOfParcel(int parcelID)
        {
            XElement parcelsRoot = XMLtools.LoadListFromXMLElement(parcelsPath);
            XElement myParcel;
            foreach (var item in parcelsRoot.Elements())
            {
                if (int.Parse(item.Element("ID").Value) == parcelID)
                {
                    myParcel = item;
                    myParcel.Element("confirmRecieving").Value = true.ToString();
                    break;
                }
            }
            XMLtools.SaveListToXMLElement(parcelsRoot, parcelsPath);
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
            XElement configRoot = XMLtools.LoadListFromXMLElement(configPath);
            double[] power = new double[4];
            power[0] =double.Parse(configRoot.Element("idlePowerConsumption").Value );
            power[1] = double.Parse(configRoot.Element("lightPowerConsumption").Value);
            power[2] = double.Parse(configRoot.Element("mediumPowerConsumption").Value);
            power[3] = double.Parse(configRoot.Element("heavyPowerConsumption").Value);
            
            return power;
        }
        /// <summary>
        /// the method returns an array of numbers of parcels in each priority
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override int[] GetParcelsPriority()
        {
            XElement configRoot = XMLtools.LoadListFromXMLElement(configPath);
            int[] parcelsPriority = new int[3];
            int i = 0;
            foreach(var item in configRoot.Element("CountParcelsPriority").Elements())
            {
                parcelsPriority[i] = int.Parse (item.Value);
                i++;
            }
            return parcelsPriority;
        }
        /// <summary>
        /// the method returns the loading rate of drone
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override int GetDroneLoadingRate()
        {
            XElement configRoot = XMLtools.LoadListFromXMLElement(configPath);
            int loadingRate = int.Parse (configRoot.Element("droneLoadingRate").Value);
            return loadingRate;
        }
        #endregion

    }
}
