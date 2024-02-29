

namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

/// <summary>
/// Represents the implementation of the <see cref="ILooz"/> interface using XML storage.
/// </summary>
internal class TaskImplementation : ITask
{
    /// <summary>
    /// The XML file name for storing tasks.
    /// </summary>
    readonly string s_tasks_xml = "tasks";

    /// <summary>
    /// 
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public int Create(Task item) 
    { 
        int id = Config.NextTaskId; 
        List<Task> tasksList = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml); // load the list from the file
        Task task = item with { Id = id, CreateAtDate = item.CreateAtDate }; // create a new task with the new id and the current date
        tasksList.Add(task);
        XMLTools.SaveListToXMLSerializer(tasksList, s_tasks_xml);
        return id;
    }

    /// <summary>
    /// Deletes a task with the specified ID from the data storage.
    /// </summary>
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

    /// <summary>
    /// Reads the first task that matches the specified condition from the data storage.
    /// </summary>
    public Task? Read(Func<Task, bool> filter) // returns the first task that matches the condition
    {
        List<Task> tasksList = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml); // load the list from the file
        return tasksList.FirstOrDefault(item => filter(item));
    }

    /// <summary>
    /// Reads a task with the specified ID from the data storage.
    /// </summary>
    public Task? Read(int id)
    {
        List<Task> tasksList = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml); // load the list from the file
        if (tasksList.Any(t => t.Id == id))
            return tasksList.FirstOrDefault(t => t.Id == id);
        return null;
    }

    /// <summary>
    /// Reads all tasks from the data storage based on the specified filter.
    /// </summary>
    public IEnumerable<Task?> ReadAll(Func<Task, bool>? filter = null) 
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

    /// <summary>
    /// Updates an existing task in the data storage.
    /// </summary>
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

