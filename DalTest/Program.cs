namespace DalTest;
using Dal;
using DalApi;
using DO;



    internal class Program
    {
    /// <summary>
    /// Main program class
    /// </summary>
    /// 
        private static IEngineer? s_dalEngineer = new EngineerImplementation();
        private static ITask? s_dalTask = new TaskImplementation();
        private static IDependence? s_dalDependence = new DependenceImplementation()
        ;
    /// <summary>
    /// Entry point of the program
    /// </summary>
    /// <param name="args">Command line arguments</param>

    static void Main(string[] args)
        {
            try 
            { 
                Initialization.Do(s_dalEngineer, s_dalDependence, s_dalTask);
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
            return 0; // exit from program
        }

            Console.WriteLine("1. Exit");
            Console.WriteLine("2. Create");
            Console.WriteLine("3. Read");
            Console.WriteLine("4. Read All");
            Console.WriteLine("5. Update");
            Console.WriteLine("6. Delete");
            choice = int.Parse(Console.ReadLine()!);

            
            
            int id=0;
            string? name=null;
            string? email=null;
            double cost=0;
            DO.EngineerExperience level=0;


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
                        readValuesForCreateEngineer(ref id, ref name!, ref email!, ref level, ref cost); //function below

                            engineer = new Engineer(id, name, email, level, cost);
                            s_dalEngineer?.Create(engineer);
                            Console.WriteLine(id);
                            break;
                        case 3:
                            Console.WriteLine("Enter id: ");
                            id = int.Parse(Console.ReadLine()!);
                            printEngineer(s_dalEngineer!.Read(id)!);
                            break;
                        case 4:
                            Console.WriteLine("Read All");
                            List<Engineer> engineers =  s_dalEngineer!.ReadAll();
                            foreach (var e in engineers)
                            {
                                printEngineer(e);
                            
                        }
                            break;
                        case 5:
                            try
                            {
                            readValuesForUpdateEngineer(ref id, ref name!, ref email!, ref level, ref cost); //function below

                            engineer = new Engineer(id, name, email, level, cost);
                                s_dalEngineer?.Update(engineer);
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
                                s_dalEngineer!.Delete(id);
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
                            Console.WriteLine("Enter id: ");
                            id = int.Parse(Console.ReadLine()!);
                            Console.WriteLine("Enter alias: ");
                            alias = Console.ReadLine()!;
                            Console.WriteLine("Enter description: ");
                            description = Console.ReadLine()!;
                            Console.WriteLine("Enter createDate: ");
                            createDate = DateTime.Parse(Console.ReadLine()!);
                            Console.WriteLine("Enter level: ");
                            level = (DO.EngineerExperience)int.Parse(Console.ReadLine()!);

                            task = new Task(id, alias, description, false, createDate, level);
                            s_dalTask?.Create(task);
                            Console.WriteLine(task.Id);

                            break;
                        case 3:
                            Console.WriteLine("Enter id: ");
                            id = int.Parse(Console.ReadLine()!);
                            printTask(s_dalTask!.Read(id)!);
                            break;
                        case 4:
                            Console.WriteLine("Read All");
                            List<Task> tesks = s_dalTask!.ReadAll();
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
                                printTask(s_dalTask!.Read(id)!);

                                Console.WriteLine("Enter alias: ");
                                alias = Console.ReadLine()!;
                                if (alias == " ")
                                    alias = s_dalTask!.Read(id)!.Alias;

                                Console.WriteLine("Enter description: ");
                                description = Console.ReadLine()!;
                                if (description == " ")
                                    description = s_dalTask!.Read(id)!.Description;

                                Console.WriteLine("Enter createDate: ");
                                createDate = DateTime.Parse(Console.ReadLine()!);
                                Console.WriteLine("Enter level: ");
                                level = (DO.EngineerExperience)int.Parse(Console.ReadLine()!);

                                task = new Task(id, alias, description, false, createDate, level);
                                s_dalTask?.Update(task);
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
                                s_dalTask!.Delete(id);
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
                        readValuesForCreateDependence(ref dependenceId, ref dependeOn); //function below

                        dependence = new Dependence(0, dependenceId, dependeOn);
                            s_dalDependence?.Create(dependence);
                            Console.WriteLine(dependence.Id);
                            break;
                        case 3:
                            Console.WriteLine("Enter id: ");
                            id = int.Parse(Console.ReadLine()!);
                            printDependence(s_dalDependence!.Read(id)!);
                            break;
                        case 4:
                            Console.WriteLine("Read All");
                            List<Dependence> dependences = s_dalDependence!.ReadAll();
                            foreach (var d in dependences)
                            {
                                printDependence(d);               
                        }
                            break;
                        case 5:
                            try
                            {
                            readValuesForUpdateDependence(ref id, ref dependenceId, ref dependeOn); //function below
                            dependence = new Dependence(id, dependenceId, dependeOn);
                                s_dalDependence?.Update(dependence);
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
                                s_dalDependence!.Delete(id);
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
        Console.Write("IsMilestone: " + task.IsMilestone + ", " );
        Console.Write("CreateAtDate: " + task.CreateAtDate + ", ");
        Console.Write("Complexity: " + task.Complexity);
        Console.WriteLine(  );
    }

    /// <summary>
    /// Prints details of a dependence
    /// </summary>
    /// <param name="dependence">Dependence object</param>
    public static void printDependence(Dependence dependence)
    {
        Console.Write("Id: " + dependence.Id +  ", ");
        Console.Write("DependentTask: " + dependence.DependentTask +  ", ");
        Console.Write("DependsOnTask: " + dependence.DependsOnTask);
        Console.WriteLine( );
    }

    /// <summary>
    /// Reads user input to set values for creating an Engineer.
    /// </summary>
    /// <param name="id">Reference to the Engineer's id</param>
    /// <param name="name">Reference to the Engineer's name</param>
    /// <param name="email">Reference to the Engineer's email</param>
    /// <param name="level">Reference to the Engineer's level</param>
    /// <param name="cost">Reference to the Engineer's cost</param>
    public static void readValuesForCreateEngineer(ref int id, ref string name, ref string email, ref DO.EngineerExperience level, ref double cost)
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
    public static void readValuesForUpdateEngineer(ref int id, ref string name, ref string email, ref DO.EngineerExperience level, ref double cost)
    {
        Console.WriteLine("Enter id: ");
        id = int.Parse(Console.ReadLine()!);

        // Print existing Engineer details before updating
        printEngineer(s_dalEngineer!.Read(id));

        Console.WriteLine("Enter name: ");
        name = Console.ReadLine()!;
        // If name is empty, retain the existing name
        if (string.IsNullOrWhiteSpace(name))
            name = s_dalEngineer!.Read(id)!.Name;

        Console.WriteLine("Enter email: ");
        email = Console.ReadLine()!;
        // If email is empty, retain the existing email
        if (string.IsNullOrWhiteSpace(email))
            email = s_dalEngineer!.Read(id)!.Email;

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
        Console.WriteLine("Enter Dependence id: ");
        dependenceId = int.Parse(Console.ReadLine()!);

        Console.WriteLine("Enter Dependence on: ");
        dependeOn = int.Parse(Console.ReadLine()!);
    }

    /// <summary>
    /// Reads user input to set values for updating a Dependence.
    /// </summary>
    /// <param name="dependenceId">Reference to the Dependence id</param>
    /// <param name="dependeOn">Reference to the updated Dependence on</param>
    public static void readValuesForUpdateDependence(ref int id, ref int dependenceId, ref int dependeOn)
    {
        Console.WriteLine("Enter id to update: ");
        id = int.Parse(Console.ReadLine()!);

        // Displaying information about the existing Dependence
        printDependence(s_dalDependence!.Read(id)!);

        Console.WriteLine("Enter Dependence id: ");
        dependenceId = int.Parse(Console.ReadLine()!);

        Console.WriteLine("Enter updated Dependence on: ");
        dependeOn = int.Parse(Console.ReadLine()!);
    }


}
