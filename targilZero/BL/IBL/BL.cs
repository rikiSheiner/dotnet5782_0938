using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.IDAL;
using BL.IBL.BO;


namespace BL.IBL
{
    public class BL : IBL
    {
        private IDal datafield;
        private List<Drone> dronesBL;
        private double[] powerConsumption;
        int droneChargeRate;

        #region Constructor
        public BL()
        {
            datafield = new DAL.DalObject.DataSource();
            powerConsumption = datafield.GetPowerConsumption(); //צריכת חשמל ע"י הרחפנים
            droneChargeRate = datafield.GetDroneLoadingRate(); //קצב טעינת הרחפנים
            IEnumerable<DAL.Drone> dronesList = datafield.GetDrones();
            dronesBL = new List<Drone>();

            //הוספת הרחפנים שנמצאים  בשכבת הנתונים לרשימת הרחפנים שישנה בשכבה הלוגית
            DAL.Drone temp;
            Drone d;
            Random r = new Random();
            for (int i = 0; i < dronesList.Count(); i++)
            {
                temp = dronesList.ElementAt(i);
                d = new Drone();
                d.ID = temp.ID;
                d.model = temp.model;
                d.maxWeight = (Enums.WeightCategories)temp.maxWeight;
                d.battery = r.Next(20, 41);
                d.droneStatus = Enums.DroneStatuses.available;
                d.location = new LogicalEntities.Location(r.Next(0, 360), r.Next(0, 360));
                dronesBL.Add(d);
            }

            Drone tempD;
            bool parcelToDrone = false, findParcel = false;
            DAL.Parcel desired = new DAL.Parcel();

            DateTime one = new DateTime(1, 1, 1);
            for (int i = 0; i < dronesList.Count(); i++)
            {
                tempD = dronesBL[i];
                foreach (DAL.Parcel p in datafield.GetParcels())
                {
                    if (p.droneID == dronesBL[i].ID)
                    { 
                        desired = p;
                        findParcel = true;
                        break; 
                    }
                }
                parcelToDrone = desired.scheduled < DateTime.Now && desired.scheduled != one && (desired.delivered == one || desired.delivered > DateTime.Now);
                if (parcelToDrone) //אם החבילה שויכה אך לא נאספה
                {
                    //עדכון מצב הרחפן למבצע משלוח

                    tempD.droneStatus = Enums.DroneStatuses.delivery;

                    //מציאת מיקום החבילה
                    DAL.Customer send = datafield.FindAndGetCustomer(desired.senderID);
                    DAL.Customer dest = datafield.FindAndGetCustomer(desired.targetID);
                    LogicalEntities.Location locationSender = new LogicalEntities.Location(send.longitude, send.latitude);
                    LogicalEntities.Location locationTarget = new LogicalEntities.Location(dest.longitude, dest.latitude);

                    //מיקום הרחפן
                    if (desired.pickedUp > DateTime.Now || desired.pickedUp == one) //אם החבילה שויכה ולא נאספה
                    {
                        //מיקום הרחפן בתחנה הקרובה לשולח
                        var closeStation = HelpMethods.FindCloseStation(locationSender, datafield.GetBasisStations());
                        DAL.Station s = datafield.GetBasisStations().ElementAt(closeStation.indexCloseStation);
                        tempD.location = new LogicalEntities.Location(s.longitude, s.latitude);

                    }
                    else if (desired.delivered > DateTime.Now || desired.delivered == one) //אם החבילה נאספה אך עוד לא סופקה 
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
                    tempD.droneStatus = (Enums.DroneStatuses)(r.Next(0, 2));
                    dronesBL[i] = tempD;
                }

                if (dronesBL[i].droneStatus == Enums.DroneStatuses.available)
                {
                    HelpMethods.DataCloseStation cs = HelpMethods.FindCloseStation(dronesBL[i], datafield.GetBasisStations());
                    int min = Math.Min((int)cs.distance * (int)powerConsumption[0], 101);
                    tempD.battery = r.Next(min, 101);
                    
                    List<DAL.Customer> help = GetListCustomersGotParcelsFromDrone(dronesBL [i].ID );
                    if (help.Count > 0)
                    {
                        DAL.Customer customer = help.ElementAt(r.Next(0, help.Count()));
                        tempD.location = new LogicalEntities.Location(customer.longitude, customer.latitude);
                    }
                    else
                        tempD.location = new LogicalEntities.Location (datafield.GetCustomers().ElementAt(0).longitude, datafield.GetCustomers().ElementAt(0).latitude);
                }
                else if (dronesBL[i].droneStatus == Enums.DroneStatuses.maintenance)
                {
                    tempD.battery = r.Next(0, 21);
                    DAL.Station helpStation = datafield.GetBasisStations().ElementAt(r.Next(0, datafield.GetBasisStations().Count()));
                    tempD.location = new LogicalEntities.Location(helpStation.longitude, helpStation.latitude);
                }

            }

        }


