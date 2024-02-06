namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
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

    public void SetStartDate(DateTime? startDate)
    {
        XElement root = XMLTools.LoadListFromXMLElement("data-config");
        root.Element("ProjectStartDate")!.Value = startDate.ToString()!;
        XMLTools.SaveListToXMLElement(root, "data-config");
    }
}
