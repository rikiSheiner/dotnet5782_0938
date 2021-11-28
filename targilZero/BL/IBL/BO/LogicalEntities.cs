using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.IBL.BO
{
    public class LogicalEntities
    {
        public struct Location //מיקום
        {
            public double longitude { get; set; }
            public double latitude { get; set; }
            public Location (double lo, double la) { longitude = lo; latitude = la; }
            public override string ToString()
            {
                return '<'+longitude.ToString () +','+latitude.ToString () +'>' ;
            }
        }
    }
}
