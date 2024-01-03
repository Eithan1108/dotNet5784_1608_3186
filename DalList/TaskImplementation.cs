
namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

public class TaskImplementation : ITask
{
    public int Create(Task item)
    {

        int nextId = DataSource.Config.NextTaskId;
        Task task = item with { Id = nextId };
        DataSource.Tasks.Add(task);
        return nextId;


        throw new NotImplementedException();
    }

    public void Delete(int id)
    {

        if (!(DataSource.Tasks.Exists(t => t.Id == id)))
            throw new Exception($"No task with id of {id}");
        else
        {
            DataSource.Tasks.Remove(DataSource.Tasks.Find(t => t.Id == id)!);
        }

        throw new NotImplementedException();
    }

    public Task? Read(int id)
    {

        if (DataSource.Tasks.Exists(t => t.Id == id))
            return DataSource.Tasks.Find(t => t.Id == id);
        return null;


        throw new NotImplementedException();
    }

    public List<Task> ReadAll()
    {


        return new List<Task>(DataSource.Tasks);


        throw new NotImplementedException();
    }

    public void Update(Task item)
    {

        if (! (DataSource.Tasks.Exists(t => t.Id == item.Id)) )
            throw new Exception($"No task with id of {item.Id}");
        else
        {
            DataSource.Tasks.Remove(DataSource.Tasks.Find(t => t.Id == item.Id)!);
            DataSource.Tasks.Add(item);
        }


        throw new NotImplementedException();
    }
}
