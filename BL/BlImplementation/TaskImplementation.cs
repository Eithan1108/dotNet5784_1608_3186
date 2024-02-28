


namespace BlImplementation;
using BO;
using System.Data;
using System.Security.Cryptography;

/// <summary>
/// // task implementation
/// </summary>
internal class TaskImplementation : BlApi.ITask
{
    private DalApi.IDal _dal = Factory.Get;

    /// <summary>
    /// // add or update scheduled date to the system
    /// </summary>
    /// <param name="id"></param>
    /// <param name="dateTime"></param>
    /// <param name="SchedualeProjectDate"></param>
    /// <exception cref="BlBadDateException"></exception>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    /// <exception cref="BO.BlBadIdException"></exception>
    public void AddOrUpdateSchedualeDateTine(int id, DateTime? dateTime, DateTime SchedualeProjectDate)
    {
        if (dateTime == null)
            throw new BlBadDateException("DateTime must be not null");
        DO.Task? doTask = _dal.Task.Read(id); // get task from the system

        if (doTask is null)
            throw new BO.BlDoesNotExistException($"Task with ID = {id} does not exsist");

        BO.Task boTask = DoBoAdapter(doTask); // create new BO.Task

        if (boTask.ScheduledDate != DateTime.MinValue)
            throw new BO.BlBadDateException("The new ScheduledDate is before the ForecastDate");

        if (boTask.Dependencies!.Count() == 0)
        {
            boTask.ScheduledDate = SchedualeProjectDate;
        }
        else
        {
            foreach (var i in boTask.Dependencies!)
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
                                throw new BO.BlDoesNotExistException($"Task with ID = {task.Id} does not have ScheduledDate");
                            if (boTask.ForecastDate > dateTime)
                                throw new BO.BlDoesNotExistException($"Task with ID = {task.Id} has ScheduledDate after the new ScheduledDate");
                        }
                    }
                }
            }
            boTask.ScheduledDate = dateTime;
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

    /// <summary>
    /// // add task to the system
    /// </summary>
    /// <param name="task"></param>
    /// <returns></returns>
    /// <exception cref="BO.BlBadAliasException"></exception>
    /// <exception cref="BO.BlAlreadyExistsException"></exception>
    public int AddTask(BO.Task task)
    {
        if (task.Description == "")
            throw new BO.BlBadTaskDescreptionException("Description must contain words");

        if (task.Alias == "")
            throw new BO.BlBadAliasException("alias must be not null");

        if (task.RequiredEffortTime == null)
            throw new BO.BlBadAliasException("Required Effort Time must be not null");

        if (task.Deliverables == "")
            throw new BO.BlBadAliasException("Deliverables must contain words");


        if (task.Remarks == "")
            throw new BO.BlBadRemarksException("Remarks must contain words");

        try
        {
            int taskId = _dal.Task.Create(BoDoAdapter(task));


            var dependencies = task.Dependencies ?? new List<BO.TaskInList>(); // Handle null reference
            dependencies.Select(d =>
            {
                _dal.Dependence.Create(new DO.Dependence { DependentTask = taskId, DependsOnTask = d.Id });
                return true; // Return value is irrelevant, just to satisfy LINQ syntax
            }).ToList(); // Execute LINQ query by converting it to a list

            return taskId;
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"student with ID= {task.Id} does not exsist");
        }
    }

    /// <summary>
    /// // delete task from the system
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    /// <exception cref="BO.BlAlreadyExistsException"></exception>
    public void DeleteTask(int id)
    {
        if (_dal.Task.Read(id) == null)
        {
            throw new BO.BlDoesNotExistException($"Task with ID= {id} does not exist");
        }

        var dependent = _dal.Dependence.ReadAll().FirstOrDefault(d => d.DependsOnTask == id);
        if (dependent != null)
        {
            throw new BO.BlAlreadyExistsException($"Task with ID= {id} already has dependent task {dependent.DependentTask}");
        }

        _dal.Dependence.ReadAll().Where(d => d.DependentTask == id).ToList().ForEach(d => _dal.Dependence.Delete(d.Id));

        _dal.Task.Delete(id); // delete task from the system
    }

    /// <summary>
    /// // get task from the system
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    public BO.Task GetTask(int id)
    {
        DO.Task? doTask = _dal.Task.Read(id); // get task from the system
        if (doTask is null)
            throw new BO.BlDoesNotExistException($"Task with ID= {id} does not exsist");

        BO.Task boTask = DoBoAdapter(doTask); // create new BO.Task
        return boTask;
    }

    /// <summary>
    /// // get all tasks from the system
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public IEnumerable<BO.Task> GetTasksList(Func<BO.Task, bool> filter)
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

    public IEnumerable<BO.TaskInList> TaskToTaskInListConverter(IEnumerable<BO.Task> tasks) // convert from Task to TaskInList
    {
        return tasks.Select(t => new BO.TaskInList { Id = t.Id, Description = t.Description, Alias = t.Alias, Status = t.Status });
    }


    /// <summary>
    /// // update task in the system
    /// </summary>
    /// <param name="task"></param>
    /// <exception cref="BO.BlBadAliasException"></exception>
    /// <exception cref="BO.BlBadIdException"></exception>
    public void UpdateTask(BO.Task task)
    {
        if(task.Engineer != null && task.Engineer.Id != null && task.Complexity > (BO.EngineerExperience)_dal.Engineer.Read(engineer => engineer.Id == task.Engineer!.Id)!.Level)
            throw new BlBadLevelException("Complexity must be greater than the engineer level");
        if (task.Engineer == null || task.Engineer.Id == null /*&& _dal.Task.Read(t => t.EngineerId == task.Engineer!.Id) != null*/)
            throw new BO.BlBadIdException("Id must be not null");
       

        if (task.Alias == null)
            throw new BO.BlBadAliasException("alias must be not null");

        try
        {
           

            foreach (var d in _dal.Dependence.ReadAll())
            {
                if (d.DependentTask == task.Id)
                    _dal.Dependence.Delete(d.Id);
            }


            _dal.Task.Update(BoDoAdapter(task)); // update task in the system

            var dependencies = task.Dependencies ?? new List<BO.TaskInList>(); // Handle null reference
            dependencies.Select(d =>
            {
                _dal.Dependence.Create(new DO.Dependence { DependentTask = task.Id, DependsOnTask = d.Id });
                return true; // Return value is irrelevant, just to satisfy LINQ syntax
            }).ToList(); // Execute LINQ query by converting it to a list

        }
        catch (BlBadIdException ex)
        {
            throw new BO.BlBadIdException("student with ID = {id} does not exsist");
        }
    }
    /// <summary>
    /// // adapter from BO to DO
    /// </summary>
    /// <param name="task"></param>
    /// <returns></returns>
    /// <exception cref="BO.BlBadIdException"></exception>
    private DO.Task BoDoAdapter(BO.Task task)
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

    /// <summary>
    /// // adapter from DO to BO
    /// </summary>
    /// <param name="task"></param>
    /// <returns></returns>
    private BO.Task DoBoAdapter(DO.Task task)
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


        if (boTask.ScheduledDate != DateTime.MinValue && boTask.ScheduledDate != null &&( boTask.StartDate == null || boTask.StartDate==DateTime.MinValue))
            boTask.Status = Status.Scheduled;
        else if (boTask.StartDate != null && boTask.StartDate != DateTime.MinValue && (boTask.CompleteDate == null ||boTask.CompleteDate==DateTime.MinValue))
            boTask.Status = Status.OnTrack;
        else if (boTask.CompleteDate != null && boTask.CompleteDate!=DateTime.MinValue)
            boTask.Status = Status.Done;
        else
            boTask.Status = Status.Unsheduled;


        List<BO.TaskInList> dependencies = new List<BO.TaskInList>();
        foreach (var d in _dal.Dependence.ReadAll())
        {
            if (d.DependentTask == task.Id)
            {
                DO.Task? dependentTask = _dal.Task.Read(id => id.Id == d.DependsOnTask);
                if (dependentTask != null)
                {
                    BO.TaskInList taskInList = new BO.TaskInList
                    {
                        Id = dependentTask.Id,
                        Description = dependentTask.Description,
                        Alias = dependentTask.Alias
                    };
                    if (dependentTask.ScheduledDate != null && dependentTask.StartDate == null)
                        taskInList.Status = Status.Scheduled;
                    else if (dependentTask.StartDate != null && dependentTask.CompleteDate == null)
                        taskInList.Status = Status.OnTrack;
                    else if (dependentTask.CompleteDate != null)
                        taskInList.Status = Status.Done;
                    else
                        taskInList.Status = Status.Unsheduled;
                    dependencies.Add(taskInList);
                }
            }
        }
        boTask.Dependencies = dependencies;
        return boTask;
    }

    public void StartTask(int id, int engid)
    {
        // TODO: Cheack if all dependencies are done
        //foreach (var d in _dal.Dependence.ReadAll())
        //{
        //    if (d.DependentTask == id)
        //    {
        //        DO.Task? dependentTask = _dal.Task.Read(id => id.Id == d.DependsOnTask);
        //        if (dependentTask != null && dependentTask.CompleteDate == null)
        //            throw new BO.BlNotExistsException($"Task with ID = {dependentTask.Id} is not done");

        //        // todo: update the task with StartTime Date.Now
        //        Task task = DoBoAdapter(_dal.Task.Read(id)!);
        //        if (task != null)
        //        {
        //            task.StartDate = DateTime.Now;
        //            _dal.Task.Update(BoDoAdapter(task));
        //        }

        //    }
        //}

        Task task = DoBoAdapter(_dal.Task.Read(id)!);
        if (task != null)
        {
            task.Engineer = new EngineerInTask { Id = engid };
            task.StartDate = DateTime.Now;
            _dal.Task.Update(BoDoAdapter(task));
        }

    }

    public void StopTask(int id) 
    {
       Task task =DoBoAdapter(_dal.Task.Read(id)!);
        if (task != null)
        {
            task.CompleteDate = DateTime.Now;
            _dal.Task.Update(BoDoAdapter(task));
        }   
    }
}

