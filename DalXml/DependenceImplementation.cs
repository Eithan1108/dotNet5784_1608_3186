

namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

/// <summary>
/// Represents the implementation of the <see cref="IDependence"/> interface using XML storage.
/// </summary>
internal class DependenceImplementation : IDependence
{
    /// <summary>
    /// The XML file name for storing dependences.
    /// </summary>
    readonly string s_dependences_xml = "dependences";

    /// <summary>
    /// 
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public int Create(Dependence item)
    {      
        XElement dependencesList = XMLTools.LoadListFromXMLElement(s_dependences_xml); // load the list from the file
        XElement newItem = new XElement("dependence"); // create new element
        int id = Config.NextDependenceId;
        newItem.Add(new XElement("Id", id));
        if (item.DependentTask != null)
            newItem.Add(new XElement("DependentTask", item.DependentTask));
        if (item.DependsOnTask != null)
            newItem.Add(new XElement("DependsOnTask", item.DependsOnTask));
        dependencesList.Add(newItem);
        XMLTools.SaveListToXMLElement(dependencesList, s_dependences_xml); // save the list to the file
        return id;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="DalDoesNotExistException"></exception>
    public void Delete(int id) // delete by id
    {
        XElement dependencesList = XMLTools.LoadListFromXMLElement(s_dependences_xml);

        if (!(dependencesList.Elements().Any(t => int.Parse(t.Element("Id")!.Value) == id)))
            throw new DalDoesNotExistException($"No dependence with id of {id}");

        else
        {
            (from item in dependencesList.Elements() // find the element
                where int.Parse(item.Element("Id")!.Value) == id
                select item
             ).FirstOrDefault()?.Remove();

            XMLTools.SaveListToXMLElement(dependencesList, s_dependences_xml); // save the list to the file
        }

    }

    /// <summary>
    /// Reads the first dependence that matches the specified condition from the data storage.
    /// </summary>
    public Dependence? Read(Func<Dependence, bool> filter)  // read by filter
    {
        return XMLTools.LoadListFromXMLElement(s_dependences_xml).Elements()
            .Select(castDependenceFromXelement).FirstOrDefault(filter);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Dependence? Read(int id) 
    {
        XElement? element =  XMLTools.LoadListFromXMLElement(s_dependences_xml).Elements()
                .FirstOrDefault(dep => id == (int?)dep.Element("Id"));
        return (element == null ? null : (castDependenceFromXelement(element)));
    }

    /// <summary>
    /// Reads all dependences from the data storage based on the specified filter.
    /// </summary>
    public IEnumerable<Dependence?> ReadAll(Func<Dependence, bool>? filter = null) // read all by filter
    {
        var dependencesList = XMLTools.LoadListFromXMLElement(s_dependences_xml).Elements();
        if (filter == null)
            return dependencesList!.Select(castDependenceFromXelement);

        return from item in dependencesList 
               let dependence = castDependenceFromXelement(item) 
               where !filter(dependence)
                    select dependence;

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="item"></param>
    /// <exception cref="DalDoesNotExistException"></exception>
    public void Update(Dependence item)
    {
        XElement dependencesList = XMLTools.LoadListFromXMLElement(s_dependences_xml);
        XElement? element = (from dependenceElement in dependencesList.Elements() // find the element by id
                            where (int?)dependenceElement.Element("Id") == item.Id
                            select dependenceElement
                            ).FirstOrDefault();
        if(element == null)
            throw new DalDoesNotExistException($"Dependence with id of {item.Id} does not exist");

        element.Element("DependentTask")!.Value = item.DependentTask.ToString()!; // update the element
        element.Element("DependsOnTask")!.Value = item.DependsOnTask.ToString()!;

        XMLTools.SaveListToXMLElement(dependencesList, s_dependences_xml); // save the list to the file
    }

    /// <summary>
    /// Reads all dependences from the data storage based on the specified filter.
    /// </summary>
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
