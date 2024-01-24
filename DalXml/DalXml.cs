
using DalApi;
using System.Diagnostics;
namespace Dal;

/// <summary>
/// Represents a sealed class providing XML implementation for the data access layer.
/// </summary>
sealed internal class DalXml : IDal
{

    /*
     * We implemented the class using thread-safe and lazy initialization.
     * Thread-safe ensures that there will not be a situation where several processes will try to
     * create an instance of the class, and a lack of synchronization will 
     * occur (as mentioned, only one instance will be created in the Singleton class).
     * Lazy initialization will prevent the compiler from compiling the instance creation
     * command unnecessarily, but only at the moment the instance is created.
    */

    // *** we can use this code for DalList and DalXml becouse they will never be used together *** //

    private static Lazy<DalXml> instance = null; // Singleton with lazy initialization
    private static readonly object padlock = new object(); // for thread-safe to know which thread is currently use the instance
    private DalXml() { } // private CTOR for Singleton
    public static Lazy<DalXml> Instance // call to create the instance
    {
        get
        {
            lock (padlock) // check if the theard have padlock (only one thread can have padlock at a time) if so, allowd to create the instance
            {
                if (instance == null) // if the instance is null (not created yet) create the instance
                {
                    instance = new Lazy<DalXml>(); // create the instance
                }
                return instance; // if the instance is not null (created) return the instance
            }
        }
    }





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
