

namespace DO;

/// <summary>
/// Represents an Engineer entity with information about an engineer in the system.
/// </summary>
public record Engineer
(
    /// <summary>
    /// Gets or sets the unique identifier for the Engineer entity.
    /// </summary>
    int Id,

    /// <summary>
    /// Gets or sets the name of the engineer.
    /// </summary>
    string Name,

    /// <summary>
    /// Gets or sets the email of the engineer.
    /// </summary>
    string Email,

    /// <summary>
    /// Gets or sets the experience level of the engineer.
    /// </summary>
    EngineerExperience Level,

    /// <summary>
    /// Gets or sets the cost associated with the engineer.
    /// </summary>
    double Cost
)
{
    /// <summary>
    /// Initializes a new instance of the Engineer record with default values.
    /// </summary>
    public Engineer() : this(0, "", "", DO.EngineerExperience.Beginner, 0) { } // empty constructor
}


