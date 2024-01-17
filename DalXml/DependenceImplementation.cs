

namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

internal class DependenceImplementation : IDependence
{
    readonly string s_dependences_xml = "dependences";

    public int Create(Dependence item)
    {
        XElement dependencesList = XMLTools.LoadListFromXMLElement(s_dependences_xml);
        XElement newItem = new XElement("dependence");
        newItem.Add(new XElement("Id", Config.NextDependenceId));
        if (item.DependentTask != null)
            newItem.Add(new XElement("DependentTask", item.DependentTask));
        if (item.DependsOnTask != null)
            newItem.Add(new XElement("DependsOnTask", item.DependsOnTask));
        dependencesList.Add(newItem);
        XMLTools.SaveListToXMLElement(dependencesList, s_dependences_xml);
        return int.
            Parse(newItem.Element("Id")!.Value);
    }

    public void Delete(int id)
    {
        XElement dependencesList = XMLTools.LoadListFromXMLElement(s_dependences_xml);

        if (!(dependencesList.Elements().Any(t => int.Parse(t.Element("Id")!.Value) == id)))
            throw new DalDoesNotExistException($"No dependence with id of {id}");

        else
        {
            (from item in dependencesList.Elements()
                where int.Parse(item.Element("Id")!.Value) == id
                select item
             ).FirstOrDefault()?.Remove(); // ******************************************************************** //

            XMLTools.SaveListToXMLElement(dependencesList, s_dependences_xml);
        }

    }

    public Dependence? Read(Func<Dependence, bool> filter)
    {
        return XMLTools.LoadListFromXMLElement(s_dependences_xml).Elements()
            .Select(castDependenceFromXelement).FirstOrDefault(filter);
    }

    public Dependence? Read(int id)
    {
        XElement? element =  XMLTools.LoadListFromXMLElement(s_dependences_xml).Elements()
                .FirstOrDefault(dep => id == (int?)dep.Element("Id"));
        return (element == null ? null : (castDependenceFromXelement(element)));
    }

    public IEnumerable<Dependence?> ReadAll(Func<Dependence, bool>? filter = null)
    {
        var dependencesList = XMLTools.LoadListFromXMLElement(s_dependences_xml).Elements();
        if (dependencesList == null)
            return dependencesList!.Select(castDependenceFromXelement);
        return dependencesList.Select(castDependenceFromXelement).Where(filter!);

    }

    public void Update(Dependence item)
    {
        XElement dependencesList = XMLTools.LoadListFromXMLElement(s_dependences_xml);
        XElement? element = (from dependenceElement in dependencesList.Elements()
                            where (int?)dependenceElement.Element("Id") == item.Id
                            select dependenceElement
                            ).FirstOrDefault();
        if(element == null)
            throw new DalDoesNotExistException($"Dependence with id of {item.Id} does not exist");

        element.Element("DependentTask")!.Value = item.DependentTask.ToString()!;
        element.Element("DependsOnTask")!.Value = item.DependsOnTask.ToString()!;

        XMLTools.SaveListToXMLElement(dependencesList, s_dependences_xml);
    }

    Dependence castDependenceFromXelement (XElement xElement)
    {
        return new
            (
                Id: int.Parse(xElement.Element("Id")!.Value),
                DependentTask: int.Parse(xElement.Element("DependentTask")!.Value),
                DependsOnTask: int.Parse(xElement.Element("DependsOnTask")!.Value)
            );
    }
}
