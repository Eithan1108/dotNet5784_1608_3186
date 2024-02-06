
namespace BlApi;

/// <summary>
/// // interface for the BL layer
/// </summary>
public interface IBl
{
    public ITask Task { get; }
    public IEngineer Engineer { get; }

    public ILooz Looz { get; }
}
