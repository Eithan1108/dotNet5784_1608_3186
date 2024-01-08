

namespace DalApi;

using DO;

public interface IEngineer
{
    int Create(Engineer item); /// <summary> Creates new entity object in DAL </summary>
    Engineer? Read(int id); /// <summary> Reads entity object by its ID </summary>
    List<Engineer> ReadAll(); /// <summary> Stage 1 only, Reads all entity objects </summary>
    void Update(Engineer item); /// <summary> Updates entity object </summary>
    void Delete(int id); /// <summary> Deletes an object by its Id </summary>
}
