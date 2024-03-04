

namespace DalTest;
using DalApi;
using DO;
using System;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

/// <summary>
/// Represents initialization data for testing purposes.
/// </summary>
public static class Initialization
{
    private static IDal? s_dal;
    private static readonly Random s_random = new();
    private static void createTasks()
    {
        string[][] nasaEngineersTasks = new string[][]
{
    new string[] { "Design rocket propulsion system", "DRPS" },
    new string[] { "Conduct wind tunnel testing", "CWTT", "DRPS" },
    new string[] { "Optimize fuel efficiency", "OFE", "DRPS" },
    new string[] { "Develop spacecraft control algorithms", "DSCA" },
    new string[] { "Test spacecraft navigation systems", "TSNS", "DSCA" },
    new string[] { "Implement guidance and navigation software", "IGNS", "DSCA" },
    new string[] { "Design satellite communication systems", "DSCS" },
    new string[] { "Integrate communication antennas", "ICA", "DSCS" },
    new string[] { "Test satellite communication signals", "TSCS", "DSCS" },
    new string[] { "Create 3D models for space mission simulations", "C3DM", "DRPS"},
    new string[] { "Simulate spacecraft trajectories", "SST", "C3DM" },
    new string[] { "Analyze structural integrity of spacecraft", "ASIC", "C3DM" },
    new string[] { "Develop automated docking systems", "DADS" },
    new string[] { "Test docking procedures in microgravity", "TDPIM", "DADS" },
    new string[] { "Implement autonomous navigation for space probes", "IANSP" },
    new string[] { "Test AI-powered spacecraft navigation", "TAISN", "IANSP" },
    new string[] { "Design deployable solar panels", "DDSP" },
    new string[] { "Test solar panel deployment mechanisms", "TSPDM", "DDSP" },
    new string[] { "Develop thermal control systems", "DTCS" },
    new string[] { "Test spacecraft thermal management", "TSTM", "DTCS" },
    new string[] { "Create contingency plans for mission failures", "CCPFMF" },
    new string[] { "Conduct risk analysis for space missions", "CRASM", "CCPFMF" },
    new string[] { "Implement emergency response protocols", "IERP", "CCPFMF" },
    new string[] { "Design space habitat systems", "DSHS" },
    new string[] { "Test life support systems in simulated space conditions", "TLSSSSC", "DSHS" },
    new string[] { "Develop robotics for extraterrestrial exploration", "DREETE" },
    new string[] { "Test robotic systems in analog space environments", "TRSASE", "DREETE" },
    new string[] { "Optimize power generation for deep space missions", "OPGDSM" },
    new string[] { "Test advanced propulsion concepts", "TAPC", "OPGDSM" },
    new string[] { " Test AI-powered spacecraft navigation", "TAPSN", "TAPC" },
    new string[] { "Test robotic systems in analog space environments", "TRSIASE", "TAPC" }

};
        string taskName;
        string taskNickname;
        foreach (var task in nasaEngineersTasks)
        {
            taskName = task[0];
            taskNickname = task[1];

            DateTime CreateDate = DateTime.Now.AddDays(-1 * s_random.Next(1, 31));
            int experience = s_random.Next(0, 5);



            DO.EngineerExperience level;
            switch (experience)
            {
                case 0:
                    level = EngineerExperience.Beginner;
                    break;

                case 1:
                    level = EngineerExperience.AdvancedBeginner;
                    break;
                case 2:
                    level = EngineerExperience.Intermediate;
                    break;
                case 3:
                    level = EngineerExperience.Advanced;
                    break;
                default:
                    level = EngineerExperience.Expert;
                    break;
            }
            //Task t = new Task(0, taskNickname, taskName, false, CreateAtDate,level);
            Task t = new Task { Id = 0, Alias = taskNickname, Description = taskName, CreateAtDate=CreateDate,Complexity=level, RequiredEffortTime=GenerateRandomRequiredTime() };
            
            s_dal!.Task.Create(t); ;
        }
    }

