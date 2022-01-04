using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using DAL.DalApi;


namespace DalXml
{
    internal static class XMLtools
    {
        private static string configPath = @"C:\Users\1\Source\Repos\sheiner32\dotnet5782_0938\targilZero\configXML.xml";
        static XMLtools() { }
        
        #region SaveLoadWithXElement

        //save a specific xml file according the name- throw exception in case of problems..
        //for the using with XElement..
        public static void SaveListToXMLElement(XElement rootElement, string filePath)
        {
            try
            {
                rootElement.Save(filePath);
            }
            catch (Exception ex)
            {
                throw new LoadingException(filePath, $"fail to create xml file: {filePath}", ex);
            }
        }
        //load a specific xml file according the name- throw exception in case of problems..
        //for the using with XElement..
        //string outputDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDoc‌​uments), "xml"); string filename = string.Format("{0}.{1}", Guid.NewGuid().ToString(), "xml"); string fullFileName
        //= Path.Combine(outputDirectory, filename); docSave.Save(fullFileName);
        public static XElement LoadListFromXMLElement(string filePath)
        {
            try
            {
               
                if (File.Exists(filePath))
                {
                    return XElement.Load(filePath);
                    
                }
                else
                {
                    XElement rootElement = new XElement(filePath);
                    rootElement.Save(filePath);
                    return rootElement;
                }
            }
            catch (Exception ex)
            {
                throw new LoadingException(filePath, $"fail to load xml file: {filePath}", ex);
            }
        }
        #endregion

        #region SaveLoadWithXMLSerializer

        //save a complete listin a specific file- throw exception in case of problems..
        //for the using with XMLSerializer..
        public static void SaveListToXMLSerializer<T>(List<T> list, string filePath)
        {
            try
            {
                FileStream file = new FileStream(filePath, FileMode.Create);
                XmlSerializer x = new XmlSerializer(list.GetType());
                x.Serialize(file, list);
                file.Close();
            }
            catch (Exception ex)
            {
                throw new LoadingException(filePath, $"fail to create xml file: {filePath}", ex);
            }
        }

        //load a complete list from a specific file- throw exception in case of problems..
        //for the using with XMLSerializer..
        public static List<T> LoadListFromXMLSerializer<T>(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    
                    XmlRootAttribute xRoot = new XmlRootAttribute();
                    if (typeof (T).ToString () == "Customer")
                        xRoot.ElementName = "ArrayOfCustomers";
                    else if (typeof(T).ToString () == "Drone")
                        xRoot.ElementName = "ArrayOfDrones";
                    else if (typeof(T).ToString() == "DroneCharge")
                        xRoot.ElementName = "ArrayOfDronesCharge";
                    else if (typeof(T).ToString() == "Parcel")
                        xRoot.ElementName = "ArrayOfParcels";
                    else if (typeof(T).ToString() == "Station")
                        xRoot.ElementName = "ArrayOfStations";
                    else if (typeof(T).ToString() == "User")
                        xRoot.ElementName = "ArrayOfUsers";
                    else if(filePath == configPath )
                        xRoot.ElementName = "Config";

                    xRoot.IsNullable = true;
                    XmlSerializer xmlser = new XmlSerializer(typeof(List<T>),xRoot);
                    StreamReader srdr = new StreamReader(filePath);
                    List<T> p = (List<T>)xmlser.Deserialize(srdr);
                    srdr.Close();
                    return p;
                }
                else
                    return new List<T>();
            }
            catch (Exception ex)
            {
                throw new LoadingException(filePath, $"fail to load xml file: {filePath}", ex);
            }
        }
        #endregion
    }
}
