
namespace DalApi;
using DO;



    public interface ITask
    {
        int Create(Task item); /// <summary> Creates new entity task in DAL </summary>
        Task? Read(int id); /// <summary> Reads entity task by its ID </summary>
        List<Task> ReadAll(); /// <summary> Stage 1 only, Reads all entity task </summary>
        void Update(Task item); /// <summary> Updates entity task </summary>
        void Delete(int id); /// <summary> Deletes an task by its Id </summary>
    }

