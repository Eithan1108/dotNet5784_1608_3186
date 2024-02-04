

namespace BO;

public class Engineer
{
    public int? Id { get; init; }
    public string Name { get; set; }
    public string Email { get; set; }
    public BO.EngineerExperience Level { get; set; }
    public double Cost { get; set; }
    public BO.TaskInEngineer Task { get; set; }

    //    public Engineer(int? id, string name, string email, BO.EngineerExperience level, double cost) ///check this
    //    {
    //        Id = id;
    //        Name = name;
    //        Email = email;
    //        Level = level;
    //        Cost = cost;
    //    }
}
