using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.DalApi;

namespace DAL
{
    public static class DalFactory
    {
        public static IDal GetDal(string type)
        {
            IDal data;
            if (type == "DalObject")
                data = DalObject.DalObject.Instance ;
            else if (type == "DALXml")
                data = DalXml.DalXml.Instance;
            else
                throw new WrongInputException("incorrect string input");

            return data;
        }
    }
}
