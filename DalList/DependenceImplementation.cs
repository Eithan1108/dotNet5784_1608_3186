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
        if (!(DataSource.Dependences.Exists(t => t.Id == id)))
            throw new Exception($"No dependence with id of {id}");
        else
        {
            DataSource.Dependences.Remove(DataSource.Dependences.Find(t => t.Id == id)!);
        }
    }

    /// <inheritdoc/>
    public Dependence? Read(int id)
    {
        if (DataSource.Dependences.Exists(t => t.Id == id))
            return DataSource.Dependences.Find(t => t.Id == id);
        return null;
    }

    /// <inheritdoc/>
    public List<Dependence> ReadAll()
    {
        return new List<Dependence>(DataSource.Dependences);
    }

    /// <inheritdoc/>
    public void Update(Dependence item)
    {
        if (!(DataSource.Dependences.Exists(t => t.Id == item.Id)))
            throw new Exception($"No dependence with id of {item.Id}");
        else
        {
            DataSource.Dependences.Remove(DataSource.Dependences.Find(t => t.Id == item.Id)!);
            DataSource.Dependences.Add(item);
        }
    }
}
