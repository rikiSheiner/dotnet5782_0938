﻿using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using DalApi;

namespace DalXml
{
    internal static class XMLtools
    {
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
                    List<T> list;
                    XmlSerializer x = new XmlSerializer(typeof(List<T>));
                    FileStream file = new FileStream(filePath, FileMode.Open);
                    list = (List<T>)x.Deserialize(file);
                    file.Close();
                    return list;
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