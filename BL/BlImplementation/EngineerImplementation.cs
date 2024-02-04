
namespace BlImplementation;
using BLApi;
using BO;
using System.Linq;

internal class EngineerImplementation : IEngineer
{
    private DalApi.IDal dal_ = Factory.Get;

    public int AddEngineer(BO.Engineer engineer) // add engineer to the system
    {
        if (engineer.Id < 0)

            throw new BadIdException("id must be positive");

        if (engineer.Name == null)
            throw new BadNameException("name must be not null");
        if (engineer.Email == null)
            throw new BadEmailException("email must be not null");
        if (engineer.Cost <= 0)
            throw new BadCostException("cost must be positive");

        try
        {
            int engineerId = dal_.Engineer.Create(engineer.BoDoAdapter()); // create new DO.Engineer
            return engineerId;
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExsistExeption($"engineer with ID= {engineer.Id} already exsist");
        }
    }

    public void DeleteEngineer(int id) //done
    {
        if (checkIfEngineerWorksOnTask(id) != null)
            throw new BlAlreadyExsistExeption($"engineer with ID= {id} already works on task {checkIfEngineerWorksOnTask(id).Alias}");
        dal_.Engineer.Delete(id); // delete DO.Engineer from the system
    }

    public BO.Engineer GetEngineer(int id) //done
    {
        DO.Engineer? doEngineer = dal_.Engineer.Read(id); // get DO.Engineer from the system
        if (doEngineer is null)
            throw new BlNotFoundException($"engineer with ID= {id} does not exsist");

        //BO.Engineer boEngineer = new BO.Engineer { Id = doEngineer.Id, Name = doEngineer.Name, Email = doEngineer.Email, Level = (BO.EngineerExperience)doEngineer.Level, Cost = doEngineer.Cost }; // create new BO.Engineer
        BO.Engineer boEngineer = doEngineer.DoBoAdapter(checkIfEngineerWorksOnTask(doEngineer.Id));
        return boEngineer;
    }


    public IEnumerable<BO.Engineer> GetEngineersList(Func<DO.Engineer, bool> filter = null) // get all engineers from the system
    {
        IEnumerable<BO.Engineer> engineers = null;
        if (filter is null)
        {
            engineers = from item in dal_.Engineer.ReadAll()
                        select (item.DoBoAdapter(checkIfEngineerWorksOnTask(item.Id))); // create new BO.Engineer
        }
        else
        {
            engineers = from item in dal_.Engineer.ReadAll()
                        where filter(item)
                        select (item.DoBoAdapter(checkIfEngineerWorksOnTask(item.Id)));
        }
        return engineers;

    }

    public void UpdateEngineer(BO.Engineer engineer)
    {
        if (engineer.Id < 0)
            throw new BadIdException("id must be positive");
        if (engineer.Name == null)
            throw new BadNameException("name must be not null");
        if (engineer.Email == null)
            throw new BadEmailException("email must be not null");
        if (engineer.Cost < 0)
            throw new BadCostException("cost must be positive");
        DO.Engineer doEngineer = engineer.BoDoAdapter(); // create new DO.Engineer


        try
        {

            DO.Engineer? oldEngineer = dal_.Engineer.Read(doEngineer.Id); // get DO.Engineer from the system

            if ((DO.EngineerExperience)oldEngineer!.Level < doEngineer.Level)  // check if engineer level can be increased cannot be null
                throw new BlBadIdException("engineer level can not be decreased");


            dal_.Engineer.Update(doEngineer); // update DO.Engineer in the system

            var taskToMakeNull = from item in dal_.Task.ReadAll()
                                 where item.EngineerId == oldEngineer.Id
                                 select item;

            DO.Task? task = taskToMakeNull.FirstOrDefault();
            if (task == null) return;

            updateTasksAfterChangingEngineer(task, null);


            var newtask = from item in dal_.Task.ReadAll()
                          where item.Id == engineer.Task.Id
                          select item;
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
        var task = from item in dal_.Task.ReadAll()
                   where item.EngineerId == id
                   select item;
        if (task.FirstOrDefault() is null)
            return null;
        return new TaskInEngineer { Id = task.FirstOrDefault()!.Id, Alias = task.FirstOrDefault()!.Alias };
    }


    private void updateTasksAfterChangingEngineer(DO.Task oldTask, int? workingEngineerId)
    {
        DO.Task newTask = oldTask with { EngineerId = workingEngineerId };
        dal_.Task.Update(newTask);
    }

}
