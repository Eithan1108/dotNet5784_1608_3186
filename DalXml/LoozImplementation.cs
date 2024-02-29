namespace Dal;
using DalApi;
using System;
using System.Xml.Linq;

/// <summary>
/// Represents the implementation of the ITask interface for interaction with the data source.
/// </summary>
internal class LoozImplementation : ILooz
{
    public DateTime? GetEndDate()
    {
        XElement root = XMLTools.LoadListFromXMLElement("data-config");
        XElement loozElement = root.Element("ProjectEndDate")!;
        return (DateTime?)loozElement;
    }

    public DateTime? GetProjectDataScreen()
    {
        XElement root = XMLTools.LoadListFromXMLElement("data-config");
        XElement loozElement = root.Element("ProjectDataScreen")!;
        if (loozElement.Value == "")
            return null;
        return DateTime.Parse(loozElement.Value);
    }

    public DateTime? GetStartDate()
    {
        XElement root = XMLTools.LoadListFromXMLElement("data-config");
        XElement loozElement = root.Element("ProjectStartDate")!;
        if(loozElement.Value == "")
            return null;

        return DateTime.Parse(loozElement.Value);
    }



    public void SetEndDate(DateTime? endDate)
    {
        XElement root = XMLTools.LoadListFromXMLElement("data-config");
        root.Element("ProjectEndDate")!.Value = endDate.ToString()!;
        XMLTools.SaveListToXMLElement(root, "data-config");
    }

    public void SetProjectDataScreen(DateTime? projectDataScreen)
    {
        XElement root = XMLTools.LoadListFromXMLElement("data-config");
        root.Element("ProjectDataScreen")!.Value = projectDataScreen.ToString()!;
        XMLTools.SaveListToXMLElement(root, "data-config");
    }

    public void SetStartDate(DateTime? startDate)
    {
        XElement root = XMLTools.LoadListFromXMLElement("data-config");
        root.Element("ProjectStartDate")!.Value = startDate.ToString()!;
        XMLTools.SaveListToXMLElement(root, "data-config");
    }
}
