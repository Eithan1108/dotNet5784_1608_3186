namespace Dal;
using DalApi;
using System;
using System.Xml.Linq;


internal class ManagerImplementation : IManager
{
    public string GetManagerEmail()
    {
        XElement root = XMLTools.LoadListFromXMLElement("data-config");
        XElement loozElement = root.Element("ManagerEmail")!;
        if (loozElement.Value == "")
            return null;
        return loozElement.Value;
    }

    public string GetManagerPassWord()
    {
        XElement root = XMLTools.LoadListFromXMLElement("data-config");
        XElement loozElement = root.Element("ManagerPassWord")!;
        if (loozElement.Value == "")
            return null;
        return loozElement.Value;
    }

    public void SetManagerEmail(string managerEmail)
    {
        XElement root = XMLTools.LoadListFromXMLElement("data-config");
        root.Element("ManagerEmail")!.Value = managerEmail;
        XMLTools.SaveListToXMLElement(root, "data-config");
    }

    public void SetManagerPassWord(string managerPassWord)
    {
        XElement root = XMLTools.LoadListFromXMLElement("data-config");
        root.Element("ManagerPassWord")!.Value = managerPassWord;
        XMLTools.SaveListToXMLElement(root, "data-config");
    }
}

