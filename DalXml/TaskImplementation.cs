

namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

internal class TaskImplementation : ITask
{
    readonly string s_tasks_xml = "tasks";

    public int Create(Task item)
    {
        int id = Config.NextTaskId;
        List<Task> tasksList = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml);
        Task task = item with { Id = id, CreateAtDate = DateTime.Now };
        tasksList.Add(task);
        XMLTools.SaveListToXMLSerializer(tasksList, s_tasks_xml);
        return id;
    }

    public void Delete(int id)
    {
        List<Task> tasksList = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml);
        if (!(tasksList.Any(t => t.Id == id)))
            throw new DalDoesNotExistException($"No task with id of {id}");
        else
        {
            tasksList.Remove(tasksList.FirstOrDefault(t => t.Id == id)!);
            XMLTools.SaveListToXMLSerializer(tasksList, s_tasks_xml);
        }
    }

    public Task? Read(Func<Task, bool> filter)
    {
        List<Task> tasksList = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml);
        return tasksList.FirstOrDefault(item => filter(item));
    }

    public Task? Read(int id)
    {
        List<Task> tasksList = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml);
        if (tasksList.Any(t => t.Id == id))
            return tasksList.FirstOrDefault(t => t.Id == id);
        return null;
    }

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

    public void Update(Task item)
    {
        List<Task> tasksList = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml);
        if (!(tasksList.Any(t => t.Id == item.Id)))
            throw new DalDoesNotExistException($"No task with id of {item.Id}");
        else
        {
            tasksList.Remove(tasksList.FirstOrDefault(t => t.Id == item.Id)!);
            tasksList.Add(item);
            XMLTools.SaveListToXMLSerializer(tasksList, s_tasks_xml);
        }
    }
}