    private static void createDependences() 
    {
        Dependence[] dependences = [
            new Dependence(0, 2, 1),
            new Dependence(0, 3, 1),
            new Dependence(0, 5, 4),
            new Dependence(0, 6, 4),
            new Dependence(0, 8, 7),
            new Dependence(0, 9, 7),
            new Dependence(0, 10, 1),
            new Dependence(0, 11, 10),
            new Dependence(0, 11, 1),
            new Dependence(0, 12, 10),
            new Dependence(0, 12, 1),
            new Dependence(0, 14, 13),
            new Dependence(0, 15, 12),
            new Dependence(0, 15, 10),
            new Dependence(0, 15, 11),
            new Dependence(0, 16, 15),
            new Dependence(0, 18, 17),
            new Dependence(0, 20, 19),
            new Dependence(0, 22, 21),
            new Dependence(0, 23, 21),
            new Dependence(0, 25, 24),
            new Dependence(0, 26, 24),
            new Dependence(0, 27, 26),
            new Dependence(0, 28, 11),
            new Dependence(0, 28, 10),
            new Dependence(0, 28, 1),
            new Dependence(0, 29, 28),
            new Dependence(0, 29, 11),
            new Dependence(0, 29, 10),
            new Dependence(0, 29, 1),
            new Dependence(0, 30, 29),
            new Dependence(0, 30, 28),
            new Dependence(0, 30, 11),
            new Dependence(0, 30, 10),
            new Dependence(0, 30, 1),
            new Dependence(0, 31, 29),
            new Dependence(0, 31, 28),
            new Dependence(0, 31, 11),
            new Dependence(0, 31, 10),
            new Dependence(0, 31, 1),
        ];
        //Task 2: Conduct wind tunnel testing
        //Depends on: 1

        //Task 3: Optimize fuel efficiency
        //Depends on: 1

        //Task 5: Implement guidance and navigation software
        //Depends on: 4

        //Task 6: Design satellite communication systems
        //Depends on: 4

        //Task 8: Test satellite communication signals
        //Depends on: 7

        //Task 9: Create 3D models for space mission simulations
        //Depends on: 7

        //Task 10: Simulate spacecraft trajectories
        //Depends on: 1

        //Task 11: Analyze structural integrity of spacecraft
        //Depends on: 10, 1

        //Task 12: Analyze structural integrity of spacecraft
        //Depends on: 10, 1

        //Task 14: Develop automated docking systems
        //Depends on: 13

        //Task 16: Implement autonomous navigation for space probes
        //Depends on: 15

        //Task 18: Design deployable solar panels
        //Depends on: 17

        //Task 20: Develop thermal control systems
        //Depends on: 19

        //Task 22: Create contingency plans for mission failures
        //Depends on: 21

        //Task 23: Conduct risk analysis for space missions
        //Depends on: 21

        //Task 25: Design space habitat systems
        //Depends on: 24

        //Task 27: Develop robotics for extraterrestrial exploration
        //Depends on: 26

        //Task 29: Optimize power generation for deep space missions
        //Depends on: 28

        foreach ( Dependence de in dependences )
        {
            s_dal!.Dependence.Create(de);
        }
    }

    private static void createEngineers() {
        string[] Names = {
            "Maximilian Müller",
            "Sophia Wagner",
            "Leon Schmidt",
            "Emma Becker",
            "Lukas Hoffmann",
            "Mia Schneider",
            "Paul Fischer",
            "Emily Weber",
            "Jonas Richter",
            "Laura Keller"
           };

        foreach (var engineer in Names)
        {
            int id = s_random.Next(100000000, 400000000);
            string[] names = engineer.Split(' ');
            string email = $"{(names[0].ToLower()[0])}{names[1].ToLower()}@gmail.com";
            int experience = s_random.Next(0, 5);
            DO.EngineerExperience level;
            double cost;
            switch (experience)
            {
                case 0:
                    level = EngineerExperience.Beginner;
                    cost = s_random.Next(200, 900) / 10;
                    break;

                case 1:
                    level = EngineerExperience.AdvancedBeginner;
                    cost = s_random.Next(900, 1900) / 10;
                    break;
                case 2:
                    level = EngineerExperience.Intermediate;
                    cost = s_random.Next(1900, 3000) / 10;
                    break;
                case 3:
                    level = EngineerExperience.Advanced;
                    cost = s_random.Next(3000, 4200) / 10;
                    break;
                default:
                    level = EngineerExperience.Expert;
                    cost = s_random.Next(4200, 7000) / 10;
                    break;
            }

            Engineer e = new Engineer(id, engineer, email, level, cost);
            s_dal!.Engineer.Create(e);
        }
    }

    /// <summary>
    /// Performs the initialization of the data for testing purposes.
    /// </summary>
    /// <param name="dalEngineer">The engineer data access layer.</param>
    /// <param name="dalDependence">The dependence data access layer.</param>
    /// <param name="dalTask">The task data access layer.</param>


    static TimeSpan GenerateRandomRequiredTime()
    {
        Random rnd = new Random();
        int totalDays = rnd.Next(1, 12); // Random number of days between 1 and 11 (inclusive)

        // Construct the TimeSpan with the random number of days
        TimeSpan randomTimeSpan = TimeSpan.FromDays(totalDays);

        return randomTimeSpan;
    }

    public static void Do()
    {
         s_dal = Factory.Get; // ************************************* why not DalApi.Factory.Get
        createDependences();
        createTasks();
        createEngineers();
    }

}
