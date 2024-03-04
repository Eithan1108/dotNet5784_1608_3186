
namespace BlApi;

/// <summary>
/// // interface for the BL layer
/// </summary>
public interface IBl
{
    public ITask Task { get; }
    public IEngineer Engineer { get; }

    public ILooz Looz { get; }

    public DateTime Clock { get; }

    public void AddHourInPl(int hour);

    public void AddDaysInPl(int days);

    public void InitialClock();

    public void Reset();

    public void setProjectStartDate(DateTime date);

    public void setProjectEndDate(DateTime date);

    public bool projectStarted();

}
