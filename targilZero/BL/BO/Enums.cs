using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.BO
{
    public class Enums
    {

        #region אנומרציות הנדרשות עבור השכבה הלוגית ושכבת הנתונים
        public enum WeightCategories { light, intermediate, heavy }
        public enum DroneStatuses { available, maintenance, delivery }
        public enum Priorities { normal, quick, emergency }
        public enum NamesOfPeople { Chana, Ester, Chaya, Dvora, Shalom, Josef, Dan, Shira, Talya, Michal }
        public enum ParcelStatuses { defined, assigned, collected, supplied }
        #endregion
    }
}
