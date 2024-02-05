
namespace BlImplementation;
using BO;
using System.Linq;

internal class EngineerImplementation : BlApi.IEngineer
{
    private DalApi.IDal _dal = Factory.Get; 
    public int AddEngineer(BO.Engineer engineer) // add engineer to the system
    {
        if (engineer.Id < 0)
            throw new BO.BlBadIdException("id must be positive");
        if (engineer.Name == null)
            throw new BO.BlBadNameException("name must be not null");
        if (engineer.Email == null)
            throw new BO.BlBadEmailException("email must be not null");
        if (engineer.Cost <= 0)
            throw new BO.BlBadCostException("cost must be positive");

        try
        {
            int engineerId = _dal.Engineer.Create(BoDoAdapter(engineer)); // create new DO.Engineer in the system
            return engineerId;
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"engineer with ID= {engineer.Id} already exsist");
        }
    }

    public void DeleteEngineer(int id) //delete engineer from the system 
    {
        if (checkIfEngineerWorksOnTask(id) != null)
            throw new BO.BlAlreadyExistsException($"engineer with ID= {id} already works on task {checkIfEngineerWorksOnTask(id).Alias}");

        _dal.Engineer.Delete(id); // delete DO.Engineer from the system
    }

    public BO.Engineer GetEngineer(int id) 
    {
        DO.Engineer? doEngineer = _dal.Engineer.Read(id); // get DO.Engineer from the system

        if (doEngineer is null)
            throw new BO.BlDoesNotExistException($"engineer with ID= {id} does not exsist");

        BO.Engineer boEngineer = DoBoAdapter(doEngineer, checkIfEngineerWorksOnTask(doEngineer.Id)); // create new BO.Engineer
        return boEngineer;
    }


    public IEnumerable<BO.Engineer> GetEngineersList(Func<BO.Engineer, bool>? filter = null) // get all engineers from the system
    {
        IEnumerable<BO.Engineer> engineers = null;

        if (filter is null)
        {
            engineers = from item in _dal.Engineer.ReadAll() // get all DO.Engineer from the system
                        select (DoBoAdapter(item, checkIfEngineerWorksOnTask(item.Id))); // create new BO.Engineer
        }
        else
        {
            engineers = from item in _dal.Engineer.ReadAll() 
                        where filter(DoBoAdapter(item, checkIfEngineerWorksOnTask(item.Id)))
                        select (DoBoAdapter(item, checkIfEngineerWorksOnTask(item.Id))); 
        }
        return engineers;

    }

    public void UpdateEngineer(BO.Engineer engineer) // update engineer in the system
    {
        if (engineer.Id < 0)
            throw new BO.BlBadIdException("id must be positive");
        if (engineer.Name == null)
            throw new BO.BlBadNameException("name must be not null");
        if (engineer.Email == null)
            throw new BO.BlBadEmailException("email must be not null");
        if (engineer.Cost < 0)
            throw new BO.BlBadCostException("cost must be positive");
        DO.Engineer doEngineer = BoDoAdapter(engineer); // create new DO.Engineer


        try
        {

            DO.Engineer? oldEngineer = _dal.Engineer.Read(doEngineer.Id); // get DO.Engineer from the system

            if ((DO.EngineerExperience)oldEngineer!.Level > doEngineer.Level)  // check if engineer level can be increased cannot be null
                throw new BlBadIdException("engineer level can not be decreased");


            _dal.Engineer.Update(doEngineer); // update DO.Engineer in the system

            var taskToMakeNull = from item in _dal.Task.ReadAll() 
                                 where item.EngineerId == oldEngineer.Id // get all tasks that the engineer works on
                                 select item;

            DO.Task? task = taskToMakeNull.FirstOrDefault(); // get first task that the engineer works on
            if (task == null) return;

            updateTasksAfterChangingEngineer(task, null); // update task in the system


            var newtask = from item in _dal.Task.ReadAll()
                          where item.Id == engineer.Task.Id
                          select item; // get all tasks that the engineer works on

            task = newtask.FirstOrDefault(); 
            if (task == null) return;

            updateTasksAfterChangingEngineer(task, engineer.Task.Id); 

        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BlBadIdException($"engineer with ID= {engineer.Id} does not exsist");
        }

    }

    private BO.TaskInEngineer? checkIfEngineerWorksOnTask(int id) // check if engineer works on task
    {
        var task = from item in _dal.Task.ReadAll()
                   where item.EngineerId == id
                   select item;
        if (task.FirstOrDefault() is null)
            return null;
        return new TaskInEngineer { Id = task.FirstOrDefault()!.Id, Alias = task.FirstOrDefault()!.Alias };
    }


    private void updateTasksAfterChangingEngineer(DO.Task oldTask, int? workingEngineerId) // update task after changing engineer
    {
        DO.Task newTask = oldTask with { EngineerId = workingEngineerId }; 
        _dal.Task.Update(newTask);
    }

    private DO.Engineer BoDoAdapter(BO.Engineer engineer) // adapter from BO.Engineer to DO.Engineer
    {
        if (engineer.Id is null)

            throw new BO.BlBadIdException("id must be positive");

        DO.Engineer doEngineer = new DO.Engineer((int)engineer.Id!, engineer.Name, engineer.Email, (DO.EngineerExperience)engineer.Level, engineer.Cost); // create new DO.Engineer
        return doEngineer;
    }

    private BO.Engineer DoBoAdapter(DO.Engineer engineer, BO.TaskInEngineer taskInEngineer)
    {
        BO.Engineer boEngineer = new BO.Engineer { Id = engineer.Id, Name = engineer.Name, Email = engineer.Email, Level = (BO.EngineerExperience)engineer.Level, Cost = engineer.Cost, Task = taskInEngineer }; // create new BO.Engineer
        return boEngineer;
    }

}
