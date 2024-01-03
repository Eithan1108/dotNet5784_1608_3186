



namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;


public class DependenceImplementation : IDependence
{
    public int Create(Dependence item)
    {
        int nextId = DataSource.Config.NextDependenceId;
        Dependence dependence = item with { Id = nextId };
        DataSource.Dependences.Add(dependence);
        return nextId;


        throw new NotImplementedException();
    }

    public void Delete(int id)
    {
        if (!(DataSource.Dependences.Exists(t => t.Id == id)))
            throw new Exception($"No task with id of {id}");
        else
        {
            DataSource.Dependences.Remove(DataSource.Dependences.Find(t => t.Id == id)!);
        }

        throw new NotImplementedException();
    }

    public Dependence? Read(int id)
    {

        if (DataSource.Dependences.Exists(t => t.Id == id))
            return DataSource.Dependences.Find(t => t.Id == id);
        return null;

        throw new NotImplementedException();
    }

    public List<Dependence> ReadAll()
    {

        return new List<Dependence>(DataSource.Dependences);


        throw new NotImplementedException();
    }

    public void Update(Dependence item)
    {

        if (!(DataSource.Dependences.Exists(t => t.Id == item.Id)))
            throw new Exception($"No task with id of {item.Id}");
        else
        {
            DataSource.Dependences.Remove(DataSource.Dependences.Find(t => t.Id == item.Id)!);
            DataSource.Dependences.Add(item);
        }


        throw new NotImplementedException();
    }
}
