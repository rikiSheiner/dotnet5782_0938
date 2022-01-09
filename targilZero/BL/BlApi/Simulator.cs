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
            //אם הרחפן במצב פנוי אז ננסה לשייך אליו חבילה
            //אם לא ניתן לשייך חבילה יש לשלוח אותו לטעינה 
            //כי אנו מניחים שהסיבה לכך שלא ניתן לשלוח לטעינה היא חוסר בטריה
            //אבל אם פשוט אין חבילות לשייך צריך להפסיק את פעולת הסימולטור

            //אם הרחפן במצב תחזוקה נשחרר אותו מטעינה

            //אם הרחפן במצב משלוח נבדוק באיזה שלב של המשלוח הוא נמצא
            //ובהתאם לכך נסיים את ביצוע המשלוח
            //בדיקת השלב תתבצע ע"י מציאת הזמנים הקשורים לשליחת חבילה בשדות של החבילה עצמה

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
                        try
                        {
                            blObject.AssignParcelToDrone(droneID);
                        }
                        catch (UpdateProblemException upe)
                        {
                            if(upe.Message == "There are not parcels in the storage.")
                            {
                                parcelsEnded = true;
                            }
                            else if(upe.Message == "the drone does not have enough batery")
                            {
                                blObject.CreateDroneCharge(droneID);
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
