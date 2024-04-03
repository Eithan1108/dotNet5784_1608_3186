
namespace BlApi;

/// <summary>
/// // interface for the BL layer
/// </summary>
public interface IBl
{
    public ITask Task { get; }
    public IEngineer Engineer { get; }

    public ILooz Looz { get; }

    public IManager Manager { get; }


    // clock of the system 
    public DateTime Clock { get; }

    public void AddHourInPl(int hour);

    public void AddDaysInPl(int days);

    public void InitialClock();

    public void Reset(bool wothManager);

    public void setProjectStartDate(DateTime date);

    public void setProjectEndDate(DateTime date);

    public bool projectStarted();

    public void ExportToPdf();


}
