using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.IBL;

namespace BL.IBL.BO
{
    public class HelpMethods
    {

        /*נניח כי מרחק של מעלה 1 בקו אורך וכן בקו רוחב הוא 50 ק"מ */
        /// <summary>
        /// calculates the distance betwen two locations and returns it
        /// </summary>
        /// <param name="location1"></param>
        /// <param name="location2"></param>
        /// <returns></returns>
        public static double CalculateDistance(LogicalEntities.Location location1, LogicalEntities.Location location2)
        {
            //נשתמש בנוסחת מרחק לחישוב המרחק בין שני מיקומים
            double x1 = location2.longitude - location1.longitude;
            double x2 = location2.latitude - location1.latitude;
            return 50 * Math.Sqrt(x1 * x1 + x2 * x2);
        }

        public struct DataCloseStation
        {
            public int indexCloseStation { get; set; }
            public double distance { get; set; }
            public DataCloseStation (int index, double d) { indexCloseStation = index; distance = d; }
        }


        // נעשה שהחישוב מרחק מינימלי יקבל ערך שלילי כל עוד לא מצאנו תחנה עם חריצי טעינה פנויים
        //וכאשר מצאנו תחנה עם חריצי טעינה פנויים נעדכן את המחרק המינימלי להיו המחרק שמצאנו
        //ובעצם בכל איטרציה נבדוק האם המרחק חיובי אם כן נבצע השוואה ונעדכן רק אם קטן מהנוכחי
        // אחרת נעדכן רק אם יש בתחנה חריצי טעינה פנויים

        /*נניח כי ליחידת מרחק של 1 ק"מ צריכת הסוללה היא 1%
         * נניח כי מרחק של מעלה 1 בקו אורך וכן בקו רוחב הוא 50 ק"מ */
        /// <summary>
        /// finds the close station to the drone with empty charge slots and returns it
        /// we also check whether the drone has enough battery 
        /// </summary>
        /// <param name="drone"></param>
        /// <returns></returns>

        public static DataCloseStation FindCloseStation(Drone drone, IEnumerable<DAL.Station> stationsList)
        {
            int indexCloseStation = -1;
            LogicalEntities.Location locationStation = new LogicalEntities.Location(stationsList.ElementAt(indexCloseStation+1).longitude, stationsList.ElementAt(indexCloseStation+1).latitude);
            double minDistance = -1;
            double tempDistance = CalculateDistance(locationStation, drone.location);
            if (stationsList.ElementAt(0).chargeSlots > 0 && tempDistance <= drone.battery)
            {
                minDistance = tempDistance;
                indexCloseStation = 0;
            }

            for (int i = 1; i < stationsList.Count(); i++)
            {
                if (stationsList.ElementAt(i).chargeSlots > 0)
                {
                    locationStation = new LogicalEntities.Location(stationsList.ElementAt(i).longitude, stationsList.ElementAt(i).latitude);
                    tempDistance = CalculateDistance(locationStation, drone.location);
                    if ((minDistance < 0 || tempDistance < minDistance) && tempDistance <= drone.battery) //זאת אומרת שזו התחנה הראשונה שמצאנו עם חריצי טעינה פנויים
                    {
                        minDistance = tempDistance;
                        indexCloseStation = i;
                    }
                }
            }
            return new DataCloseStation(indexCloseStation, minDistance);
        }

        /// <summary>
        /// finds the closed station to given location of sender of parcel
        /// </summary>
        /// <param name="location"></param>
        /// <param name="stationsList"></param>
        /// <returns></returns>
        public static DataCloseStation FindCloseStation(LogicalEntities.Location location, IEnumerable<DAL.Station> stationsList)
        {
            int indexCloseStation = 0;
            LogicalEntities.Location locationStation = new LogicalEntities.Location(stationsList.ElementAt(indexCloseStation).longitude, stationsList.ElementAt(indexCloseStation).latitude);
            double tempDistance = CalculateDistance(locationStation, location);
            double minDistance = tempDistance;
            indexCloseStation = 0;
            
            for (int i = 1; i < stationsList.Count(); i++)
            {
                locationStation = new LogicalEntities.Location(stationsList.ElementAt(i).longitude, stationsList.ElementAt(i).latitude);
                tempDistance = CalculateDistance(locationStation, location);
                if ( tempDistance < minDistance) 
                {
                    minDistance = tempDistance;
                    indexCloseStation = i;
                }
            }
            return new DataCloseStation(indexCloseStation, minDistance);
        }

    }

    


}

