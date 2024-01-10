namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;

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
        if (!(DataSource.Tasks.Exists(t => t.Id == id)))
            throw new Exception($"No task with id of {id}");
        else
        {
            DataSource.Tasks.Remove(DataSource.Tasks.Find(t => t.Id == id)!);
        }
    }

    /// <inheritdoc/>
    public Task? Read(int id)
    {
        if (DataSource.Tasks.Exists(t => t.Id == id))
            return DataSource.Tasks.Find(t => t.Id == id);
        return null;
    }

    /// <inheritdoc/>
    public List<Task> ReadAll()
    {
        return new List<Task>(DataSource.Tasks);
    }

    /// <inheritdoc/>
    public void Update(Task item)
    {
        if (!(DataSource.Tasks.Exists(t => t.Id == item.Id)))
            throw new Exception($"No task with id of {item.Id}");
        else
        {
            DataSource.Tasks.Remove(DataSource.Tasks.Find(t => t.Id == item.Id)!);
            DataSource.Tasks.Add(item);
        }
    }
}
