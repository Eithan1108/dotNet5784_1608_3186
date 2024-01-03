
namespace DO;
public record Task
(
   int Id, 
   string Alias,
   string Description,
   bool IsMilestone, 
   DateTime CreateAtDate,
   DO.EngineerExperience Complexity,
    DateTime? ScheduledDate = null,
   TimeSpan? RequiredEffortTime = null,
   DateTime? StartDate = null,
   DateTime? CompleteDate = null,
   DateTime? DeadlineDate = null,
   string? Deliverables = null,
   string? Remarks = null,
   int? EngineerId = null
)
{
    public Task() : this(0, "", "", false, DateTime.MinValue, DO.EngineerExperience.Beginner) { } //empty constructor
}
