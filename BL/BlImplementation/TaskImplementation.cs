


namespace BlImplementation;
using BO;
using System.Data;
using System.Security.Cryptography;

internal class TaskImplementation : BlApi.ITask
{
    private DalApi.IDal _dal = Factory.Get;

    public void AddOrUpdateSchedualeDateTine(int id, DateTime dateTime) // add or update scheduled date to the system
    {
        DO.Task? doTask = _dal.Task.Read(id); // get task from the system
        if (doTask is null)
            throw new BO.BlDoesNotExistException($"Task with ID= {id} does not exsist");
        BO.Task boTask = DoBoAdapter(doTask); // create new BO.Task

        if (boTask.Deoendencies == null)
            return;
        foreach (var i in boTask.Deoendencies)
        {
            IEnumerable<DO.Dependence?> doDependences = _dal.Dependence.ReadAll(d => d.DependentTask == i.Id); // get all dependencies from the system
            foreach (var doDependence in doDependences)
            {
                if (doDependence != null)
                {
                    DO.Task? task = _dal.Task.Read(t => t.Id == doDependence.DependsOnTask);
                    if (task != null)
                    {
                        if (task.ScheduledDate == null)
                            throw new BO.BlDoesNotExistException($"Task with ID= {task.Id} does not have ScheduledDate");
                        if (task.ScheduledDate <= dateTime)
                            throw new BO.BlDoesNotExistException($"Task with ID= {task.Id} has ScheduledDate after the new ScheduledDate");
                    }
                }
            }
        }


        try
        {
            _dal.Task.Update(BoDoAdapter(boTask)); // update task in the system
        }
        catch (BlBadIdException ex)
        {
            throw new BO.BlBadIdException($"Task with ID= {id} does not exsist");
        }
    }

    public int AddTask(BO.Task task) // add task to the system
    {


        if (task.Alias == null)
            throw new BO.BlBadAliasException("alias must be not null");

        try
        {
            int taskId = _dal.Task.Create(BoDoAdapter(task));
            task.Deoendencies!.Select(d => _dal.Dependence.Create(new DO.Dependence { DependentTask = d.Id, DependsOnTask = taskId })); // create all dependencies
            return taskId;
        }

        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"student with ID= {task.Id} does not exsist");
        }


    }

    public void DeleteTask(int id)
    {
        if (_dal.Task.Read(id) == null)
            throw new BO.BlDoesNotExistException($"Task with ID= {id} does not exsist");

        var dependent = from d in _dal.Dependence.ReadAll()
                        where d.DependsOnTask == id
                        select d;
        if (dependent.FirstOrDefault() != null)
            throw new BO.BlAlreadyExistsException($"Task with ID= {id} already has dependent task {dependent.FirstOrDefault()!.DependentTask}");

        foreach (var d in _dal.Dependence.ReadAll())
        {
            if (d.DependentTask == id)
                _dal.Dependence.Delete(d.Id);
        }

        _dal.Task.Delete(id); // delete task from the system
    }

    public BO.Task GetTask(int id)
    {
        DO.Task? doTask = _dal.Task.Read(id); // get task from the system
        if (doTask is null)
            throw new BO.BlDoesNotExistException($"Task with ID= {id} does not exsist");

        BO.Task boTask = DoBoAdapter(doTask); // create new BO.Task
        return boTask;
    }

    public IEnumerable<BO.Task> GetTasksList(Func<BO.Task, bool> filter) //taskinlist type according to Dan haracaz
    {
        if (filter is null)
        {
            return from item in _dal.Task.ReadAll()
                   select DoBoAdapter(item); // create new BO.Task

        }
        else
        {
            return from item in _dal.Task.ReadAll()
                   where filter(DoBoAdapter(item))
                   select DoBoAdapter(item); // create new BO.Task

        }
    }

    public void UpdateTask(BO.Task task) // update task in the system
    {

        if (task.Alias == null)
            throw new BO.BlBadAliasException("alias must be not null");

        try
        {
            _dal.Task.Update(BoDoAdapter(task)); // update task in the system
        }
        catch (BlBadIdException ex)
        {
            throw new BO.BlBadIdException("student with ID= {id} does not exsist");
        }

    }

    private DO.Task BoDoAdapter(BO.Task task) // adapter from BO to DO
    {


        if (task.Complexity == null) throw new BO.BlBadIdException("Complexity must be positive");
        //DO.Task doTask = new DO.Task(0, task.Alias, task.Description, false, task.CreatedAtDate, (DO.EngineerExperience)task.Complexity,
        //    task.ScheduledDate, task.RequiredEffortTime, task.StartDate, task.CompleteDate, task.DeadLineDate, task.Deliverables,
        //    task.Remarks, task.Engineer != null ? task.Engineer.Id : null);
        DO.Task doTask = new DO.Task
        {
            Id = task.Id,
            Alias = task.Alias,
            Description = task.Description,
            CreateAtDate = task.CreatedAtDate,
            Complexity = (DO.EngineerExperience)task.Complexity,
            ScheduledDate = task.ScheduledDate,
            RequiredEffortTime = task.RequiredEffortTime,
            StartDate = task.StartDate,
            CompleteDate = task.CompleteDate,
            Deliverables = task.Deliverables,
            Remarks = task.Remarks,
            EngineerId = task.Engineer != null ? task.Engineer.Id : null
        };
        return doTask;
    }
    private BO.Task DoBoAdapter(DO.Task task) // adapter from DO to BO
    {
        BO.Task boTask = new BO.Task
        {
            Id = task.Id,
            Alias = task.Alias,
            Description = task.Description,
            CreatedAtDate = task.CreateAtDate,
            Complexity = (BO.EngineerExperience)task.Complexity,
            ScheduledDate = task.ScheduledDate ?? default,
            RequiredEffortTime = task.RequiredEffortTime ?? default,
            StartDate = task.StartDate ?? default,
            CompleteDate = task.CompleteDate ?? default,
            Deliverables = task.Deliverables ?? "",
            Remarks = task.Remarks ?? "",
        };

        if (task.EngineerId != null)
        {
            DO.Engineer? engineer = _dal.Engineer.Read(id => id.Id == task.EngineerId);
            if (engineer != null)
                boTask.Engineer = new BO.EngineerInTask { Id = engineer.Id, Name = engineer.Name };
        }


        if (boTask.ScheduledDate != null && task.StartDate == null)
            boTask.Status = Status.Scheduled;
        else if (boTask.StartDate != null && task.CompleteDate == null)
            boTask.Status = Status.OnTrack;
        else if (boTask.CompleteDate != null)
            boTask.Status = Status.Done;
        else
            boTask.Status = Status.Unsheduled;


        boTask.Deoendencies = _dal.Dependence.ReadAll()
                               .Where(d => d.DependentTask == task.Id)
                               .Select(d =>
                               {
                                   DO.Task task = _dal.Task.Read(id => id.Id == d.DependsOnTask)!;
                                   BO.TaskInList taskInList = new BO.TaskInList
                                   {
                                       Id = d.DependsOnTask,
                                       Description = task.Description,
                                       Alias = task.Alias
                                   };

                                   if (task.ScheduledDate != null && task.StartDate == null)
                                       taskInList.Status = Status.Scheduled;
                                   else if (task.StartDate != null && task.CompleteDate == null)
                                       taskInList.Status = Status.OnTrack;
                                   else if (task.CompleteDate != null)
                                       taskInList.Status = Status.Done;
                                   else
                                       taskInList.Status = Status.Unsheduled;

                                   return taskInList;
                               });
        return boTask;
    }
}

