namespace DalTest;
using Dal;
using DalApi;
using DO;



    internal class Program
    {

        private static IEngineer? s_dalEngineer = new EngineerImplementation();
        private static ITask? s_dalTask = new TaskImplementation();
        private static IDependence? s_dalDependence = new DependenceImplementation()
        ;

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

        private static int subMenu()
        {
            int choice;
            int menuChoice = menu();

            Console.WriteLine("1. Exit");
            Console.WriteLine("2. Create");
            Console.WriteLine("3. Read");
            Console.WriteLine("4. Read All");
            Console.WriteLine("5. Update");
            Console.WriteLine("6. Delete");
            choice = int.Parse(Console.ReadLine()!);

            
            
            int id;
            string name;
            string email;
            double cost;
            DO.EngineerExperience level;


            string alias;
            string description;
            DateTime createDate;

            int dependenceId;
            int dependeOn;

            Task task;
            Engineer engineer;
            Dependence dependence;


            int print;
            


            switch (menuChoice)
            {
                case 1:
                    
                    switch (choice)
                    {
                        case 1:
                            Console.WriteLine("Engineer");
                            break;
                        case 2:
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
                                Console.WriteLine("Enter id: ");
                                id = int.Parse(Console.ReadLine()!);

                                printEngineer(s_dalEngineer!.Read(id)!);



                                Console.WriteLine("Enter name: ");
                                name = Console.ReadLine()!;
                                if (name == " ")
                                    name = s_dalEngineer!.Read(id)!.Name;

                                Console.WriteLine("Enter email: ");
                                email = Console.ReadLine()!;
                                if (email == " ")
                                    email = s_dalEngineer!.Read(id)!.Email;

                                Console.WriteLine("Enter level: ");
                                level = (DO.EngineerExperience)int.Parse(Console.ReadLine()!);



                                Console.WriteLine("Enter cost: ");
                                cost = double.Parse(Console.ReadLine()!);


                                engineer = new Engineer(id, name, name, level, cost);
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
                    switch (choice)
                    {
                        case 1:
                            Console.WriteLine("Dependence");
                            break;
                        case 2:

                            Console.WriteLine("Enter Dependece id: ");
                            dependenceId = int.Parse(Console.ReadLine()!);
                            Console.WriteLine("Enter Dependece on: ");
                            dependeOn = int.Parse(Console.ReadLine()!);
                            
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
                                Console.WriteLine("Enter Dependece id: ");
                                dependenceId = int.Parse(Console.ReadLine()!);
                                printDependence(s_dalDependence!.Read(dependenceId)!);

                                Console.WriteLine("Enter Dependece on: ");
                                dependeOn = int.Parse(Console.ReadLine()!);
                                dependence = new Dependence(0, dependenceId, dependeOn);
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


    public static void printEngineer(Engineer engineer)
    {
        Console.WriteLine("Engineer: ");
        Console.WriteLine("Id: " + engineer.Id);
        Console.WriteLine("Name: " + engineer.Name);
        Console.WriteLine("Email: " + engineer.Email);
        Console.WriteLine("Level: " + engineer.Level);
        Console.WriteLine("Cost: " + engineer.Cost);
    }
    public static void printTask(Task task)
    {
        Console.WriteLine("Task: ");
        Console.WriteLine("Id: " + task.Id);
        Console.WriteLine("Alias: " + task.Alias);
        Console.WriteLine("Description: " + task.Description);
        Console.WriteLine("IsMilestone: " + task.IsMilestone);
        Console.WriteLine("CreateAtDate: " + task.CreateAtDate);
        Console.WriteLine("Complexity: " + task.Complexity);
    }

    public static void printDependence(Dependence dependence)
    {
        Console.WriteLine("Dependence: ");
        Console.WriteLine("Id: " + dependence.Id);
        Console.WriteLine("DependentTask: " + dependence.DependentTask);
        Console.WriteLine("DependsOnTask: " + dependence.DependsOnTask);
    }
}
