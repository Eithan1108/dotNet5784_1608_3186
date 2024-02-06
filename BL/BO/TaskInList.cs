
namespace BO;
/// <summary>
/// // task in list business object
/// </summary>
public class TaskInList
{
    public int Id { get; init; }
    public string Description { get; set; }
    public string Alias { get; set; }
    public BO.Status? Status { get; set; }

    public override string ToString() => this.ToStringProperty(); // return the object as string
}
