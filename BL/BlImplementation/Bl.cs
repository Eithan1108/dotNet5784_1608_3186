
namespace BlImplementation;
using BlApi;
 
/// <summary>
/// // This class is the main class of the BL layer
/// </summary>
internal class Bl : IBl
{
    public ITask Task =>  new TaskImplementation(); // create new TaskImplementation
    public IEngineer Engineer =>  new EngineerImplementation(); // create new EngineerImplementation

    public ILooz Looz =>  new LoozImplementation(); // create new LoozImplementation
}

