

namespace Dal;

/// <summary>
/// Represents the data source for the DAL, including configuration and lists of tasks, engineers, and dependencies.
/// </summary>
internal static class DataSource
{
    /// <summary>
    /// Configuration settings for the data source.
    /// </summary>
    internal static class Config
    {
        /// <summary>
        /// The starting identifier for tasks.
        /// </summary>
        internal const int startTaskId = 1;

        /// <summary>
        /// The starting identifier for dependencies.
        /// </summary>
        internal const int startDependenceId = 1;

        private static int nextTaskId = startTaskId;
        /// <summary>
        /// Gets the next available task identifier.
        /// </summary>
        internal static int NextTaskId { get => nextTaskId++; }

        private static int nextDependenceId = startDependenceId;
        /// <summary>
        /// Gets the next available dependence identifier.
        /// </summary>
        internal static int NextDependenceId { get => nextDependenceId++; }

        internal static DateTime? ProjectStartDate { get; set; }
        internal static DateTime? ProjectEndDate { get; set; }
        internal static DateTime? ProjectDataScreen { get; set; }
    }

    /// <summary>
    /// The list of tasks in the data source.
    /// </summary>
    internal static List<DO.Task> Tasks { get; } = new();

    /// <summary>
    /// The list of engineers in the data source.
    /// </summary>
    internal static List<DO.Engineer> Engineers { get; } = new();

    /// <summary>
    /// The list of dependencies in the data source.
    /// </summary>
    internal static List<DO.Dependence> Dependences { get; } = new();
}
