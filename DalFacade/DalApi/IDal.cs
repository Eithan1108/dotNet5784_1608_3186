

namespace DalApi
{
    public interface IDal
    {
        IEngineer Engineer { get; }
        ITask Task { get; }
        IDependence Dependence { get; }

        ILooz Looz { get; }

        IManager Manager { get; }
    }
}
