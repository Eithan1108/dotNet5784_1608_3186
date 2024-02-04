


namespace BlImplementation;
using BLApi;

internal class TaskImplementation : ITask
{
    private DalApi.IDal _dal = Factory.Get;

    public void AddOrUpdateSchedualeDateTine(int id, DateTime dateTime)
    {
        throw new NotImplementedException();
    }

    public int AddTask(BO.Task task) // add task to the system
    {
        if (task.Id < 0)
            throw new BadIdException("id must be positive");

        if (task.Alias == null)
            throw new BadAliasException("alias must be not null");

         
        try
        {
            task.Deoendencies.Select(d => _dal.Dependence.Create(new DO.Dependence { DependentTask = task.Id, DependsOnTask = d.Id })); // create all dependencies

            int taskId = _dal.Task.Create(BoDoAdapter(task));   
            return taskId;
        }

        catch(DO.DalAlreadyExistsException ex)
        {
            throw new BlAlreadyExsistExeption( "student with ID= {id} does not exsist");
        }

        
    }

    public void DeleteTask(int id)
    {
       _dal.Task.Delete(id); // delete task from the system
        ////////////////////////////////////////////////////////
    }

    public BO.Task GetTask(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<BO.Task> GetTasksList(Func<BO.Task, bool> filter) //taskinlist type according to Dan haracaz
    {
        throw new NotImplementedException();
    }

    public void UpdateTask(BO.Task task) // update task in the system
    {

        if (task.Id < 0)
            throw new BadIdException("id must be positive");

        if (task.Alias == null)
            throw new BadAliasException("alias must be not null");

        try
        {
            _dal.Task.Update(BoDoAdapter(task)); // update task in the system
        }
        catch (DalBadIdException ex)
        {
                throw new BlBadIdException( "student with ID= {id} does not exsist");
        }

    }

    private DO.Task BoDoAdapter(BO.Task task) // adapter from BO to DO
    {
        if (task.Id == null) throw new BadIdException("id must be positive");
        if (task.Engineer == null) throw new BadInputException("engineer must be positive");
        if (task.Complexity == null) throw new BadInputException("Complexity must be positive");

        DO.Task doTask = new DO.Task((int)task.Id!, task.Alias, task.Description, false, task.CreatedAtDate, (DO.EngineerExperience)task.Complexity,
            task.ScheduledDate, task.RequiredEffortTime, task.StartDate, task.CompleteDate, task.DeadLineDate, task.Deliverables,
            task.Remarks, task.Engineer.Id);

        return doTask;
    }


    
}

