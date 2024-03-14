
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

    public DateTime Clock { get; }

    public void AddHourInPl(int hour);

    public void AddDaysInPl(int days);

    public void InitialClock();

    public void Reset(bool wothManager);

    public void setProjectStartDate(DateTime date);

    public void setProjectEndDate(DateTime date);

    public bool projectStarted();

    public void SetManagerEmail(String managerEmail);

    public void SetManagerPassWord(String managerPassword);

    public bool ManagerExist();

    public void CreateManager(string email, string password);

    public bool ManagerLogIn(string password);

    public void ResetManager();



}
