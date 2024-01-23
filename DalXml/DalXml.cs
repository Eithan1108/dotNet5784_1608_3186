
using DalApi;
namespace Dal;

/// <summary>
/// Represents a sealed class providing XML implementation for the data access layer.
/// </summary>
sealed public class DalXml : IDal
{
    /// <summary>
    /// Gets an instance of the <see cref="IEngineer"/> interface using the XML implementation.
    /// </summary>
    public IEngineer Engineer => new EngineerImplementation();

    /// <summary>
    /// Gets an instance of the <see cref="ITask"/> interface using the XML implementation.
    /// </summary>
    public ITask Task => new TaskImplementation();

    /// <summary>
    ///  Gets an instance of the <see cref="IDependence"/> interface using the XML implementation.
    /// </summary>
    public IDependence Dependence => new DependenceImplementation();
}