        #endregion

        #region Adding new entity
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
            DAL.Station stationOfDrone = datafield.FindAndGetStation(stationNum);
            d.location = new LogicalEntities.Location(stationOfDrone.longitude, stationOfDrone.latitude);

            datafield.AddDrone(id, model, maxWeight);
            dronesBL.Add(d);

            //charge the new drone in station
            datafield.AddDroneCharge(id, stationNum, true, DateTime.Now);
        }

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

            datafield.AddParcel(senderID, targetID, weight, priority, 0);
        }
        #endregion

        #region Updating
        public override void UpdateDrone(int id, string model) 
        {
            int indexDrone = datafield.FindDrone(id);
            int indexBL = FindDrone(id);
            if (indexDrone < 0 || indexBL < 0)
                throw new UpdateProblemException("This id number of drone does not exist.");

            DAL.Drone temp = datafield.GetDrones().ElementAt(indexDrone);
            datafield.DeleteDrone(id);
            temp.model = model;
            datafield.AddDrone(temp.ID, temp.model, (int)temp.maxWeight);

            Drone droneInBL = dronesBL[indexBL];
            droneInBL.model = model;
            dronesBL[indexBL] = droneInBL;
        }

        public override void UpdateStation(int id, int name = -1, int chargeSlots = -1)
        {
            int indexStation = datafield.FindStation(id);
            if (indexStation < 0)
                throw new UpdateProblemException("This id number of station does not exist.");

            DAL.Station temp = datafield.GetBasisStations().ElementAt(indexStation);
            datafield.DeleteStation(id);
            if (name > 0)
                temp.name = name;
            if (chargeSlots >= 0)
            {
                temp.chargeSlots = chargeSlots;

                var dronesDAL = datafield.GetDronesInCharge();
                IEnumerable<DroneCharge> droneChargeStation = from DroneCharge item in dronesDAL
                                                              where item.stationID == id
                                                              select item;
                temp.chargeSlots -= dronesDAL.Count();
            }


            datafield.AddStation(temp.stationID, temp.name, temp.longitude, temp.latitude, temp.chargeSlots);
        }

        public override void UpdateCustomer(int id, string name = "", string phoneNum = "")
        {
            int indexCustomer = datafield.FindCustomer(id);
            if (indexCustomer < 0)
                throw new UpdateProblemException("This id number of customer does not exist.");

            DAL.Customer temp = datafield.GetCustomers().ElementAt(indexCustomer);
            datafield.DeleteCustomer(id);
            if (name != "")
                temp.name = name;
            if (phoneNum != "")
                temp.phone = phoneNum;
            datafield.AddCustomer(temp.ID, temp.name, temp.phone, temp.longitude, temp.latitude);
        }

        public override void CreateDroneCharge(int droneID)
        {
            int index = FindDrone(droneID); //find drone in BLdrones
            int indexInDronesDAL = datafield.FindDrone(droneID);
            if (index < 0 || indexInDronesDAL < 0)
                throw new UpdateProblemException("Drone not found.");

            if (dronesBL[index].droneStatus != Enums.DroneStatuses.available)
                throw new UpdateProblemException("Can't charge drone " + droneID);

            HelpMethods.DataCloseStation closeStation = HelpMethods.FindCloseStation(dronesBL[index], datafield.GetBasisStations());
            if (closeStation.indexCloseStation < 0)
                throw new UpdateProblemException("There is not station where the drone can be charged.");

            int stationID = datafield.GetBasisStations().ElementAt(closeStation.indexCloseStation).stationID;
            datafield.CreateDroneCharge(stationID, droneID);

            //עדכון נתוני הרחפן שאותו מטעינים
            Drone temp = dronesBL[index];
            temp.battery = temp.battery - (int)closeStation.distance * (int)(powerConsumption[0]);
            DAL.Station stationOfCharging = datafield.GetBasisStations().ElementAt(closeStation.indexCloseStation);
            LogicalEntities.Location locationStation = new LogicalEntities.Location(stationOfCharging.longitude, stationOfCharging.latitude);
            temp.location = locationStation;
            temp.droneStatus = Enums.DroneStatuses.maintenance;
            temp.stationID = stationOfCharging.stationID;
            dronesBL[index] = temp;
        }

        public override void EndDroneCharge(int droneID, int hoursOfCharging)
        {
            int indexDrone = FindDrone(droneID);
            int indexDroneCharge = datafield.FindDroneInCharge(droneID);
            if (indexDrone < 0)
                throw new UpdateProblemException("This drone does not exist in the storage.");
            if (indexDroneCharge < 0)
                throw new UpdateProblemException("This drone is not in charging.");
            if (dronesBL[indexDrone].droneStatus != Enums.DroneStatuses.maintenance)
                throw new UpdateProblemException("This drone is not in maintenance.");

            Drone temp = dronesBL[indexDrone];

            int indexStation = datafield.FindStation(temp.stationID);
            if (indexStation < 0)
                throw new UpdateProblemException("The station where the drone was charged does not exist.");

            datafield.EndDroneCharge(droneID, hoursOfCharging);
            //עדכון נתוני הרחפן שאותו משחררים מטעינה
            temp.battery += hoursOfCharging * (int)powerConsumption[3];
            if (temp.battery > 100)
                temp.battery = 100;
            temp.droneStatus = Enums.DroneStatuses.available;
            dronesBL[indexDrone] = temp;

        }

        public override void AssignParcelToDrone(int droneID)
        {
            int indexDrone = datafield.FindDrone(droneID);
            int indexDroneBL = FindDrone(droneID);
            if (indexDrone < 0)
                throw new UpdateProblemException("Wrong drone id number.");
            if (dronesBL[indexDroneBL].droneStatus != Enums.DroneStatuses.available)
                throw new UpdateProblemException("Drone does not available.");
            if (datafield.GetParcels().Count() == 0)
                throw new UpdateProblemException("There are not parcels in the storage.");

            bool foundParcel = false; //האם מצאנו חבילה לשיוך לרחפן
            IEnumerable<DAL.Parcel> parcelsList = datafield.GetParcels();
            DAL.Parcel parcelToAssign = parcelsList.ElementAt(0);
            DAL.Parcel helpParcel = parcelsList.ElementAt(0);
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
                    if (helpParcel.priority == DAL.IDAL.DO.Priorities.emergency)
                    {
                        //מצאנו חבילה בעדיפות גבוהה עם משקל מקסימלי אפשרי עבור רחפן
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
            //רק אם לא מצאנו חבילה מתאימה בעדיפות גבוהה עלינו לחפש חבילה בעדיפות נמוכה יותר 
            if (!foundParcel && datafield.GetParcelsPriority()[1] > 0)
            {
                for (int i = 0; i < parcelsList.Count(); i++)
                {
                    helpParcel = parcelsList.ElementAt(i);
                    if (helpParcel.priority == DAL.IDAL.DO.Priorities.quick)
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
                    if (helpParcel.priority == DAL.IDAL.DO.Priorities.normal)
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
                    if (distanceParcelDrone < minDistance)
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

        }

        public override void CollectParcel(int droneID)
        {
            int indexDrone = datafield.FindDrone(droneID);
            int indexDroneBL = FindDrone(droneID);
            if (indexDrone < 0 || indexDroneBL < 0)
                throw new UpdateProblemException("Wrong drone id number.");
            if (dronesBL[indexDroneBL].droneStatus != Enums.DroneStatuses.delivery)
                throw new UpdateProblemException("Drone does not do delivery.");

            //מציאת החבילה שאותה הרחפן שולח
            IEnumerable<DAL.Parcel> parcelsList = datafield.GetParcels();
            DateTime timeOne = new DateTime(1, 1, 1);
            int i = 0; //מיקום החבילה שאותה הרחפן שולח ברשימת החבילות
            for (i = 0; i < parcelsList.Count(); i++)
            {
                if (parcelsList.ElementAt(i).droneID == droneID)
                {
                    if (parcelsList.ElementAt(i).pickedUp != timeOne)
                        throw new UpdateProblemException("The parcel has been delivered already.");
                    break;
                }
            }

            datafield.ParcelCollection(parcelsList.ElementAt(i).ID, droneID);

            DAL.Customer sender = datafield.FindAndGetCustomer(parcelsList.ElementAt(i).senderID);
            LogicalEntities.Location locationSender = new LogicalEntities.Location(sender.longitude, sender.latitude);

            Drone d = dronesBL[indexDroneBL];
            d.battery = d.battery - (int)(HelpMethods.CalculateDistance(d.location, locationSender)) * (int)powerConsumption[0];
            d.location = locationSender;
            dronesBL[indexDroneBL] = d;
        }

        public override void ParcelDelivery(int droneID)
        {
            int indexDroneBL = FindDrone(droneID);
            int indexDrone = datafield.FindDrone(droneID);

            if (indexDroneBL < 0 || indexDrone < 0)
                throw new UpdateProblemException("Wrong drone id number.");
            if (dronesBL[indexDroneBL].droneStatus != Enums.DroneStatuses.delivery)
                throw new UpdateProblemException("The drone is not in deivery.");

            //מציאת החבילה שאותה הרחפן מספק
            IEnumerable<DAL.Parcel> parcelsList = datafield.GetParcels();
            DateTime timeOne = new DateTime(1, 1, 1);
            int i = 0; //מיקום החבילה שאותה הרחפן מספק ברשימת החבילות
            for (i = 0; i < parcelsList.Count(); i++)
            {
                if (parcelsList.ElementAt(i).droneID == droneID)
                {
                    if (parcelsList.ElementAt(i).delivered != timeOne)
                        throw new UpdateProblemException("The parcel has been delivered already.");
                    break;
                }
            }

            Drone tempDrone = dronesBL[indexDroneBL];
            tempDrone.droneStatus = Enums.DroneStatuses.available;
            DAL.Customer sender = datafield.FindAndGetCustomer(parcelsList.ElementAt(i).senderID);
            LogicalEntities.Location locationSender = new LogicalEntities.Location(sender.longitude, sender.latitude);
            DAL.Customer target = datafield.FindAndGetCustomer(parcelsList.ElementAt(i).senderID);
            LogicalEntities.Location locationTarget = new LogicalEntities.Location(target.longitude, target.latitude);
            double distanceSenderTarget = HelpMethods.CalculateDistance(locationTarget, locationSender);
            double pc = powerConsumption[1 + (int)parcelsList.ElementAt(i).weight];
            tempDrone.battery -= (int)distanceSenderTarget * (int)pc;
            dronesBL[indexDroneBL] = tempDrone;
        }
        #endregion

        #region Getting item for presenting
        public override StationToList GetStation(int id)
        {
            int index = datafield.FindStation(id);
            if (index < 0)
                throw new GetDetailsProblemException("This station does not exist in the storage.");
            var tempStation = datafield.GetBasisStations().ElementAt(index);
            return ConvertStationToStationInList(tempStation);
        }
        public override DroneToList GetDrone(int id)
        {
            int index = FindDrone(id);
            if (index < 0)
                throw new GetDetailsProblemException("This drone does not exist in the storage.");
            var tempDrone = dronesBL[index];
            return ConvertDroneToDroneInList(tempDrone);
        }
        public override CustomerToList GetCustomer(int id)
        {
            int index = datafield.FindCustomer(id);
            if (index < 0)
                throw new GetDetailsProblemException("This customer does not exist in the storage.");
            var tempCustomer = datafield.GetCustomers().ElementAt(index);
            return ConvertCustomerToCustomerInList(tempCustomer);
        }
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
        public override IEnumerable<StationToList> GetListStations()
        {
            IEnumerable<DAL.Station> tempStations = datafield.GetBasisStations();
            List<StationToList> listStations = new();
            foreach (DAL.Station current in tempStations)
            {
                listStations.Add(ConvertStationToStationInList(current));
            }
            return listStations;
        }
        public override IEnumerable<DroneToList> GetListDrones()
        {
            List<DroneToList> listDrones = new();
            foreach (Drone current in dronesBL)
            {
                listDrones.Add(ConvertDroneToDroneInList(current));
            }
            return listDrones;
        }
        public override IEnumerable<CustomerToList> GetListCustomers()
        {
            IEnumerable<DAL.Customer> tempCustomers = datafield.GetCustomers();
            List<CustomerToList> listCustomers = new();
            foreach (DAL.Customer current in tempCustomers)
            {
                listCustomers.Add(ConvertCustomerToCustomerInList(current));
            }
            return listCustomers;
        }
        public override IEnumerable<ParcelToList> GetListParcels()
        {
            IEnumerable<DAL.Parcel> tempParcels = datafield.GetParcels();
            List<ParcelToList> listParcels = new();
            foreach (DAL.Parcel current in tempParcels)
            {
                listParcels.Add(ConvertParcelToParcelInList(current));
            }
            return listParcels;
        }
        public override IEnumerable<ParcelToList> GetListParcelsNoDrone()
        {
            IEnumerable<ParcelToList> parcels = GetListParcels();
            IEnumerable<ParcelToList> parcelsNoDrone = from ParcelToList item in parcels
                                                       where item.ID < 0
                                                       select item;
            return parcelsNoDrone;
        }
        public override IEnumerable<StationToList> GetListStationsNotFull()
        {
            IEnumerable<StationToList> stations = GetListStations();
            IEnumerable<StationToList> stationsNotFull = from StationToList item in stations
                                                         where item.availableChargeSlots > 0
                                                         select item;
            return stationsNotFull;
        }
        #endregion

        #region Converting of entity to enitity in list
        internal override StationToList ConvertStationToStationInList(DAL.Station s)
        {
            StationToList stationToList = new StationToList();
            stationToList.ID = s.stationID;
            stationToList.name = s.name.ToString();
            stationToList.fullChargeSlots = CountFullChargeSlots(s.stationID);
            stationToList.availableChargeSlots = s.chargeSlots - stationToList.fullChargeSlots;
            stationToList.dronesInCharge = new();
            foreach (DAL.DroneCharge droneInCharge in datafield.GetDronesInCharge ())
            {
                if (droneInCharge.stationID == s.stationID && droneInCharge.activeCharge)
                    stationToList.dronesInCharge.Add(datafield.FindAndGetDrone(droneInCharge.droneID));
            }

            return stationToList;
        }
        internal override DroneToList ConvertDroneToDroneInList(Drone d)
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
        internal override ParcelToList ConvertParcelToParcelInList(DAL.Parcel p)
        {
            ParcelToList parcelToList = new ParcelToList();
            parcelToList.ID = p.ID;
            parcelToList.nameOfSender = datafield.FindAndGetCustomer(p.senderID).name;
            parcelToList.nameOfTarget = datafield.FindAndGetCustomer(p.targetID).name;

            DateTime oneTime = new DateTime(1, 1, 1);
            if (p.delivered < DateTime.Now && p.delivered != oneTime)
                parcelToList.parcelStatus = Enums.ParcelStatuses.supplied;
            else if (p.pickedUp < DateTime.Now && p.pickedUp != oneTime)
                parcelToList.parcelStatus = Enums.ParcelStatuses.collected;
            else if (p.droneID != 0)
                parcelToList.parcelStatus = Enums.ParcelStatuses.assigned;
            else
                parcelToList.parcelStatus = Enums.ParcelStatuses.defined;

            parcelToList.weight = (Enums.WeightCategories)p.weight;
            parcelToList.priority = (Enums.Priorities)p.priority;
            if (p.droneID > 0 && datafield .FindDrone (p.droneID ) >= 0  )
                parcelToList.droneSender = datafield.FindAndGetDrone(p.droneID);

            return parcelToList;
        }
        internal override CustomerToList ConvertCustomerToCustomerInList(DAL.Customer c)
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

        #region Help Methods
        public override int FindDrone(int id)
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
            IEnumerable<DAL.DroneCharge> droneCharges = datafield.GetDronesInCharge();
            foreach (DAL.DroneCharge current in droneCharges)
            {
                if (current.stationID == stationID)
                    counter++;
            }
            return counter;
        }

        internal override int FindParcelInDrone(int droneID)
        {
            IEnumerable<DAL.Parcel> parcels = datafield.GetParcels();
            foreach (DAL.Parcel current in parcels)
            {
                if (current.droneID == droneID)
                    return current.ID;
            }
            return -1;
        }

        internal override int[] CountParcelsInEachStatus(int customerID)
        {
            int[] parcelsStatuses = new int[4] { 0, 0, 0, 0 };
            //0= numParcelsSentAndDelivered, 1=numParcelsSentNotDelivered, 2=numParcelsRecieved, 3=numParcelsInDelivery
            IEnumerable<DAL.Parcel> parcels = datafield.GetParcels();
            DateTime oneTime = new DateTime(1, 1, 1);
            foreach (DAL.Parcel current in parcels)
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
        internal override List<DAL.Customer> GetListCustomersGotParcelsFromDrone(int droneID)
        {
            List<DAL.Customer> customersOfDrone = new();
            DAL.Customer customer = new();
            foreach (DAL .Parcel parcel in datafield .GetParcels ())
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
    }
}
