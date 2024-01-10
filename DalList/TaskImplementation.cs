namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Represents the implementation of the ITask interface for interaction with the data source.
/// </summary>
internal class TaskImplementation : ITask
{
    /// <inheritdoc/>
    public int Create(Task item)
    {
        int nextId = DataSource.Config.NextTaskId;
        Task task = item with { Id = nextId };
        DataSource.Tasks.Add(task);
        return nextId;
    }

    /// <inheritdoc/>
    public void Delete(int id)
    {
        if (!(DataSource.Tasks.Any(t => t.Id == id)))
            throw new DalDoesNotExistException($"No task with id of {id}");
        else
        {
           DataSource.Tasks.Remove(DataSource.Tasks.FirstOrDefault(t => t.Id == id)!);
        }
    }
    public Task? Read(int id)
    {
        if (DataSource.Tasks.Any(t => t.Id == id))
            return DataSource.Tasks.FirstOrDefault(t => t.Id == id);
        return null;
    }

    /// <inheritdoc/>
    public Task? Read(Func<Task, bool> filter)
    {

        return DataSource.Tasks.FirstOrDefault(item => filter(item));
    }

    /// <inheritdoc/>
    public IEnumerable<Task> ReadAll(Func<Task, bool>? filter = null)
    {
        if (filter != null)
        {
            return from item in DataSource.Tasks
                   where filter(item)
                   select item;
        }
        return from item in DataSource.Tasks
               select item;
    }

    /// <inheritdoc/>
    public void Update(Task item)
    {
        if (!(DataSource.Tasks.Any(t => t.Id == item.Id)))
            throw new DalDoesNotExistException($"No task with id of {item.Id}");
        else
        {
            DataSource.Tasks.Remove(DataSource.Tasks.FirstOrDefault(t => t.Id == item.Id)!);
            DataSource.Tasks.Add(item);
        }
    }
}
