

namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

internal class TaskImplementation : ITask
{
    readonly string s_tasks_xml = "tasks";

    public int Create(Task item) // returns the id of the new task
    {
        int id = Config.NextTaskId; 
        List<Task> tasksList = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml); // load the list from the file
        Task task = item with { Id = id, CreateAtDate = DateTime.Now }; // create a new task with the new id and the current date
        tasksList.Add(task);
        XMLTools.SaveListToXMLSerializer(tasksList, s_tasks_xml);
        return id;
    }

    public void Delete(int id)
    {
        List<Task> tasksList = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml);
        if (!(tasksList.Any(t => t.Id == id))) // if there is no task with the given id
            throw new DalDoesNotExistException($"No task with id of {id}");
        else
        {
            tasksList.Remove(tasksList.FirstOrDefault(t => t.Id == id)!); // remove the task from the list
            XMLTools.SaveListToXMLSerializer(tasksList, s_tasks_xml);
        }
    }

    public Task? Read(Func<Task, bool> filter) // returns the first task that matches the condition
    {
        List<Task> tasksList = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml); // load the list from the file
        return tasksList.FirstOrDefault(item => filter(item));
    }

    public Task? Read(int id)
    {
        List<Task> tasksList = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml); // load the list from the file
        if (tasksList.Any(t => t.Id == id))
            return tasksList.FirstOrDefault(t => t.Id == id);
        return null;
    }

    public IEnumerable<Task?> ReadAll(Func<Task, bool>? filter = null) // returns all the tasks that matches the condition
    {
        List<Task> tasksList = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml);
        if (filter != null)
        {
            return from item in tasksList
                   where filter(item)
                   select item;
        }
        return from item in tasksList
               select item;
    }

    public void Update(Task item)
    {
        List<Task> tasksList = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml);
        if (!(tasksList.Any(t => t.Id == item.Id))) // if there is no task with the same id
            throw new DalDoesNotExistException($"No task with id of {item.Id}");
        else
        {
            tasksList.Remove(tasksList.FirstOrDefault(t => t.Id == item.Id)!); // remove the old task
            tasksList.Add(item);
            XMLTools.SaveListToXMLSerializer(tasksList, s_tasks_xml);
        }
    }
}

