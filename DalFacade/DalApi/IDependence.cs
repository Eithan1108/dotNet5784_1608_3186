
namespace DalApi;
using DO;

/// <summary>
/// defines the methods for interacting with Dependence entities in the data access layer (DAL).
/// </summary
public interface IDependence
{
    /// <summary>
    /// Creates a new Dependence entity in the DAL.
    /// </summary>
    /// <param name="item">The Dependence object to be created.</param>
    /// <returns>The identifier of the newly created Dependence entity.</returns>
    int Create(Dependence item); //Creates new entity object in DAL
    Dependence? Read(int id); //Reads entity object by its ID 
    List<Dependence> ReadAll(); //stage 1 only, Reads all entity objects
    void Update(Dependence item); //Updates entity object
    void Delete(int id); //Deletes an object by its Id
}
