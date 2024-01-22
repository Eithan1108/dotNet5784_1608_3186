

namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

internal class EngineerImplementation : IEngineer
{
    readonly string s_engineers_xml = "engineers";

    public int Create(Engineer item)
    {
        List<Engineer> engineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_xml); // load the list from the file
        Engineer engineer = item;
        engineers.Add(engineer);
        XMLTools.SaveListToXMLSerializer(engineers, s_engineers_xml); // save the list to the file
        return item.Id;
    }

    public void Delete(int id)
    {
        List<Engineer> engineerList = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_xml); // load the list from the file
        if (!(engineerList.Any(e => e.Id == id)))
            throw new DalDoesNotExistException($"No engineer with id of {id}");
        else
        {
            engineerList.Remove(engineerList.FirstOrDefault(e => e.Id == id)!); // remove the engineer from the list
            XMLTools.SaveListToXMLSerializer(engineerList, s_engineers_xml); // save the list to the file
        }
    }

    public Engineer? Read(Func<Engineer, bool> filter) // returns the first engineer that matches the condition
    {
        List<Engineer> engineerList = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_xml);
        return engineerList.FirstOrDefault(item => filter(item));
    }

    public Engineer? Read(int id)
    {
        List<Engineer> engineerList = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_xml);
        if (engineerList.Any(e => e.Id == id)) 
            return engineerList.FirstOrDefault(t => t.Id == id);
        return null;
    }

    public IEnumerable<Engineer?> ReadAll(Func<Engineer, bool>? filter = null)
    {
        List<Engineer> engineerList = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_xml);
        if (filter != null) // if there is a filter
        {
            return from item in engineerList
                   where filter(item)
                   select item;
        }
        return from item in engineerList // if there is no filter
               select item;
    }

    public void Update(Engineer item)
    {
        List<Engineer> engineerList = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_xml); // load the list from the file
        if (!(engineerList.Any(e => e.Id == item.Id))) // if there is no engineer with the same id
            throw new DalDoesNotExistException($"No engineer with id of {item.Id}");
        else
        {
            engineerList.Remove(engineerList.FirstOrDefault(t => t.Id == item.Id)!); // remove the old engineer
            engineerList.Add(item);
            XMLTools.SaveListToXMLSerializer(engineerList, s_engineers_xml); // save the list to the file
        }
    }
}
