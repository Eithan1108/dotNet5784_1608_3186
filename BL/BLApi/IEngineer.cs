

namespace BLApi;

public interface IEngineer
{
    public IEnumerable<BO.Engineer> GetEngineersList(Func<DO.Engineer, bool> filter);
    public BO.Engineer GetEngineer(int id);
    public int AddEngineer(BO.Engineer engineer);
    public void DeleteEngineer(int id);
    public void UpdateEngineer(BO.Engineer engineer);
}
