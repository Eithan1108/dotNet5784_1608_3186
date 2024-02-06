
namespace BO;

/// <summary>
/// // task business object
/// </summary>
public class Task // comments and bounos
{
    public int Id { get; init; }
    public string Description { get; set; }
    public string Alias { get; set; }
    public DateTime CreatedAtDate { get; init; }
    public BO.Status? Status { get; set; }
    public IEnumerable<BO.TaskInList>? Dependencies { get; set; }
    public BO.MilestoneInTask? Milestone { get; set; }
    public TimeSpan? RequiredEffortTime { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? ScheduledDate { get; set; }
    public DateTime? ForecastDate { get; set; }
    public DateTime? DeadLineDate { get; set; }
    public DateTime? CompleteDate { get; set; }
    public string? Deliverables { get; set; }
    public string? Remarks { get; set; }
    public BO.EngineerInTask? Engineer { get; set; }
    public BO.EngineerExperience? Complexity { get; set; }

    public override string ToString() => this.ToStringProperty(); // return the object as string

}
