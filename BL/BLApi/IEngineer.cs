

namespace BlApi;
 /// <summary>
 /// // interface for the engineer
 /// </summary>
public interface IEngineer
{
    public IEnumerable<BO.Engineer> GetEngineersList(Func<BO.Engineer, bool> filter); // get list of all engineers
    public BO.Engineer GetEngineer(int id); // get engineer by id
    public int AddEngineer(BO.Engineer engineer); // add new engineer to the system
    public void DeleteEngineer(int id); // delete engineer from the system
    public void UpdateEngineer(BO.Engineer engineer); // update engineer in the system

}
