using DO;

public record Task
(
    int Id,
    string Alias,
    string Description,
    DateTime CreatedAtDate,
    bool IsMileStone = false,
    bool isActive = true,
    DateTime? ScheduledDate = null,
    DateTime? StartDate = null,
    TimeSpan? RequiredEffortTime = null,
    DateTime? DeadlineDate = null,
    DateTime? CompleteDate = null,
    string? Deliverables = null,
    string? Remarks = null,
    int? EngineerId = null,
    EngineerExperience? Complexity = null
)
{
    public Task() : this(Id: 0, Alias: "", Description: "", CreatedAtDate: DateTime.Now) { }
    public bool ShouldSerializeScheduledDate() { return ScheduledDate.HasValue; }
    public bool ShouldSerializeStartDate() { return StartDate.HasValue; }
    public bool ShouldSerializeRequiredEffortTime() { return RequiredEffortTime.HasValue; }
    public bool ShouldSerializeDeadlineDate() { return DeadlineDate.HasValue; }
    public bool ShouldSerializeCompleteDate() { return CompleteDate.HasValue; }
    public bool ShouldSerializeDeliverables() { return !string.IsNullOrEmpty(Deliverables); }
    public bool ShouldSerializeRemarks() { return !string.IsNullOrEmpty(Remarks); }
    public bool ShouldSerializeEngineerId() { return EngineerId.HasValue; }
    public bool ShouldSerializeComplexity() { return Complexity.HasValue; }
}