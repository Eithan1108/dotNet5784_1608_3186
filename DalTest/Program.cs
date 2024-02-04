namespace DalTest;
using Dal;
using DalApi;
using DO;
using System.Diagnostics.Metrics;
using System.Reflection.Emit;
using System.Xml.Linq;
using System;
using System.Linq;

internal class Program
{
    /// <summary>
    /// Main program class
    /// </summary>
    ///
    static readonly string s_tasks_xml = "tasks";
    static readonly string s_engineers_xml = "engineers";
    static readonly string s_dependence_xml = "dependences";
    static readonly string s_config_xml = "data-config";
    static readonly IDal s_dal = Factory.Get;

    /// <summary>
    /// Entry point of the program
    /// </summary>
    /// <param name="args">Command line arguments</param>

    static void Main(string[] args)
    {
        try
        {
            int choices = 1;
            while (choices != 0)
            {
                choices = subMenu();
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    } 

    /// <summary>
    /// Displays the main menu and reads the user's choice
    /// </summary>
    /// <returns>User's choice</returns>
    private static int menu()
    {
        int choice;
        Console.WriteLine("0. Exit");
        Console.WriteLine("1. Engineer");
        Console.WriteLine("2. Task");
        Console.WriteLine("3. Dependence");
        Console.WriteLine("4. Initial data");
        choice = int.Parse(Console.ReadLine()!);
        return choice;
    }

    /// <summary>
    /// Displays the submenu based on the main menu choice
    /// </summary>
    /// <returns>User's choice</returns>
    private static int subMenu()
    {
        int choice;

        int menuChoice = menu();

        if (menuChoice == 0)
        {
            return 0;  // exit from program
        }

        if (menuChoice != 4)  // if not initial data dont offer submenu
        {
            Console.WriteLine("1. Exit");
            Console.WriteLine("2. Create");
            Console.WriteLine("3. Read");
            Console.WriteLine("4. Read All");
            Console.WriteLine("5. Update");
            Console.WriteLine("6. Delete");
            choice = int.Parse(Console.ReadLine()!);
        }
        else
        {
            choice = 4;  // if initial choice is garbage
        }

        int id = 0;
        string? name = null;
        string? email = null;
        double cost = 0;
        DO.EngineerExperience level = 0;

        string alias;
        string description;
        DateTime createDate;

        int dependenceId = 0;
        int dependeOn = 0;

        Task task;
        Engineer engineer;
        Dependence dependence;

        int print;

        switch (menuChoice)
        {
            // Manage Engineers
            case 1:

                switch (choice)
                {
                    case 1:
                        Console.WriteLine("Engineer");
                        break;
                    case 2:
                        readValuesForCreateEngineer(ref id, ref name!, ref email!, ref level, ref cost);  // function below

                        engineer = new Engineer(id, name, email, level, cost);
                        s_dal.Engineer?.Create(engineer);
                        Console.WriteLine(id);
                        break;
                    case 3:
                        Console.WriteLine("Enter id: ");
                        id = int.Parse(Console.ReadLine()!);
                        printEngineer(s_dal.Engineer!.Read(engineer => id == engineer.Id)!);
                        break;
                    case 4:
                        Console.WriteLine("Read All");
                        IEnumerable<Engineer?> engineers = s_dal.Engineer!.ReadAll();
                        foreach (var e in engineers)
                        {
                            printEngineer(e);
                        }
                        break;
                    case 5:
                        try
                        {
                            readValuesForUpdateEngineer(ref id, ref name!, ref email!, ref level, ref cost);  // function below

                            engineer = new Engineer(id, name, email, level, cost);
                            s_dal.Engineer?.Update(engineer);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;
                    case 6:
                        Console.WriteLine("Enter id: ");
                        id = int.Parse(Console.ReadLine()!);
                        try
                        {
                            s_dal.Engineer!.Delete(id);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;

                    default:
                        break;
                }
                break;
            case 2:
                // Manage Tasks
                switch (choice)
                {
                    case 1:
                        Console.WriteLine("Task");
                        break;
                    case 2:
                        Console.WriteLine("Enter alias: ");
                        alias = Console.ReadLine()!;
                        Console.WriteLine("Enter description: ");
                        description = Console.ReadLine()!;
                        Console.WriteLine("Enter createDate: ");
                        createDate = DateTime.Parse(Console.ReadLine()!);
                        Console.WriteLine("Enter level: ");
                        level = (DO.EngineerExperience)int.Parse(Console.ReadLine()!);

                        task = new Task(0, alias, description, false, createDate, level);
                        s_dal.Task?.Create(task);
                        Console.WriteLine(task.Id);

                        break;
                    case 3:
                        Console.WriteLine("Enter id: ");
                        id = int.Parse(Console.ReadLine()!);
                        printTask(s_dal.Task!.Read(task => id == task.Id)!);
                        break;
                    case 4:
                        Console.WriteLine("Read All");
                        IEnumerable<Task?> tesks = s_dal.Task!.ReadAll();
                        foreach (var t in tesks)
                        {
                            printTask(t);
                        }
                        break;
                    case 5:
                        try
                        {
                            Console.WriteLine("Enter id: ");
                            id = int.Parse(Console.ReadLine()!);
                            printTask(s_dal.Task!.Read(task => id == task.Id)!);

                            Console.WriteLine("Enter alias: ");
                            alias = Console.ReadLine()!;
                            if (alias == " ")
                                alias = s_dal.Task!.Read(task => id == task.Id)!.Alias;

                            Console.WriteLine("Enter description: ");
                            description = Console.ReadLine()!;
                            if (description == " ")
                                description = s_dal.Task!.Read(task => id == task.Id)!.Description;

                            Console.WriteLine("Enter createDate: ");
                            createDate = DateTime.Parse(Console.ReadLine()!);
                            Console.WriteLine("Enter level: ");
                            level = (DO.EngineerExperience)int.Parse(Console.ReadLine()!);

                            task = new Task(id, alias, description, false, createDate, level);
                            s_dal.Task?.Update(task);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;
                    case 6:
                        Console.WriteLine("Enter id: ");
                        id = int.Parse(Console.ReadLine()!);
                        try
                        {
                            s_dal.Task!.Delete(id);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;
                    default:
                        break;
                }
                break;
            case 3:
                // Manage Dependences
                switch (choice)
                {
                    case 1:
                        Console.WriteLine("Dependence");
                        break;
                    case 2:
                        readValuesForCreateDependence(ref dependenceId, ref dependeOn);  // function below

                        dependence = new Dependence(0, dependenceId, dependeOn);
                        Console.WriteLine(s_dal.Dependence?.Create(dependence));
                        break;
                    case 3:
                        Console.WriteLine("Enter id: ");
                        id = int.Parse(Console.ReadLine()!);
                        printDependence(s_dal.Dependence!.Read(dependence => id == dependence.Id)!);
                        break;
                    case 4:
                        Console.WriteLine("Read All");
                        IEnumerable<Dependence?> dependences = s_dal.Dependence!.ReadAll();
                        foreach (var d in dependences)
                        {
                            printDependence(d);
                        }
                        break;
                    case 5:
                        try
                        {
                            readValuesForUpdateDependence(ref id, ref dependenceId, ref dependeOn);  // function below
                            dependence = new Dependence(id, dependenceId, dependeOn);
                            s_dal.Dependence?.Update(dependence);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;
                    case 6:
                        Console.WriteLine("Enter id: ");
                        id = int.Parse(Console.ReadLine()!);
                        try
                        {
                            s_dal.Dependence!.Delete(id);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;
                    default:
                        break;
                }
                break;
            case 4:  // Initial data
                Console.Write("Would you like to create Initial data? (Y/N)");
                string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input");
                if (ans == "Y")
                {
                    // clear data
                    reSetXmlData();
                    // initialize data
                    Initialization.Do();
                }
                break;
            default:
                break;
        }
        return choice;
    }

    /// <summary>
    /// Prints details of an engineer
    /// </summary>
    /// <param name="engineer">Engineer object</param>
    public static void printEngineer(Engineer? engineer) 
    {
        if (engineer != null)
        {
            Console.Write("Id: " + engineer.Id + ", "); 
            Console.Write("Name: " + engineer.Name + ", ");
            Console.Write("Email: " + engineer.Email + ", ");
            Console.Write("Level: " + engineer.Level + ", ");
            Console.Write("Cost: " + engineer.Cost);
            Console.WriteLine();
        }
    }

    /// <summary>
    /// Prints details of a task
    /// </summary>
    /// <param name="task">Task object</param>
    public static void printTask(Task task)
    {
        Console.Write("Id: " + task.Id + ", ");
        Console.Write("Alias: " + task.Alias + ", ");
        Console.Write("Description: " + task.Description + ", ");
        Console.Write("IsMilestone: " + task.IsMilestone + ", ");
        Console.Write("CreateAtDate: " + task.CreateAtDate + ", ");
        Console.Write("Complexity: " + task.Complexity);
        Console.WriteLine();
    }

    /// <summary>
    /// Prints details of a dependence
    /// </summary>
    /// <param name="dependence">Dependence object</param>
    public static void printDependence(Dependence dependence)
    {
        Console.Write("Id: " + dependence.Id + ", ");
        Console.Write("DependentTask: " + dependence.DependentTask + ", ");
        Console.Write("DependsOnTask: " + dependence.DependsOnTask);
        Console.WriteLine();
    }

    /// <summary>
    /// Reads user input to set values for creating an Engineer.
    /// </summary>
    /// <param name="id">Reference to the Engineer's id</param>
    /// <param name="name">Reference to the Engineer's name</param>
    /// <param name="email">Reference to the Engineer's email</param>
    /// <param name="level">Reference to the Engineer's level</param>
    /// <param name="cost">Reference to the Engineer's cost</param>
    public static void readValuesForCreateEngineer(ref int id, ref string name, ref string email,
                                                   ref DO.EngineerExperience level, ref double cost)
    {
        Console.WriteLine("Enter id: ");
        id = int.Parse(Console.ReadLine()!);
        Console.WriteLine("Enter name: ");
        name = Console.ReadLine()!;
        Console.WriteLine("Enter email: ");
        email = Console.ReadLine()!;
        Console.WriteLine("Enter level: ");
        level = (DO.EngineerExperience)int.Parse(Console.ReadLine()!);
        Console.WriteLine("Enter cost: ");
        cost = double.Parse(Console.ReadLine()!);
    }

    /// <summary>
    /// Reads user input to set values for updating an Engineer.
    /// </summary>
    /// <param name="id">Reference to the Engineer's id</param>
    /// <param name="name">Reference to the Engineer's name</param>
    /// <param name="email">Reference to the Engineer's email</param>
    /// <param name="level">Reference to the Engineer's level</param>
    /// <param name="cost">Reference to the Engineer's cost</param>
    public static void readValuesForUpdateEngineer(ref int id, ref string name, ref string email,
                                                   ref DO.EngineerExperience level, ref double cost)
    {
        Console.WriteLine("Enter id: ");
        id = int.Parse(Console.ReadLine()!);
        int idVal =
            id;  // Because id is by ref we cant pass in read function so we created temp id by val

        // Print existing Engineer details before updating
        printEngineer(s_dal.Engineer!.Read(engineer => idVal == engineer.Id));

        Console.WriteLine("Enter name: ");
        name = Console.ReadLine()!;
        // If name is empty, retain the existing name
        if (string.IsNullOrWhiteSpace(name))
            name = s_dal.Engineer!.Read(engineer => idVal == engineer.Id)!.Name;

        Console.WriteLine("Enter email: ");
        email = Console.ReadLine()!;
        // If email is empty, retain the existing email
        if (string.IsNullOrWhiteSpace(email))
            email = s_dal.Engineer!.Read(engineer => idVal == engineer.Id)!.Email;

        Console.WriteLine("Enter level: ");
        level = (DO.EngineerExperience)int.Parse(Console.ReadLine()!);

        Console.WriteLine("Enter cost: ");
        cost = double.Parse(Console.ReadLine()!);
    }

    /// <summary>
    /// Reads user input to set values for creating a Dependence.
    /// </summary>
    /// <param name="dependenceId">Reference to the Dependence id</param>
    /// <param name="dependeOn">Reference to the Dependence on</param>
    public static void readValuesForCreateDependence(ref int dependenceId, ref int dependeOn)
    {
        Console.WriteLine("Enter task id: "); 
        dependenceId = int.Parse(Console.ReadLine()!);

        Console.WriteLine("Enter the task that its dependent on: ");
        dependeOn = int.Parse(Console.ReadLine()!);
    }

    /// <summary>
    /// Reads user input to set values for updating a Dependence.
    /// </summary>
    /// <param name="dependenceId">Reference to the Dependence id</param>
    /// <param name="dependeOn">Reference to the updated Dependence on</param>
    public static void readValuesForUpdateDependence(ref int id, ref int dependenceId,
                                                     ref int dependeOn)
    {
        Console.WriteLine("Enter id to update: ");
        id = int.Parse(Console.ReadLine()!);
        int idVal =
            id;  // Because id is by ref we cant pass in read function so we created temp id by val

        // Displaying information about the existing Dependence
        printDependence(s_dal.Dependence!.Read(dependenceId => idVal == dependenceId.Id)!);

        Console.WriteLine("Enter task id: ");
        dependenceId = int.Parse(Console.ReadLine()!);

        Console.WriteLine("Enter the task that its dependent on: ");
        dependeOn = int.Parse(Console.ReadLine()!);
    }

    public static void reSetXmlData()  // clear data
    {
        List<Engineer> engineersClear = new List<Engineer>();
        List<Dependence> dependenceClear = new List<Dependence>();
        List<Task> tasksClear = new List<Task>();
        XMLTools.SaveListToXMLSerializer<Engineer>(engineersClear, s_engineers_xml);
        XMLTools.SaveListToXMLSerializer<Task>(tasksClear, s_tasks_xml);
        XMLTools.SaveListToXMLSerializer<Dependence>(dependenceClear, s_dependence_xml);
        // initialize data conigfile
        XElement configRestart = XMLTools.LoadListFromXMLElement(s_config_xml);
        configRestart.Element("NextTaskId")!.Value = "1";
        configRestart.Element("NextDependenceId")!.Value = "1";
        XMLTools.SaveListToXMLElement(configRestart, s_config_xml);
    }
}

// Run example:
/*
 * 0. Exit
1. Engineer
2. Task
3. Dependence
4. Initial data
4
Would you like to create Initial data? (Y/N)Y
0. Exit
1. Engineer
2. Task
3. Dependence
4. Initial data
1
1. Exit
2. Create
3. Read
4. Read All
5. Update
6. Delete
4
Read All
Id: 236236145, Name: Maximilian M?ller, Email: mm?ller@gmail.com, Level: Beginner, Cost: 54
Id: 105650644, Name: Sophia Wagner, Email: swagner@gmail.com, Level: AdvancedBeginner, Cost: 156
Id: 161606668, Name: Leon Schmidt, Email: lschmidt@gmail.com, Level: Advanced, Cost: 326
Id: 365780839, Name: Emma Becker, Email: ebecker@gmail.com, Level: Beginner, Cost: 63
Id: 228603326, Name: Lukas Hoffmann, Email: lhoffmann@gmail.com, Level: Intermediate, Cost: 206
Id: 122195830, Name: Mia Schneider, Email: mschneider@gmail.com, Level: Beginner, Cost: 29
Id: 307035250, Name: Paul Fischer, Email: pfischer@gmail.com, Level: Advanced, Cost: 378
Id: 323532069, Name: Emily Weber, Email: eweber@gmail.com, Level: AdvancedBeginner, Cost: 169
Id: 307110840, Name: Jonas Richter, Email: jrichter@gmail.com, Level: Expert, Cost: 476
Id: 161724074, Name: Laura Keller, Email: lkeller@gmail.com, Level: Beginner, Cost: 45
0. Exit
1. Engineer
2. Task
3. Dependence
4. Initial data
1
1. Exit
2. Create
3. Read
4. Read All
5. Update
6. Delete
5
Enter id:
161606668
Id: 161606668, Name: Leon Schmidt, Email: lschmidt@gmail.com, Level: Advanced, Cost: 326
Enter name:
maoz
Enter email:

Enter level:
4
Enter cost:
74
0. Exit
1. Engineer
2. Task
3. Dependence
4. Initial data
1
1. Exit
2. Create
3. Read
4. Read All
5. Update
6. Delete
4
Read All
Id: 236236145, Name: Maximilian M?ller, Email: mm?ller@gmail.com, Level: Beginner, Cost: 54
Id: 105650644, Name: Sophia Wagner, Email: swagner@gmail.com, Level: AdvancedBeginner, Cost: 156
Id: 365780839, Name: Emma Becker, Email: ebecker@gmail.com, Level: Beginner, Cost: 63
Id: 228603326, Name: Lukas Hoffmann, Email: lhoffmann@gmail.com, Level: Intermediate, Cost: 206
Id: 122195830, Name: Mia Schneider, Email: mschneider@gmail.com, Level: Beginner, Cost: 29
Id: 307035250, Name: Paul Fischer, Email: pfischer@gmail.com, Level: Advanced, Cost: 378
Id: 323532069, Name: Emily Weber, Email: eweber@gmail.com, Level: AdvancedBeginner, Cost: 169
Id: 307110840, Name: Jonas Richter, Email: jrichter@gmail.com, Level: Expert, Cost: 476
Id: 161724074, Name: Laura Keller, Email: lkeller@gmail.com, Level: Beginner, Cost: 45
Id: 161606668, Name: maoz, Email: lschmidt@gmail.com, Level: Expert, Cost: 74
0. Exit
1. Engineer
2. Task
3. Dependence
4. Initial data
2
1. Exit
2. Create
3. Read
4. Read All
5. Update
6. Delete
6
Enter id:
3
0. Exit
1. Engineer
2. Task
3. Dependence
4. Initial data
2
1. Exit
2. Create
3. Read
4. Read All
5. Update
6. Delete
4
Read All
Id: 1, Alias: DRPS, Description: Design rocket propulsion system, IsMilestone: False, CreateAtDate:
23/01/2024 17:37:52, Complexity: Expert Id: 2, Alias: CWTT, Description: Conduct wind tunnel
testing, IsMilestone: False, CreateAtDate: 23/01/2024 17:37:52, Complexity: Expert Id: 4, Alias:
DSCA, Description: Develop spacecraft control algorithms, IsMilestone: False, CreateAtDate:
23/01/2024 17:37:52, Complexity: AdvancedBeginner Id: 5, Alias: TSNS, Description: Test spacecraft
navigation systems, IsMilestone: False, CreateAtDate: 23/01/2024 17:37:52, Complexity:
AdvancedBeginner Id: 6, Alias: IGNS, Description: Implement guidance and navigation software,
IsMilestone: False, CreateAtDate: 23/01/2024 17:37:52, Complexity: AdvancedBeginner Id: 7, Alias:
DSCS, Description: Design satellite communication systems, IsMilestone: False, CreateAtDate:
23/01/2024 17:37:52, Complexity: Beginner Id: 8, Alias: ICA, Description: Integrate communication
antennas, IsMilestone: False, CreateAtDate: 23/01/2024 17:37:52, Complexity: AdvancedBeginner Id: 9,
Alias: TSCS, Description: Test satellite communication signals, IsMilestone: False, CreateAtDate:
23/01/2024 17:37:52, Complexity: AdvancedBeginner Id: 10, Alias: C3DM, Description: Create 3D models
for space mission simulations, IsMilestone: False, CreateAtDate: 23/01/2024 17:37:52, Complexity:
Expert Id: 11, Alias: SST, Description: Simulate spacecraft trajectories, IsMilestone: False,
CreateAtDate: 23/01/2024 17:37:52, Complexity: Intermediate Id: 12, Alias: ASIC, Description:
Analyze structural integrity of spacecraft, IsMilestone: False, CreateAtDate: 23/01/2024 17:37:52,
Complexity: Expert Id: 13, Alias: DADS, Description: Develop automated docking systems, IsMilestone:
False, CreateAtDate: 23/01/2024 17:37:52, Complexity: Intermediate Id: 14, Alias: TDPIM,
Description: Test docking procedures in microgravity, IsMilestone: False, CreateAtDate: 23/01/2024
17:37:52, Complexity: Beginner Id: 15, Alias: IANSP, Description: Implement autonomous navigation
for space probes, IsMilestone: False, CreateAtDate: 23/01/2024 17:37:52, Complexity: Beginner Id:
16, Alias: TAISN, Description: Test AI-powered spacecraft navigation, IsMilestone: False,
CreateAtDate: 23/01/2024 17:37:52, Complexity: AdvancedBeginner Id: 17, Alias: DDSP, Description:
Design deployable solar panels, IsMilestone: False, CreateAtDate: 23/01/2024 17:37:52, Complexity:
Advanced Id: 18, Alias: TSPDM, Description: Test solar panel deployment mechanisms, IsMilestone:
False, CreateAtDate: 23/01/2024 17:37:52, Complexity: Expert Id: 19, Alias: DTCS, Description:
Develop thermal control systems, IsMilestone: False, CreateAtDate: 23/01/2024 17:37:52, Complexity:
Advanced Id: 20, Alias: TSTM, Description: Test spacecraft thermal management, IsMilestone: False,
CreateAtDate: 23/01/2024 17:37:52, Complexity: Beginner Id: 21, Alias: CCPFMF, Description: Create
contingency plans for mission failures, IsMilestone: False, CreateAtDate: 23/01/2024 17:37:52,
Complexity: Beginner Id: 22, Alias: CRASM, Description: Conduct risk analysis for space missions,
IsMilestone: False, CreateAtDate: 23/01/2024 17:37:52, Complexity: Beginner Id: 23, Alias: IERP,
Description: Implement emergency response protocols, IsMilestone: False, CreateAtDate: 23/01/2024
17:37:52, Complexity: Beginner Id: 24, Alias: DSHS, Description: Design space habitat systems,
IsMilestone: False, CreateAtDate: 23/01/2024 17:37:52, Complexity: Expert Id: 25, Alias: TLSSSSC,
Description: Test life support systems in simulated space conditions, IsMilestone: False,
CreateAtDate: 23/01/2024 17:37:52, Complexity: Advanced Id: 26, Alias: DREETE, Description: Develop
robotics for extraterrestrial exploration, IsMilestone: False, CreateAtDate: 23/01/2024 17:37:52,
Complexity: AdvancedBeginner Id: 27, Alias: TRSASE, Description: Test robotic systems in analog
space environments, IsMilestone: False, CreateAtDate: 23/01/2024 17:37:52, Complexity: Intermediate
Id: 28, Alias: OPGDSM, Description: Optimize power generation for deep space missions, IsMilestone:
False, CreateAtDate: 23/01/2024 17:37:52, Complexity: AdvancedBeginner Id: 29, Alias: TAPC,
Description: Test advanced propulsion concepts, IsMilestone: False, CreateAtDate: 23/01/2024
17:37:52, Complexity: Expert Id: 30, Alias: TAPSN, Description:  Test AI-powered spacecraft
navigation, IsMilestone: False, CreateAtDate: 23/01/2024 17:37:52, Complexity: Intermediate Id: 31,
Alias: TRSIASE, Description: Test robotic systems in analog space environments, IsMilestone: False,
CreateAtDate: 23/01/2024 17:37:52, Complexity: Expert 0. Exit
1. Engineer
2. Task
3. Dependence
4. Initial data
3
1. Exit
2. Create
3. Read
4. Read All
5. Update
6. Delete
4
Read All
Id: 1, DependentTask: 2, DependsOnTask: 1
Id: 2, DependentTask: 3, DependsOnTask: 1
Id: 3, DependentTask: 5, DependsOnTask: 4
Id: 4, DependentTask: 6, DependsOnTask: 4
Id: 5, DependentTask: 8, DependsOnTask: 7
Id: 6, DependentTask: 9, DependsOnTask: 7
Id: 7, DependentTask: 10, DependsOnTask: 1
Id: 8, DependentTask: 11, DependsOnTask: 10
Id: 9, DependentTask: 11, DependsOnTask: 1
Id: 10, DependentTask: 12, DependsOnTask: 10
Id: 11, DependentTask: 12, DependsOnTask: 1
Id: 12, DependentTask: 14, DependsOnTask: 13
Id: 13, DependentTask: 15, DependsOnTask: 12
Id: 14, DependentTask: 15, DependsOnTask: 10
Id: 15, DependentTask: 15, DependsOnTask: 11
Id: 16, DependentTask: 16, DependsOnTask: 15
Id: 17, DependentTask: 18, DependsOnTask: 17
Id: 18, DependentTask: 20, DependsOnTask: 19
Id: 19, DependentTask: 22, DependsOnTask: 21
Id: 20, DependentTask: 23, DependsOnTask: 21
Id: 21, DependentTask: 25, DependsOnTask: 24
Id: 22, DependentTask: 26, DependsOnTask: 24
Id: 23, DependentTask: 27, DependsOnTask: 26
Id: 24, DependentTask: 28, DependsOnTask: 11
Id: 25, DependentTask: 28, DependsOnTask: 10
Id: 26, DependentTask: 28, DependsOnTask: 1
Id: 27, DependentTask: 29, DependsOnTask: 28
Id: 28, DependentTask: 29, DependsOnTask: 11
Id: 29, DependentTask: 29, DependsOnTask: 10
Id: 30, DependentTask: 29, DependsOnTask: 1
Id: 31, DependentTask: 30, DependsOnTask: 29
Id: 32, DependentTask: 30, DependsOnTask: 28
Id: 33, DependentTask: 30, DependsOnTask: 11
Id: 34, DependentTask: 30, DependsOnTask: 10
Id: 35, DependentTask: 30, DependsOnTask: 1
Id: 36, DependentTask: 31, DependsOnTask: 29
Id: 37, DependentTask: 31, DependsOnTask: 28
Id: 38, DependentTask: 31, DependsOnTask: 11
Id: 39, DependentTask: 31, DependsOnTask: 10
Id: 40, DependentTask: 31, DependsOnTask: 1
0. Exit
1. Engineer
2. Task
3. Dependence
4. Initial data
3
1. Exit
2. Create
3. Read
4. Read All
5. Update
6. Delete
5
Enter id to update:
30
Id: 30, DependentTask: 29, DependsOnTask: 1
Enter task id:
35
Enter the task that its dependent on:
40
0. Exit
1. Engineer
2. Task
3. Dependence
4. Initial data
3
1. Exit
2. Create
3. Read
4. Read All
5. Update
6. Delete
4
Read All
Id: 1, DependentTask: 2, DependsOnTask: 1
Id: 2, DependentTask: 3, DependsOnTask: 1
Id: 3, DependentTask: 5, DependsOnTask: 4
Id: 4, DependentTask: 6, DependsOnTask: 4
Id: 5, DependentTask: 8, DependsOnTask: 7
Id: 6, DependentTask: 9, DependsOnTask: 7
Id: 7, DependentTask: 10, DependsOnTask: 1
Id: 8, DependentTask: 11, DependsOnTask: 10
Id: 9, DependentTask: 11, DependsOnTask: 1
Id: 10, DependentTask: 12, DependsOnTask: 10
Id: 11, DependentTask: 12, DependsOnTask: 1
Id: 12, DependentTask: 14, DependsOnTask: 13
Id: 13, DependentTask: 15, DependsOnTask: 12
Id: 14, DependentTask: 15, DependsOnTask: 10
Id: 15, DependentTask: 15, DependsOnTask: 11
Id: 16, DependentTask: 16, DependsOnTask: 15
Id: 17, DependentTask: 18, DependsOnTask: 17
Id: 18, DependentTask: 20, DependsOnTask: 19
Id: 19, DependentTask: 22, DependsOnTask: 21
Id: 20, DependentTask: 23, DependsOnTask: 21
Id: 21, DependentTask: 25, DependsOnTask: 24
Id: 22, DependentTask: 26, DependsOnTask: 24
Id: 23, DependentTask: 27, DependsOnTask: 26
Id: 24, DependentTask: 28, DependsOnTask: 11
Id: 25, DependentTask: 28, DependsOnTask: 10
Id: 26, DependentTask: 28, DependsOnTask: 1
Id: 27, DependentTask: 29, DependsOnTask: 28
Id: 28, DependentTask: 29, DependsOnTask: 11
Id: 29, DependentTask: 29, DependsOnTask: 10
Id: 30, DependentTask: 35, DependsOnTask: 40
Id: 31, DependentTask: 30, DependsOnTask: 29
Id: 32, DependentTask: 30, DependsOnTask: 28
Id: 33, DependentTask: 30, DependsOnTask: 11
Id: 34, DependentTask: 30, DependsOnTask: 10
Id: 35, DependentTask: 30, DependsOnTask: 1
Id: 36, DependentTask: 31, DependsOnTask: 29
Id: 37, DependentTask: 31, DependsOnTask: 28
Id: 38, DependentTask: 31, DependsOnTask: 11
Id: 39, DependentTask: 31, DependsOnTask: 10
Id: 40, DependentTask: 31, DependsOnTask: 1
0. Exit
1. Engineer
2. Task
3. Dependence
4. Initial data
0
 */