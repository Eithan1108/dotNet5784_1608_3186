namespace BO;

/// <summary>
/// //engineer business object
/// </summary>
public enum Status // status of the task
{
    Unsheduled,
    Scheduled,
    OnTrack,
    InJeopardy,
    Done
}

public enum EngineerExperience // experience of the engineer
{
    Beginner,
    AdvancedBeginner,
    Intermediate,
    Advanced,
    Expert,
    All
}