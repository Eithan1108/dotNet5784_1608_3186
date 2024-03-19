


namespace BlImplementation;
using BO;
using DO;
using System.Data;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;

/// <summary>
/// // task implementation
/// </summary>
internal class TaskImplementation : BlApi.ITask
{
    private DalApi.IDal _dal = Factory.Get;
    private readonly Bl _bl;
    internal TaskImplementation(Bl bl) => _bl = bl;

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
        if (task.Engineer != null && task.Engineer.Id != null && task.Complexity > (BO.EngineerExperience)_dal.Engineer.Read(engineer => engineer.Id == task.Engineer!.Id)!.Level)
            throw new BlBadLevelException("Complexity must be greater than the engineer level");

        //if (task.Engineer == null || task.Engineer.Id == null) // *&& _dal.Task.Read(t => t.EngineerId == task.Engineer!.Id) != null*/)
        //    throw new BO.BlBadIdException("Id must be not null");


        if (task.Alias == null)
            throw new BO.BlBadAliasException("alias must be not null");


        if (task.Dependencies != null)
        {
            var circularDeps = task.Dependencies
                .Where(d => CheckForCircles(task.Id, d.Id))
                .ToList();

            // Throw if any found
            if (circularDeps.Any())
            {
                throw new BlBadLevelException("Found circular dependency");
            }
        }




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
            throw new BO.BlBadIdException("Student with id = {id} does not exist");
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
            StartDate = task.StartDate ?? null,
            CompleteDate = task.CompleteDate ?? null,
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
            StartDate = task.StartDate ?? null,
            CompleteDate = task.CompleteDate ?? null,
            Deliverables = task.Deliverables ?? "",
            Remarks = task.Remarks ?? "",
        };

        if (task.EngineerId != null)
        {
            DO.Engineer? engineer = _dal.Engineer.Read(id => id.Id == task.EngineerId);
            if (engineer != null)
                boTask.Engineer = new BO.EngineerInTask { Id = engineer.Id, Name = engineer.Name };
        }

        boTask.Status = Status.Unsheduled;
        if (boTask.ScheduledDate != DateTime.MinValue && boTask.ScheduledDate != null)
            boTask.Status = Status.Scheduled;
        if (boTask.ScheduledDate != DateTime.MinValue && boTask.ScheduledDate != null && boTask.ScheduledDate < _bl.Clock)
            boTask.Status = Status.InJeopardy;
        if (boTask.StartDate != null && boTask.StartDate != DateTime.MinValue )
            boTask.Status = Status.OnTrack;
        if (boTask.CompleteDate != null && boTask.CompleteDate != DateTime.MinValue)
            boTask.Status = Status.Done;
        
         


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
                    taskInList.Status = Status.Unsheduled;
                    if (dependentTask.ScheduledDate != null && dependentTask.ScheduledDate != DateTime.MinValue)
                        taskInList.Status = Status.Scheduled;
                    if (dependentTask.ScheduledDate != null &&  dependentTask.ScheduledDate < _bl.Clock &&dependentTask.ScheduledDate!=DateTime.MinValue)
                        taskInList.Status = Status.InJeopardy;
                    if (dependentTask.StartDate != null && dependentTask.StartDate != DateTime.MinValue)
                        taskInList.Status = Status.OnTrack;
                    if (dependentTask.CompleteDate != null && dependentTask.CompleteDate != DateTime.MinValue)
                        taskInList.Status = Status.Done;

                    dependencies.Add(taskInList);
                }
            }
        }
        boTask.Dependencies = dependencies;
        return boTask;
    }

    public void StartTask(int id, int engid)
    {

        BO.Task task = DoBoAdapter(_dal.Task.Read(id)!);

        var lists = from item in _dal.Dependence.ReadAll()
                    where item.DependentTask == id
                    select DoBoAdapter(_dal.Task.Read(item.DependsOnTask)!).Status == Status.Done;

        if (lists.Any())
            throw new BlStartBeforeDependence("You want to start this task but she dependence on a task that have not done yet");



        if (task != null)
        {
            task.Engineer = new EngineerInTask { Id = engid };
            task.StartDate = _bl.Clock;
            _dal.Task.Update(BoDoAdapter(task));
        }

    }

    public void StopTask(int id)
    {
        BO.Task task = DoBoAdapter(_dal.Task.Read(id)!);
        if (task != null)
        {
            task.CompleteDate = _bl.Clock;
            _dal.Task.Update(BoDoAdapter(task));
        }
    }

    public bool CheckForCircles(int dependent, int dependOn)
    {
        // Check if there is a direct dependency from dependent to dependOn
        if (_dal.Dependence.Read(t => (t.DependentTask == dependOn && t.DependsOnTask == dependent)) != null)
        {
            return true;
        }

        // Check for indirect dependencies recursively
        foreach (var item in _dal.Dependence.ReadAll().Where(t => t.DependentTask == dependOn))
        {
            // Check for cycles recursively
            if (CheckForCircles(dependent, item.DependsOnTask))
            {
                return true; // Cycle detected
            }
        }

        return false; // No cycle detected
    }



    public DateTime? ForcastDateCalc(TimeSpan? req, DateTime? sched)
    {

        return sched + req;
    }

    public void AutoScheduleSystem(DateTime? ScheduleProjectDate)
    {
        DateTime date = _bl.Clock;
        DateTime roundedScheduleProjectDate = new DateTime(ScheduleProjectDate!.Value.Year, ScheduleProjectDate.Value.Month, ScheduleProjectDate.Value.Day, ScheduleProjectDate.Value.Hour, ScheduleProjectDate.Value.Minute, 0);


        if (ScheduleProjectDate == null || roundedScheduleProjectDate < date.Date)
            throw new BlStartProjectBeforeClock($"Schedule project start date cant be before date now ");


        Queue<DO.Task> queue = new Queue<DO.Task>();
        foreach (var item in _dal.Task.ReadAll())
        {
            queue.Enqueue(item);
        }

        while (queue.Count > 0)
        {
            DO.Task task = queue.Dequeue();
            if (!AutoScheduleDate(task, ScheduleProjectDate))
                queue.Enqueue(task);
        }
        _bl.setProjectStartDate(ScheduleProjectDate.Value); // set the project start date
        _bl.setProjectEndDate(LastEndTask());  // set the project end date


    }

    public bool AutoScheduleDate(DO.Task task, DateTime? ScheduleProjectDate)
    {
        DateTime? date = ScheduleProjectDate;

        if (task.Id == 26) { };

        foreach (DO.Dependence depend in _dal.Dependence.ReadAll())
        {
            if (depend.DependentTask == task.Id)
            {
                DO.Task temp = _dal.Task.Read(X => X.Id == depend.DependsOnTask)!;
                if (temp.ScheduledDate == DateTime.MinValue)
                {
                    return false;
                }
                if (ForcastDateCalc(temp.RequiredEffortTime, temp.ScheduledDate) > date)
                {
                    date = ForcastDateCalc(temp.RequiredEffortTime, temp.ScheduledDate);
                }
            }
        }

        _dal.Task.Update(task with { ScheduledDate = date });
        return true;
    }

    public DateTime LastEndTask()
    {
        //return the task that end last
        DateTime? date = DateTime.MinValue;
        DateTime? temp = DateTime.MinValue;

        foreach (var item in _dal.Task.ReadAll())
        {
            temp = ForcastDateCalc(item!.RequiredEffortTime, item.ScheduledDate);

            if (temp > date)
                date = temp;
        }
        return date.Value; // return the last end task

    }

    public bool CheckIfDepInJopardy(BO.Task task)
    {
        if (task.Dependencies.Count() == 0)
        {
            return false;
        }
        else
        {
            foreach (var item in task.Dependencies)
            {
                if (item.Status == BO.Status.InJeopardy)
                {
                    return true;
                }

                // Recursively check if dependency is in jeopardy
                if (CheckIfDepInJopardy(GetTask(item.Id)))
                {
                    return true; // If any dependency is in jeopardy, return true
                }
            }
        }
        return false;
    }

}

