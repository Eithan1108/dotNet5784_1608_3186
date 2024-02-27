
namespace BlImplementation;
using BO;
using System.Data;
using System.Linq;
using System.Net.Mail;


/// <summary>
/// // engineer implementation
/// </summary>
internal class EngineerImplementation : BlApi.IEngineer
{
    private DalApi.IDal _dal = Factory.Get; // get instance of DalApi.IDal


    /// <summary>
    /// // add engineer to the system
    /// </summary>
    /// <param name="engineer"></param>
    /// <returns></returns>
    /// <exception cref="BO.BlBadIdException"></exception>
    /// <exception cref="BO.BlBadNameException"></exception>
    /// <exception cref="BO.BlBadEmailException"></exception>
    /// <exception cref="BO.BlBadCostException"></exception>
    /// <exception cref="BO.BlAlreadyExistsException"></exception>
    public int AddEngineer(BO.Engineer engineer) 
    {
        if (engineer.Id <= 0 && engineer.Id is int)
            throw new BO.BlBadIdException("Id must be positive");
        if (engineer.Name == null)
            throw new BO.BlBadNameException("Name must be not null");
        if (engineer.Email == null)
            throw new BO.BlBadEmailException("Email must be not null");
        if (!IsEmailValid(engineer.Email)) // check if email is valid
            throw new BlBadEmailException("Email format is not valid");
        if (engineer.Cost <= 0)
            throw new BO.BlBadCostException("Cost must be positive");
        if(_dal.Engineer.Read(en => en.Id == engineer.Id) != null)
            throw new BlAlreadyExistsException($"Engineer with ID= {engineer.Id} already exsist");
        

        try
        {
            int engineerId = _dal.Engineer.Create(BoDoAdapter(engineer)); // create new DO.Engineer in the system
            return engineerId;
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"Engineer with ID= {engineer.Id} already exsist");
        }
    }

    /// <summary>
    /// // delete engineer from the system
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="BO.BlAlreadyExistsException"></exception>
    public void DeleteEngineer(int id)  
    {
        if (checkIfEngineerWorksOnTask(id) != null)
            throw new BO.BlAlreadyExistsException($"engineer with ID= {id} already works on task {checkIfEngineerWorksOnTask(id).Alias}");

        _dal.Engineer.Delete(id); // delete DO.Engineer from the system
    }

    /// <summary>
    /// // get engineer from the system
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    public BO.Engineer GetEngineer(int id) 
    {
        DO.Engineer? doEngineer = _dal.Engineer.Read(id); // get DO.Engineer from the system

        if (doEngineer is null)
            throw new BO.BlDoesNotExistException($"engineer with ID= {id} does not exsist");

        BO.Engineer boEngineer = DoBoAdapter(doEngineer, checkIfEngineerWorksOnTask(doEngineer.Id)); // create new BO.Engineer
        return boEngineer;
    }

    /// <summary>
    /// // get all engineers from the system
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public IEnumerable<BO.Engineer> GetEngineersList(Func<BO.Engineer, bool>? filter = null) 
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

    /// <summary>
    /// // update engineer in the system
    /// </summary>
    /// <param name="engineer"></param>
    /// <exception cref="BO.BlBadIdException"></exception>
    /// <exception cref="BO.BlBadNameException"></exception>
    /// <exception cref="BO.BlBadEmailException"></exception>
    /// <exception cref="BO.BlBadCostException"></exception>
    /// <exception cref="BlBadIdException"></exception>
    public void UpdateEngineer(BO.Engineer engineer) 
    {
        if (engineer.Id < 0)
            throw new BO.BlBadIdException("id must be positive");
        if (engineer.Name == null)
            throw new BO.BlBadNameException("name must be not null");
        if (engineer.Email == null)
            throw new BO.BlBadEmailException("email must be not null");
        if (!IsEmailValid(engineer.Email)) // check if email is valid
            throw new BlBadEmailException("Email format is not valid");
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

    /// <summary>
    /// // check if engineer works on task
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    private BO.TaskInEngineer? checkIfEngineerWorksOnTask(int id) 
    {
        var task = from item in _dal.Task.ReadAll()
                   where item.EngineerId == id && item.StartDate != null && item.CompleteDate == null // get all tasks that the engineer works on
                   select item;
        if (task.FirstOrDefault() is null)
            return null;
        return new TaskInEngineer { Id = task.FirstOrDefault()!.Id, Alias = task.FirstOrDefault()!.Alias };
    }

    /// <summary>
    /// // update tasks after changing engineer
    /// </summary>
    /// <param name="oldTask"></param>
    /// <param name="workingEngineerId"></param>
    private void updateTasksAfterChangingEngineer(DO.Task oldTask, int? workingEngineerId) 
    {
        DO.Task newTask = oldTask with { EngineerId = workingEngineerId }; 
        _dal.Task.Update(newTask);
    }

    /// <summary>
    /// // adapter from BO.Engineer to DO.Engineer
    /// </summary>
    /// <param name="engineer"></param>
    /// <returns></returns>
    /// <exception cref="BO.BlBadIdException"></exception>
    private DO.Engineer BoDoAdapter(BO.Engineer engineer) 
    {
        if (engineer.Id is null)

            throw new BO.BlBadIdException("id must be positive");

        DO.Engineer doEngineer = new DO.Engineer((int)engineer.Id!, engineer.Name, engineer.Email, (DO.EngineerExperience)engineer.Level, engineer.Cost); // create new DO.Engineer
        return doEngineer;
    }

    /// <summary>
    /// // adapter from DO.Engineer to BO.Engineer
    /// </summary>
    /// <param name="engineer"></param>
    /// <param name="taskInEngineer"></param>
    /// <returns></returns>
    private BO.Engineer DoBoAdapter(DO.Engineer engineer, BO.TaskInEngineer taskInEngineer)
    {
        BO.Engineer boEngineer = new BO.Engineer { Id = engineer.Id, Name = engineer.Name, Email = engineer.Email, Level = (BO.EngineerExperience)engineer.Level, Cost = engineer.Cost, Task = taskInEngineer }; // create new BO.Engineer
        return boEngineer;
    }

    private static bool IsEmailValid(string email)
    {
        try
        {
            MailAddress mail = new MailAddress(email);
            return true;
        }
        catch (FormatException)
        {
            return false;
        }
    } // check if email is valid in format xxxx@xxxx.xxxx

}
