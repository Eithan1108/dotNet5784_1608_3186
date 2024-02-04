

namespace BLApi;

public interface ITask
{
    public IEnumerable<BO.Task> GetTasksList(Func<BO.Task, bool> filter);
    public BO.Task GetTask(int id);
    public int AddTask(BO.Task task);
    public void DeleteTask(int id);
    public void UpdateTask(BO.Task task);
    public void AddOrUpdateSchedualeDateTine(int id, DateTime dateTime);
}
