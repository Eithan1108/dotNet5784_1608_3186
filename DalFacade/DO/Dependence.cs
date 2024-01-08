

namespace DO;

/// <summary>
/// Represents a Dependence entity with information about task dependencies in the system.
/// </summary>
public record Dependence
(
    /// <summary>
    /// Gets or sets the unique identifier for the Dependence entity.
    /// </summary>
    int Id,

    /// <summary>
    /// Gets or sets the identifier of the dependent task.
    /// </summary>
    int? DependentTask = null,

    /// <summary>
    /// Gets or sets the identifier of the task on which the current task depends.
    /// </summary>
    int? DependsOnTask = null
)
{
    /// <summary>
    /// Initializes a new instance of the Dependence record with default values.
    /// </summary>
    public Dependence() : this(0) { } // empty constructor
}
