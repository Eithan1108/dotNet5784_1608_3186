
namespace BO;
/// <summary>
/// // task in engineer business object
/// </summary>
public class TaskInEngineer
{
    public int? Id { get; init; }
    public string Alias { get; set; }

    public override string ToString() => this.ToStringProperty(); // return the object as string
}
