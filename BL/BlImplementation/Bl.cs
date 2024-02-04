
namespace BlImplementation;
using BLApi;

internal class Bl : IBl
{
    public ITask Task =>  new TaskImplementation();
    public IEngineer Engineer =>  new EngineerImplementation();
}

