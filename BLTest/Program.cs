namespace BLTest;
using Dal;
using BlApi;
using BO;
using System.Diagnostics.Metrics;
using System.Reflection.Emit;
using System.Xml.Linq;
using System;
using System.Linq;
using DalApi;
using System.Collections.Generic;
using DalTest;
using DO;
using System.Threading.Tasks;

internal class Program
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get;
    static readonly string s_tasks_xml = "tasks";
    static readonly string s_engineers_xml = "engineers";
    static readonly string s_dependence_xml = "dependences";
    static readonly string s_config_xml = "data-config";

    static void Main(string[] args)
    {
        resetDataOffer();

        int mainChoices;
        bool flagStarted = false;
        bool flagScheduleExsist = false; // flag to check if the schedule exsist
        if (s_bl.Looz.GetStartDate() != DateTime.MinValue&& s_bl.Looz.GetStartDate!=null)
            flagScheduleExsist = true;
        if (  s_bl.Looz.GetStartDate() < DateTime.Now)
        {
            mainChoices = 0;
            flagStarted = true;
        }
        else
        {
            if(flagScheduleExsist)
            {
                Console.WriteLine("The project has already scheduled, you can only create the dates");
                mainChoices = 0; // if the schedule exsist the user can only create the dates
            }
            else
            {
                mainChoices = menu();
            }
    
        }

        while (mainChoices != 0)
        {

            switch (mainChoices)
            {
                case 1:
                    int engineerChoices = engineerMenu();
                    while (engineerChoices != 6)
                    {
                        switch (engineerChoices)
                        {
                            case 1:
                                try
                                {
                                    addEngineerMain();
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                                break;
                            case 2:
                                try
                                {
                                    updateEngineerMain();
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                                break;
                            case 3:
                                try
                                {
                                    deleteEngineerMain();
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                                break;
                            case 4:
                                try
                                {
                                    readEngineerMain();
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                                break;
                            case 5:
                                try
                                {
                                    readAllEngineersMain();
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                                break;

                        }
                        engineerChoices = engineerMenu();
                    }
                    break;
                case 2:
                    int taskChoices = taskMenu();
                    while (taskChoices != 6)
                    {
                        switch (taskChoices)
                        {
                            case 1:
                                try
                                {
                                    addTaskMain();
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                                break;
                            case 2:
                                try
                                {
                                    updateTaskMain();
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                                break;
                            case 3:
                                try
                                {
                                    deleteTaskMain();
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                                break;
                            case 4:
                                try
                                {
                                    readTaskMain();
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                                break;
                            case 5:
                                try
                                {
                                    readAllTasksMain();
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                                break;
                        }
                        taskChoices = taskMenu();
                    }
                    break;
            }
            mainChoices = menu();
        }


        if (!flagStarted) {
            try
            {
                createSchedule();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        try
        {
            machingTasksAndEngineers();
        }
        catch (Exception)
        {

            throw;
        }
    }

    /// <summary>
    /// //initialization data offer
    /// </summary>
    private static void initializationDataOffer()
    {

            Console.Write("Would you like to create Initial data? (Y/N)");
            string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input");
            if (ans == "Y" || ans == "y")
            {
                Console.WriteLine("Loading...");
                // clear data
                reSetXmlData();
                // initialize data
                Initialization.Do();
            }
    }
    /// <summary>
    /// // reset xml data
    /// </summary>
    
    private static void resetDataOffer()
    {
        Console.Write("Would you like to reset the data? (Y/N)");
        string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input");
        if (ans == "Y" || ans == "y")
        {
            Console.WriteLine("Loading...");
            reSetXmlData();
        }
            initializationDataOffer();
    }
    public static void reSetXmlData()
    {
        List<DO.Engineer> engineersClear = new List<DO.Engineer>();
        List<Dependence> dependenceClear = new List<Dependence>();
        List<DO.Task> tasksClear = new List<DO.Task>();
        XMLTools.SaveListToXMLSerializer<DO.Engineer>(engineersClear, s_engineers_xml);
        XMLTools.SaveListToXMLSerializer<DO.Task>(tasksClear, s_tasks_xml);
        XMLTools.SaveListToXMLSerializer<Dependence>(dependenceClear, s_dependence_xml);
        // initialize data conigfile
        XElement configRestart = XMLTools.LoadListFromXMLElement(s_config_xml);
        configRestart.Element("NextTaskId")!.Value = "1";
        configRestart.Element("NextDependenceId")!.Value = "1";
        configRestart.Element("ProjectStartDate")!.Value = "";
        XMLTools.SaveListToXMLElement(configRestart, s_config_xml);
    }


    /// <summary>
    /// //menus
    /// </summary>
    /// <returns></returns>
    private static int menu()
    {
        int choice;
        Console.WriteLine("0. Exit (after exiting you will be pointed to create the schedule)");
        Console.WriteLine("1. Engineer menu");
        Console.WriteLine("2. Task menu");

        choice = int.Parse(Console.ReadLine()!);
        return choice;
    } // main menu
    private static int taskMenu()
    {
        int choice;
        Console.WriteLine("1. Add Task");
        Console.WriteLine("2. Update Task");
        Console.WriteLine("3. Delete Task");
        Console.WriteLine("4. Get Task");
        Console.WriteLine("5. Get All Tasks");
        Console.WriteLine("6. Return to main menu");

        choice = int.Parse(Console.ReadLine());
        if (choice < 1 || choice > 6)
            throw new ArgumentOutOfRangeException("choice must be between 1 and 6");
        return choice;

    } // task menu
    private static int engineerMenu()
    {
        int choice;
        Console.WriteLine("1. Add Engineer");
        Console.WriteLine("2. Update Engineer");
        Console.WriteLine("3. Delete Engineer");
        Console.WriteLine("4. Get Engineer");
        Console.WriteLine("5. Get All Engineers");
        Console.WriteLine("6. Return to main menu");

        choice = int.Parse(Console.ReadLine());
        if (choice < 1 || choice > 6)
            throw new ArgumentOutOfRangeException("choice must be between 1 and 6");
        return choice;
    } // engineer menu


    /// <summary>
    /// // create schedule functions
    /// </summary>
    /// <exception cref="Exception"></exception>
    private static void createSchedule()
    {
        Console.WriteLine("How would you like to create the schedule? manualy or aoutomatily? (m/a)");
        string choice = Console.ReadLine()!;
        if (choice == "m")
        {
            createScheduleManually();
        }
        else if (choice == "a")
        {
            createScheduleAutomatically();
        }
        else
        {
            throw new Exception("Wrong input");
        }
    } // create schedule
    private static void createScheduleManually()
    {
        Console.WriteLine("Enter the date you want to start the schedule (dd/mm/yyyy): ");
        DateTime startDate = DateTime.Parse(Console.ReadLine()!);


        while ((s_bl.Task.GetTasksList(t => t.ScheduledDate == DateTime.MinValue).Count()) > 0)
        {
            Console.WriteLine("Still got " + s_bl.Task.GetTasksList(t => t.ScheduledDate == DateTime.MinValue).Count() + " schedule dates to set.");
            try
            {
                Console.WriteLine("Enter the task id you want to set scheduled date for: ");
                int taskId = int.Parse(Console.ReadLine()!);
                Console.WriteLine("Enter the schedule date for task: " + s_bl.Task.GetTask(taskId)!.Description + " (dd/mm/yyyy): ");
                DateTime updatedScheduleDate = DateTime.Parse(Console.ReadLine()!);
                s_bl.Task.AddOrUpdateSchedualeDateTine(taskId, updatedScheduleDate, startDate);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        s_bl.Looz.SetStartDate(startDate);


    } // create schedule manually
    private static void createScheduleAutomatically()
    {
        Console.WriteLine("Enter the date you want to start the schedule (dd/mm/yyyy): ");
        DateTime startDate = DateTime.Parse(Console.ReadLine()!);
        DateTime? dateTime = startDate;
        DateTime? compareDate = startDate;
        foreach (var item in s_bl.Task.GetTasksList(null))
        {
            if (item.Dependencies is null) //if the task has no dependencies it will start at the start date
            {
                dateTime = startDate;
            }
            else
            {
                foreach (var dependnce in item.Dependencies)
                {
                    if (s_bl.Task.GetTask(dependnce.Id).ForecastDate > compareDate) //if the task has dependencies it will start after the last dependency
                    {
                        compareDate = s_bl.Task.GetTask(dependnce.Id).DeadLineDate;
                    }
                }
                dateTime = compareDate;
            }
            s_bl.Task.AddOrUpdateSchedualeDateTine(item.Id, dateTime, startDate);
        }
    } // create schedule automatically


    /// <summary>
    /// //main functions
    /// </summary>
    /// <returns></returns>
    private static int addTaskMain() // add task to the system
    {
        string? description;
        string? alias;
        string? remarks;
        BO.EngineerExperience complexity;
        string? deliverables;


        Console.WriteLine("Enter description: ");
        description = Console.ReadLine();

        Console.WriteLine("Enter alias: ");
        alias = Console.ReadLine();

        Console.WriteLine("Enter required effort time (in days): ");
        int requiredEffortTime = int.Parse(Console.ReadLine());
        TimeSpan _RequiredEffortTime = TimeSpan.FromDays(requiredEffortTime);


        Console.WriteLine("Enter remarks: ");
        remarks = Console.ReadLine();

        Console.WriteLine("Enter complexity: ");
        complexity = (BO.EngineerExperience)Enum.Parse(typeof(BO.EngineerExperience), Console.ReadLine());

        Console.WriteLine("Enter deliverables: ");
        deliverables = Console.ReadLine();

        string depenedenciesChoice;
        Console.WriteLine("Does your task depend on other tasks? (y/n): ");
        depenedenciesChoice = Console.ReadLine()!;
        List<TaskInList> depend = new List<TaskInList>();

        if (depenedenciesChoice == "y")
        {
            Console.WriteLine("Enter the id's of the taskes which your new one dependes on (enter -1 to stop inserting id's): ");
            int dependId = int.Parse(Console.ReadLine()!);

            while (dependId != -1)
            {
                BO.Task dependencTask = s_bl.Task.GetTask(dependId);
                depend.Add(new TaskInList { Id = dependId, Alias = dependencTask.Alias, Description = dependencTask.Description });
                dependId = int.Parse(Console.ReadLine()!);

            }
        }

        BO.Task newTask = new BO.Task // create new task
        {
            Description = description,
            Alias = alias,
            RequiredEffortTime = _RequiredEffortTime,
            Remarks = remarks,
            Complexity = (BO.EngineerExperience)complexity,
            Deliverables = deliverables,
            Dependencies = depend,
            CreatedAtDate = DateTime.Now
        };

        s_bl.Task.AddTask(newTask);

        return newTask.Id;

    }
    private static void deleteTaskMain() // update task in the system
    {
        Console.WriteLine("Enter id to delete: ");
        int id = int.Parse(Console.ReadLine()!);
        s_bl.Task.DeleteTask(id);
    }
    private static void updateTaskMain() // update task in the system
    {
        int id;
        string alias;
        string description;
        BO.EngineerExperience level;
        string? remarks;
        string? deliverables;


        Console.WriteLine("Enter id: ");
        id = int.Parse(Console.ReadLine()!);

        Console.WriteLine("Enter alias: ");
        alias = Console.ReadLine()!;
        if (alias == " ")
            alias = s_bl.Task!.GetTask(id)!.Alias;


        Console.WriteLine("Enter description: ");
        description = Console.ReadLine()!;
        if (description == " ")
            description = s_bl.Task!.GetTask(id)!.Description;


        Console.WriteLine("Enter required effort time (in days, if you dont want to change put 0): ");
        int requiredEffortTime = int.Parse(Console.ReadLine());
        TimeSpan? _RequiredEffortTime;
        if (requiredEffortTime is 0)
            _RequiredEffortTime = s_bl.Task!.GetTask(id)!.RequiredEffortTime;
        else
            _RequiredEffortTime = TimeSpan.FromDays(requiredEffortTime);


        Console.WriteLine("Enter level: ");
        level = (BO.EngineerExperience)Enum.Parse(typeof(BO.EngineerExperience), Console.ReadLine()!);

        Console.WriteLine("Enter remarks: ");
        remarks = Console.ReadLine()!;
        if (remarks == " ")
            remarks = s_bl.Task!.GetTask(id)!.Remarks;

        Console.WriteLine("Enter deliverables: ");
        deliverables = Console.ReadLine()!;
        if (deliverables == " ")
            deliverables = s_bl.Task!.GetTask(id)!.Deliverables;

        Console.WriteLine("Do you want to update your dependenc (will restart all the depndence list in this task)? (y/n)");
        string depenedenciesChoice = Console.ReadLine()!;
        IEnumerable<TaskInList> depend = new List<TaskInList>();
        if (depenedenciesChoice == "y")
        {
            s_bl.Task.GetTasksList(null);
            Console.WriteLine("Enter the id's of the taskes wich your new one dependes on (enter -1 to stop inserting id's): ");
            int dependId = int.Parse(Console.ReadLine()!);

            while (dependId != -1)
            {

                BO.Task dependencTask = s_bl.Task.GetTask(dependId);
                depend = new List<TaskInList>();
                depend.Append(new TaskInList { Id = dependId, Alias = dependencTask.Alias, Description = dependencTask.Description });
                dependId = int.Parse(Console.ReadLine()!);
            }
        }
        else
        {
            depend = s_bl.Task.GetTask(id)!.Dependencies;
        }


        // build new task with the new details
        BO.Task newTask = new BO.Task
        {
            Id = id,
            Alias = alias,
            Description = description,
            Complexity = level,
            RequiredEffortTime = _RequiredEffortTime,
            Deliverables = deliverables,
            Remarks = remarks,
            Dependencies = depend,
        };

        s_bl.Task.UpdateTask(newTask);

    }
    private static void readAllTasksMain() // get all tasks from the system TODO
    {
        Console.WriteLine("Enter filter: ");
        Console.WriteLine("Enter 0 to get all tasks");
        Console.WriteLine("Enter 1 to filter by engineer level");
        Console.WriteLine("Enter 2 to filter by status");

        int filter = int.Parse(Console.ReadLine()!);
        if (filter == 0)
        {
            foreach (var item in s_bl.Task.GetTasksList(null))
            {
                Console.WriteLine(item);
            }
        }
        else if (filter == 1)
        {
            Console.WriteLine("Enter level: ");
            BO.EngineerExperience level = (BO.EngineerExperience)Enum.Parse(typeof(BO.EngineerExperience), Console.ReadLine()!);
            foreach (var item in s_bl.Task.GetTasksList(task => task.Complexity == level))
            {
                Console.WriteLine(item);
            }

        }
        else if (filter == 2)
        {
            Console.WriteLine("Enter status: ");
            BO.Status status = (BO.Status)Enum.Parse(typeof(BO.Status), Console.ReadLine()!);
            foreach (var item in s_bl.Task.GetTasksList(task => task.Status == status))
            {
                Console.WriteLine(item);
            }
        }
    }
    private static void readTaskMain() // get task from the system
    {
        Console.WriteLine("Enter id: ");
        int id = int.Parse(Console.ReadLine()!);
        Console.WriteLine(s_bl.Task.GetTask(id));
    }
    private static int addEngineerMain()
    {
        int? Id;
        string name;
        string email;
        BO.EngineerExperience level;
        double cost;

        Console.WriteLine("Enter id: ");
        Id = int.Parse(Console.ReadLine());

        Console.WriteLine("Enter name: ");
        name = Console.ReadLine();

        Console.WriteLine("Enter email: ");
        email = Console.ReadLine();

        Console.WriteLine("Enter level (0-4): ");
        level = (BO.EngineerExperience)Enum.Parse(typeof(BO.EngineerExperience), Console.ReadLine());

        Console.WriteLine("Enter cost: ");
        cost = double.Parse(Console.ReadLine());

        BO.Engineer newEngineer = new BO.Engineer // create new engineer
        {
            Id = Id,
            Name = name,
            Email = email,
            Level = level,
            Cost = cost
        };
        s_bl.Engineer.AddEngineer(newEngineer); // add engineer to the system

        return newEngineer.Id ?? 0;
    } // add engineer to the system
    private static void deleteEngineerMain() // delete engineer from the system
    {
        Console.WriteLine("Enter id to delete: ");
        int id = int.Parse(Console.ReadLine()!);
        s_bl.Engineer.DeleteEngineer(id);
    }
    private static void updateEngineerMain()
    {

        int id;
        string name;
        string email;
        BO.EngineerExperience level;
        double cost;

        Console.WriteLine("Enter id: ");
        id = int.Parse(Console.ReadLine()!);

        Console.WriteLine("Enter name: ");
        name = Console.ReadLine()!;
        if (name == " ")
            name = s_bl.Engineer!.GetEngineer(id)!.Name;

        Console.WriteLine("Enter email: ");
        email = Console.ReadLine()!;
        if (email == " ")
            email = s_bl.Engineer!.GetEngineer(id)!.Email;

        Console.WriteLine("Enter level: ");
        level = (BO.EngineerExperience)Enum.Parse(typeof(BO.EngineerExperience), Console.ReadLine()!);

        Console.WriteLine("Enter cost: ");
        cost = double.Parse(Console.ReadLine()!);

        BO.Engineer newEngineer = new BO.Engineer
        {
            Id = id,
            Name = name,
            Email = email,
            Level = level,
            Cost = cost
        };
        s_bl.Engineer.UpdateEngineer(newEngineer);
    } //update engineer in the system
    private static void readAllEngineersMain() // get all engineers from the system 
    {
        Console.WriteLine("Enter filter: ");
        Console.WriteLine("Enter 0 to get all engineers");
        Console.WriteLine("Enter 1 to filter by engineer level");
        Console.WriteLine("Enter 2 to get only engineers with no tasks");
        int filter = int.Parse(Console.ReadLine()!);
        if (filter == 0)
        {
            foreach (var item in s_bl.Engineer.GetEngineersList(null))
            {
                Console.WriteLine(item);
            }
        }
        else if (filter == 1)
        {
            Console.WriteLine("Enter level: ");
            BO.EngineerExperience level = (BO.EngineerExperience)Enum.Parse(typeof(BO.EngineerExperience), Console.ReadLine()!);
            foreach (var item in s_bl.Engineer.GetEngineersList(engineer => engineer.Level == level))
            {
                Console.WriteLine(item);
            }

        }
        else if (filter == 2)
        {
            foreach (var item in s_bl.Engineer.GetEngineersList(engineer => engineer.Task == null))
            {
                Console.WriteLine(item);
            }
        }
    }
    private static void readEngineerMain() // get engineer from the system
    {
        Console.WriteLine("Enter id: ");
        int id = int.Parse(Console.ReadLine()!);
        Console.WriteLine(s_bl.Engineer.GetEngineer(id));
    }


    /// Maching tasks and engineers
    public static void machingTasksAndEngineers()
    {
        Console.WriteLine("Would you like to mach between tasks and engineers? (y/n)");
        Console.WriteLine("Insert stop to stop maching");
        string? ans = Console.ReadLine();
        while (ans != "stop")
        {
            try
            {
                if (ans == "Y" || ans == "y")
                {
                    Console.WriteLine("Would ypi like to pair engineer to task or task to engineer? (0 - engineer for task, 1 - task for engineer)");
                    string? ans2 = Console.ReadLine();
                    if (ans2 == "0")
                    {
                        PairEngineerToTask();
                    }
                    else if (ans2 == "1")
                    {
                        PairTaskToEngineer();
                    }
                    else
                    {
                        throw new FormatException("Wrong input");
                    }
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            ans = Console.ReadLine();
        }
    }
    public static void PairEngineerToTask()
    {
        Console.WriteLine("Enter the id of the task you want to pair with an engineer: ");
        int taskId = int.Parse(Console.ReadLine()!);
        Console.WriteLine("Enter the id of the engineer you want to pair with the task: ");
        int engineerId = int.Parse(Console.ReadLine()!);
        BO.Engineer engineer = s_bl.Engineer.GetEngineer(engineerId);
        BO.Task task = s_bl.Task.GetTask(taskId);
        task.Engineer = new EngineerInTask { Id = engineer.Id, Name = engineer.Name };
        s_bl.Task.UpdateTask(task);
    }
    public static void PairTaskToEngineer()
    {
        Console.WriteLine("Enter the id of the engineer you want to pair with a task: ");
        int engineerId = int.Parse(Console.ReadLine()!);
        Console.WriteLine("Enter the id of the task you want to pair with the engineer: ");
        int taskId = int.Parse(Console.ReadLine()!);
        BO.Engineer engineer = s_bl.Engineer.GetEngineer(engineerId);
        BO.Task task = s_bl.Task.GetTask(taskId);
        task.Engineer = new EngineerInTask { Id = engineer.Id, Name = engineer.Name };
        s_bl.Task.UpdateTask(task);
    }

}
