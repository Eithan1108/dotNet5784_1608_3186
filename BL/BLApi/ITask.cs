﻿

namespace BlApi;

/// <summary>
/// // Interface for Task
/// </summary>
public interface ITask
{
    public IEnumerable<BO.Task> GetTasksList(Func<BO.Task, bool> filter); //list of tasks
    public BO.Task GetTask(int id); // get task by id
    public int AddTask(BO.Task task); // add task to the system
    public void DeleteTask(int id); // delete task from the system
    public void UpdateTask(BO.Task task); // update task in the system
    public void AddOrUpdateSchedualeDateTine(int id, DateTime? dateTime, DateTime SchedualeProjectDate); // add or update scheduale date time
}
