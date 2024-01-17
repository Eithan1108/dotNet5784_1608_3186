

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
        List<Engineer> engineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_xml);
        Engineer engineer = item;
        engineers.Add(engineer);
        XMLTools.SaveListToXMLSerializer(engineers, s_engineers_xml);
        return item.Id;
    }

    public void Delete(int id)
    {
        List<Engineer> engineerList = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_xml);
        if (!(engineerList.Any(e => e.Id == id)))
            throw new DalDoesNotExistException($"No engineer with id of {id}");
        else
        {
            engineerList.Remove(engineerList.FirstOrDefault(e => e.Id == id)!);
            XMLTools.SaveListToXMLSerializer(engineerList, s_engineers_xml);
        }
    }

    public Engineer? Read(Func<Engineer, bool> filter)
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
        if (filter != null)
        {
            return from item in engineerList
                   where filter(item)
                   select item;
        }
        return from item in engineerList
               select item;
    }

    public void Update(Engineer item)
    {
        List<Engineer> engineerList = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_xml);
        if (!(engineerList.Any(e => e.Id == item.Id)))
            throw new DalDoesNotExistException($"No engineer with id of {item.Id}");
        else
        {
            engineerList.Remove(engineerList.FirstOrDefault(t => t.Id == item.Id)!);
            engineerList.Add(item);
            XMLTools.SaveListToXMLSerializer(engineerList, s_engineers_xml);
        }
    }
}
