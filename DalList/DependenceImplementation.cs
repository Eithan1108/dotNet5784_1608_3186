namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;

/// <summary>
/// Represents the implementation of the IDependence interface for interaction with the data source.
/// </summary>
internal class DependenceImplementation : IDependence
{
    /// <inheritdoc/>
    public int Create(Dependence item)
    {
        int nextId = DataSource.Config.NextDependenceId;
        Dependence dependence = item with { Id = nextId };
        DataSource.Dependences.Add(dependence);
        return nextId;
    }

    /// <inheritdoc/>
    public void Delete(int id)
    {
        if (!(DataSource.Dependences.Any(t => t.Id == id)))
            throw new DalDoesNotExistException($"No dependence with id of {id}");
        else
        {
            DataSource.Dependences.Remove(DataSource.Dependences.FirstOrDefault(t => t.Id == id)!);
        }
    }

    /// <inheritdoc/>
    public Dependence? Read(Func<Dependence, bool> filter)
    {

        return DataSource.Dependences.FirstOrDefault(item => filter(item));
    }

    /// <inheritdoc/>
    public IEnumerable<Dependence> ReadAll(Func<Dependence, bool>? filter = null)
    {
        if (filter != null)
        {
            return from item in DataSource.Dependences
                   where filter(item)
                   select item;
        }
        return from item in DataSource.Dependences
               select item;
    }
    /// <inheritdoc/>
    public void Update(Dependence item)
    {
        if (!(DataSource.Dependences.Any(t => t.Id == item.Id)))
            throw new DalDoesNotExistException($"No dependence with id of {item.Id}");
        else
        {
            DataSource.Dependences.Remove(DataSource.Dependences.FirstOrDefault (t => t.Id == item.Id)!);
            DataSource.Dependences.Add(item);
        }
    }
}
