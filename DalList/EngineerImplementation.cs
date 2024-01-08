namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;

/// <summary>
/// Represents the implementation of the IEngineer interface for interaction with the data source.
/// </summary>
public class EngineerImplementation : IEngineer
{
    /// <inheritdoc/>
    public int Create(Engineer item)
    {
        if (DataSource.Engineers.Exists(e => e.Id == item.Id))
            throw new Exception($"Engineer with id {item.Id} already exists");
        else
        {
            DataSource.Engineers.Add(item);
            return item.Id;
        }
    }

    /// <inheritdoc/>
    public void Delete(int id)
    {
        if (!(DataSource.Engineers.Exists(t => t.Id == id)))
            throw new Exception($"No engineer with id of {id}");
        else
        {
            DataSource.Engineers.Remove(DataSource.Engineers.Find(t => t.Id == id)!);
        }
    }

    /// <inheritdoc/>
    public Engineer? Read(int id)
    {
        if (DataSource.Engineers.Exists(t => t.Id == id))
            return DataSource.Engineers.Find(t => t.Id == id);
        return null;
    }

    /// <inheritdoc/>
    public List<Engineer> ReadAll()
    {
        return new List<Engineer>(DataSource.Engineers);
    }

    /// <inheritdoc/>
    public void Update(Engineer item)
    {
        if (!(DataSource.Engineers.Exists(t => t.Id == item.Id)))
            throw new Exception($"No engineer with id of {item.Id}");
        else
        {
            DataSource.Engineers.Remove(DataSource.Engineers.Find(t => t.Id == item.Id)!);
            DataSource.Engineers.Add(item);
        }
    }
}
