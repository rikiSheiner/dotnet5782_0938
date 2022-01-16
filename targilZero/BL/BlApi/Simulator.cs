using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.BO;
using System.Threading;


namespace BL.BlApi
{
    class Simulator
    {
        //fields
        private const double droneSpeed = 100000;
        private const int delayTime = 1000; 
        //constrcutor
        public Simulator (BL blObject, int droneID, Action updateDisplay, Func<bool> checkStop)
        {
            bool parcelsEnded = false;
            int index,hours;
            string status;
            DateTime timeStartCharge, timeOne = new DateTime(1, 1, 1);
            Drone droneCurrent;
            DAL.DalApi.DO.DroneCharge droneCharge;
            DAL.DalApi.DO.Parcel parcelInDrone;

            while (!checkStop() && !parcelsEnded )
            {
                index = blObject.FindDrone(droneID);
                droneCurrent = blObject.dronesBL[index];
                status = droneCurrent.droneStatus.ToString ();
                switch (status)
                {
                    case "available":
                        if (droneCurrent.battery <= 15) //low battery- less than 15%
                        {
                            try{blObject.CreateDroneCharge(droneID);}
                            catch (Exception) { }
                        }
                        else
                        {
                            try
                            {
                                blObject.AssignParcelToDrone(droneID);
                            }
                            catch (UpdateProblemException upe)
                            {
                                if (upe.Message == "There are not parcels in the storage.")
                                {
                                    parcelsEnded = true;
                                }
                                else
                                {
                                    try { blObject.CreateDroneCharge(droneID); }
                                    catch(Exception) {  }
                                }
                            }
                        }
                        
                        Thread.Sleep(delayTime);
                        break;
                    case "maintenance":
                        droneCharge = blObject.FindAndGetDroneInCharge(droneID);
                        timeStartCharge = droneCharge.start;
                        hours = DateTime.Now.Hour+DateTime .Now.DayOfYear *24 - timeStartCharge.Hour-timeStartCharge .DayOfYear *24 ;
                        blObject.EndDroneCharge(droneID, Math.Max (hours,1));
                        Thread.Sleep(delayTime);
                        break;
                    case "delivery":
                        parcelInDrone  = blObject.FindAndGetParcel(blObject.FindParcelInDrone(droneID));
                        if(parcelInDrone .pickedUp == null || parcelInDrone .pickedUp == timeOne)
                            blObject.CollectParcel(droneID);
                        blObject.ParcelDelivery(droneID);
                        Thread.Sleep(delayTime);
                        break;
                }
                updateDisplay();
            }
        }
    }
}
