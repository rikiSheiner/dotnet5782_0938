using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.BlApi
{
    public static class BlFactory
    {
        public static IBL GetBl()
        {
            IBL data = BL.Instance;
            return data;
        }
    }
}
