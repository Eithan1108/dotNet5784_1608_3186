namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;

/// <summary>
/// Represents the implementation of the IEngineer interface for interaction with the data source.
/// </summary>
internal class EngineerImplementation : IEngineer
{
    /// <inheritdoc/>
    public int Create(Engineer item)
    {
        if (DataSource.Engineers.Any(e => e.Id == item.Id))
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
        if (!(DataSource.Engineers.Any(t => t.Id == id)))
            throw new Exception($"No engineer with id of {id}");
        else
        {
            DataSource.Engineers.Remove(DataSource.Engineers.FirstOrDefault(t => t.Id == id)!);
        }
    }
    public Engineer? Read(int id)
    {
        if (DataSource.Engineers.Any(t => t.Id == id))
            return DataSource.Engineers.FirstOrDefault(t => t.Id == id);
        return null;
    }

    /// <inheritdoc/>
    public Engineer? Read(Func<Engineer, bool> filter)
    {

        return DataSource.Engineers.FirstOrDefault(item => filter(item));
    }

    /// <inheritdoc/>
    public IEnumerable<Engineer> ReadAll(Func<Engineer, bool>? filter = null)
    {
        if (filter != null)
        {
            return from item in DataSource.Engineers
                   where filter(item)
                   select item;
        }
        return from item in DataSource.Engineers
               select item;
    }

    /// <inheritdoc/>
    public void Update(Engineer item)
    {
        if (!(DataSource.Engineers.Any(t => t.Id == item.Id)))
            throw new Exception($"No engineer with id of {item.Id}");
        else
        {
            DataSource.Engineers.Remove(DataSource.Engineers.FirstOrDefault(t => t.Id == item.Id)!);
            DataSource.Engineers.Add(item);
        }
    }
}
