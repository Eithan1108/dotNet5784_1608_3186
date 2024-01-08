
namespace DO;

/// <summary>
/// Represents a Task entity with information about a task in the system.
/// </summary>
public record Task
(
    /// <summary>
    /// Gets or sets the unique identifier for the Task entity.
    /// </summary>
    int Id,

    /// <summary>
    /// Gets or sets the alias of the task.
    /// </summary>
    string Alias,

    /// <summary>
    /// Gets or sets the description of the task.
    /// </summary>
    string Description,

    /// <summary>
    /// Gets or sets a value indicating whether the task is a milestone.
    /// </summary>
    bool IsMilestone,

    /// <summary>
    /// Gets or sets the creation date of the task.
    /// </summary>
    DateTime CreateAtDate,

    /// <summary>
    /// Gets or sets the complexity level of the task.
    /// </summary>
    DO.EngineerExperience Complexity,

    /// <summary>
    /// Gets or sets the scheduled date for the task.
    /// </summary>
    DateTime? ScheduledDate = null,

    /// <summary>
    /// Gets or sets the required effort time for the task.
    /// </summary>
    TimeSpan? RequiredEffortTime = null,

    /// <summary>
    /// Gets or sets the start date of the task.
    /// </summary>
    DateTime? StartDate = null,

    /// <summary>
    /// Gets or sets the completion date of the task.
    /// </summary>
    DateTime? CompleteDate = null,

    /// <summary>
    /// Gets or sets the deadline date for the task.
    /// </summary>
    DateTime? DeadlineDate = null,

    /// <summary>
    /// Gets or sets the deliverables associated with the task.
    /// </summary>
    string? Deliverables = null,

    /// <summary>
    /// Gets or sets any remarks or notes related to the task.
    /// </summary>
    string? Remarks = null,

    /// <summary>
    /// Gets or sets the identifier of the engineer assigned to the task.
    /// </summary>
    int? EngineerId = null
)
{
    /// <summary>
    /// Initializes a new instance of the Task record with default values.
    /// </summary>
    public Task() : this(0, "", "", false, DateTime.MinValue, DO.EngineerExperience.Beginner) { } // empty constructor
}

