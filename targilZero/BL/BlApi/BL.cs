using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using DAL.DalApi;
using BL.BO;


namespace BL.BlApi
{
    public class BL : IBL
    {
        #region fields
        internal IDal datafield;
        internal List<Drone> dronesBL;
        private double[] powerConsumption;
        private int droneChargeRate;
        #endregion

        private static readonly Lazy<BL> lazy = new Lazy<BL>(() => new BL());

        internal static BL Instance { get { return lazy.Value; } }

        #region constructor
        /// <summary>
        /// constructor
        /// </summary>
        internal BL()
        {
            //datafield = DAL.DalFactory.GetDal("DalObject");
            datafield = DAL.DalFactory.GetDal("DALXml");

            powerConsumption = datafield.GetPowerConsumption(); //צריכת חשמל ע"י הרחפנים
            droneChargeRate = datafield.GetDroneLoadingRate(); //קצב טעינת הרחפנים
            IEnumerable<DAL.DalApi.DO.Drone> dronesList = datafield.GetDrones();
            dronesBL = new List<Drone>();

            //הוספת הרחפנים שנמצאים  בשכבת הנתונים לרשימת הרחפנים שישנה בשכבה הלוגית
            DAL.DalApi.DO.Drone temp;
            Drone d;
            Random r = new Random();
            for (int i = 0; i < dronesList.Count(); i++)
            {
                temp = dronesList.ElementAt(i);
                d = new Drone();
                d.ID = temp.ID;
                d.model = temp.model;
                d.maxWeight = (Enums.WeightCategories)temp.maxWeight;
                d.battery = r.Next(30,51);
                d.droneStatus = Enums.DroneStatuses.available;
                d.location = new LogicalEntities.Location(r.Next(0, 360), r.Next(0, 360));
                dronesBL.Add(d);
            }

            Drone tempD;
            bool parcelToDrone = false, findParcel = false;
            DAL.DalApi.DO.Parcel desired = new DAL.DalApi.DO.Parcel();

            for (int i = 0; i < dronesList.Count(); i++)
            {
                tempD = dronesBL[i];
                foreach (DAL.DalApi.DO.Parcel p in datafield.GetParcels())
                {
                    if (p.droneID == dronesBL[i].ID)
                    { 
                        desired = p;
                        findParcel = true;
                        break; 
                    }
                }
                parcelToDrone = desired.scheduled != null && desired.scheduled < DateTime.Now  && (desired.delivered == null || desired.delivered > DateTime.Now);
                if (parcelToDrone) //אם החבילה שויכה אך לא נאספה
                {
                    //עדכון מצב הרחפן למבצע משלוח

                    tempD.droneStatus = Enums.DroneStatuses.delivery;

                    //מציאת מיקום החבילה
                    DAL.DalApi.DO.Customer send = datafield.FindAndGetCustomer(desired.senderID);
                    DAL.DalApi.DO.Customer dest = datafield.FindAndGetCustomer(desired.targetID);
                    LogicalEntities.Location locationSender = new LogicalEntities.Location(send.longitude, send.latitude);
                    LogicalEntities.Location locationTarget = new LogicalEntities.Location(dest.longitude, dest.latitude);

                    //מיקום הרחפן
                    if ( desired.pickedUp == null || desired.pickedUp > DateTime.Now) //אם החבילה שויכה ולא נאספה
                    {
                        //מיקום הרחפן בתחנה הקרובה לשולח
                        var closeStation = HelpMethods.FindCloseStation(locationSender, datafield.GetBasisStations());
                        DAL.DalApi.DO.Station s = datafield.GetBasisStations().ElementAt(closeStation.indexCloseStation);
                        tempD.location = new LogicalEntities.Location(s.longitude, s.latitude);

                    }
                    else if ( desired.delivered == null || desired.delivered > DateTime.Now) //אם החבילה נאספה אך עוד לא סופקה 
                    {
                        //מיקום הרחפן במיקום השולח
                        tempD.location = locationSender;
                    }

                    // טיפול במצב סוללת הרחפן
                    var closeStationToDest = HelpMethods.FindCloseStation(locationSender, datafield.GetBasisStations());
                    double battery = HelpMethods.CalculateDistance(locationTarget, locationSender);
                    battery *= powerConsumption[(int)desired.weight + 1];
                    battery += powerConsumption[0] * closeStationToDest.distance;
                    tempD.battery = r.Next(Math.Min((int)battery, 100), 101);

                    dronesBL[i] = tempD;
                }
                else
                {
                    tempD.droneStatus = (Enums.DroneStatuses)r.Next(0, 2);
                    dronesBL[i] = tempD;
                }

                if (dronesBL[i].droneStatus == Enums.DroneStatuses.available)
                {
                    HelpMethods.DataCloseStation cs = HelpMethods.FindCloseStation(dronesBL[i], datafield.GetBasisStations());
                    int min = Math.Min((int)cs.distance * (int)powerConsumption[3], 101);
                    tempD.battery = r.Next(min, 101);
                    
                    List<DAL.DalApi.DO.Customer> help = GetListCustomersGotParcelsFromDrone(dronesBL [i].ID );
                    if (help.Count > 0)
                    {
                        DAL.DalApi.DO.Customer customer = help.ElementAt(r.Next(0, help.Count()));
                        tempD.location = new LogicalEntities.Location(customer.longitude, customer.latitude);
                    }
                    else
                        tempD.location = new LogicalEntities.Location (datafield.GetCustomers().ElementAt(0).longitude, datafield.GetCustomers().ElementAt(0).latitude);
                }
                else if (dronesBL[i].droneStatus == Enums.DroneStatuses.maintenance)
                {
                    tempD.battery = r.Next(0,21);
                    DAL.DalApi.DO.Station helpStation = datafield.GetBasisStations().ElementAt(r.Next(0, datafield.GetBasisStations().Count()));
                    tempD.location = new LogicalEntities.Location(helpStation.longitude, helpStation.latitude);
                    
                    datafield.CreateDroneCharge(helpStation.stationID, tempD.ID);
                }

                dronesBL[i] = tempD;
            }

        }
        #endregion 

