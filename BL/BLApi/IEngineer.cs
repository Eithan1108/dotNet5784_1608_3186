

namespace BLApi;

public interface IEngineer
{
    public IEnumerable<BO.Engineer> GetEngineersList(Func<BO.Engineer, bool> filter);
    public BO.Engineer GetEngineer(int id);

    public void AddEngineer(BO.Engineer engineer);
    public void DeleteEngineer(int id);
    public void UpdateEngineer(BO.Engineer engineer);
}
