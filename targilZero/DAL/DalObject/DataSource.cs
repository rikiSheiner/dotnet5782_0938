using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.DalApi;
using DAL.DalApi.DO;

namespace DAL.DalObject
{
    public class DataSource
    {
        #region lists of the entities         
        internal static List<Drone> drones = new List<Drone>();
        internal static List<Station> basisStations = new List<Station>();
        internal static List<Customer> customers = new List<Customer>();
        internal static List<Parcel> parcels = new List<Parcel>();
        internal static List<DroneCharge> dronesInCharge = new List<DroneCharge>();
        internal static List<User> users = new List<User>();
        #endregion 

        internal class Config
        {
            #region fields of class Config
            internal static int idNumberParcels = 1;//מספר מזהה רץ עבור חבילות
            internal static int countActive = 0;
            internal static double idlePowerConsumption = 0.01; //צריכת חשמל במצב פנוי
            internal static double lightPowerConsumption = 0.02; //צריכת חשמל במצב נושא משקל קל
            internal static double mediumPowerConsumption = 0.03; //צריכת חשמל במצב נושא משקל בינוני
            internal static double heavyPowerConsumption = 0.04; //צריכת חשמל במצב נושא משקל כבד
            internal static int droneLoadingRate = 40; //קצב טעינת רחפן בשעה באחוזים
            internal static int[] CountParcelsPriority = new int[3]; //מערך עבור מניית מספר החבילות שהן בעדיפות מסוימת
                                                                     //מקום 0=normal, מקום 1=quick,מקום 2 =emergency
            #endregion
        }

        #region initialize with data the lists of entities
        public static void Initialize()
        {
            Random r = new Random();

            int id = 111111111;
            for (int i = 0; i < 3; i++)
            {
                Config.CountParcelsPriority[i] = 0;
            }

            //הוספת 2 תחנות בסיס
            basisStations.Add(new Station(1, 111, r.Next(1, 360), r.Next(1, 360), 10));
            basisStations.Add(new Station(2, 222, r.Next(1, 360), r.Next(1, 360), 13));

            //הוספת 5 רחפנים למאגר
            for (int i = 0; i < 5; i++)
            {
                drones.Add(new Drone(i + 1, "m" + (i + 1), (Enums.WeightCategories)(r.Next(0, 3))));
            }

            //הוספת 10 לקוחות
            for (int i = 0; i < 10; i++)
            {
                customers.Add(new Customer(id, ((Enums.NamesOfPeople)(i)).ToString(), (r.Next(0520000000, 0589999999)).ToString(), r.Next(1, 360), r.Next(1, 360)));
                id++;
            }

            //הוספת 10 חבילות
            for (int i = 0; i < 10; i++)
            {
                parcels.Add(new Parcel(Config.idNumberParcels, r.Next(1, 5), r.Next(6, 10), (Enums.WeightCategories)(i % 3),
                    (Enums.Priorities)((i + 1) % 3), i));
                Config.CountParcelsPriority[(i + 1) % 3]++;
                Config.idNumberParcels++;
            }

            //הוספת משתמש אחד למערכת שהוא המנהל
            users.Add(new User("rsheiner", "riki3240", true));

        }
        #endregion


    }
}
