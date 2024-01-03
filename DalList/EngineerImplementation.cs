

namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

public class EngineerImplementation : IEngineer
{
    public int Create(Engineer item)
    {

        if(DataSource.Engineers.Exists(e => e.Id == item.Id))
            throw new Exception($"Engineer with id {item.Id} already exists");

        else
        {
            DataSource.Engineers.Add(item);
            return item.Id;
        }


        throw new NotImplementedException();
    }

    public void Delete(int id)
    {
        if (!(DataSource.Engineers.Exists(t => t.Id == id)))
            throw new Exception($"No task with id of {id}");
        else
        {
            DataSource.Engineers.Remove(DataSource.Engineers.Find(t => t.Id == id)!);
        }

        throw new NotImplementedException();
    }

    public Engineer? Read(int id)
    {
        if (DataSource.Engineers.Exists(t => t.Id == id))
            return DataSource.Engineers.Find(t => t.Id == id);
        return null;


        throw new NotImplementedException();
    }

    public List<Engineer> ReadAll()
    {


        return new List<Engineer>(DataSource.Engineers);


        throw new NotImplementedException();
    }

    public void Update(Engineer item)
    {

        if (!(DataSource.Engineers.Exists(t => t.Id == item.Id)))
            throw new Exception($"No task with id of {item.Id}");
        else
        {
            DataSource.Engineers.Remove(DataSource.Engineers.Find(t => t.Id == item.Id)!);
            DataSource.Engineers.Add(item);
        }


        throw new NotImplementedException();
    }
}