        #region Adding new entity
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override void AddStation(int id, int name, double longitude, double latitude, int cs)
        {
            if (id < 0)
                throw new AddingProblemException("invalid identity number");
            if (name < 0)
                throw new AddingProblemException("invalid name");
            if (longitude < 0 || longitude > 360)
                throw new AddingProblemException("invlaid longitude.longitude must be in the range 0-360");
            if (latitude < 0 || latitude > 360)
                throw new AddingProblemException("invlaid latitude.latitude must be in the range 0-360");
            if (cs < 0)
                throw new AddingProblemException("invalid number of charge slots.");
            datafield.AddStation(id, name, longitude, latitude, cs);

        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public override void AddDrone(int id, string model, int maxWeight, int stationNum)
        {
            if (id < 0)
                throw new AddingProblemException("invalid identity number");
            if (model == "")
                throw new AddingProblemException("invalid model name");
            if (maxWeight > 2 || maxWeight < 0)
                throw new AddingProblemException("invalid weight category");
            if (datafield.FindStation(stationNum) < 0)
                throw new AddingProblemException("invalid id number of station");

            Drone d = new Drone();
            Random r = new Random();
            d.ID = id;
            d.model = model;
            d.maxWeight = (Enums.WeightCategories)maxWeight;
            d.droneStatus = Enums.DroneStatuses.maintenance;
            d.battery = r.Next(20, 41);
            d.stationID = stationNum;
            int indexStation = datafield.FindStation(stationNum);
            DAL.DalApi.DO.Station stationOfDrone = datafield.FindAndGetStation(stationNum);
            d.location = new LogicalEntities.Location(stationOfDrone.longitude, stationOfDrone.latitude);

            datafield.AddDrone(id, model, maxWeight);
            dronesBL.Add(d);

            datafield.CreateDroneCharge(d.stationID , d.ID);
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override void AddCustomer(int id, string name, string phoneNumber, double longitude, double latitude)
        {
            if (id < 0) //הערה- ניתן להוסיף בדיקת תקינות של מספר ספרות- לדרוש 9 ספרות כמו ת"ז אמיתי
                throw new AddingProblemException("invalid id number of customer");
            if (name == "")
                throw new AddingProblemException("invalid name of customer");
            if (longitude < 0 || longitude > 360)
                throw new AddingProblemException("invlaid longitude.longitude must be in the range 0-360");
            if (latitude < 0 || latitude > 360)
                throw new AddingProblemException("invlaid latitude.latitude must be in the range 0-360");

            datafield.AddCustomer(id, name, phoneNumber, longitude, latitude);

        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override void AddParcel(int senderID, int targetID, int weight, int priority)
        {
            if (datafield.FindCustomer(senderID) < 0)
                throw new AddingProblemException("invalid sender id");
            if (datafield.FindCustomer(targetID) < 0)
                throw new AddingProblemException("invalid target id");
            if (weight > 2 || weight < 0)
                throw new AddingProblemException("invalid weight category");
            if (priority > 2 || priority < 0)
                throw new AddingProblemException("invalid priority category");

            datafield.AddParcel(senderID, targetID, weight, priority);
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override void AddUser(string name, string password,bool access)
        {
            if (name == "")
                throw new AddingProblemException("invalid user name");
            if (password == "")
                throw new AddingProblemException("invalid password");

            datafield.AddUser(name,password,access);
        }
        #endregion

        #region Removing of item from list
        [MethodImpl(MethodImplOptions.Synchronized)]
        override public void DeleteDrone(int id)
        {
            //אי אפשר למחוק רחפן כאשר יש חבילה שמשויכת אליו
            foreach (DAL.DalApi.DO.Parcel parcel in datafield.GetParcels())
            {
                if (parcel.droneID == id)
                    throw new DeletedProblemException("Can't delete drone, because there is parcel assigned to the drone.");
            }

            datafield.DeleteDrone(id);
            dronesBL.RemoveAt (dronesBL.FindIndex(drone => drone.ID == id));
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        override public void DeleteCustomer(int id)
        {
            //אי אפשר למחוק לקוח כאשר יש חבילה שהוא המקור או היעד שלה
            foreach (DAL.DalApi.DO.Parcel parcel in datafield .GetParcels ())
            {
                if (parcel.senderID == id)
                    throw new DeletedProblemException("Can't delete customer, because he sends parcel.");
                else if (parcel.targetID == id)
                    throw new DeletedProblemException("Can't delete customer, because he have to get parcel.");
            }

            datafield.DeleteCustomer(id);
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        override public void DeleteStation(int id)
        {
            //אי אפשר למחוק תחנה כאשר יש רחפנים בטעינה בתחנה
            foreach (DAL .DalApi .DO .DroneCharge droneInCharge in datafield .GetDronesInCharge ())
            {
                if (droneInCharge.stationID == id && droneInCharge.activeCharge)
                    throw new DeletedProblemException("Can't delete station, because there is drone in charge in this station.");
            }

            datafield.DeleteStation(id);
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        override public void DeleteParcel(int id)
        {
            //אי אפשר למחוק חבילה כאשר החבילה משויכת לרחפן
            DAL.DalApi.DO.Parcel parcel = datafield.FindAndGetParcel(id);
            if (parcel.droneID > 0)
                throw new DeletedProblemException("Can't delete parcel, because it assigned to drone.");

            datafield.DeleteParcel(id);
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        override public void DeleteDroneInCharge(int id)
        {
            //אי אפשר למחוק רחפן בטעינה כאשר הטעינה פעילה
            DAL.DalApi.DO.DroneCharge droneCharge = datafield.FindAndGetDroneInCharge(id);
            if (droneCharge.activeCharge)
                throw new DeletedProblemException("Can't delete drone in charge, because the charge is active.");

            datafield.DeleteDroneInCharge(id);
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        override public void DeleteUser(string name, string password)
        {
            datafield.DeleteUser(name, password);
        }
        #endregion 

        #region Find item's index in list
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override int FindDrone(int id)
        {
            return datafield.FindDrone(id);
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override int FindCustomer(int id)
        {
            return datafield.FindCustomer(id);
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override int FindStation(int id)
        {
            return datafield.FindStation(id);
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override int FindParcel(int id)
        {
            return datafield.FindParcel(id);
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override int FindDroneInCharge(int id)
        {
            return datafield.FindDroneInCharge(id);
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override int FindUser(string name, string password)
        {
            return datafield.FindUser(name, password);
        }
        #endregion

        #region Find and get item
        [MethodImpl(MethodImplOptions.Synchronized)]
        override public DAL.DalApi.DO.Drone FindAndGetDrone(int id)
        {
            return datafield.FindAndGetDrone(id);
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        override public DAL.DalApi.DO.Customer FindAndGetCustomer(int id)
        {
            return datafield.FindAndGetCustomer(id);
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        override public DAL.DalApi.DO.Station FindAndGetStation(int id)
        {
            return datafield.FindAndGetStation(id);
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        override public DAL.DalApi.DO.Parcel FindAndGetParcel(int id)
        {
            return datafield.FindAndGetParcel(id);
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        override public DAL.DalApi.DO.DroneCharge FindAndGetDroneInCharge(int id)
        {
            return datafield.FindAndGetDroneInCharge(id);
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        override public DAL.DalApi.DO.User FindAndGetUser(string name, string password)
        {
            return datafield.FindAndGetUser(name, password);
        }
        #endregion

        #region Updating
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override void UpdateSendingOfParcel(int parcelID)
        {
            DAL.DalApi.DO.Parcel parcelSent = datafield.FindAndGetParcel(parcelID);
            if (parcelSent.pickedUp == null)
                throw new UpdateProblemException("Parcel have not been sent");
            datafield.UpdateSendingOfParcel(parcelID);
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override void UpdateRecievingOfParcel(int parcelID)
        {
            DAL.DalApi.DO.Parcel parcelRecieved = datafield.FindAndGetParcel(parcelID);
            if (parcelRecieved.delivered < DateTime.Now)
                throw new UpdateProblemException("Parcel have not been recieved");
            datafield.UpdateRecievingOfParcel(parcelID);
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override void UpdateDrone(int id, string model) 
        {
            if (datafield.FindDrone(id) < 0)
                throw new UpdateProblemException("The drone does not exist in the system");
            datafield.UpdateDrone(id, model);
            int indexBL = FindDroneBL(id);
            if (indexBL < 0)
                throw new UpdateProblemException("This id number of drone does not exist.");

            Drone droneInBL = dronesBL[indexBL];
            droneInBL.model = model;
            dronesBL[indexBL] = droneInBL;
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override void UpdateStation(int id, int name = -1, int chargeSlots = -1)
        {
            if (datafield.FindStation(id) < 0)
                throw new UpdateProblemException("The station does not exist in the system");
            datafield.UpdateStation(id, name, chargeSlots);
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override void UpdateCustomer(int id, string name = "", string phoneNum = "")
        {
            if (datafield.FindCustomer(id) < 0)
                throw new UpdateProblemException("The customer does not exist in the system");
            datafield.UpdateCustomer(id, name, phoneNum);
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override void UpdateUser(string uName,string oldPassword, string newPassword)
        {
            if (datafield.FindUser(uName, oldPassword ) < 0)
                throw new UpdateProblemException("The user does not exist in the system");
            datafield.UpdateUser(uName, oldPassword, newPassword);
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override void CreateDroneCharge(int droneID)
        {
            int index = FindDroneBL(droneID); //find drone in BLdrones
            int indexInDronesDAL = datafield.FindDrone(droneID);
            if (index < 0 || indexInDronesDAL < 0)
                throw new UpdateProblemException("Drone not found.");

            if (dronesBL[index].droneStatus == Enums.DroneStatuses.delivery)
                throw new UpdateProblemException("Can't charge drone " + droneID);

            HelpMethods.DataCloseStation closeStation = HelpMethods.FindCloseStation(dronesBL[index], datafield.GetBasisStations());
            if (closeStation.indexCloseStation < 0)
                throw new UpdateProblemException("There is not station where the drone can be charged.");

            lock(datafield )
            {
                int stationID = datafield.GetBasisStations().ElementAt(closeStation.indexCloseStation).stationID;
                datafield.CreateDroneCharge(stationID, droneID);

                //עדכון נתוני הרחפן שאותו מטעינים
                Drone temp = dronesBL[index];
                temp.battery = temp.battery - (int)closeStation.distance * (int)(powerConsumption[0]);
                DAL.DalApi.DO.Station stationOfCharging = datafield.GetBasisStations().ElementAt(closeStation.indexCloseStation);
                LogicalEntities.Location locationStation = new LogicalEntities.Location(stationOfCharging.longitude, stationOfCharging.latitude);
                temp.location = locationStation;
                temp.droneStatus = Enums.DroneStatuses.maintenance;
                temp.stationID = stationOfCharging.stationID;
                dronesBL[index] = temp;
            }
            
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override void EndDroneCharge(int droneID, int hoursOfCharging)
        {
            int indexDrone = FindDroneBL(droneID);
            int indexDroneCharge = datafield.FindDroneInCharge(droneID);
            if (indexDrone < 0)
                throw new UpdateProblemException("This drone does not exist in the storage.");
            if (indexDroneCharge < 0)
                throw new UpdateProblemException("This drone is not in charging.");
            if (dronesBL[indexDrone].droneStatus != Enums.DroneStatuses.maintenance)
                throw new UpdateProblemException("This drone is not in maintenance.");

            Drone temp = dronesBL[indexDrone];

            //int indexStation = datafield.FindStation(temp.stationID);
            //if (indexStation < 0)
            //    throw new UpdateProblemException("The station where the drone was charged does not exist.");
            lock(datafield )
            {
                datafield.EndDroneCharge(droneID, hoursOfCharging);
                //עדכון נתוני הרחפן שאותו משחררים מטעינה
                temp.battery += hoursOfCharging * datafield.GetDroneLoadingRate();
                if (temp.battery > 100)
                    temp.battery = 100;
                temp.droneStatus = Enums.DroneStatuses.available;
                dronesBL[indexDrone] = temp;
            }
           

        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override void AssignParcelToDrone(int droneID)
        {
            DateTime timeOne = new DateTime(1, 1, 1);
            int indexDrone = datafield.FindDrone(droneID);
            int indexDroneBL = FindDroneBL(droneID);
            if (indexDrone < 0)
                throw new UpdateProblemException("Wrong drone id number.");
            if (dronesBL[indexDroneBL].droneStatus != Enums.DroneStatuses.available)
                throw new UpdateProblemException("Drone does not available.");
            if (datafield.GetParcels().Count() == 0)
                throw new UpdateProblemException("There are not parcels in the storage.");

            bool foundParcel = false; //האם מצאנו חבילה לשיוך לרחפן
            IEnumerable<DAL.DalApi.DO.Parcel> parcelsList =from DAL.DalApi.DO.Parcel item in datafield.GetParcels()
                                                           where item.pickedUp == null || item.pickedUp == timeOne
                                                           select item;
            DAL.DalApi.DO.Parcel parcelToAssign = parcelsList.ElementAt(0);
            DAL.DalApi.DO.Parcel helpParcel = parcelsList.ElementAt(0);
            Drone droneToParcel = dronesBL[indexDroneBL];
            Enums.WeightCategories weightOfDrone = droneToParcel.maxWeight;

            //ניתן לחפש קודם כל חבילה שהיא בעדיפות הכי גבוהה וגם ההמשקל שלה אפשרי עבור הרחפן
            //שלב שני- אם לא מצאנו כזו חבילה נמשיך לחפש לפי מרחק
            //נעבור על כל החבילות ונחפש חבילה שהמרחק שלה מהרחפן הוא הקטן ביותר 
            //ניתן לדעת זאת על פי התז של הלקוח שהזמין את החבילה ובדיקת קווי אורך ורוחב שלו

            //חיפוש לפי עדיפות ואח"כ לפי משקל
            if (datafield.GetParcelsPriority()[2] > 0)
            {

                for (int i = 0; i < parcelsList.Count(); i++)
                {
                    helpParcel = parcelsList.ElementAt(i);
                    if (helpParcel.priority == DAL.DalApi.DO.Enums.Priorities.emergency )
                    {
                        //מצאנו חבילה בעדיפות גבוהה עם משקל מקסימלי אפשרי עבור רחפן
                        if ((int)(helpParcel.weight) == (int)(droneToParcel.maxWeight))
                        {
                            foundParcel = true;
                            parcelToAssign = helpParcel;
                            break;
                        }
                        //מצאנו משהו לא אידיאלי לכן נמשיך לחפש
                        else if ((int)(helpParcel.weight) < (int)(droneToParcel.maxWeight) )
                        {
                            foundParcel = true;
                            parcelToAssign = helpParcel;
                        }
                    }
                }
            }
            //רק אם לא מצאנו חבילה מתאימה בעדיפות גבוהה עלינו לחפש חבילה בעדיפות נמוכה יותר 
            if (!foundParcel && datafield.GetParcelsPriority()[1] > 0)
            {
                for (int i = 0; i < parcelsList.Count(); i++)
                {
                    helpParcel = parcelsList.ElementAt(i);
                    if (helpParcel.priority == DAL.DalApi.DO.Enums.Priorities.quick )
                    {
                        //מצאנו חבילה בעדיפות בינונית עם משקל מקסימלי אפשרי עבור רחפן
                        if ((int)(helpParcel.weight) == (int)(droneToParcel.maxWeight))
                        {
                            foundParcel = true;
                            parcelToAssign = helpParcel;
                            break;
                        }
                        //מצאנו משהו לא אידיאלי לכן נמשיך לחפש
                        else if ((int)(helpParcel.weight) < (int)(droneToParcel.maxWeight))
                        {
                            foundParcel = true;
                            parcelToAssign = helpParcel;
                        }
                    }
                }
            }
            if (!foundParcel)
            {
                for (int i = 0; i < parcelsList.Count(); i++)
                {
                    helpParcel = parcelsList.ElementAt(i);
                    if (helpParcel.priority == DAL.DalApi.DO.Enums.Priorities.normal )
                    {
                        //מצאנו חבילה בעדיפות נמוכה עם משקל מקסימלי אפשרי עבור רחפן
                        if ((int)(helpParcel.weight) == (int)(droneToParcel.maxWeight))
                        {
                            foundParcel = true;
                            parcelToAssign = helpParcel;
                            break;
                        }
                        //מצאנו משהו לא אידיאלי לכן נמשיך לחפש
                        else if ((int)(helpParcel.weight) < (int)(droneToParcel.maxWeight))
                        {
                            foundParcel = true;
                            parcelToAssign = helpParcel;
                        }
                    }
                }
            }


            //חיפוש לפי מרחק 
            if (!foundParcel)
            {
                LogicalEntities.Location locationParcel, locationDrone;
                locationDrone = dronesBL[indexDroneBL].location;
                int indexCustomer = datafield.FindCustomer(parcelsList.ElementAt(0).senderID);
                double longitude = datafield.GetCustomers().ElementAt(indexCustomer).longitude;
                double latitude = datafield.GetCustomers().ElementAt(indexCustomer).latitude;
                locationParcel = new LogicalEntities.Location(longitude, latitude);
                double distanceParcelDrone = HelpMethods.CalculateDistance(locationParcel, locationDrone);
                double minDistance = distanceParcelDrone;

                for (int i = 1; i < parcelsList.Count(); i++)
                {
                    indexCustomer = datafield.FindCustomer(parcelsList.ElementAt(i).senderID);
                    longitude = datafield.GetCustomers().ElementAt(indexCustomer).longitude;
                    latitude = datafield.GetCustomers().ElementAt(indexCustomer).latitude;
                    locationParcel = new LogicalEntities.Location(longitude, latitude);
                    distanceParcelDrone = HelpMethods.CalculateDistance(locationParcel, locationDrone);
                    if (distanceParcelDrone < minDistance )
                    {
                        minDistance = distanceParcelDrone;
                        parcelToAssign = parcelsList.ElementAt(i);
                        foundParcel = true;
                    }
                }
            }

            if (foundParcel) //אז נשייך את החבילה לרחפן ונבצע את הדרוש לכך
            {
                droneToParcel.droneStatus = Enums.DroneStatuses.delivery;
                dronesBL[indexDroneBL] = droneToParcel;
                datafield.ParcelToDrone(parcelToAssign.ID, droneID);

            }
            else
            {
                throw new UpdateProblemException("the drone does not have enough batery");
            }

        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override void CollectParcel(int droneID)
        {
            int indexDrone = datafield.FindDrone(droneID);
            int indexDroneBL = FindDroneBL(droneID);
            if (indexDrone < 0 || indexDroneBL < 0)
                throw new UpdateProblemException("Wrong drone id number.");
            if (dronesBL[indexDroneBL].droneStatus != Enums.DroneStatuses.delivery)
                throw new UpdateProblemException("Drone does not in delivery.");

            //מציאת החבילה שאותה הרחפן שולח
            IEnumerable<DAL.DalApi.DO.Parcel> parcelsList = datafield.GetParcels();
            DateTime timeOne = new DateTime(1, 1, 1);
            int i = 0; //מיקום החבילה שאותה הרחפן שולח ברשימת החבילות
            for (i = 0; i < parcelsList.Count(); i++)
            {
                if (parcelsList.ElementAt(i).droneID == droneID)
                {
                    if (parcelsList.ElementAt(i).pickedUp != null && parcelsList.ElementAt(i).pickedUp != timeOne)
                        throw new UpdateProblemException("The parcel has been delivered already.");
                    break;
                }
            }

            datafield.ParcelCollection(parcelsList.ElementAt(i).ID, droneID);

            DAL.DalApi.DO.Customer sender = datafield.FindAndGetCustomer(parcelsList.ElementAt(i).senderID);
            LogicalEntities.Location locationSender = new LogicalEntities.Location(sender.longitude, sender.latitude);

            Drone d = dronesBL[indexDroneBL];
            d.battery = d.battery - (int)(HelpMethods.CalculateDistance(d.location, locationSender)) * (int)powerConsumption[0];
            d.location = locationSender;
            dronesBL[indexDroneBL] = d;
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override void ParcelDelivery(int droneID)
        {
            int indexDroneBL = FindDroneBL(droneID);
            int indexDrone = datafield.FindDrone(droneID);

            if (indexDroneBL < 0 || indexDrone < 0)
                throw new UpdateProblemException("Wrong drone id number.");
            if (dronesBL[indexDroneBL].droneStatus != Enums.DroneStatuses.delivery)
                throw new UpdateProblemException("The drone is not in deivery.");

            //מציאת החבילה שאותה הרחפן מספק
            IEnumerable< DAL.DalApi.DO.Parcel > parcelsList = datafield.GetParcels();
            DateTime timeOne = new DateTime(1, 1, 1);
            int i = 0; //מיקום החבילה שאותה הרחפן מספק ברשימת החבילות
            for (i = 0; i < parcelsList.Count(); i++)
            {
                if (parcelsList.ElementAt(i).droneID == droneID)
                {
                    if (parcelsList.ElementAt(i).delivered != null && parcelsList.ElementAt(i).delivered != timeOne)
                        throw new UpdateProblemException("The parcel has been delivered already.");
                    else
                        datafield.DeliveryParcel(parcelsList.ElementAt(i).ID, droneID);
                    break;
                }
            }

            Drone tempDrone = dronesBL[indexDroneBL];
            tempDrone.droneStatus = Enums.DroneStatuses.available;
            DAL.DalApi.DO.Customer sender = datafield.FindAndGetCustomer(parcelsList.ElementAt(i).senderID);
            LogicalEntities.Location locationSender = new LogicalEntities.Location(sender.longitude, sender.latitude);
            DAL.DalApi.DO.Customer target = datafield.FindAndGetCustomer(parcelsList.ElementAt(i).senderID);
            LogicalEntities.Location locationTarget = new LogicalEntities.Location(target.longitude, target.latitude);
            double distanceSenderTarget = HelpMethods.CalculateDistance(locationTarget, locationSender);
            double pc = powerConsumption[1 + (int)parcelsList.ElementAt(i).weight];
            tempDrone.battery -= (int)distanceSenderTarget * (int)pc;
            tempDrone.location = locationTarget;
            
            dronesBL[indexDroneBL] = tempDrone;
        }
        #endregion

        #region Check if item exist
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override bool IsUserExist(string name, string password)
        {
            if (datafield.FindUser(name, password) >= 0)
                return true;
            return false;
        }
        #endregion 

        #region Getting item for presenting
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override StationToList GetStation(int id)
        {
            int index = datafield.FindStation(id);
            if (index < 0)
                throw new GetDetailsProblemException("This station does not exist in the storage.");
            var tempStation = datafield.GetBasisStations().ElementAt(index);
            return ConvertStationToStationInList(tempStation);
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override DroneToList GetDrone(int id)
        {
            int index = FindDroneBL(id);
            if (index < 0)
                throw new GetDetailsProblemException("This drone does not exist in the storage.");
            var tempDrone = dronesBL[index];
            return ConvertDroneToDroneInList(tempDrone);
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override CustomerToList GetCustomer(int id)
        {
            int index = datafield.FindCustomer(id);
            if (index < 0)
                throw new GetDetailsProblemException("This customer does not exist in the storage.");
            var tempCustomer = datafield.GetCustomers().ElementAt(index);
            return ConvertCustomerToCustomerInList(tempCustomer);
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override ParcelToList GetParcel(int id)
        {
            int index = datafield.FindParcel(id);
            if (index < 0)
                throw new GetDetailsProblemException("This parcel does not exist in the storage.");
            var tempParcel = datafield.GetParcels().ElementAt(index);
            return ConvertParcelToParcelInList(tempParcel);
        }
        #endregion

        #region Presenting of lists
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override IEnumerable<StationToList> GetListStations()
        {
            IEnumerable< DAL.DalApi.DO.Station > tempStations = datafield.GetBasisStations();
            List<StationToList> listStations = new();
            foreach (DAL.DalApi.DO.Station current in tempStations)
            {
                listStations.Add(ConvertStationToStationInList(current));
            }
            return listStations;
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override IEnumerable<DroneToList> GetListDrones()
        {
            List<DroneToList> listDrones = new();
            foreach (Drone current in dronesBL)
            {
                listDrones.Add(ConvertDroneToDroneInList(current));
            }
            return listDrones;
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override IEnumerable<CustomerToList> GetListCustomers()
        {
            IEnumerable<DAL.DalApi.DO.Customer> tempCustomers = datafield.GetCustomers();
            List<CustomerToList> listCustomers = new();
            foreach (DAL.DalApi.DO.Customer current in tempCustomers)
            {
                listCustomers.Add(ConvertCustomerToCustomerInList(current));
            }
            return listCustomers;
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override IEnumerable<ParcelToList> GetListParcels()
        {
            IEnumerable< DAL.DalApi.DO.Parcel > tempParcels = datafield.GetParcels();
            List<ParcelToList> listParcels = new();
            foreach (DAL.DalApi.DO.Parcel current in tempParcels)
            {
                listParcels.Add(ConvertParcelToParcelInList(current));
            }
            return listParcels;
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override IEnumerable<User> GetListUsers()
        {
            IEnumerable<DAL.DalApi.DO.User> tempUsers = datafield.GetUsers();
            List<User> listUsers = new();
            User user=new();
            foreach (DAL.DalApi.DO.User current in tempUsers)
            {
                user.UserName = current.UserName;
                user.UserPassword = current.UserPassword;
                user.UserAccessManagement = current.UserAccessManagement;

                listUsers.Add(user);
            }
            return listUsers;
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override IEnumerable<ParcelToList> GetListParcelsWithCondition(Predicate<ParcelToList> parcelCondition)
        {
            IEnumerable<ParcelToList> parcels = GetListParcels();
            IEnumerable<ParcelToList> parcelsWithCondition = from ParcelToList item in parcels
                                                       where parcelCondition (item)
                                                       select item;
            return parcelsWithCondition;
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override IEnumerable<StationToList> GetListStationsWithCondition(Predicate<StationToList> stationCondition)
        {
            IEnumerable<StationToList> stations = GetListStations();
            IEnumerable<StationToList> stationsWithCondition = from StationToList item in stations
                                                         where stationCondition (item)
                                                         select item;
            return stationsWithCondition;
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override IEnumerable<DroneToList> GetListDronesWithCondition(Predicate<DroneToList> droneCondition)
        {
            IEnumerable<DroneToList> drones = GetListDrones();
            IEnumerable<DroneToList> dronesWithCondition = from DroneToList item in drones
                                                                 where droneCondition(item)
                                                                 select item;
            return dronesWithCondition;
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override IEnumerable<CustomerToList> GetListCustomersWithCondition(Predicate<CustomerToList> customerCondition)
        {
            IEnumerable<CustomerToList> customers = GetListCustomers();
            IEnumerable<CustomerToList> customersWithCondition = from CustomerToList item in customers
                                                               where customerCondition(item)
                                                               select item;
            return customersWithCondition;
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override IEnumerable<User> GetListUsersWithCondition(Predicate<User> userCondition)
        {
            IEnumerable<User> users = GetListUsers();
            IEnumerable<User> usersWithCondition = from User item in users
                                                   where userCondition(item)
                                                   select item;
            return usersWithCondition;
        }
        #endregion

        #region Converting of entity to enitity in list
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override StationToList ConvertStationToStationInList(DAL.DalApi.DO.Station s)
        {
            StationToList stationToList = new StationToList();
            stationToList.ID = s.stationID;
            stationToList.name = s.name.ToString();
            stationToList.fullChargeSlots = CountFullChargeSlots(s.stationID);
            stationToList.availableChargeSlots = s.chargeSlots - stationToList.fullChargeSlots;
            stationToList.dronesInCharge = new();

            Drone help;
            int indexBL;

            foreach (DAL.DalApi.DO.DroneCharge droneInCharge in datafield.GetDronesInCharge ())
            {
                if (droneInCharge.stationID == s.stationID && droneInCharge.activeCharge)
                {
                    indexBL = FindDroneBL(droneInCharge.droneID);
                    if(indexBL >=0 )
                    {
                        help = dronesBL[indexBL];
                        stationToList.dronesInCharge.Add(ConvertDroneToDroneInList(help));
                    }
                }
            }
            return stationToList;
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override DroneToList ConvertDroneToDroneInList(Drone d)
        {
            DroneToList droneToList = new DroneToList();
            droneToList.ID = d.ID;
            droneToList.model = d.model;
            droneToList.location = d.location;
            droneToList.maxWeight = d.maxWeight;
            droneToList.battery = d.battery;
            droneToList.droneStatus = d.droneStatus;
            droneToList.parcelInDroneID = FindParcelInDrone(d.ID);

            return droneToList;
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override ParcelToList ConvertParcelToParcelInList(DAL.DalApi.DO.Parcel p)
        {
            ParcelToList parcelToList = new ParcelToList();
            parcelToList.ID = p.ID;
            parcelToList.nameOfSender = datafield.FindAndGetCustomer(p.senderID).name;
            parcelToList.nameOfTarget = datafield.FindAndGetCustomer(p.targetID).name;
            parcelToList.confirmedSending = p.confirmedSending;
            parcelToList.confirmRecieving = p.confirmRecieving;
            DateTime timeOne = new DateTime(1, 1, 1);

            if ( p.delivered != null &&  p.delivered !=timeOne && p.delivered <= DateTime.Now)
                parcelToList.parcelStatus = Enums.ParcelStatuses.supplied;
            else if (p.pickedUp != null && p.pickedUp !=timeOne && p.pickedUp <= DateTime.Now )
                parcelToList.parcelStatus = Enums.ParcelStatuses.collected;
            else if ((p.scheduled != null && p.scheduled != timeOne && p.scheduled <= DateTime.Now) && p.droneID >-1)
                parcelToList.parcelStatus = Enums.ParcelStatuses.assigned;
            else
                parcelToList.parcelStatus = Enums.ParcelStatuses.defined;
            
            parcelToList.weight = (Enums.WeightCategories)p.weight;
            parcelToList.priority = (Enums.Priorities)p.priority;
           
            if (p.droneID > -1 && datafield.FindDrone(p.droneID) >= 0)
                parcelToList.droneSender = datafield.FindAndGetDrone(p.droneID);

            return parcelToList;
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override CustomerToList ConvertCustomerToCustomerInList(DAL.DalApi.DO.Customer c)
        {
            CustomerToList customerToList = new CustomerToList();
            customerToList.ID = c.ID;
            customerToList.name = c.name;
            customerToList.phoneNumber = c.phone;
            int[] parcelStatuses = CountParcelsInEachStatus(c.ID);
            customerToList.numParcelsSentAndDelivered = parcelStatuses[0];
            customerToList.numParcelsSentNotDelivered = parcelStatuses[1];
            customerToList.numParcelsRecieved = parcelStatuses[2];
            customerToList.numParcelsInDelivery = parcelStatuses[3];

            return customerToList;
        }
        #endregion

        #region Converting of entity of DAL to enetity of BL
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override Customer ConvertCustomerDalToCustomerBL(DAL.DalApi.DO.Customer customerDAL)
        {
            Customer customerBL=new Customer();
            customerBL.ID = customerDAL.ID;
            customerBL.location = new LogicalEntities.Location(customerDAL.longitude, customerDAL.latitude);
            customerBL.name = customerDAL.phone;
            customerBL.phone = customerDAL.phone;

            List<ParcelToList> parcelsFrom=new();
            List<ParcelToList> parcelsTo = new();

            
            foreach (DAL.DalApi.DO.Parcel parcel in datafield .GetParcels ())
            {
                if (parcel.senderID == customerBL.ID)
                    parcelsFrom.Add(ConvertParcelToParcelInList(parcel));
                else if (parcel.targetID == customerBL.ID)
                    parcelsTo.Add(ConvertParcelToParcelInList (parcel));
            }

            customerBL.parcelsFromCustomer = parcelsFrom;
            customerBL.parcelsToCustomer = parcelsTo;

            return customerBL;
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override Drone ConvertDroneDalToDroneBL(DAL.DalApi.DO.Drone droneDAL)
        {
            int indexBL = FindDroneBL(droneDAL.ID);
            if (indexBL < 0)
                throw new GetDetailsProblemException("The drone does not exist in the system");
            return dronesBL[indexBL];
        }
        #endregion

        #region Help Methods
        public override int FindDroneBL(int id)
        {
            for (int i = 0; i < dronesBL.Count; i++)
            {
                if (dronesBL[i].ID == id)
                    return i;
            }
            return -1;
        }

        internal override int CountFullChargeSlots(int stationID)
        {
            int counter = 0;
            IEnumerable<DAL.DalApi.DO.DroneCharge> droneCharges = datafield.GetDronesInCharge();
            foreach (DAL.DalApi.DO.DroneCharge current in droneCharges)
            {
                if (current.stationID == stationID)
                    counter++;
            }
            return counter;
        }

        internal override int FindParcelInDrone(int droneID)
        {
            IEnumerable<DAL.DalApi.DO.Parcel> parcels = datafield.GetParcels();
            DateTime timeOne = new DateTime(1, 1, 1);
            foreach (DAL.DalApi.DO.Parcel current in parcels)
            {
                if (current.droneID == droneID && (current .delivered == null|| current .delivered == timeOne ))
                    return current.ID;
            }
            return -1;
        }

        internal override int[] CountParcelsInEachStatus(int customerID)
        {
            int[] parcelsStatuses = new int[4] { 0, 0, 0, 0 };
            //0= numParcelsSentAndDelivered, 1=numParcelsSentNotDelivered, 2=numParcelsRecieved, 3=numParcelsInDelivery
            IEnumerable<DAL.DalApi.DO.Parcel> parcels = datafield.GetParcels();
            DateTime oneTime = new DateTime(1, 1, 1);
            foreach (DAL.DalApi.DO.Parcel current in parcels)
            {
                if (current.senderID == customerID)
                {
                    if (current.delivered < DateTime.Now && current.delivered != oneTime)
                        parcelsStatuses[0]++;
                    else
                        parcelsStatuses[1]++;
                }
                else if (current.targetID == customerID)
                {
                    if (current.delivered < DateTime.Now && current.delivered != oneTime)
                        parcelsStatuses[2]++;
                    else
                        parcelsStatuses[3]++;
                }
            }
            return parcelsStatuses;
        }
        internal override List<DAL.DalApi.DO.Customer> GetListCustomersGotParcelsFromDrone(int droneID)
        {
            List<DAL.DalApi.DO.Customer> customersOfDrone = new();
            DAL.DalApi.DO.Customer customer = new();
            foreach (DAL.DalApi.DO.Parcel parcel in datafield .GetParcels ())
            {
                if(parcel.droneID == droneID )
                {
                    if (datafield.FindCustomer(parcel.targetID) >= 0)
                    {
                        customer = datafield.FindAndGetCustomer(parcel.targetID);
                        customersOfDrone.Add(customer );
                        Console.WriteLine(customer );
                    }
                }
            }
            return customersOfDrone;
        }
        #endregion

        #region function for acting of the drones' simulator
        public override void AutomaticDroneAct(int droneID, Action updateDisplay, Func<bool> checkStop)
        {
            Simulator simulator= new Simulator(this, droneID, updateDisplay, checkStop);
        }
        #endregion 
    }

}